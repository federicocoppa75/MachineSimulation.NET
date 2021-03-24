using Machine.ViewModels.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using MDT = Machine.Data.Tools;

namespace Machine.ViewModels.Helpers
{
    public static class ToolDataToViewModelConverter
    {
        public static ToolViewModel ToViewModel(this MDT.Tool tool)
        {
            if (tool is MDT.CountersinkTool ct) return Convert(ct);
            else if (tool is MDT.DiskOnConeTool dct) return Convert(dct);
            else if (tool is MDT.DiskTool dt) return Convert(dt);
            else if (tool is MDT.PointedTool pt) return Convert(pt);
            else if (tool is MDT.SimpleTool st) return Convert(st);
            else if (tool is MDT.TwoSectionTool tst) return Convert(tst);
            else throw new ArgumentException();
        }

        private static CountersinkToolViewModel Convert(MDT.CountersinkTool tool)
        {
            var vm = Convert<CountersinkToolViewModel>(tool);

            vm.Diameter1 = tool.Diameter1;
            vm.Diameter2 = tool.Diameter2;
            vm.Length1 = tool.Length1;
            vm.Length2 = tool.Length2;
            vm.Length3 = tool.Length3;
            vm.UsefulLength = tool.UsefulLength;

            return vm;
        }

        private static DiskOnConeToolViewModel Convert(MDT.DiskOnConeTool tool)
        {
            var vm = ConvertDiskTool<DiskOnConeToolViewModel>(tool);

            vm.PostponemntDiameter = tool.PostponemntDiameter;
            vm.PostponemntLength = tool.PostponemntLength;

            return vm;
        }

        private static DiskToolViewModel Convert(MDT.DiskTool tool) => ConvertDiskTool<DiskToolViewModel>(tool);

        private static PointedToolViewModel Convert(MDT.PointedTool tool)
        {
            var vm = Convert<PointedToolViewModel>(tool);

            vm.ConeHeight = tool.ConeHeight;
            vm.Diameter = tool.Diameter;
            vm.StraightLength = tool.StraightLength;
            vm.UsefulLength = tool.UsefulLength;

            return vm;
        }

        private static SimpleToolViewModel Convert(MDT.SimpleTool tool)
        {
            var vm = Convert<SimpleToolViewModel>(tool);

            vm.Diameter = tool.Diameter;
            vm.Length = tool.Length;
            vm.UsefulLength = tool.UsefulLength;

            return vm;
        }

        private static TwoSectionToolViewModel Convert(MDT.TwoSectionTool tool)
        {
            var vm = Convert<TwoSectionToolViewModel>(tool);

            vm.Diameter1 = tool.Diameter1;
            vm.Diameter2 = tool.Diameter2;
            vm.Length1 = tool.Length1;
            vm.Length2 = tool.Length2;
            vm.UsefulLength = tool.UsefulLength;

            return vm;
        }

        private static T Convert<T>(MDT.Tool tool) where T : ToolViewModel, new()
        {
            var vm = new T()
            {
                ConeModelFile = tool.ConeModelFile,
                Description = tool.Description,
                Name = tool.Name,
                ToolID = tool.ToolID,
                ToolLinkType = tool.ToolLinkType
            };

            return vm;
        }

        private static T ConvertDiskTool<T>(MDT.DiskTool tool) where T : DiskToolViewModel, new()
        {
            var vm = Convert<T>(tool);

            vm.BodyThickness = tool.BodyThickness;
            vm.CuttingRadialThickness = tool.CuttingRadialThickness;
            vm.CuttingThickness = tool.CuttingThickness;
            vm.Diameter = tool.Diameter;
            vm.RadialUsefulLength = tool.RadialUsefulLength;

            return vm;
        }
    }
}
