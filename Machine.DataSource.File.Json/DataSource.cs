using Machine.Data.Converters;
using Machine.Data.Extensions.ViewModels;
using Machine.Data.MachineElements;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MDTooling = Machine.Data.Toolings;
using MDTools = Machine.Data.Tools;

namespace Machine.DataSource.File.Json
{
    public class DataSource : DataSourceBase, IDataSource, INameProvider
    {
        private string _lastMachineFile;
        private string _lastToolingFile;
        private string _lastToolsFile;

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
                LoadMachine(dlg.FileName, (m) =>
                {
                    if (m != null) Kernel.Machines.Add(m.ToViewModel());
                });

                _lastMachineFile = dlg.FileName;
            }
        }

        protected override void SaveMachineCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("SaveFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "json";
            dlg.Filter = "Machine JSON struct |*.json";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                SaveMachine(dlg.FileName, Kernel.Machines[0].ToModel());
            }
        }

        protected override bool SaveMachineCommandCanExecute() => true;

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

                _lastToolingFile = dlg.FileName;
            }
        }

        protected override void LoadToolsCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jTools";
            dlg.Filter = "Tools (JSON) |*.jTools";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var toolset = LoadTools(dlg.FileName);

                if((toolset != null) && (toolset.Tools.Count > 0))
                {
                    Messenger.Send(new LoadToolsMessage()
                    {
                        Tools = toolset.Tools.Select(t => t as Data.Interfaces.Tools.ITool)
                    });
                }

                _lastToolsFile = dlg.FileName;
            }
        }

        protected override void SaveToolsCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("SaveFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jTools";
            dlg.Filter = "Tools (JSON) |*.jTools";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                Messenger.Send(new SaveToolsMessage()
                {
                    GetTools = (tools) =>
                    {
                        if((tools != null) && (tools.Count() > 0))
                        {
                            var name = Path.GetFileNameWithoutExtension(dlg.FileName);
                            var toolset = new MDTools.ToolSet() { Name = name };

                            foreach (var tool in tools) toolset.Tools.Add(tool as MDTools.Tool);

                            SaveTools(dlg.FileName, toolset);
                        }
                    }
                });
            }
        }

        protected override bool SaveToolsCommandCanExecute() => true;

        protected override bool SaveToolingCommandCanExecute() => true;

        protected override void SaveToolingCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("SaveFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jTooling";
            dlg.Filter = "Tooling (JSON) |*.jTooling";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var id = 0;
                var toolig = new MDTooling.Tooling()
                {
                    Machine = _lastMachineFile,
                    Tools = _lastToolsFile
                };

                Messenger.Send(new SaveToolingMessage()
                {
                    AddToolUnit = (thId, tool) =>
                    {
                        toolig.Units.Add(new MDTooling.ToolingUnit()
                        {
                            ToolingUnitID = id++,
                            ToolHolderId = thId,
                            ToolName = tool
                        });
                    }
                });

                SaveTooling(dlg.FileName, toolig);

                _lastToolingFile = dlg.FileName;
            }
        }

        protected override void LoadEnvironmentCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jEnv";
            dlg.Filter = "Environment (JSON) |*.jEnv";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                if(ZipArchiveHelper.ImportEnvironment(dlg.FileName, out string machineFile, out string toolsFile,  out string toolingFile))
                {
                    LoadMachine(machineFile, (m) =>
                    {
                        if (m != null)
                        {
                            Kernel.Machines.Add(m.ToViewModel());
                            _lastMachineFile = machineFile;

                            LoadTooling(toolingFile, (tooling) =>
                            {
                                var toolset = LoadTools(tooling.Tools);

                                SetTooling(tooling, toolset);
                                _lastToolingFile = tooling.Tools;
                            });                            
                        }
                    });
                }
            }
        }

        protected override bool LoadEnvironmentCommandCanExecute() => true;

        protected override void SaveEnvironmentCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("SaveFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "jEnv";
            dlg.Filter = "Environment (JSON) |*.jEnv";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                ZipArchiveHelper.ExportEnvironment(dlg.FileName, _lastMachineFile, _lastToolsFile, _lastToolingFile);
            }
        }

        protected override bool SaveEnvironmentCommandCanExecute() => true;

        internal static void LoadMachine(string fileName, Action<MachineElement> manageData)
        {
            JsonSerializer serializer = new JsonSerializer();

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new LinkJsonConverter());
            serializer.Converters.Add(new MachineElementJsonConverter());

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var m = serializer.Deserialize<MachineElement>(reader);

                manageData?.Invoke(m);
            }
        }

        internal static void LoadTooling(string fileName, Action<MDTooling.Tooling> manageData)
        {
            JsonSerializer serializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var m = serializer.Deserialize<MDTooling.Tooling>(reader);

                manageData?.Invoke(m);
            }
        }

        internal static MDTools.ToolSet LoadTools(string fileName)
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

        internal static void SaveMachine(string fileName, MachineElement machine) => SaveMachine(machine, () => new StreamWriter(fileName));

        internal static void SaveMachine(MachineElement machine, Func<StreamWriter> getSTreamWriter)
        {
            JsonSerializer serializer = new JsonSerializer();

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new LinkJsonConverter());
            serializer.Converters.Add(new MachineElementJsonConverter());

            using (StreamWriter sw = getSTreamWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, machine);
            }
        }

        internal static void SaveTooling(string fileName, MDTooling.Tooling tooling) => SaveTooling(tooling, () => new StreamWriter(fileName));

        internal static void SaveTooling(MDTooling.Tooling tooling, Func<StreamWriter> getStreamWriter)
        {
            JsonSerializer serializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };

            using (StreamWriter sw = getStreamWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, tooling);
            }
        }

        internal static void SaveTools(string fileName, MDTools.ToolSet toolSet) => SaveTools(toolSet, () => new StreamWriter(fileName));

        internal static void SaveTools(MDTools.ToolSet toolSet, Func<StreamWriter> getStreamWriter)
        {
            JsonSerializer serializer = new JsonSerializer();

            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = getStreamWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, toolSet);
            }
        }

        private void LoadTooling(string fileName)
        {
            LoadTooling(fileName, (m) =>
            {
                if ((m != null)/* && CheckToolingMachine(m.Machine)*/)
                {
                    var fileInfo = new FileInfo(m.Tools);
                    var machFileInfo = new FileInfo(m.Machine);
                    string path = Path.GetDirectoryName(fileName);
                    string toolFile = fileInfo.Exists ? m.Tools : Path.Combine(path, $"{m.Tools}.jTools");
                    string machFile = machFileInfo.Exists ? m.Machine : Path.Combine(path, $"{m.Machine}.json");

                    var toolset = LoadTools(toolFile);

                    PreSetTooling(machFile, toolFile);
                    SetTooling(m, toolset);

                    _lastToolsFile = toolFile;
                    _lastMachineFile = machFile;
                }
            });
        }

        private void PreSetTooling(string machFile, string toolFile)
        {
            if(GetInstance<IApplicationInformationProvider>().ApplicationType == ApplicationType.ToolingEditor)
            {
                LoadMachine(machFile, (m) =>
                {
                    if (m != null) Kernel.Machines.Add(m.ToViewModel());
                });

                var toolset = LoadTools(toolFile);

                if ((toolset != null) && (toolset.Tools.Count > 0))
                {
                    Messenger.Send(new LoadToolsMessage()
                    {
                        Tools = toolset.Tools.Select(t => t as Data.Interfaces.Tools.ITool)
                    });
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
                    if(t is MDTools.AngularTransmission at)
                    {
                        Messenger.Send(new AngularTransmissionLoadMessage() 
                        {
                            ToolHolder = item.ToolHolderId, 
                            AngularTransmission = at,
                            AppendSubSpindle = (addSubSpindle) =>
                            {
                                foreach (var item in at.Subspindles)
                                {
                                    var tool = ((item is MDTools.SubspindleEx sse) && (sse.Tool != null)) ? sse.Tool : null;
                                    addSubSpindle(item.Position, item.Direction, tool);
                                }
                            }
                        });
                    }
                    else
                    {
                        Messenger.Send(new LoadToolMessage() { ToolHolder = item.ToolHolderId, Tool = t });
                    }
                }
            }
        }

        static void ProcessForAngolarTransmissions(MDTools.ToolSet toolSet)
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
                        MDTools.Tool tool = null;

                        if(spindle.ToolName != null) dictionary.TryGetValue(spindle.ToolName, out  tool);

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
