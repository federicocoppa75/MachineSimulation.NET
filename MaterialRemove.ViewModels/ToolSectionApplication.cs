using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels
{
    struct ToolSectionApplication : BoundedImplicitFunction3d, IIndexed
    {
        private ImplicitBox3d _iBox;
        public int Index { get; set; }

        public ToolSectionApplication(Vector3f position, Vector3f upDirection, Orientation fixBaseAx, float length, float weigth, float heigth, int index = -1)
        {
            var ax1 = GetFixAx(fixBaseAx);
            var ax2 = Vector3f.Cross(upDirection, ax1);

            Index = index;
            _iBox = new ImplicitBox3d() { Box = new Box3f(position, ax1, ax2, upDirection, new Vector3f(length / 2.0, weigth / 2.0, heigth / 2.0)) };
        }

        private static Vector3f GetFixAx(Orientation fixBaseAx)
        {
            switch (fixBaseAx)
            {
                case Orientation.XPos:
                    return new Vector3f(1.0, 0.0, 0.0);
                case Orientation.XNeg:
                    return new Vector3f(-1.0, 0.0, 0.0);
                case Orientation.YPos:
                    return new Vector3f(0.0, 1.0, 0.0);
                case Orientation.YNeg:
                    return new Vector3f(0.0, -1.0, 0.0);
                case Orientation.ZPos:
                    return new Vector3f(0.0, 0.0, 1.0);
                case Orientation.ZNeg:
                    return new Vector3f(0.0, 0.0, -1.0);
                default:
                    throw new ArgumentException();
            }
        }

        public AxisAlignedBox3d Bounds() => _iBox.Box.ToAABB();
        public double Value(ref Vector3d pt) => _iBox.Value(ref pt);
        public bool Intersect(AxisAlignedBox3f box) => Bounds().Intersects(box);
        public bool Intersect(AxisAlignedBox3d box) => Bounds().Intersects(box);
    }
}
