using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IFileDialog
    {
        string FileName { get; }
        string DefaultExt { get; set; }
        string Filter { get; set; }
        bool AddExtension { get; set; }

        bool? ShowDialog();
    }
}
