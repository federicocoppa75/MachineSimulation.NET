using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Machine._3D.Geometry.Provider.Interfaces
{
    public interface IStreamProvider
    {
        Stream GetStream(string name);
    }
}
