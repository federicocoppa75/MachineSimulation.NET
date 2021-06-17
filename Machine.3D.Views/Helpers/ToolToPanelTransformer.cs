using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using System;
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
            if(tool.Parent is IToolholderElement th)
            {
                return new Point3D(th.Position.X, th.Position.Y, th.Position.Z);
            }
            else if(tool.Parent is IATToolholder atth)
            {
                return GetToolPositionOnATTH(atth);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private Point3D GetToolPositionOnATTH(IATToolholder atth)
        {
            if((atth.Parent is IAngularTransmission at) && (at.Parent is IToolholderElement th))
            {
                var p = new Point3D(th.Position.X, th.Position.Y, th.Position.Z);
                var v = new Vector3D(atth.Position.X, atth.Position.Y, atth.Position.Z);

                return p + v;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private Vector3D GetToolDirection(IToolElement tool)
        {
            if (tool.Parent is IToolholderElement th)
            {
                return new Vector3D(th.Direction.X, th.Direction.Y, th.Direction.Z);
            }
            else if (tool.Parent is IATToolholder atth)
            {
                return GetToolDirectionOnATTH(atth);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private Vector3D GetToolDirectionOnATTH(IATToolholder atth)
        {
            if ((atth.Parent is IAngularTransmission at) && (at.Parent is IToolholderElement th))
            {
                if((th.Direction.X == 0.0) && (th.Direction.Y == 0.0) && (th.Direction.Z == -1.0))
                {
                    return new Vector3D(atth.Direction.X, atth.Direction.Y, atth.Direction.Z);
                }
                else
                {
                    var v1 = new Vector3D(0.0, 0.0, -1.0);
                    var v2 = new Vector3D(th.Direction.X, th.Direction.Y, th.Direction.Z);
                    var n = Vector3D.CrossProduct(v1, v2);
                    var a = Vector3D.AngleBetween(v1, v2);

                    n.Normalize();

                    var aat = new AxisAngleRotation3D() { Axis = n, Angle = a };
                    var rt = new RotateTransform3D(aat);

                    return rt.Transform(new Vector3D(atth.Direction.X, atth.Direction.Y, atth.Direction.Z));
                }
            }
            else
            {
                throw new ArgumentException();
            }
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
