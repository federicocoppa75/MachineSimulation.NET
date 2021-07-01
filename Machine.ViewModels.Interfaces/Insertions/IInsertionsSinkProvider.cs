using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Insertions
{
    public interface IInsertionsSinkProvider
    {
        IInsertionsSink InsertionsSink { get; set; }
    }
}
