using System;
using System.Collections.Generic;
using System.Text;
using Machine.ViewModels.Links;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Toolholder;
using MD = Machine.Data;
using MDE = Machine.Data.MachineElements;
using MDL = Machine.Data.Links;

namespace Machine.Data.Extensions.ViewModels
{
    public static class Converter
    {
        public static ElementViewModel ToViewModel(this MDE.MachineElement me, ElementViewModel parent = null)
        {
            if (me is MDE.RootElement re) return Convert(re, parent);
            else if (me is MDE.InserterElement ins) return Convert(ins, parent);
            else if (me is MDE.InjectorElement inj) return Convert(inj, parent);
            else if (me is MDE.ColliderElement ce) return Convert(ce, parent);
            else if (me is MDE.ToolholderElement th) return Convert(th, parent);
            else if (me is MDE.PanelHolderElement ph) return Convert(ph, parent);
            else return Convert<ElementViewModel>(me, parent);
        }

        private static RootElementViewModel Convert(MDE.RootElement re, ElementViewModel parent)
        {
            var revm = Convert<RootElementViewModel>(re, parent);

            revm.AssemblyName = re.AssemblyName;
            revm.RootType = re.RootType;

            return revm;
        }

        private static ColliderElementViewModel Convert(MDE.ColliderElement ce, ElementViewModel parent)
        {
            var vm = Convert<ColliderElementViewModel>(ce, parent);

            vm.Radius = ce.Radius;
            vm.Type = ce.Type;
            foreach (var p in ce.Points) vm.Points.Add(p);

            return vm;
        }

        private static ToolholderElementViewModel Convert(MDE.ToolholderElement th, ElementViewModel parent)
        {
            ToolholderElementViewModel vm = null;

            switch (th.ToolHolderType)
            {
                case MD.Enums.ToolHolderType.Static:
                    vm = Convert<StaticToolholderElementViewModel>(th, parent);
                    break;
                case MD.Enums.ToolHolderType.AutoSource:
                    vm = Convert<AutoSourceToolholderElementViewModel>(th, parent);
                    break;
                case MD.Enums.ToolHolderType.AutoSink:
                    vm = Convert<AutoSyncToolholderElementViewModel>(th, parent);
                    break;
                default:
                    throw new NotImplementedException();
            }

            vm.ToolHolderId = th.ToolHolderId;
            vm.Position = th.Position;
            vm.Direction = th.Direction;

            return vm;
        }
        private static PanelHolderElementViewModel Convert(MDE.PanelHolderElement ph, ElementViewModel parent)
        {
            var vm = Convert<PanelHolderElementViewModel>(ph, parent);

            vm.PanelHolderId = ph.PanelHolderId;
            vm.PanelHolderName = ph.PanelHolderName;
            vm.Position = ph.Position;
            vm.Corner = ph.Corner;

            return vm;
        }
        private static InserterElementViewMode Convert(MDE.InserterElement ins, ElementViewModel parent)
        {
            var vm = ConverterInserter<InserterElementViewMode>(ins, parent);

            return vm;
        }
        private static InjectorElementViewModel Convert(MDE.InjectorElement inj, ElementViewModel parent) => ConverterInserter<InjectorElementViewModel>(inj, parent);


        private static T ConverterInserter<T>(MDE.InjectorElement me, ElementViewModel parent) where T : InjectorElementViewModel, new()
        {
            var vm = Convert<T>(me, parent);

            return vm;
        }

        private static T Convert<T>(MDE.MachineElement machineElement, ElementViewModel parent) where T : ElementViewModel, new()
        {
            var evm = new T()
            {
                MachineElementID = machineElement.MachineElementID,
                Name = machineElement.Name,
                ModelFile = machineElement.ModelFile,
                Color = machineElement.Color,
                Transformation = machineElement.Transformation,
                LinkToParent = ConvertLink(machineElement.LinkToParent, machineElement.Name),
                Parent = parent
            };

            foreach (var item in machineElement.Children) evm.Children.Add(item.ToViewModel(evm));

            //foreach (var item in evm.Children) item.Parent = evm;

            return evm;
        }

        private static LinkViewModel ConvertLink(MDL.Link linkToParent, string name)
        {
            LinkViewModel vm = null;

            if (linkToParent is MDL.LinearLink linearLink)
            {
                vm = new LinearLinkViewModel()
                {
                    Id = linkToParent.Id,
                    //LinkID = linearLink.LinkID,
                    Direction = linearLink.Direction,
                    Type = linearLink.Type,
                    Max = linearLink.Max,
                    Min = linearLink.Min,
                    Pos = linearLink.Pos,
                    Value = linearLink.Pos,
                    Description = name
                };
            }
            else if (linkToParent is MDL.PneumaticLink pneumaticLink)
            {
                vm = new PneumaticLinkViewModel()
                {
                    Id = pneumaticLink.Id,
                    //LinkID = pneumaticLink.LinkID,
                    Direction = pneumaticLink.Direction,
                    Type = pneumaticLink.Type,
                    OffPos = pneumaticLink.OffPos,
                    OnPos = pneumaticLink.OnPos,
                    TOff = pneumaticLink.TOff,
                    TOn = pneumaticLink.TOn,
                    ToolActivator = pneumaticLink.ToolActivator,
                    Description = name
                };
            }

            return vm;
        }

    }

}
