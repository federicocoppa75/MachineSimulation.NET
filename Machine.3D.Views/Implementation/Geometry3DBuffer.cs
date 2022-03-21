using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Implementation
{
    internal class Geometry3DBuffer : IGeometry3DBuffer, IElementBoundingBoxProvider
    {
        Dictionary<string, Geometry3D> _geometries = new Dictionary<string, Geometry3D>();

        public bool Contain(string key) => _geometries.ContainsKey(key);

        public void Add(string key, Geometry3D geometry) => _geometries[key] = geometry;

        public bool TryGet(string key, out Geometry3D geometry) => _geometries.TryGetValue(key, out geometry);

        public void Clear() => _geometries.Clear();

        public bool GetBoundingBox(IMachineElement element, out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ)
        {
            var result = TryGet(element.ModelFile, out Geometry3D geometry);

            if(result)
            {
                var bb = geometry.Bound;

                minX = bb.Minimum.X;
                minY = bb.Minimum.Y;
                minZ = bb.Minimum.Z;
                maxX = bb.Maximum.X;
                maxY = bb.Maximum.Y;
                maxZ = bb.Maximum.Z;
            }
            else
            {
                minX = 0.0;
                minY = 0.0;
                minZ = 0.0;
                maxX = 0.0;
                maxY = 0.0;
                maxZ = 0.0;
            }

            return result;
        }
    }
}
