using Machine.Data.MachineElements;
using Machine.Data.Toolings;
using Machine.Data.Tools;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Machine.DataSource.File.Json
{
    class ZipArchiveHelper
    {
        private const string _machineFileName = "machine.json";
        private const string _toolSetFileName = "tools.jTools";
        private const string _oldToolSetFileName = "toos.jTools";
        private const string _toolingFileName = "tooling.jTooling";

        private List<string> _entrieeNames = new List<string>();
        private static List<string> _externalFiles = new List<string>();

        private static string _extractPath;
        public static string ExtractPath => _extractPath;

        public static void AddFilesToEnvironment(IEnumerable<string> files)
        {
            _externalFiles.Clear();

            _externalFiles.AddRange(files);
        }

        public static bool ExportEnvironment(string exportFile, string machProjectFile, string toolsFile, string toolingFile) => (new ZipArchiveHelper()).ExportEnvironmentImplementation(exportFile, machProjectFile, toolsFile, toolingFile);

        public static bool ImportEnvironment(string importFile, out string machProjectFile, out string toolsFile, out string toolingFile) => (new ZipArchiveHelper()).ImportEnvironmentImplementation(importFile, out machProjectFile, out toolsFile, out toolingFile);

        private bool ExportEnvironmentImplementation(string exportFile, string machProjectFile, string toolsFile, string toolingFile)
        {
            bool result = false;
            var machine = GetMachine(machProjectFile);
            var toolSet = GetToolSet(toolsFile);
            var tooling = GetTooling(toolingFile);
            var info = new FileInfo(exportFile);

            if (info.Exists) info.Delete();

            if ((machine != null) && (toolSet != null) && (tooling != null))
            {
                using (var archive = ZipFile.Open(exportFile, ZipArchiveMode.Create))
                {
                    if (!AddModelsFilesToArchive(machine, archive))
                    {
                        // log errore
                    }
                    else if (!AddProjectFileToArchive(machine, archive))
                    {
                        // log error
                    }
                    else if (!AddConeModelsFilesToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if (!AddBodyModelsFilesToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if (!AddToolsetFileToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if (!AddToolingFileToArchive(tooling, archive))
                    {
                        // log error
                    }
                    else if(!AddExternalFileToArchive(archive))
                    {
                        // log error
                    }
                }
            }

            return result;
        }

        private bool ImportEnvironmentImplementation(string importFile, out string machProjectFile, out string toolsFile, out string toolingFile)
        {
            bool result = false;

            machProjectFile = string.Empty;
            toolsFile = string.Empty;
            toolingFile = string.Empty;

            if (!string.IsNullOrEmpty(importFile))
            {
                var info = new FileInfo(importFile);
                var extractPath = $"{info.DirectoryName}\\{Path.GetFileNameWithoutExtension(info.Name)}";
                var dirInfo = new DirectoryInfo(extractPath);

                if (dirInfo.Exists) dirInfo.Delete(true);

                ZipFile.ExtractToDirectory(importFile, extractPath);

                var toolFileName = dirInfo.GetFiles(_oldToolSetFileName).Length > 0 ? _oldToolSetFileName : _toolSetFileName;

                var machPrjFile = $"{extractPath}\\{_machineFileName}";
                toolsFile = $"{extractPath}\\{toolFileName}";
                toolingFile = $"{extractPath}\\{_toolingFileName}";

                var machine = GetMachine(machPrjFile);
                var toolSet = GetToolSet(toolsFile);
                var tooling = GetTooling(toolingFile);

                UpdateModelsFiles(machine, extractPath, true);
                UpdateModelsFiles(toolSet, extractPath, true);
                UpdateModelsFiles(tooling, (machine as RootElement).AssemblyName, extractPath, true);


                if (string.Compare(machPrjFile, tooling.Machine, true) != 0)
                {
                    // sistemo il nome del file della macchina
                    var fileInfo = new FileInfo(machPrjFile);

                    fileInfo.CopyTo(tooling.Machine, true);
                }

                machProjectFile = tooling.Machine;
                _extractPath = extractPath;

                result = true;
            }

            return result;
        }

        private MachineElement GetMachine(string machProjectFile)
        {
            MachineElement machine = null;

            DataSource.LoadMachine(machProjectFile, m => machine = m);

            return machine;
        }

        private ToolSet GetToolSet(string toolsFile) => DataSource.LoadTools(toolsFile);

        private Tooling GetTooling(string toolingFile)
        {
            Tooling tooling = null;

            DataSource.LoadTooling(toolingFile, (t) => tooling = t);

            return tooling;
        }

        private bool AddModelsFilesToArchive(MachineElement me, ZipArchive archive)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(me.ModelFile))
            {
                FileInfo info = new FileInfo(me.ModelFile);

                if (info.Exists)
                {
                    var name = info.Name;

                    if (!_entrieeNames.Contains(name))
                    {
                        var entry = archive.CreateEntryFromFile(me.ModelFile, name);
                        _entrieeNames.Add(name);
                    }
                }
            }

            foreach (var item in me.Children)
            {
                AddModelsFilesToArchive(item, archive);
            }

            return result;
        }

        private bool AddProjectFileToArchive(MachineElement me, ZipArchive archive)
        {
            bool result = true;

            FilterModelsNames(me);

            var entry = archive.CreateEntry(_machineFileName);

            DataSource.SaveMachine(me, () => new StreamWriter(entry.Open()));

            return result;
        }

        private void FilterModelsNames(MachineElement me)
        {
            if (!string.IsNullOrEmpty(me.ModelFile))
            {
                var info = new FileInfo(me.ModelFile);
                var name = info.Name;

                me.ModelFile = name;
            }

            foreach (var item in me.Children)
            {
                FilterModelsNames(item);
            }
        }

        private bool AddConeModelsFilesToArchive(ToolSet toolSet, ZipArchive archive)
        {
            bool result = true;

            foreach (var item in toolSet.Tools)
            {
                if (!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    FileInfo info = new FileInfo(item.ConeModelFile);

                    if (info.Exists)
                    {
                        var name = info.Name;

                        if (!_entrieeNames.Contains(name))
                        {
                            var entry = archive.CreateEntryFromFile(item.ConeModelFile, name);
                            _entrieeNames.Add(name);
                        }
                    }
                }
            }

            return result;
        }

        private bool AddBodyModelsFilesToArchive(ToolSet toolSet, ZipArchive archive)
        {
            bool result = true;

            foreach (var item in toolSet.Tools)
            {
                if ((item is AngularTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    FileInfo info = new FileInfo(at.BodyModelFile);

                    if (info.Exists)
                    {
                        var name = info.Name;

                        if (!_entrieeNames.Contains(name))
                        {
                            var entry = archive.CreateEntryFromFile(at.BodyModelFile, name);
                            _entrieeNames.Add(name);
                        }
                    }
                }
            }

            return result;
        }

        private bool AddToolsetFileToArchive(ToolSet toolSet, ZipArchive archive)
        {
            FilterModelsNames(toolSet);

            var entry = archive.CreateEntry(_toolSetFileName);

            DataSource.SaveTools(toolSet, () => new StreamWriter(entry.Open()));

            return true;
        }

        private bool AddToolingFileToArchive(Tooling tooling, ZipArchive archive)
        {
            var entry = archive.CreateEntry(_toolingFileName);

            DataSource.SaveTooling(tooling, () => new StreamWriter(entry.Open()));

            return true;
        }

        private bool AddExternalFileToArchive(ZipArchive archive)
        {
            if(_externalFiles.Count > 0)
            {
                foreach (var item in _externalFiles)
                {
                    FileInfo info = new FileInfo(item);

                    if (info.Exists)
                    {
                        var name = info.Name;

                        if (!_entrieeNames.Contains(name))
                        {
                            var entry = archive.CreateEntryFromFile(item, name);
                            _entrieeNames.Add(name);
                        }
                    }
                }
            }

            return true;
        }

        private void FilterModelsNames(ToolSet toolSet)
        {
            foreach (var item in toolSet.Tools)
            {
                if (!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    var info = new FileInfo(item.ConeModelFile);
                    item.ConeModelFile = info.Name;
                }

                if ((item is AngularTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    var info = new FileInfo(at.BodyModelFile);
                    at.BodyModelFile = info.Name;
                }
            }
        }

        private void UpdateModelsFiles(MachineElement m, string extractPath, bool save = false)
        {
            if (!string.IsNullOrEmpty(m.ModelFile))
            {
                m.ModelFile = $"{extractPath}\\{m.ModelFile}";
            }

            foreach (var item in m.Children)
            {
                UpdateModelsFiles(item, extractPath);
            }

            if (save)
            {
                var machProjectFile = $"{extractPath}\\{_machineFileName}";
                DataSource.SaveMachine(machProjectFile, m);
            }
        }

        private void UpdateModelsFiles(ToolSet toolSet, string extractPath, bool save = false)
        {
            foreach (var item in toolSet.Tools)
            {
                if (!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    item.ConeModelFile = $"{extractPath}\\{item.ConeModelFile}";
                }

                if ((item is AngularTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    at.BodyModelFile = $"{extractPath}\\{at.BodyModelFile}";
                }
            }

            if (save)
            {
                var toolSetFile = $"{extractPath}\\{_toolSetFileName}";
                DataSource.SaveTools(toolSetFile, toolSet);
            }
        }

        private void UpdateModelsFiles(Tooling tooling, string machineName, string extractPath, bool save = false)
        {
            tooling.Machine = $"{extractPath}\\{machineName}.json";
            tooling.Tools = $"{extractPath}\\{_toolSetFileName}";

            if (save)
            {
                var toolingFile = $"{extractPath}\\{_toolingFileName}";
                DataSource.SaveTooling(toolingFile, tooling);
            }
        }
    }
}
