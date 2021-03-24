using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//using GalaSoft.MvvmLight.Threading;
using Mesh.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MeshViewModel> MeshList { get; private set; } = new ObservableCollection<MeshViewModel>();

        private MeshViewModel _selected;
        public MeshViewModel Selected
        {
            get => _selected;
            set
            {
                if (Set(ref _selected, value, nameof(Selected)))
                {
                    RaisePropertyChanged(nameof(MeshGeometry));
                    (_dbDeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public MeshGeometry3D MeshGeometry => (Selected != null) ? Selected.MeshGeometry : null;

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand => _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImpl()));

        private ICommand _dbLoadCommand;
        public ICommand DbLoadCommand => _dbLoadCommand ?? (_dbLoadCommand = new RelayCommand(() => DbLoadCommandImpl()));

        private ICommand _dbDeleteCommand;
        public ICommand DbDeleteCommand => _dbDeleteCommand ?? (_dbDeleteCommand = new RelayCommand(() => DbDeleteCommandImpl(), () => Selected != null));

        public MainViewModel() : base()
        {
            //MessengerInstance.Register<WindowLoadedMessage>(this, OnWindowsLoadedMessage);
        }

        private async void FileOpenCommandImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "stl", AddExtension = true, Filter = "Mesh file |*.stl", Multiselect = true };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                foreach (var item in dlg.FileNames)
                {
                    await ReadAndStore(item);
                }                
            }
        }

        private async void DbLoadCommandImpl()
        {
            Selected = null;
            MeshList.Clear();

            using(var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Models");

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<Mesh.Data.MeshInfo>>();

                    foreach (var item in content)
                    {
                        MeshList.Add(new MeshViewModel()
                        {
                            Id = item.MeshID,
                            Name = item.Name,
                            Size = item.Size
                        });
                    }
                }
            }
        }

        //private void OnWindowsLoadedMessage(WindowLoadedMessage msg) => DbLoadCommandImpl();

        private async void DbDeleteCommandImpl()
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:44306/api/Models/{Selected.Id}");

                if (response.IsSuccessStatusCode)
                {
                    MeshList.Remove(Selected);
                    Selected = null;
                }
            }
        }

        private async Task ReadAndStore(string fileName)
        {
            var content = File.ReadAllBytes(fileName);

            if (content.Length > 0)
            {
                var name = Path.GetFileNameWithoutExtension(fileName);

                var model = new Mesh.Data.Mesh()
                {
                    Name = name,
                    Data = content
                };

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync<Mesh.Data.Mesh>("https://localhost:44306/api/Models", model);

                    if (response.IsSuccessStatusCode)
                    {
                        var info = await response.Content.ReadAsAsync<MeshInfo>();

                        MeshList.Add(new MeshViewModel()
                        {
                            Id = info.MeshID,
                            Name = info.Name,
                            Size = info.Size
                        });
                    }
                }
            }
        }
    }
}
