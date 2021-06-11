using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public interface IRemovalParameters
    {
        int NumCells { get; set; }
        int SectionsX100mm { get; set; }
    }
}
