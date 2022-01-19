using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.Machine.Bridge
{
    public class PanelExportController : MaterialRemove.ViewModels.PanelExportController
    {
        protected override bool GetFileName(out string fileName)
        {
            var result = false;
            var dlg = GetInstance<IFileDialog>("SaveFile");

            dlg.DefaultExt = "stl";
            dlg.AddExtension = true;
            dlg.Filter = "STL File format |*.stl";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                fileName = dlg.FileName;
                result = true;
            }
            else
            {
                fileName = string.Empty;
            }

            return result;
        }
    }
}
