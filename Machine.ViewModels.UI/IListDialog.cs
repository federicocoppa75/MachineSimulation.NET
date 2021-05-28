using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IListDialog
    {
        IEnumerable<string> Options { get; set; }
        string Selection { get; set; }
        string Title { get; set; }

        bool ShowDialog();
    }
}
