using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messaging;
using Machine.Views.Messages.Links;
using System;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.LinkProxies
{
    public class LinkProxyViewModel : INotifyPropertyChanged, IDisposable
    {
        protected ILinkViewModel _link;

        [PropertyOrder(0)]
        public int Id 
        {
            get => _link.Id; 
            set => _link.Id = value; 
        }

        [PropertyOrder(1)]
        public LinkMoveType MoveType => _link.MoveType;

        [PropertyOrder(2)]
        public LinkType Type
        {
            get => _link.Type;
            set => _link.Type = value;
        }

        [PropertyOrder(3)]
        public LinkDirection Direction
        {
            get => _link.Direction;
            set => _link.Direction = value;
        }

        [PropertyOrder(4)]
        public string Description 
        { 
            get => _link.Description; 
            set => _link.Description = value; 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LinkProxyViewModel(ILinkViewModel link)
        {
            _link = link;

            if (_link is INotifyPropertyChanged npc) npc.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

            if((string.Compare(e.PropertyName, nameof(Direction)) == 0) ||
                (string.Compare(e.PropertyName, nameof(Type)) == 0))
            {
                Machine.ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance().Send(new UpdateStructByLinkMessage());
            }
        }

        public override string ToString() => $"Id({Id}) Move({MoveType}) Type({Type})";

        #region IDisposable
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_link is INotifyPropertyChanged npc) npc.PropertyChanged -= OnPropertyChanged;
                    _link = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
