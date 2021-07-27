using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels
{
    struct ToolSectionApplicationX
    {
        private struct Side
        {
            public Vector3f P1;
            public Vector3f P2;
        }

        public float L { get; }
        public float W { get; }
        public float H { get; set; }
        public Orientation FixBaseAx { get; }
        public Vector3f Position { get; }
        public Vector3f UpDirection { get; }
        public int Index { get; }

        public ToolSectionApplicationX(Vector3f position, Vector3f upDirection, Orientation fixBaseAx, float length, float weigth, float heigth, int index = -1)
        {
            L = length;
            W = weigth;
            H = heigth;
            Position = position;
            UpDirection = upDirection;
            FixBaseAx = fixBaseAx;
            Index = index;
        }

        public Vector3f GetFixAx()
        {
            switch (FixBaseAx)
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

        public Vector3f Get3rdAx() => Vector3f.Cross(UpDirection, GetFixAx());

        public Vector3f[] GetVertexes()
        {
            var vertexes = new Vector3f[8];
            var nx = GetFixAx();
            var ny = Get3rdAx();
            var nz = UpDirection;
            var dx = (float)(L / 2.0);
            var dy = (float)(W / 2.0);
            var dz = (float)(H / 2.0);

            vertexes[0] = Position - nx * dx - ny * dy - nz * dz;
            vertexes[1] = Position + nx * dx - ny * dy - nz * dz;
            vertexes[2] = Position + nx * dx + ny * dy - nz * dz;
            vertexes[3] = Position - nx * dx + ny * dy - nz * dz;
            vertexes[4] = Position - nx * dx - ny * dy + nz * dz;
            vertexes[4] = Position + nx * dx - ny * dy + nz * dz;
            vertexes[6] = Position + nx * dx + ny * dy + nz * dz;
            vertexes[7] = Position - nx * dx + ny * dy + nz * dz;

            return vertexes;
        }

        public bool Intersect(AxisAlignedBox3f box)
        {
            var result = false;
            var vertexes = GetVertexes();

            for (int i = 0; i < vertexes.Length; i++)
            {
                if (box.Contains(vertexes[i]))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private Side[] GetSides()
        {
            var vertexes = GetVertexes();
            var sides = new Side[12];

            sides[0] = new Side() { P1 = vertexes[0], P2 = vertexes[1] };
            sides[1] = new Side() { P1 = vertexes[1], P2 = vertexes[2] };
            sides[2] = new Side() { P1 = vertexes[2], P2 = vertexes[3] };
            sides[3] = new Side() { P1 = vertexes[3], P2 = vertexes[0] };
            sides[4] = new Side() { P1 = vertexes[4], P2 = vertexes[5] };
            sides[5] = new Side() { P1 = vertexes[5], P2 = vertexes[6] };
            sides[6] = new Side() { P1 = vertexes[6], P2 = vertexes[7] };
            sides[7] = new Side() { P1 = vertexes[7], P2 = vertexes[0] };
            sides[8] = new Side() { P1 = vertexes[0], P2 = vertexes[4] };
            sides[9] = new Side() { P1 = vertexes[1], P2 = vertexes[5] };
            sides[10] = new Side() { P1 = vertexes[2], P2 = vertexes[6] };
            sides[11] = new Side() { P1 = vertexes[3], P2 = vertexes[7] };

            return sides;
        }

        private bool Intersect(AxisAlignedBox3f box, Side side)
        {
            var result = false;

            if(box.Contains(side.P1) || box.Contains(side.P2))
            {
                result = true;
            }
            else
            {
               
            }

            return result;
        }
    }
}
