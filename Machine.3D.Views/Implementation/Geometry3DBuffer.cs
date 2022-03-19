using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Implementation
{
    internal class Geometry3DBuffer : IGeometry3DBuffer
    {
        Dictionary<string, Geometry3D> _geometries = new Dictionary<string, Geometry3D>();

        public bool Contain(string key) => _geometries.ContainsKey(key);

        public void Add(string key, Geometry3D geometry) => _geometries[key] = geometry;

        public bool TryGet(string key, out Geometry3D geometry) => _geometries.TryGetValue(key, out geometry);

        public void Clear() => _geometries.Clear();
    }
}
