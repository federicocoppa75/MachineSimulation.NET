using Machine.Steps.ViewModels.Extensions.LinkMovementsItems;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Steps.ViewModels.Extensions
{
    public class LinkMovementManager : ILinkMovementManager
    {
        List<ILinearLinkMovementItem> _items = new List<ILinearLinkMovementItem>();
        Dictionary<int, ILinksMovementsGroup> _itemsGroups = new Dictionary<int, ILinksMovementsGroup>();
        object _lockObj1 = new object();
        object _lockObj2 = new object();
        DateTime _lastProcess;
        bool _firtProcessCall;
        int _processing = 0;

        private IProcessCaller _processCaller;
        private IProcessCaller ProcessCaller => _processCaller ?? (_processCaller = Machine.ViewModels.Ioc.SimpleIoc<IProcessCaller>.GetInstance());

        //private IDispatcherHelper _dispatcherHelper;
        //private IDispatcherHelper DispatcherHelper => _dispatcherHelper ?? (_dispatcherHelper = Machine.ViewModels.Ioc.SimpleIoc<IDispatcherHelper>.GetInstance());

        private IMessenger _messenger;
        private IMessenger Messenger => _messenger ?? (_messenger = Machine.ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance());

        public int MinTimespam { get; set; } = 50;

        private bool _enable;
        public bool Enable 
        {
            get => _enable;
            set
            {
                if(_enable != value)
                {
                    _enable = value;

                    if(_enable)
                    {
                        _firtProcessCall = true;
                        ProcessCaller.ProcessRequest += OnProcess;
                    }
                    else
                    {
                        ProcessCaller.ProcessRequest -= OnProcess;
                    }
                }             
            }
        }
        public double TimespanFactor { get; set; } = 1.0;

        public void Add(int linkId, double targetValue, double duration, int notifyId)
        {
            lock (_lockObj1)
            {
                Messenger.Send(new GetLinkMessage()
                {
                    Id = linkId,
                    SetLink = (link) =>
                    {
                        _items.Add(LinearLinkMovementItem.Create(link, targetValue, duration * TimespanFactor, notifyId));
                    }
                });
            }
        }

        public void Add(int groupId, int linkId, double targetValue, double duration, int notifyId)
        {
            lock (_lockObj2)
            {
                if (!_itemsGroups.TryGetValue(groupId, out ILinksMovementsGroup group))
                {
                    group = LinksMovementsGroup.Create(groupId, duration * TimespanFactor, notifyId);
                    _itemsGroups.Add(groupId, group);
                }


                Messenger.Send(new GetLinkMessage()
                {
                    Id = linkId,
                    SetLink = (link) =>
                    {
                        group.Add(link, targetValue);
                    }
                });
            }
        }

        public void Add(int linkId, double targetValue, double duration, ILinkMovementManager.ArcComponentData data, int notifyId)
        {
            lock (_lockObj2)
            {
                if (!_itemsGroups.TryGetValue(data.GroupId, out ILinksMovementsGroup group))
                {
                    group = LinksMovementsGroup.Create(data.GroupId, duration * TimespanFactor, notifyId);
                    _itemsGroups.Add(data.GroupId, group);
                }

                Messenger.Send(new GetLinkMessage()
                {
                    Id = linkId,
                    SetLink = (link) =>
                    {
                        group.Add(link, targetValue, data);
                    }
                });
            }
        }

        private void OnProcess(object sender, DateTime e)
        {
            if (Interlocked.CompareExchange(ref _processing, 1, 0) == 0)
            {
                if (_firtProcessCall)
                {
                    _lastProcess = e;
                    _firtProcessCall = false;
                }
                else
                {
                    var elapse = e - _lastProcess;

                    if (elapse >= TimeSpan.FromMilliseconds(MinTimespam))
                    {
                        _lastProcess = e;
                        EvaluateItems(e);
                        EvaluateGroups(e);
                    }
                }

                Interlocked.Exchange(ref _processing, 0);
            }
        }

        private void EvaluateGroups(DateTime now)
        {
            lock (_lockObj2)
            {
                var completed = new Stack<int>();

                foreach (var ig in _itemsGroups.Values)
                {
                    ig.Progress(now);
                    
                    if(ig.IsCompleted)
                    {
                        completed.Push(ig.GroupId);
                        if (ig.NotifyId > 0) Messenger.Send(new ActionExecutedMessage() { Id = ig.NotifyId });
                    }
                }

                foreach (var item in completed) _itemsGroups.Remove(item);
            }
        }

        private void EvaluateItems(DateTime now)
        {
            lock (_lockObj1)
            {
                var completed = new Stack<ILinearLinkMovementItem>();

                _items.ForEach(i =>
                {
                    i.Progress(now);

                    if(i.IsCompleted)
                    {
                        completed.Push(i);
                        if(i.NotifyId > 0) Messenger.Send(new ActionExecutedMessage() { Id = i.NotifyId });
                    }
                });

                foreach (var item in completed) _items.Remove(item);
            }
        }
    }
}
