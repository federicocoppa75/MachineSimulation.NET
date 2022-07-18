using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messages.Links.Gantry;
using Machine.ViewModels.UI.Attributes;

namespace Machine.ViewModels.Links
{
    [Link("Linear")]
    public class LinearLinkViewModel : LinkViewModel, ILinearLinkViewModel, ILinkViewModel
    {
        #region private field
        private double _gantryGap;
        #endregion

        #region data properties
        private double _min;
        public double Min 
        { 
            get => _min; 
            set => Set(ref _min, value, nameof(Min)); 
        }

        private double _max;
        public double Max 
        { 
            get => _max; 
            set => Set(ref _max, value, nameof(Max)); 
        }

        private double _pos;
        public double Pos 
        { 
            get => _pos; 
            set => Set(ref _pos, value, nameof(Pos)); 
        }

        private bool _overflow;

        public bool Overflow
        {
            get => _overflow;
            set => Set(ref _overflow, value, nameof(Overflow));
        }

        #endregion

        #region view properties
        public override LinkMoveType MoveType => LinkMoveType.Linear;
        #endregion

        #region ctor
        public LinearLinkViewModel() : base()
        {
            Messenger.Register<GantryMessage>(this, OnGantryMessage);
            ValueChanged += (s, e) => UpdateOverflow();
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

        private void UpdateOverflow() => Overflow = (Value < _min) || (Value > _max);
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            Messenger.Unregister<GantryMessage>(this);
            base.Dispose(disposing);
        }
        #endregion
    }
}
