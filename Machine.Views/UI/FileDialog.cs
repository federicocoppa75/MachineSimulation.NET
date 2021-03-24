using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MW32 = Microsoft.Win32;

namespace Machine.Views.UI
{
    public class FileDialog<T> : IFileDialog where T : MW32.FileDialog, new()
    {
        private T _dlg;

        public string FileName => _dlg.FileName;
        public string DefaultExt { get; set; }
        public string Filter { get; set; }
        public bool AddExtension { get; set; }

        public bool? ShowDialog()
        {
            if (_dlg == null) _dlg = new T();

            _dlg.DefaultExt = DefaultExt;
            _dlg.Filter = Filter;
            _dlg.AddExtension = AddExtension;

            return _dlg.ShowDialog();
        }
    }
}
