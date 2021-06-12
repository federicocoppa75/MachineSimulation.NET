using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class FaceExtension
    {
        internal static bool Intersect(this ISectionFace face, ToolActionData toolActionData)
        {
            var toolBox = toolActionData.GetBound();
            var faceBox = face.GetBound();

            return faceBox.Intersects(toolBox);
        }

        internal static void ApplyAction(this ISectionFace face, ToolActionData toolActionData)
        {
            if(face is SectionFaceViewModel sfvm)
            {
                sfvm.ApplyAction(toolActionData);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal static AxisAlignedBox3d GetBound(this ISectionFace face)
        {
            var c = new Vector3d(face.CenterX, face.CenterY, face.CenterZ);

            switch (face.Orientation)
            {
                case Orientation.XPos:
                case Orientation.XNeg:
                    return new AxisAlignedBox3d(c, 0.0, face.SizeX / 2.0, face.SizeY / 2.0);
                case Orientation.YPos:
                case Orientation.YNeg:
                    return new AxisAlignedBox3d(c, face.SizeX / 2.0, 0.0, face.SizeY / 2.0);
                case Orientation.ZPos:
                case Orientation.ZNeg:
                    return new AxisAlignedBox3d(c, face.SizeX / 2.0, face.SizeY / 2.0, 0.0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static double GetDistance(this ISectionFace face, Vector3d pt)
        {
            var center = new Vector3d(face.CenterX, face.CenterY, face.CenterZ);
            var D = pt - center;
            var N = face.GetNormal();
            var U = face.GetU();
            var V = face.GetV();
            var halfW = (face.SizeX + 5.0) / 2.0;
            var halfH = (face.SizeY + 5.0) / 2.0;
            var du = U.Dot(ref D);
            var dv = V.Dot(ref D);
            var uuul = du <= halfW;
            var uull = du >= -halfW;
            var uvul = dv <= halfH;
            var uvll = dv >= -halfH;
            var nc = N.Dot(ref D);
            double result = 0.0f;

            if (uuul && uull && uvll && uvul)
            {
                result = nc;
            }
            else if (uuul && uull)
            {
                var pc = Math.Abs(dv) - halfH;
                result = Math.Sqrt(pc * pc + nc * nc);
            }
            else if (uvll && uvul)
            {
                var pc = Math.Abs(du) - halfW;
                result = Math.Sqrt(pc * pc + nc * nc);
            }
            else
            {
                var cu = Math.Abs(du) - halfW;
                var cv = Math.Abs(dv) - halfH;

                result = Math.Sqrt(nc * nc + cu * cu + cv * cv);
            }

            return result;
        }

        internal static Vector3d GetNormal(this ISectionFace face)
        {
            switch (face.Orientation)
            {
                case Orientation.XPos:
                    return new Vector3d(1.0, 0.0, 0.0);
                case Orientation.XNeg:
                    return new Vector3d(-1.0, 0.0, 0.0);
                case Orientation.YPos:
                    return new Vector3d(0.0, 1.0, 0.0);
                case Orientation.YNeg:
                    return new Vector3d(0.0, -1.0, 0.0);
                case Orientation.ZPos:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.ZNeg:
                    return new Vector3d(0.0, 0.0, -1.0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static Vector3d GetU(this ISectionFace face)
        {
            switch (face.Orientation)
            {
                case Orientation.XPos:
                    return new Vector3d(0.0, 1.0, 0.0);
                case Orientation.XNeg:
                    return new Vector3d(0.0, -1.0, 0.0);
                case Orientation.YPos:
                    return new Vector3d(-1.0, 0.0, 0.0);
                case Orientation.YNeg:
                    return new Vector3d(1.0, 0.0, 0.0);
                case Orientation.ZPos:
                    return new Vector3d(1.0, 0.0, 0.0);
                case Orientation.ZNeg:
                    return new Vector3d(1.0, 0.0, 0.0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static Vector3d GetV(this ISectionFace face)
        {
            switch (face.Orientation)
            {
                case Orientation.XPos:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.XNeg:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.YPos:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.YNeg:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.ZPos:
                    return new Vector3d(0.0, 1.0, 0.0);
                case Orientation.ZNeg:
                    return new Vector3d(0.0, -1.0, 0.0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
