using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MMT = MachineModels.Models.Tooling;
using MDT = Machine.Data.Toolings;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using MachineModels.Extensions;

namespace Client.Tooling.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MDT.Tooling> Toolings { get; private set; } = new ObservableCollection<MDT.Tooling>();

        private MDT.Tooling _selectedTooling;
        public MDT.Tooling SelectedTooling
        {
            get => _selectedTooling;
            set
            {
                if (Set(ref _selectedTooling, value, nameof(SelectedTooling)))
                {
                    RaisePropertyChanged(nameof(SelectedToolingUnits));
                }
            }
        }

        public IEnumerable<MDT.ToolingUnit> SelectedToolingUnits => (_selectedTooling != null) ? _selectedTooling.Units : null;


        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _fileClearCommand;
        public ICommand FileClearCommand { get { return _fileClearCommand ?? (_fileClearCommand = new RelayCommand(() => FileClearCommandImplementation())); } }

        private ICommand _fileOpenJsonCommand;
        public ICommand FileOpenJsonCommand { get { return _fileOpenJsonCommand ?? (_fileOpenJsonCommand = new RelayCommand(() => FileOpenJsonCommandImplementation())); } }

        private ICommand _fileSaveJsonCommand;
        public ICommand FileSaveJsonCommand { get { return _fileSaveJsonCommand ?? (_fileSaveJsonCommand = new RelayCommand(() => FileSaveJsonCommandImplementation())); } }

        private ICommand _dbSaveCommand;
        public ICommand DbSaveCommand { get { return _dbSaveCommand ?? (_dbSaveCommand = new RelayCommand(() => DbSaveCommandImplementation())); } }

        private ICommand _dbLoadCommand;
        public ICommand DbLoadCommand { get { return _dbLoadCommand ?? (_dbLoadCommand = new RelayCommand(() => DbLoadCommandImplementation())); } }

        private ICommand _dbDeleteCommand;
        public ICommand DbDeleteCommand { get { return _dbDeleteCommand ?? (_dbDeleteCommand = new RelayCommand(() => DbDeleteCommandImplementation())); } }

        public MainViewModel() : base()
        {
        }

        private void FileOpenCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tooling", AddExtension = true, Filter = "Machine tooling |*.tooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadToolingFromFile(dlg.FileName);
            }
        }

        private void FileClearCommandImplementation()
        {
            SelectedTooling = null;
            Toolings.Clear();
        }

        private void FileOpenJsonCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "jTooling", AddExtension = true, Filter = "Tooling (JSON) |*.jTooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                JsonSerializer serializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };
 
                using (StreamReader sr = new StreamReader(dlg.FileName))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var m = serializer.Deserialize<MDT.Tooling>(reader);

                    if (m != null) Toolings.Add(m);
                }
            }
        }

        private void FileSaveJsonCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "jTooling", AddExtension = true, Filter = "Tooling (JSON) |*.jTooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                JsonSerializer serializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };

                using (StreamWriter sw = new StreamWriter(dlg.FileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, SelectedTooling);
                }
            }
        }
    
        private async void DbSaveCommandImplementation()
        {
            if (SelectedTooling != null)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync<MDT.Tooling>("https://localhost:44306/api/Tooling", SelectedTooling);

                    if (response.IsSuccessStatusCode)
                    {

                    }
                }
            }
        }

        private async void DbLoadCommandImplementation()
        {
            SelectedTooling = null;
            Toolings.Clear();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Tooling");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<MDT.Tooling>>();

                    foreach (var item in content)
                    {
                        Toolings.Add(item);
                    }
                }
            }
        }

        private async void DbDeleteCommandImplementation()
        {
            if (SelectedTooling != null)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"https://localhost:44306/api/Tooling/{SelectedTooling.ToolingID}");

                    if (response.IsSuccessStatusCode)
                    {
                        Toolings.Remove(SelectedTooling);
                        SelectedTooling = null;
                    }
                }
            }
        }

        private void LoadToolingFromFile(string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MMT.Tooling));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MMT.Tooling)serializer.Deserialize(reader);

                if (m != null)
                {
                    var t = m.ToToolsData();

                    t.Name = Path.GetFileNameWithoutExtension(fileName);

                    Toolings.Clear();
                    Toolings.Add(t);
                }
            }
        }
    }
}
