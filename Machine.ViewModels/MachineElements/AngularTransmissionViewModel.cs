using Machine.Data.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;
using MDT = Machine.Data.Tools;

namespace Machine.ViewModels.MachineElements
{
    public class AngularTransmissionViewModel : ElementViewModel
    {
        private static Color _bodyColor = new Color() { A = 255, B = 128, G = 128, R = 128 };
        private static Color _toolColor = new Color() { A = 255, B = 255 };

        public MDT.AngularTransmission Tool { get; set; }
        public string BodyModelFile => (Tool != null) ? Tool.BodyModelFile : null;

        public AngularTransmissionViewModel() : base()
        {
            Color = _bodyColor;
        }

        public void ApplaySubSpindlesTooling()
        {
            int i = 1;

            foreach (var item in Tool.Subspindles)
            {
                var ssvm = new ATToolholderViewModel()
                {
                    Name = $"spindle {i++}",
                    Position = item.Position,
                    Direction = item.Direction
                };

                if((item is MDT.SubspindleEx sse) && (sse.Tool != null))
                {
                    ssvm.Children.Add(new ToolViewModel()
                    {
                        Name = sse.ToolName,
                        Tool = sse.Tool,
                        Color = _toolColor,
                        IsVisible = true
                    });
                }

                Children.Add(ssvm);
            }
        }
    }
}
