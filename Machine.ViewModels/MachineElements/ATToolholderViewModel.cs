using Machine.Data.Base;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine.ViewModels.MachineElements
{
    public class ATToolholderViewModel : ElementViewModel, IATToolholder
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }

        public ATToolholderViewModel() : base()
        {
        }
    }
}
