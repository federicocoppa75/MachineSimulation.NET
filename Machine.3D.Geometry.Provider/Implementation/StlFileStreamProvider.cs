using Machine._3D.Geometry.Provider.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Machine._3D.Geometry.Provider.Implementation
{
    public class StlFileStreamProvider : IStreamProvider
    {
        public Stream GetStream(string fileName) => new FileStream(fileName, FileMode.Open, FileAccess.Read);
    }
}
