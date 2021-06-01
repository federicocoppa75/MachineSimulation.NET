using Machine.Data.Base;

namespace Machine.ViewModels.MachineElements
{
    public class ATToolholderViewModel : ElementViewModel
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }

        public ATToolholderViewModel() : base()
        {
        }
    }
}
