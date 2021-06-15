using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class VolumeExtension
    {
        private static Tuple<double, double, int, double> _calcolatedCubeSize;

        internal static AxisAlignedBox3d GetBound(this ISectionVolume section) => new AxisAlignedBox3d(new Vector3d(section.CenterX, section.CenterY, section.CenterZ), section.SizeX / 2.0, section.SizeY / 2.0, section.SizeZ / 2.0);
    }
}
