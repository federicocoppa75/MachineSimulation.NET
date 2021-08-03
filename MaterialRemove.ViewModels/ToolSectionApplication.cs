using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels
{
    struct ToolSectionApplication : BoundedImplicitFunction3d, IIndexed, IIntersector
    {
        Vector3f _position;
        Orientation _fixBaseAx;
        Vector3f _upDirection;
        Vector3f _ax1;
        Vector3f _ax2;
        float[] _halfDim;

        public int Index { get; }

        public ToolSectionApplication(Vector3f position, Vector3f upDirection, Orientation fixBaseAx, float length, float weigth, float heigth, int index = -1)
        {
            Index = index;

            _position = position;
            _upDirection = upDirection;
            _fixBaseAx = fixBaseAx;
            _ax1 = GetFixAx(fixBaseAx);
            _ax2 = Vector3f.Cross(_upDirection, _ax1);
            _halfDim = new float[] { length / 2.0f, weigth / 2.0f, heigth / 2.0f };
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
            var ax2 = _ax2 * _halfDim[1];
            var ax3 = _upDirection * _halfDim[2];

            var points = new Vector3f[4];

            points[0] = _position - ax2 - ax3;
            points[1] = _position + ax2 - ax3;
            points[2] = _position - ax2 + ax3;
            points[3] = _position + ax2 + ax3;

            var pMin = points[0];
            var pMax = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                if (pMin.x > points[i].x) pMin.x = points[i].x;
                if (pMin.y > points[i].y) pMin.y = points[i].y;
                if (pMin.z > points[i].z) pMin.z = points[i].z;
                if (pMax.x < points[i].x) pMax.x = points[i].x;
                if (pMax.y < points[i].y) pMax.y = points[i].y;
                if (pMax.z < points[i].z) pMax.z = points[i].z;
            }

            switch (_fixBaseAx)
            {
                case Orientation.XPos:
                case Orientation.XNeg:
                    pMin.x -= _halfDim[0];
                    pMax.x += _halfDim[0];
                    break;
                case Orientation.YPos:
                case Orientation.YNeg:
                    pMin.y -= _halfDim[1];
                    pMax.y += _halfDim[1];
                    break;
                case Orientation.ZPos:
                case Orientation.ZNeg:
                    pMin.z -= _halfDim[2];
                    pMax.z += _halfDim[2];
                    break;
                default:
                    break;
            }

            return new AxisAlignedBox3d(pMin, pMax);
        }

        public double Value(ref Vector3d pt)
        {
            var v = (Vector3f)pt - _position;
            var a = v.Dot(_ax1);
            var b = v.Dot(_ax2);
            var c = v.Dot(_upDirection);
            var insX = (a >= -_halfDim[0]) && (a <= _halfDim[0]);
            var insY = (b >= -_halfDim[1]) && (a <= _halfDim[1]);
            var insZ = (c >= -_halfDim[2]) && (a <= _halfDim[2]);
            var r1 = Math.Abs(a) - _halfDim[0];
            var r2 = Math.Abs(b) - _halfDim[1];
            var r3 = Math.Abs(c) - _halfDim[2];

            if (insX && insY && insZ)
            {
                var result = r1;

                if (result < r2) result = r2;
                if (result < r3) result = r3;

                return result;
            }
            else
            {
                var result = 0.0f;

                if (r1 > 0.0) result += r1 * r1;
                if (r2 > 0.0) result += r2 * r2;
                if (r3 > 0.0) result += r3 * r3;

                return Math.Sqrt(result);
            }
        }

        public bool Intersect(AxisAlignedBox3f box) => Bounds().Intersects(box);
        public bool Intersect(AxisAlignedBox3d box) => Bounds().Intersects(box);

        #region IIntersector
        public bool Intersect(IPanel panel)
        {
            var panelBox = new AxisAlignedBox3d(new Vector3d(), panel.SizeX / 2.0, panel.SizeY / 2.0, panel.SizeZ / 2.0);
            var toolBox = Bounds();

            return panelBox.Intersects(toolBox);
        }

        public bool Intersect(IPanelSection section)
        {
            var sectionBox = section.GetBound();
            var toolBox = Bounds();

            return sectionBox.Intersects(toolBox);
        }

        public bool Intersect(ISectionFace face)
        {
            var toolBox = Bounds();
            var faceBox = face.GetBound();

            return faceBox.Intersects(toolBox);
        }
        #endregion
    }
}
