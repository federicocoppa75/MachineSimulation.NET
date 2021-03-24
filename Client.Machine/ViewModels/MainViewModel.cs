using Client.Machine.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MD = Machine.Data.MachineElements;
using MDE = Machine.Data.Enums;
using MM = MachineModels.Models;
using MachineInfo = Machine.Data.MachineInfo;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Machine.Data.Converters;
using System.IO;

namespace Client.Machine.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MD.MachineElement> Machines { get; set; } = new ObservableCollection<MD.MachineElement>();

        private MD.MachineElement _selectedItem;
        public MD.MachineElement SelectedItem
        {
            get { return _selectedItem; }
            set 
            { 
                if(Set(ref _selectedItem, value, nameof(SelectedItem)))
                {

                }
            }
        }


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

        private void FileOpenCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "xml", AddExtension = true, Filter = "Machine struct |*.xml" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadMachineFromFile(dlg.FileName);
            }
        }

        private void FileClearCommandImplementation()
        {
            SelectedItem = null;
            Machines.Clear();
        }

        private void FileOpenJsonCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "json", AddExtension = true, Filter = "Machine JSON struct |*.json" };
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
                    var m = serializer.Deserialize<MD.MachineElement>(reader);

                    if (m != null) Machines.Add(m);
                }
            }
        }

        private void FileSaveJsonCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "json", AddExtension = true, Filter = "Machine JSON struct |*.json" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add(new LinkJsonConverter());
                serializer.Converters.Add(new MachineElementJsonConverter());

                using (StreamWriter sw = new StreamWriter(dlg.FileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, SelectedItem);
                }
            }
        }

        private async Task DbSaveCommandImplementation()
        {
            if(SelectedItem is MD.RootElement re)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync<MD.RootElement>("https://localhost:44306/api/Machine", re);

                    if (response.IsSuccessStatusCode)
                    {
                        var info = await response.Content.ReadAsAsync<int>();
                    }
                }
            }
        }

        private async void DbLoadCommandImplementation()
        {
            Machines.Clear();
            SelectedItem = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Machine");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<MachineInfo>>();

                    foreach (var item in content)
                    {
                        var rr = await client.GetAsync($"https://localhost:44306/api/Machine/{item.MachineElementID}");

                        if (rr.IsSuccessStatusCode)
                        {
                            var cc = await rr.Content.ReadAsAsync<MD.MachineElement>(new[]
                            {
                                new JsonMediaTypeFormatter()
                                {
                                    SerializerSettings = new JsonSerializerSettings()
                                    {
                                        Converters = new List<JsonConverter>()
                                        {
                                            new LinkJsonConverter(),
                                            new MachineElementJsonConverter()
                                        },
                                        NullValueHandling = NullValueHandling.Ignore,
                                    }
                                }
                            });

                            Machines.Add(cc);
                        }
                    }

                    //var tasks = new List<Task<MD.MachineElement>>();

                    //foreach (var item in content)
                    //{
                    //    var rr = await client.GetAsync($"https://localhost:44306/api/Machine/{item.MachineElementID}");

                    //    if (rr.IsSuccessStatusCode)
                    //    {
                    //        tasks.Add(rr.Content.ReadAsAsync<MD.MachineElement>(new[]
                    //        {
                    //            new JsonMediaTypeFormatter()
                    //            {
                    //                SerializerSettings = new JsonSerializerSettings()
                    //                {
                    //                    Converters = new List<JsonConverter>()
                    //                    {
                    //                        new LinkJsonConverter(),
                    //                        new MachineElementJsonConverter()
                    //                    },
                    //                    NullValueHandling = NullValueHandling.Ignore,
                    //                }
                    //            }
                    //        }));

                    //        //Machines.Add(cc);
                    //    }
                    //}

                    //var elements = await Task.WhenAll<MD.MachineElement>(tasks.ToArray());

                    //foreach (var item in elements) Machines.Add(item);
                }
            }
        }

        private async void DbDeleteCommandImplementation()
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:44306/api/Machine/{SelectedItem.MachineElementID}");

                if (response.IsSuccessStatusCode)
                {
                    Machines.Remove(SelectedItem);
                    SelectedItem = null;
                }
            }
        }

        private void LoadMachineFromFile(string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MM.MachineElement));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var m = (MM.MachineElement)serializer.Deserialize(reader);

                if (m != null)
                {
                    var vm = m.ToMachineData(true);

                    if(vm is MD.RootElement re)
                    {
                        re.AssemblyName = Path.GetFileNameWithoutExtension(fileName);
                        re.RootType = MDE.RootType.CX220;
                    }

                    Machines.Clear();
                    Machines.Add(vm);
                }
            }
        }

        private MachineElementViewModel ToViewModel(MM.MachineElement model)
        {
            var vm = new MachineElementViewModel() { Name = model.Name };

            foreach (var item in model.Children)
            {
                vm.Children.Add(ToViewModel(item));
            }

            return vm;
        }
    }
}
