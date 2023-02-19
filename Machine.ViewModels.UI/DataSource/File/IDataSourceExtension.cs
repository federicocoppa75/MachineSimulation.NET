using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI.DataSource.File
{
    public interface IDataSourceExtension
    {
        IEnumerable<string> Files { get; }

        bool OnEnvironmentLoaded(string extractionPath);
    }
}
