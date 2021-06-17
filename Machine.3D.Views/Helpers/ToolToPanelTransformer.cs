using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Toolholder;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using MDB = Machine.Data.Base;

namespace Machine._3D.Views.Helpers
{
    class ToolToPanelTransformer : IToolToPanelTransformer
    {
        IPanelElement _panel;
        IEnumerable<IToolElement> _tools;

        public ToolToPanelTransformer(IPanelElement panel, IEnumerable<IToolElement> tools)
        {
            _panel = panel;
            _tools = tools;
        }

        public IEnumerable<ToolPosition> Transform()
        {
            var positions = new List<ToolPosition>();
            var matrix1 = GetPanelChainTransformation();

            matrix1.Invert();

            foreach (var tool in _tools)
            {
                var matrix = tool.GetChainTransformation();
                var p0 = GetToolPosition(tool);
                var d0 = GetToolDirection(tool);
                var p1 = matrix.Transform(p0);
                var d1 = matrix.Transform(d0);
                var p2 = matrix1.Transform(p1);
                var d2 = matrix1.Transform(d1);

                var tp = p2 + d2 * tool.WorkLength;

                positions.Add(new ToolPosition()
                {
                    Point = new MDB.Point() { X = tp.X, Y = tp.Y, Z = tp.Z },
                    Direction = new MDB.Vector() { X = d2.X, Y = d2.Y, Z = d2.Z },
                    Radius = tool.WorkRadius,
                    Length = tool.UsefulLength
                });

            }

            return positions;
        }

        private Point3D GetToolPosition(IToolElement tool)
        {
            var p = (tool.Parent as ToolholderElementViewModel).Position;

            return new Point3D(p.X, p.Y, p.Z);
        }

        private Vector3D GetToolDirection(IToolElement tool)
        {
            var d = (tool.Parent as ToolholderElementViewModel).Direction;

            return new Vector3D(d.X, d.Y, d.Z);
        }

        private Matrix3D GetPanelChainTransformation()
        {
            var matrix = _panel.GetChainTransformation();

            if (_panel is IMovablePanel mp)
            {
                var m = Matrix3D.Identity;

                m.OffsetX = mp.OffsetX;
                matrix.Append(m);
            }

            var mc = Matrix3D.Identity;

            mc.OffsetX = _panel.CenterX;
            mc.OffsetY = _panel.CenterY;
            mc.OffsetZ = _panel.CenterZ;

            matrix.Append(mc);

            return matrix;
        }
    }
}
