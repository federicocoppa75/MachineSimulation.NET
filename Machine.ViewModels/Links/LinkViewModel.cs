using Machine.Data.Enums;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public abstract class LinkViewModel : BaseViewModel, ILinkViewModel
    {
        #region data properties
        //public int LinkID { get; set; }
        private int _id;
        public int Id 
        { 
            get => _id; 
            set => Set(ref _id, value, nameof(Id)); 
        }

        private LinkDirection _direction;
        public LinkDirection Direction 
        { 
            get => _direction; 
            set => Set(ref _direction, value, nameof(Direction)); 
        }

        private LinkType _type;
        public LinkType Type 
        { 
            get => _type; 
            set => Set(ref _type, value, nameof(Type)); 
        }
        #endregion

        #region view properties
        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (Set(ref _value, value, nameof(Value)))
                {
                    ValueChanged?.Invoke(this, _value);
                }
            }
        }
        public abstract LinkMoveType MoveType { get; }

        private string _description;
        public string Description 
        { 
            get => _description; 
            set => Set(ref _description, value, nameof(Description)); 
        }
        #endregion

        #region view events
        public event EventHandler<double> ValueChanged;
        #endregion

        #region ctor
        public LinkViewModel() : base()
        {
            Messenger.Register<GetLinkMessage>(this, OnGetLinkMessage);
        }
        #endregion

        #region private methods
        private void OnGetLinkMessage(GetLinkMessage msg)
        {
            if ((msg.Id == Id) || (msg.Id == -1))
            {
                msg.SetLink(this);
            }
        }
        #endregion
    }
}
