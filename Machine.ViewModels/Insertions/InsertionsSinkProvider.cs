using Machine.ViewModels.Interfaces.Insertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Insertions
{
    public class InsertionsSinkProvider : IInsertionsSinkProvider
    {
        public IInsertionsSink InsertionsSink { get; set; }
    }
}
