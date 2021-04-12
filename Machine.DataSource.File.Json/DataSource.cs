using Machine.Data.Converters;
using Machine.Data.MachineElements;
using Machine.ViewModels;
using Machine.ViewModels.Helpers;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.Messages;
using Machine.ViewModels.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MDTooling = Machine.Data.Toolings;
using MDTools = Machine.Data.Tools;

namespace Machine.DataSource.File.Json
{
    public class DataSource : DataSourceBase, IDataSource, INameProvider
    {
        private IKernelViewModel _kernel;
        public IKernelViewModel Kernel => _kernel ?? (_kernel = GetInstance<IKernelViewModel>());

        public override string Name => "File.JSON";

        protected override void LoadMachineCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "json";
            dlg.Filter = "Machine JSON struct |*.json";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add(new LinkJsonConverter());
                serializer.Converters.Add(new MachineElementJsonConverter());

                using (StreamReader sr = new StreamReader(dlg.FileName))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var m = serializer.Deserialize<MachineElement>(reader);

                    if (m != null) Kernel.Machines.Add(m.ToViewModel());
                }
            }
        }

        protected override void LoadToolingCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jTooling";
            dlg.Filter = "Tooling (JSON) |*.jTooling";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadTooling(dlg.FileName);
            }
        }

        private void LoadTooling(string fileName)
        {
            JsonSerializer serializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var m = serializer.Deserialize<MDTooling.Tooling>(reader);

                if ((m != null) && CheckToolingMachine(m.Machine))
                {
                    string path = Path.GetDirectoryName(fileName);
                    string toolFile = Path.Combine(path, $"{m.Tools}.jTools");

                    var toolset = LoadTools(toolFile);

                    SetTooling(m, toolset);
                }
            }
        }

        private void SetTooling(MDTooling.Tooling tooling, MDTools.ToolSet toolset)
        {
            foreach (var item in tooling.Units)
            {
                var t = toolset.Tools.FirstOrDefault(e => string.Compare(e.Name, item.ToolName) == 0);

                if (t != null)
                {
                    Messenger.Send(new LoadToolMessage() { ToolHolder = item.ToolHolderId, Tool = t });
                }
            }
        }

        private MDTools.ToolSet LoadTools(string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new ToolJsonConverter());

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var m = serializer.Deserialize<MDTools.ToolSet>(reader);

                ProcessForAngolarTransmissions(m);

                return m;
                //if (m != null) Toolsets.Add(m);
            }
        }

        private void ProcessForAngolarTransmissions(MDTools.ToolSet toolSet)
        {
            Dictionary<string, MDTools.Tool> dictionary = null;

            foreach (var item in toolSet.Tools)
            {
                if(item is MDTools.AngularTransmission at)
                {
                    if (dictionary == null) dictionary = toolSet.Tools.ToDictionary(t => t.Name, t => t);

                    var spindlesEx = new List<MDTools.SubspindleEx>();

                    foreach (var spindle in at.Subspindles)
                    {
                        dictionary.TryGetValue(spindle.ToolName, out MDTools.Tool tool);

                        spindlesEx.Add(new MDTools.SubspindleEx()
                        {
                            Tool = tool,
                            Position = spindle.Position,
                            Direction = spindle.Direction
                        });
                    }

                    at.Subspindles.Clear();
                    spindlesEx.ForEach(e => at.Subspindles.Add(e));
                }
            }
        }

        private bool CheckToolingMachine(string machine)
        {
            var result = false;

            foreach (var item in Kernel.Machines)
            {
                if ((item is RootElementViewModel r) && string.Compare(r.AssemblyName, machine) == 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
