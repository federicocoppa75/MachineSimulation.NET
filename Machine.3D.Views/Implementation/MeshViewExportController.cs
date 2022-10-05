using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.UI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine._3D.Views.Implementation
{
    internal class MeshViewExportController : IViewExportController
    {
        Viewport3DX _viewport;

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(() => ExportCommandImpl(), () => IsCommandExecutable()));

        public MeshViewExportController(Viewport3DX viewport)
        {
            _viewport = viewport;
        }

        private bool IsCommandExecutable() => true;

        private void ExportCommandImpl()
        {
            var dlg = Machine.ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("SaveFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "obj";
            dlg.Filter = "Alias Wavefront Object |*.obj|BMP|*.bmp|JPEG|*.jpeg;*.jpg|PNG|*.png";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var ext = System.IO.Path.GetExtension(dlg.FileName);

                if(string.Compare(ext, "obj") == 0)
                {
                    using (var exporter = new ObjExporterEx(dlg.FileName))
                    {
                        exporter.Export(_viewport);
                    }
                }
                else
                {
                    _viewport.SaveScreen(dlg.FileName);
                }
            }            
        }
    }
}
