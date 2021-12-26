using Machine.ViewModels.Interfaces;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Text;
using MMM = MachineModels.Models;
using MachineModels.Extensions;
using Machine.Data.Extensions.ViewModels;
using MD = Machine.Data.MachineElements;
using MDE = Machine.Data.Enums;
using System.IO;
using MDTooling = Machine.Data.Toolings;
using MDTools = Machine.Data.Tools;
using System.Linq;
using Machine.ViewModels.Messages.Tooling;

namespace Machine.DataSource.File.Xml
{
    public class DataSource : DataSourceBase, IDataSource, INameProvider
    {
        private string _lastMachineFile;
        private string _lastToolingFile;
        private string _lastToolsFile;

        private IKernelViewModel _kernel;
        public IKernelViewModel Kernel => _kernel ?? (_kernel = GetInstance<IKernelViewModel>());

        public override string Name => "File.XML";

        protected override void LoadMachineCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "Machine XML struct |*.xml";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadMachine(dlg.FileName, (m) =>
                {
                    if (m != null)
                    {
                        Kernel.Machines.Add(m.ToViewModel());
                    }
                });

                _lastMachineFile = dlg.FileName;
            }
        }

        protected override void LoadToolingCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "tooling";
            dlg.Filter = "Tooling (XML) |*.tooling";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadTooling(dlg.FileName);

                _lastToolingFile = dlg.FileName;
            }
        }

        protected override void LoadEnvironmentCommandImplementation()
        {
            var dlg = ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "env";
            dlg.Filter = "Environment (XML) |*.env";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                if (MachineModels.IO.ZipArchiveHelper.ImportEnvironment(dlg.FileName, out string machineFile, out string toolsFile, out string toolingFile))
                {
                    LoadMachine(machineFile, (m) =>
                    {
                        if (m != null)
                        {
                            Kernel.Machines.Add(m.ToViewModel());
                            _lastMachineFile = machineFile;

                            LoadTooling(toolingFile, (tooling) =>
                            {
                                //var toolset = LoadTools(tooling.Tools);
                                var toolset = LoadTools(toolsFile);

                                SetTooling(tooling, toolset);
                                _lastToolingFile = tooling.Tools;
                            });
                        }
                    });
                }
            }
        }

        protected override bool LoadEnvironmentCommandCanExecute() => true;

        protected override void SaveEnvironmentCommandImplementation() => throw new NotImplementedException();

        protected override bool SaveEnvironmentCommandCanExecute() => false;

        protected override void SaveMachineCommandImplementation() => throw new NotImplementedException();

        protected override bool SaveMachineCommandCanExecute() => false;

        internal static void LoadMachine(string fileName, Action<MD.MachineElement> manageData)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MMM.MachineElement));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MMM.MachineElement)serializer.Deserialize(reader);

                if (m != null)
                {
                    var md = m.ToMachineData(true);

                    if (md is MD.RootElement re)
                    {
                        re.AssemblyName = Path.GetFileNameWithoutExtension(fileName);
                        re.RootType = MDE.RootType.CX220;
                    }

                    manageData?.Invoke(md);
                }
            }
        }

        private void LoadTooling(string fileName)
        {
            LoadTooling(fileName, (m) =>
            {
                if ((m != null)/* && CheckToolingMachine(m.Machine)*/)
                {
                    var fileInfo = new FileInfo(m.Tools);
                    string path = Path.GetDirectoryName(fileName);
                    string toolFile = fileInfo.Exists ? m.Tools : Path.Combine(path, $"{m.Tools}.tools");

                    var toolset = LoadTools(toolFile);

                    SetTooling(m, toolset);

                    _lastToolsFile = toolFile;
                }
            });
        }

        internal static void LoadTooling(string fileName, Action<MDTooling.Tooling> manageData)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MMM.Tooling.Tooling));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MMM.Tooling.Tooling)serializer.Deserialize(reader);

                if (m != null)
                {
                    var t = m.ToToolsData();

                    t.Name = Path.GetFileNameWithoutExtension(fileName);

                    manageData?.Invoke(t);
                }
            }
        }

        internal static MDTools.ToolSet LoadTools(string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MMM.Tools.ToolSet));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MMM.Tools.ToolSet)serializer.Deserialize(reader);

                if (m != null)
                {
                    var ts = m.ToToolsData();

                    ts.Name = Path.GetFileNameWithoutExtension(fileName);

                    return ts;
                }
                else
                {
                    return null;
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
                    if (t is MDTools.AngularTransmission at)
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
    }
}
