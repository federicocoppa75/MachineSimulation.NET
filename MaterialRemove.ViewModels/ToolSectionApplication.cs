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
        //public float L { get; }
        //public float W { get; }
        //public float H { get; set; }
        //public Orientation FixBaseAx { get; }
        //public Vector3f Position { get; }
        //public Vector3f UpDirection { get; }
        public int Index { get; }
        public Box3f Box { get; private set; }

        public ToolSectionApplication(Vector3f position, Vector3f upDirection, Orientation fixBaseAx, float length, float weigth, float heigth, int index = -1)
        {
            //L = length;
            //W = weigth;
            //H = heigth;
            //Position = position;
            //UpDirection = upDirection;
            //FixBaseAx = fixBaseAx;
            var ax1 = GetFixAx(fixBaseAx);
            var ax2 = Vector3f.Cross(upDirection, ax1);

            Index = index;
            Box = new Box3f(position, ax1, ax2, upDirection, new Vector3f(length / 2.0, weigth / 2.0, heigth / 2.0));
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

        public AxisAlignedBox3d Bounds()
        {
            var implicitBox = new ImplicitBox3d() { Box = Box };

            return implicitBox.Bounds();
        }

        public double Value(ref Vector3d pt)
        {
            var implicitBox = new ImplicitBox3d() { Box = Box };

            return implicitBox.Value(ref pt);
        }

        public bool Intersect(AxisAlignedBox3f box)
        {
            var result = false;
            var implicitBox = new ImplicitBox3d() { Box = Box };
            var bound = implicitBox.Bounds();

            if(bound.Intersects(box))
            {
                var segments = GetSegments(box);

                for (int i = 0; i < segments.Length; i++)
                {
                    var intersect = new IntrSegment3Box3(segments[i], Box, true);

                    if(intersect.Compute().Result == IntersectionResult.Intersects)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private static Segment3f[] GetSegments(AxisAlignedBox3f box)
        {
            var segments = new Segment3f[12];
            var x = new Vector3f(box.Extents.x, 0.0f, 0.0f);
            var y = new Vector3f(0.0f, box.Extents.y, 0.0f);
            var z = new Vector3f(0.0f, 0.0f, box.Extents.z);

            segments[0] = new Segment3f(box.Min, box.Min + x);
            segments[1] = new Segment3f(box.Min + x, box.Min + x + y);
            segments[2] = new Segment3f(box.Min + x + y, box.Min + y);
            segments[3] = new Segment3f(box.Min + y, box.Min);
            segments[4] = new Segment3f(box.Min + z, box.Min + z + x);
            segments[5] = new Segment3f(box.Min + z + x, box.Min + z + x + y);
            segments[6] = new Segment3f(box.Min + z + x + y, box.Min + z + y);
            segments[7] = new Segment3f(box.Min + z + y, box.Min + z);
            segments[8] = new Segment3f(box.Min, box.Min + z);
            segments[9] = new Segment3f(box.Min + x, box.Min + x + z);
            segments[10] = new Segment3f(box.Min + x + y, box.Min + x + y + z);
            segments[11] = new Segment3f(box.Min + y, box.Min + y + z);

            return segments;
        }
    }
}
