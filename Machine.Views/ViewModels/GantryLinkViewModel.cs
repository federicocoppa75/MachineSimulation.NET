using Machine.ViewModels.Base;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messages.Links.Gantry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class GantryLinkViewModel : BaseViewModel
    {
        public int Master { get; set; }

        private int _slave;
        public int Slave 
        { 
            get => _slave; 
            set
            {
                var last = _slave;

                if (Set(ref _slave, value, nameof(Slave)))
                {
                    EvaluateGantry(_slave, last);
                }
            }
        }
        public List<int> CompatibleLinks { get; set; }

        private bool _isEnable;
        public bool IsEnable 
        { 
            get => _isEnable; 
            set => Set(ref _isEnable, value, nameof(IsEnable)); 
        }

        public GantryLinkViewModel() : base()
        {
            Messenger.Register<GantryMessage>(this, OnGantryMessage);
        }

        private void OnGantryMessage(GantryMessage msg)
        {
            if (msg.Slave == Master) IsEnable = !msg.State;
        }

        public void Initialize()
        {
            CompatibleLinks = new List<int>() { -1 };
            FillCompatibleLinks();
            _slave = -1;
            IsEnable = CompatibleLinks.Count > 1;
        }

        private void FillCompatibleLinks()
        {
            Messenger.Send(new GetLinkMessage() 
            { 
                Id = Master,
                SetLink = (master) =>
                {
                    Messenger.Send(new GetLinkMessage()
                    {
                        Id = -1,
                        SetLink = (link) =>
                        {
                            if((link.MoveType == Data.Enums.LinkMoveType.Linear) && 
                                (link.Id != Master) && 
                                (link.Direction == master.Direction))
                            {
                                CompatibleLinks.Add(link.Id);
                            }
                        }
                    });
                }
            });
        }

        private void EvaluateGantry(int slave, int last)
        {
            if(last == -1)
            {
                Messenger.Send(new GantryMessage() { Master = Master, Slave = slave, State = true });
            }
            else if(slave == -1)
            {
                Messenger.Send(new GantryMessage() { Master = Master, Slave = last, State = false });
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
