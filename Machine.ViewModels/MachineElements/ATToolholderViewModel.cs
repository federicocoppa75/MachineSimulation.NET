using Machine.Data.Base;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine.ViewModels.MachineElements
{
    public class ATToolholderViewModel : ElementViewModel, IATToolholder
    {
        public Point Position { get; set; } = new Point();
        public Vector Direction { get; set; } = new Vector() { Z = -1 };

        public ATToolholderViewModel() : base()
        {
        }
    }
}
