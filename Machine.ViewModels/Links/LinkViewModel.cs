using Machine.Data.Enums;
using Machine.ViewModels.Base;
using Machine.ViewModels.Messages.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public abstract class LinkViewModel : BaseViewModel
    {
        #region data properties
        //public int LinkID { get; set; }
        public int Id { get; set; }
        public LinkDirection Direction { get; set; }
        public LinkType Type { get; set; }
        #endregion

        #region view properties
        private double _value;
        public double Value 
        {
            get => _value; 
            set
            {
                if(Set(ref _value, value, nameof(Value)))
                {
                    ValueChanged?.Invoke(this, _value);
                }
            }
        }
        public abstract LinkMoveType MoveType { get; }
        public string Description { get; set; }
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
            if((msg.Id == Id) || (msg.Id == -1))
            {
                msg?.SetLink(this);
            }
        }
        #endregion
    }
}
