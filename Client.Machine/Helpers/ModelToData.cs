using System;
using System.Collections.Generic;
using System.Text;
using MM = MachineModels;
using MD = Machine.Data;

namespace Client.Machine.Helpers
{
    public static class ModelToData
    {
        public static MD.MachineElements.MachineElement ToMachineData(this MM.Models.MachineElement model, bool isRoot = false)
        {
            var data = Create(model, isRoot);

            data.Name = model.Name;
            data.ModelFile = model.ModelFile;
            data.LinkToParent = ConvertLink(model);
            data.Color = model.Color.ToMachineData();
            data.Transformation = model.TrasformationMatrix3D.ToMachineData();

            var list = new List<MD.MachineElements.MachineElement>();
            foreach (var item in model.Children)
            {
                data.Children.Add(item.ToMachineData());
                //list.Add(item.ToMachineData());
            }

            //data.Children = list.ToArray();

            return data;
        }

        private static MD.MachineElements.Matrix ToMachineData(this MM.Models.Matrix3D m)
        {
            return new MD.MachineElements.Matrix()
            {
                M11 = m.M11,
                M12 = m.M12,
                M13 = m.M13,
                //M14 = m.M14,
                M21 = m.M21,
                M23 = m.M23,
                //M24 = m.M24,
                M31 = m.M31,
                M32 = m.M32,
                M33 = m.M33,
                //M34 = m.M34,
                OffsetX = m.OffsetX,
                OffsetY = m.OffsetY,
                M22 = m.M22,
                OffsetZ = m.OffsetZ,
                //M44 = m.M44
            };
        }

        private static MD.MachineElements.Vector ToMachineData(this MM.Models.Vector v)
        {
            return new MD.MachineElements.Vector()
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z
            };
        }

        private static MD.MachineElements.Point ToMachineDataPoint(this MM.Models.Vector v)
        {
            return new MD.MachineElements.Point()
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z
            };
        }

        private static MD.MachineElements.Color ToMachineData(this MM.Models.Color c)
        {
            return new MD.MachineElements.Color()
            {
                B = c.B,
                G = c.G,
                R = c.R,
                A = c.A
            };
        }

        private static MD.Links.Link ConvertLink(MM.Models.MachineElement model)
        {
            switch (model.LinkToParentType)
            {
                case MM.Enums.LinkType.LinearPosition:
                    return (model.LinkToParentData as MM.Models.Links.LinearPosition).ToMachineData();
                case MM.Enums.LinkType.LinearPneumatic:
                    return (model.LinkToParentData as MM.Models.Links.LinearPneumatic).ToMachineData();
                case MM.Enums.LinkType.RotaryPneumatic:
                    return (model.LinkToParentData as MM.Models.Links.RotaryPneumatic).ToMachineData();
                default:
                    return null;
            }
        }

        private static MD.Links.Link ToMachineData(this MM.Models.Links.LinearPosition link)
        {
            return new MD.Links.LinearLink()
            {
                Direction = ConvertLinkDirection(link.Direction),
                Id = link.Id,
                Max = link.Max,
                Min = link.Min,
                Pos = link.Pos,
                Type = MD.Enums.LinkType.Linear
            };
        }

        private static MD.Enums.LinkDirection ConvertLinkDirection(MM.Enums.LinkDirection d)
        {
            switch (d)
            {
                case MM.Enums.LinkDirection.X:
                    return MD.Enums.LinkDirection.X;
                    
                case MM.Enums.LinkDirection.Y:
                    return MD.Enums.LinkDirection.Y;

                case MM.Enums.LinkDirection.Z:
                    return MD.Enums.LinkDirection.Z; 
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static MD.Links.Link ToMachineData(this MM.Models.Links.LinearPneumatic link)
        {
            return new MD.Links.PneumaticLink()
            {
                Direction = ConvertLinkDirection(link.Direction),
                Id = link.Id,
                OffPos = link.OffPos,
                OnPos = link.OnPos,
                TOff = link.TOff,
                TOn = link.TOn,
                ToolActivator = link.ToolActivator,
                Type = MD.Enums.LinkType.Linear
            };
        }

        private static MD.Links.Link ToMachineData(this MM.Models.Links.RotaryPneumatic link)
        {
            return new MD.Links.PneumaticLink()
            {
                Direction = ConvertLinkDirection(link.Direction),
                Id = link.Id,
                OffPos = link.OffPos,
                OnPos = link.OnPos,
                TOff = link.TOff,
                TOn = link.TOn,
                ToolActivator = link.ToolActivator,
                Type = MD.Enums.LinkType.Rotary
            };
        }

        private static MD.MachineElements.MachineElement Create(MM.Models.MachineElement model, bool isRoot = false)
        {
            if(model.ColiderType != MM.Enums.ColliderGeometry.None)
            {
                var pc = model.Collider as MM.Models.Colliders.PointsCollider;

                var c = new MD.MachineElements.ColliderElement()
                {
                    Radius = pc.Radius
                };

                foreach (var item in pc.Points)
                {
                    c.Points.Add(item.ToMachineDataPoint());
                }

                return c;
            }
            else if(model.HasPanelHolder)
            {
                var ph = model.PanelHolder;

                return new MD.MachineElements.PanelHolderElement()
                {
                    PanelHolderId = ph.Id,
                    PanelHolderName = ph.Name,
                    Position = ph.Position.ToMachineData(),
                    Corner = ConvertPanelLoadType(ph.Corner)
                };
            }
            else if(model.InserterType != MM.Enums.InserterType.None)
            {
                var insn = CreateInserter(model);

                insn.InserterId = model.Inserter.Id;
                insn.Position = model.Inserter.Position.ToMachineData();
                insn.Direction = model.Inserter.Direction.ToMachineData();
                insn.InserterColor = model.Inserter.Color.ToMachineData();

                return insn;
            }
            else if(model.ToolHolderType != MM.Enums.ToolHolderType.None)
            {
                var th = model.ToolHolderData as MM.Models.ToolHolders.ToolHolder;

                return new MD.MachineElements.ToolholderElement()
                {
                    ToolHolderId = th.Id,
                    Position = th.Position.ToMachineData(),
                    Direction = th.Direction.ToMachineData(),
                    ToolHolderType = ConvertToolHolderType(model.ToolHolderType)
                };
            }
            else
            {
                return isRoot ? new MD.MachineElements.RootElement() : new MD.MachineElements.MachineElement();
            }
        }

        private static MD.Enums.ToolHolderType ConvertToolHolderType(MM.Enums.ToolHolderType toolHolderType)
        {
            switch (toolHolderType)
            {
                case MM.Enums.ToolHolderType.Static:
                    return MD.Enums.ToolHolderType.Static;
                case MM.Enums.ToolHolderType.AutoSource:
                    return MD.Enums.ToolHolderType.AutoSource;
                case MM.Enums.ToolHolderType.AutoSink:
                    return MD.Enums.ToolHolderType.AutoSink;
                default:
                    throw new ArgumentException();
            }
        }

        private static MD.MachineElements.InjectorElement CreateInserter(MM.Models.MachineElement model)
        {
            switch (model.InserterType)
            {
                case MM.Enums.InserterType.Injector:
                    return new MD.MachineElements.InjectorElement();

                case MM.Enums.InserterType.Inserter:
                    var ins = new MD.MachineElements.InserterElement();
                    var m = model.Inserter as MM.Models.Inserters.Inserter;

                    ins.Diameter = m.Diameter;
                    ins.Length = m.Length;
                    ins.LoaderLinkId = m.LoaderLinkId;
                    ins.DischargerLinkId = m.DischargerLinkId;

                    return ins;

                default:
                    throw new ArgumentException();
            }
        }

        private static MD.Enums.PanelLoadType ConvertPanelLoadType(MM.Enums.PanelLoadType pt)
        {
            switch (pt)
            {
                case MM.Enums.PanelLoadType.Corner1:
                    return MD.Enums.PanelLoadType.Corner1;
                case MM.Enums.PanelLoadType.Corner2:
                    return MD.Enums.PanelLoadType.Corner2;
                case MM.Enums.PanelLoadType.Corner3:
                    return MD.Enums.PanelLoadType.Corner3;
                case MM.Enums.PanelLoadType.Corner4:
                    return MD.Enums.PanelLoadType.Corner4;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
