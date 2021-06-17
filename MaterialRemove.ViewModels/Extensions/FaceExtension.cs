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

        internal static Task<bool> IntersectAsync(this ISectionFace face, ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                var toolBox = await TaskHelper.ToAsync(() => toolActionData.GetBound());
                var faceBox = await TaskHelper.ToAsync(() =>  face.GetBound());

                return faceBox.Intersects(toolBox);
            });
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

        internal static Task ApplyActionAsync(this ISectionFace face, ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                if (face is SectionFaceViewModel sfvm)
                {
                    await sfvm.ApplyActionAsync(toolActionData);
                }
                else
                {
                    throw new NotImplementedException();
                }
            });
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

        internal static AxisAlignedBox3d GetFilterBox(this ISectionFace face)
        {
            switch (face.Orientation)
            {
                case Orientation.XPos:
                    return GetFilterBoxXPos(face);
                case Orientation.XNeg:
                    return GetFilterBoxXNeg(face);
                case Orientation.YPos:
                    return GetFilterBoxYPos(face);
                case Orientation.YNeg:
                    return GetFilterBoxYNeg(face);
                case Orientation.ZPos:
                    return GetFilterBoxZPos(face);
                case Orientation.ZNeg:
                    return GetFilterBoxZNeg(face);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static AxisAlignedBox3d GetFilterBoxZPos(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.z -= 0.0001;//0.1;
            box.Max.z += 0.0001;
            box.Max.x -= 0.0001;
            box.Max.y -= 0.0001;

            return box;
        }

        private static AxisAlignedBox3d GetFilterBoxZNeg(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.z -= 0.0001;
            box.Max.z += 0.0001; //0.1;
            box.Max.x -= 0.0001;
            box.Max.y -= 0.0001;

            return box;
        }

        private static AxisAlignedBox3d GetFilterBoxXPos(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.x -= 0.0001; // 0.1;
            box.Max.x += 0.0001;
            box.Max.z -= 0.0001;
            box.Max.y -= 0.0001;

            return box;
        }

        private static AxisAlignedBox3d GetFilterBoxXNeg(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.x -= 0.0001;
            box.Max.x += 0.0001;//0.1;
            box.Max.z -= 0.0001;
            box.Max.y -= 0.0001;

            return box;
        }

        private static AxisAlignedBox3d GetFilterBoxYPos(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.y -= 0.0001;//0.1;
            box.Max.y += 0.0001;
            box.Max.x -= 0.0001;
            box.Max.z -= 0.0001;

            return box;
        }

        private static AxisAlignedBox3d GetFilterBoxYNeg(ISectionFace face)
        {
            var box = face.GetBound();

            box.Min.y -= 0.0001;
            box.Max.y += 0.0001;//0.1;
            box.Max.x -= 0.0001;
            box.Max.z -= 0.0001;

            return box;
        }
    }
}
