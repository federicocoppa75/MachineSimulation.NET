using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using MDB = Machine.Data.Base;
using MVMME = Machine.ViewModels.MachineElements;
using MDT = Machine.Data.Tools;
using MDIT = Machine.Data.Interfaces.Tools;
using MRI = MaterialRemove.Interfaces;

namespace Machine._3D.Views.Helpers
{
    class ToolToPanelTransformer : IToolToPanelTransformer
    {
        const int _nSections = 24;
        IPanelElement _panel;
        IEnumerable<IToolElement> _tools;

        public ToolToPanelTransformer(IPanelElement panel, IEnumerable<IToolElement> tools)
        {
            _panel = panel;
            _tools = tools;
        }

        public IList<ToolPosition> Transform()
        {
            var positions = new List<ToolPosition>();
            var matrix1 = _panel.GetChainTransformation();

            matrix1.Invert();

            foreach (var tool in _tools)
            {
                GetToolPositionAndDirection(tool, matrix1, out Point3D tp, out Vector3D d2);

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

        public Task<IList<ToolPosition>> TransformAsync()
        {
            return Task.Run(async () =>
            {
                var positions = new List<ToolPosition>();
                var matrix1 = _panel.GetChainTransformation();
                var tasks = new List<Task<ToolPosition>>();

                matrix1.Invert();

                foreach (var tool in _tools)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        GetToolPositionAndDirection(tool, matrix1, out Point3D tp, out Vector3D d2);

                        return new ToolPosition()
                        {
                            Point = new MDB.Point() { X = tp.X, Y = tp.Y, Z = tp.Z },
                            Direction = new MDB.Vector() { X = d2.X, Y = d2.Y, Z = d2.Z },
                            Radius = tool.WorkRadius,
                            Length = tool.UsefulLength
                        };
                    }));
                }

                return await Task.WhenAll(tasks).ContinueWith((t) =>
                {
                    return t.Result as IList<ToolPosition>;
                });

                //return Task.WhenAll(tasks).ContinueWith((t) => t.Result as IEnumerable<ToolPosition>);
            });
        }

        public void TransformAndApplay()
        {
            var matrix1 = _panel.GetChainTransformation();

            matrix1.Invert();

            foreach (var tool in _tools)
            {
                GetToolPositionAndDirection(tool, matrix1, out Point3D tp, out Vector3D d2);
                ApplayTool(tool, tp, d2);
            }
        }

        public Task<int> TransformAndApplayAsync()
        {
            return Task.Run(async () =>
            {
                var matrix1 = _panel.GetChainTransformation();
                var tasks = new List<Task>();

                matrix1.Invert();

                foreach (var tool in _tools)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        GetToolPositionAndDirection(tool, matrix1, out Point3D tp, out Vector3D d2);
                        await ApplayToolAsync(tool, tp, d2);
                    }));
                }

                await Task.WhenAll(tasks);

                return 0;
            });
        }

        private void GetToolPositionAndDirection(IToolElement tool, Matrix3D panelMatrix, out Point3D position, out Vector3D direction)
        {
            var matrix = tool.GetChainTransformation();
            var p0 = GetToolPosition(tool);
            var d0 = GetToolDirection(tool);
            var p1 = matrix.Transform(p0);
            var d1 = matrix.Transform(d0);
            var p2 = panelMatrix.Transform(p1);
            var d2 = panelMatrix.Transform(d1);

            position = p2 + d2 * tool.WorkLength;
            direction = d2;
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

        private void ApplayTool(IToolElement tool, Point3D position, Vector3D direction)
        {
            var t = (tool as MVMME.ToolViewModel).Tool;

            //if (t is MDT.DiskTool dt) ApplyTool(dt, position, direction);
            //else if (t is MDT.DiskOnConeTool doct) ApplyTool(doct, position, direction);
            if (t is MDT.PointedTool pt) ApplyTool(pt, position, direction);
            else if (t is MDT.CountersinkTool ct) ApplyTool(ct, position, direction);
            else ApplyTool(t, position, direction);
        }

        private void ApplyTool(MDIT.ITool t, Point3D position, Vector3D direction)
        {
            var ta = new MRI.ToolActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)t.GetTotalLength(),
                Radius = (float)t.GetTotalDiameter() / 2.0f
            };

            if (_panel is MRI.IPanel panel) panel.ApplyAction(ta);
        }

        private void ApplyTool(MDT.DiskTool dt, Point3D position, Vector3D direction)
        {
            var radius = dt.Diameter / 2.0;
            var sa = 360.0 / _nSections;                                // ampiezza angolare delle sezioni
            var sh = dt.CuttingRadialThickness;                         // altezza sezione;
            var sw = radius * sa * (Math.PI * 2.0) / 360.0;             // larghezza sezione
            var sl = dt.CuttingThickness;                               // linghezza sezione
            var orientation = ToOrientation(direction);
            var n = direction;
            var r = GetRadial(orientation);
            var p = new Point3D(position.X,
                                position.Y,
                                position.Z) - n * sl / 2.0;
            //var d = new Vector3D(ToolPosition.DX - PanelPosition.DX,
            //                     ToolPosition.DY - PanelPosition.DY,
            //                     ToolPosition.DZ - PanelPosition.DZ);

            for (int i = 0; i < _nSections; i++)
            {
                var matrix = Matrix3D.Identity;
                matrix.Rotate(new Quaternion(n, i * sa));
                var radial = matrix.Transform(r);
                var sc = p + radial * (radius - sh / 2.0);
                var tsad = new MRI.ToolSectionActionData()
                {
                    PX = (float)sc.X,
                    PY = (float)sc.Y,
                    PZ = (float)sc.Z,
                    DX = (float)radial.X,
                    DY = (float)radial.Y,
                    DZ = (float)radial.Z,
                    L = (float)sl,
                    W = (float)sw,
                    H = (float)sh,
                    FixBaseAx = orientation
                };

                // if (Vector3D.DotProduct(radial, d) < 0.0) continue;

                //if (IsParallel)
                //{
                //    Panel.ApplyActionAsync(tsad);
                //}
                //else
                //{
                //    Panel.ApplyAction(tsad);
                //}

                if (_panel is MRI.IPanel panel) panel.ApplyAction(tsad);
            }
        }

        private void ApplyTool(MDT.DiskOnConeTool doct, Point3D position, Vector3D direction) => ApplyTool(doct as MDT.DiskTool, position, direction);

        private void ApplyTool(MDT.PointedTool pt, Point3D position, Vector3D direction)
        {
            var tca = new MRI.ToolConeActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)pt.ConeHeight,
                MaxRadius = (float)pt.Diameter / 2.0f,
                MinRadius = 0.0f
            };

            if (_panel is MRI.IPanel panel) panel.ApplyAction(tca);
        }

        private void ApplyTool(MDT.CountersinkTool ct, Point3D position, Vector3D direction)
        {
            var position2 = position - direction * ct.Length3;

            var ta = new MRI.ToolActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)ct.GetTotalLength(),
                Radius = (float)ct.Diameter1 / 2.0f,
            };
            var tca = new MRI.ToolConeActionData()
            {
                X = (float)position2.X,
                Y = (float)position2.Y,
                Z = (float)position2.Z,
                Orientation = ToOrientation(direction),
                Length = (float)ct.Length2,
                MaxRadius = (float)ct.Diameter2 / 2.0f,
                MinRadius = (float)ct.Diameter1 / 2.0f
            };

            if (_panel is MRI.IPanel panel)
            {
                panel.ApplyAction(ta);
                panel.ApplyAction(tca);
            }
        }

        private static MRI.Orientation ToOrientation(Vector3D direction)
        {
            var xIsNull = IsNull(direction.X);
            var yIsNull = IsNull(direction.Y);
            var zIsNull = IsNull(direction.Z);

            if (xIsNull && yIsNull && !zIsNull)
            {
                return (direction.Z > 0.0) ? MRI.Orientation.ZPos : MRI.Orientation.ZNeg;
            }
            else if (xIsNull && !yIsNull && zIsNull)
            {
                return (direction.Y > 0.0) ? MRI.Orientation.YPos : MRI.Orientation.YNeg;
            }
            else if (!xIsNull && yIsNull && zIsNull)
            {
                return (direction.X > 0.0) ? MRI.Orientation.XPos : MRI.Orientation.XNeg;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static bool IsNull(double value, double tolerance = 0.001) => (value < tolerance) && (value > -tolerance);

        private static Vector3D GetRadial(MRI.Orientation direction)
        {
            switch (direction)
            {
                case MRI.Orientation.XPos:
                case MRI.Orientation.XNeg:
                    return new Vector3D(0.0, 0.0, 1.0);
                case MRI.Orientation.YPos:
                case MRI.Orientation.YNeg:
                    return new Vector3D(1.0, 0.0, 0.0);
                case MRI.Orientation.ZPos:
                case MRI.Orientation.ZNeg:
                    return new Vector3D(1.0, 0.0, 0.0);
                default:
                    throw new NotImplementedException();
            }
        }

        private Task<int> ApplayToolAsync(IToolElement tool, Point3D position, Vector3D direction)
        {
            var t = (tool as MVMME.ToolViewModel).Tool;

            //if (t is MDT.DiskTool dt) return ApplyToolAsync(dt, position, direction);
            //else if (t is MDT.DiskOnConeTool doct) return ApplyToolAsync(doct, position, direction);
            if (t is MDT.PointedTool pt) return ApplyToolAsync(pt, position, direction);
            else if (t is MDT.CountersinkTool ct) return ApplyToolAsync(ct, position, direction);
            else return ApplyToolAsync(t, position, direction);
        }

        private Task<int> ApplyToolAsync(MDIT.ITool t, Point3D position, Vector3D direction)
        {
            return Task.Run(async () =>
            {
                var ta = new MRI.ToolActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)t.GetTotalLength(),
                    Radius = (float)t.GetTotalDiameter() / 2.0f
                };

                if (_panel is MRI.IPanel panel) await panel.ApplyActionAsync(ta);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDT.DiskTool dt, Point3D position, Vector3D direction)
        {
            return Task.Run(async () =>
            {
                var radius = dt.Diameter / 2.0;
                var sa = 360.0 / _nSections;                                // ampiezza angolare delle sezioni
                var sh = dt.CuttingRadialThickness;                         // altezza sezione;
                var sw = radius * sa * (Math.PI * 2.0) / 360.0;             // larghezza sezione
                var sl = dt.CuttingThickness;                               // linghezza sezione
                var orientation = ToOrientation(direction);
                var n = direction;
                var r = GetRadial(orientation);
                var p = new Point3D(position.X,
                                    position.Y,
                                    position.Z) - n * sl / 2.0;
                //var d = new Vector3D(ToolPosition.DX - PanelPosition.DX,
                //                     ToolPosition.DY - PanelPosition.DY,
                //                     ToolPosition.DZ - PanelPosition.DZ);
                var tasks = new List<Task>();

                for (int i = 0; i < _nSections; i++)
                {
                    var matrix = Matrix3D.Identity;
                    matrix.Rotate(new Quaternion(n, i * sa));
                    var radial = matrix.Transform(r);
                    var sc = p + radial * (radius - sh / 2.0);
                    var tsad = new MRI.ToolSectionActionData()
                    {
                        PX = (float)sc.X,
                        PY = (float)sc.Y,
                        PZ = (float)sc.Z,
                        DX = (float)radial.X,
                        DY = (float)radial.Y,
                        DZ = (float)radial.Z,
                        L = (float)sl,
                        W = (float)sw,
                        H = (float)sh,
                        FixBaseAx = orientation
                    };

                    // if (Vector3D.DotProduct(radial, d) < 0.0) continue;

                    //if (IsParallel)
                    //{
                    //    Panel.ApplyActionAsync(tsad);
                    //}
                    //else
                    //{
                    //    Panel.ApplyAction(tsad);
                    //}

                    if (_panel is MRI.IPanel panel) tasks.Add(panel.ApplyActionAsync(tsad));
                }

                await Task.WhenAll(tasks);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDT.DiskOnConeTool doct, Point3D position, Vector3D direction) => ApplyToolAsync(doct as MDT.DiskTool, position, direction);

        private Task<int> ApplyToolAsync(MDT.PointedTool pt, Point3D position, Vector3D direction)
        {
            return Task.Run(async () =>
            {
                var tca = new MRI.ToolConeActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)pt.ConeHeight,
                    MaxRadius = (float)pt.Diameter / 2.0f,
                    MinRadius = 0.0f
                };

                if (_panel is MRI.IPanel panel) await panel.ApplyActionAsync(tca);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDT.CountersinkTool ct, Point3D position, Vector3D direction)
        {
            return Task.Run(async () =>
            {
                var position2 = position - direction * ct.Length3;

                var ta = new MRI.ToolActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)ct.GetTotalLength(),
                    Radius = (float)ct.Diameter1 / 2.0f,
                };
                var tca = new MRI.ToolConeActionData()
                {
                    X = (float)position2.X,
                    Y = (float)position2.Y,
                    Z = (float)position2.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)ct.Length2,
                    MaxRadius = (float)ct.Diameter2 / 2.0f,
                    MinRadius = (float)ct.Diameter1 / 2.0f
                };

                if (_panel is MRI.IPanel panel)
                {
                    var tasks = new Task[]
                    {
                        panel.ApplyActionAsync(ta),
                        panel.ApplyActionAsync(tca)
                    };

                    await Task.WhenAll(tasks);                    
                }

                return 0;
            });
        }
    }
}
