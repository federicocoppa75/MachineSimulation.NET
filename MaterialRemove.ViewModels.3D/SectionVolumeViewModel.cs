using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRVM = MaterialRemove.ViewModels;

namespace MaterialRemove.ViewModels._3D
{
    class SectionVolumeViewModel : MRVM.SectionVolumeViewModel
    {
        private Geometry3D _geometry;

        public Geometry3D Geometry
        {
            get => _geometry; 
            set => Set(ref _geometry, value, nameof(Geometry));
        }

        protected override void OnActionApplied() => Geometry = GeometryHelper.Convert(InternalGeometry);
    }
}
