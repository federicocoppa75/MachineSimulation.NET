using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messages.Links.Gantry;

namespace Machine.ViewModels.Links
{
    public class LinearLinkViewModel : LinkViewModel, ILinearLinkViewModel, ILinkViewModel
    {
        #region private field
        private double _gantryGap;
        #endregion

        #region data properties
        public double Min { get; set; }
        public double Max { get; set; }
        public double Pos { get; set; }
        #endregion

        #region view properties4
        public override LinkMoveType MoveType => LinkMoveType.Linear;
        #endregion

        #region ctor
        public LinearLinkViewModel() : base()
        {
            Messenger.Register<GantryMessage>(this, OnGantryMessage);
        }
        #endregion

        #region messages handlers
        private void OnGantryMessage(GantryMessage msg)
        {
            if (msg.Slave == Id)
            {
                Messenger.Send(new GetLinkMessage()
                {
                    Id = msg.Master,
                    SetLink = (link) =>
                    {
                        _gantryGap = Value - link.Value;

                        if (msg.State)
                        {
                            link.ValueChanged += OnGantryMasterChanged;
                        }
                        else
                        {
                            link.ValueChanged -= OnGantryMasterChanged;
                        }
                    }
                });
            }
        }
        #endregion

        #region private methods
        private void OnGantryMasterChanged(object sender, double e) => Value = e + _gantryGap;
        #endregion
    }
}
