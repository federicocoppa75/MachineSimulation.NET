using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MMT = MachineModels.Models.Tools;
using MDT = Machine.Data.Tools;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Machine.Data.Converters;
using MachineModels.Extensions;

namespace Client.Tools.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MDT.ToolSet> Toolsets { get; private set; } = new ObservableCollection<MDT.ToolSet>();

        private MDT.ToolSet _selectedToolset;

        public MDT.ToolSet SelectedToolset
        {
            get => _selectedToolset;
            set
            {
                if(Set(ref _selectedToolset, value, nameof(SelectedToolset)))
                {
                    RaisePropertyChanged(nameof(SelectedToolsetTools));
                }
            }
        }

        private MDT.Tool _selectedTool;
        public MDT.Tool SelectedTool
        {
            get => _selectedTool; 
            set => Set(ref _selectedTool, value, nameof(SelectedTool));
        }


        public IEnumerable<MDT.Tool> SelectedToolsetTools => (SelectedToolset != null) ? SelectedToolset.Tools : null;

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
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tools", AddExtension = true, Filter = "Tool set file |*.tools" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadToolsFromFile(dlg.FileName);
            }
        }

        private void FileClearCommandImplementation()
        {
            SelectedToolset = null;
            Toolsets.Clear();
        }
        private void FileOpenJsonCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "jTools", AddExtension = true, Filter = "Tools set (JSON) |*.jTools" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add(new ToolJsonConverter());

                using (StreamReader sr = new StreamReader(dlg.FileName))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var m = serializer.Deserialize<MDT.ToolSet>(reader);

                    if (m != null) Toolsets.Add(m);
                }
            }
        }
        
        private void FileSaveJsonCommandImplementation()
        {
            if(SelectedToolset != null)
            {
                var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "jTools", AddExtension = true, Filter = "Tools set (JSON) |*.jTools" };
                var b = dlg.ShowDialog();

                if (b.HasValue && b.Value)
                {
                    JsonSerializer serializer = new JsonSerializer();

                    serializer.NullValueHandling = NullValueHandling.Ignore;

                    using (StreamWriter sw = new StreamWriter(dlg.FileName))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, SelectedToolset);
                    }
                }
            }
        }
        
        private async void DbSaveCommandImplementation()
        {
            if (SelectedToolset != null)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync<MDT.ToolSet>("https://localhost:44306/api/Tools", SelectedToolset);

                    if (response.IsSuccessStatusCode)
                    {

                    }
                }
            }
        }       
        
        private async void DbLoadCommandImplementation()
        {
            Toolsets.Clear();
            SelectedToolset = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Tools");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<MDT.ToolsetInfo>>();

                    foreach (var item in content)
                    {
                        var rr = await client.GetAsync($"https://localhost:44306/api/Tools/{item.ToolSetID}");

                        if (rr.IsSuccessStatusCode)
                        {
                            var cc = await rr.Content.ReadAsAsync<MDT.ToolSet>(new[]
                            {
                                new JsonMediaTypeFormatter()
                                {
                                    SerializerSettings = new JsonSerializerSettings()
                                    {
                                        Converters = new List<JsonConverter>()
                                        {
                                            new ToolJsonConverter()
                                        },
                                        NullValueHandling = NullValueHandling.Ignore
                                    }
                                }
                            });

                            Toolsets.Add(cc);
                        }
                    }
                }
            }
        }        
        
        private async void DbDeleteCommandImplementation()
        {
            if(SelectedToolset != null)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"https://localhost:44306/api/Tools/{SelectedToolset.ToolSetID}");

                    if (response.IsSuccessStatusCode)
                    {
                        Toolsets.Remove(SelectedToolset);
                        SelectedToolset = null;
                    }
                }
            }
        }


        private void LoadToolsFromFile(string fileName)
            {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MMT.ToolSet));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MMT.ToolSet)serializer.Deserialize(reader);

                if (m != null)
                {
                    var ts = m.ToToolsData();

                    ts.Name = Path.GetFileNameWithoutExtension(fileName);

                    Toolsets.Clear();
                    Toolsets.Add(ts);
                }
            }
        }
    }
}
