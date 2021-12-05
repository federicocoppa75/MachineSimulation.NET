using System;
using System.Collections.Generic;
using System.Text;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Links;
using Machine.ViewModels.MachineElements.Collider;
using Machine.ViewModels.MachineElements.Toolholder;
using MD = Machine.Data;
using MDE = Machine.Data.MachineElements;
using MDL = Machine.Data.Links;
using MVMIL = Machine.ViewModels.Interfaces.Links;
using MVMII = Machine.ViewModels.Interfaces.Indicators;

namespace Machine.Data.Extensions.ViewModels
{
    public static class ConvertBack
    {
        public static MDE.MachineElement ToModel(this IMachineElement me)
        {
            if (me is IRootElement re) return Convert(re);
            else if (me is IInserterElement inse) return Convert(inse);
            else if (me is IInjectorElement inje) return Convert(inje);
            else if (me is IColliderElement ce) return Convert(ce);
            else if (me is IToolholderElement the) return Convert(the);
            else if (me is IPanelholderElement phe) return Convert(phe);
            else return Convert<MDE.MachineElement>(me);
            return null;
        }

        private static MDE.MachineElement Convert(IRootElement re)
        {
            var m = Convert<MDE.RootElement>(re);

            m.AssemblyName = re.AssemblyName;
            m.RootType = re.RootType;

            return m;
        }

        private static MDE.MachineElement Convert(IInserterElement e)
        {
            var m = ConverterInjector<MDE.InserterElement>(e);

            m.Diameter = e.Diameter;
            m.Length = e.Length;
            m.LoaderLinkId = e.LoaderLinkId;
            m.DischargerLinkId = e.DischargerLinkId;

            return m;
        }

        private static MDE.MachineElement Convert(IInjectorElement e) => ConverterInjector<MDE.InjectorElement>(e);

        private static MDE.MachineElement Convert(IColliderElement e)
        {
            var m = Convert<MDE.ColliderElement>(e);

            m.Type = e.Type;
            m.Radius = e.Radius;

            foreach (var item in e.Points)
            {
                m.Points.Add(item.Convert());
            }

            return m;
        }

        private static MDE.MachineElement Convert(IToolholderElement e)
        {
            var m = Convert<MDE.ToolholderElement>(e);

            m.ToolHolderId = e.ToolHolderId;
            m.ToolHolderType = e.ToolHolderType;
            m.Position = e.Position.Convert();
            m.Direction = e.Direction.Convert();

            return m;
        }

        private static MDE.MachineElement Convert(IPanelholderElement e)
        {
            var m = Convert<MDE.PanelHolderElement>(e);

            m.PanelHolderId = e.PanelHolderId;
            m.PanelHolderName = e.PanelHolderName;
            m.Position = e.Position.Convert();
            m.Corner = e.Corner;

            return m;
        }

        private static T Convert<T>(IMachineElement machineElement) where T : MDE.MachineElement, new()
        {
            var m = new T()
            {
                MachineElementID = machineElement.MachineElementID,
                Name = machineElement.Name,
                ModelFile = machineElement.ModelFile,
                Color = machineElement.Color.Convert(),
                Transformation = machineElement.Transformation.Convert(),
                LinkToParent = machineElement.LinkToParent.Convert(),
            };

            foreach (var item in machineElement.Children)
            {
                if(!item.IsIndicator()) m.Children.Add(item.ToModel());
            }

            return m;
        }

        private static T ConverterInjector<T>(IInjectorElement me) where T : MDE.InjectorElement, new()
        {
            var vm = Convert<T>(me);

            vm.InserterId = me.InserterId;
            vm.Position = me.Position.Convert();
            vm.Direction = me.Direction.Convert();
            vm.InserterColor = me.InserterColor.Convert();

            return vm;
        }

        private static MDE.Point Convert(this Base.Point position)
        {
            return (position is MDE.Point p) ? p : new MDE.Point() { X = position.X, Y = position.Y, Z = position.Z };
        }

        private static MDE.Vector Convert(this Base.Vector direction)
        {
            return (direction is MDE.Vector v) ? v : new MDE.Vector() { X = direction.X, Y = direction.Y, Z = direction.Z };
        }

        private static MDE.Color Convert(this Base.Color color)
        {
            return (color is MDE.Color c) ? c : new MDE.Color() { A = color.A, R = color.R, G = color.G, B = color.B };
        }

        private static MDE.Matrix Convert(this Base.Matrix matrix)
        {
            if(matrix is MDE.Matrix m)
            {
                return m;
            }
            else if (matrix == null)
            {
                return new MDE.Matrix() { M11 = 1.0, M22 = 1.0, M33 = 1.0 };
            }
            else
            {
                return new MDE.Matrix()
                {
                    M11 = matrix.M11,
                    M12 = matrix.M12,
                    M13 = matrix.M13,
                    M21 = matrix.M21,
                    M22 = matrix.M22,
                    M23 = matrix.M23,
                    M31 = matrix.M31,
                    M32 = matrix.M32,
                    M33 = matrix.M33,
                    OffsetX = matrix.OffsetX,
                    OffsetY = matrix.OffsetY,
                    OffsetZ = matrix.OffsetZ
                };
            }
        }

        private static MDL.Link Convert(this MVMIL.ILinkViewModel link)
        {
            MDL.Link m = null;

            if(link is MVMIL.ILinearLinkViewModel llvm)
            {
                m = new MDL.LinearLink()
                {
                    Max = llvm.Max,
                    Min = llvm.Min,
                    Pos = llvm.Pos
                };
            }
            else if(link is MVMIL.IPneumaticLinkViewModel plvm)
            {
                var plm = new MDL.PneumaticLink()
                {
                    OffPos = plvm.OffPos,
                    OnPos = plvm.OnPos,
                    TOff = plvm.TOff,
                    TOn = plvm.TOn,
                    ToolActivator = plvm.ToolActivator
                };

                m = plm;
            }

            if(m != null)
            {
                m.Id = link.Id;
                m.Direction = link.Direction;
                m.Type = link.Type;
            }

            return m;
        }

        private static bool IsIndicator(this IMachineElement element) => element is MVMII.IIndicatorProxy;
    }
}
