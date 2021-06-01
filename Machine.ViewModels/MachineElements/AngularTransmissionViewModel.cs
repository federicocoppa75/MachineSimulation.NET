using MDIT = Machine.Data.Interfaces.Tools;
using Machine.Data.Base;

namespace Machine.ViewModels.MachineElements
{
    public class AngularTransmissionViewModel : ElementViewModel
    {
        private static Color _bodyColor = new Color() { A = 255, B = 128, G = 128, R = 128 };
        private static Color _toolColor = new Color() { A = 255, B = 255 };

        public MDIT.IAngularTransmission Tool { get; set; }
        public string BodyModelFile => (Tool != null) ? Tool.BodyModelFile : null;

        public AngularTransmissionViewModel() : base()
        {
            Color = _bodyColor;
        }

        internal void AppendSubSpindle(Point position, Vector direction, MDIT.ITool tool)
        {
            var i = 0;
            var ssvm = new ATToolholderViewModel()
            {
                Name = $"spindle {i++}",
                Position = position,
                Direction = direction,
                Parent = this
            };

            if(tool != null)
            {
                ssvm.Children.Add(new ToolViewModel()
                {
                    Name = tool.Name,
                    Tool = tool,
                    Color = _toolColor,
                    IsVisible = true,
                    Parent = ssvm
                });
            }

            Children.Add(ssvm);
        }
    }
}
