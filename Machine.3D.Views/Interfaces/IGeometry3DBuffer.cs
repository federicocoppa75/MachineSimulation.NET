using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Interfaces
{
    internal interface IGeometry3DBuffer
    {
        bool Contain(string key);
        bool TryGet(string key, out Geometry3D geometry);
        void Add(string key, Geometry3D geometry);
        void Clear();
    }
}
