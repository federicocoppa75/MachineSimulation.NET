using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Base
{
    public class Matrix
    {
        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M13 { get; set; }
        //public double M14 { get; set; } sempre 0
        public double M21 { get; set; }
        public double M23 { get; set; }
        //public double M24 { get; set; }   sempre 0
        public double M31 { get; set; }
        public double M32 { get; set; }
        public double M33 { get; set; }
        //public double M34 { get; set; }   sempre 0
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double M22 { get; set; }
        public double OffsetZ { get; set; }
        //public double M44 { get; set; }   sempre 1

        public Matrix()
        {
        }

        public Matrix(Matrix matrix)
        {
            M11 = matrix.M11;
            M12 = matrix.M12;
            M13 = matrix.M13;                
            M21 = matrix.M21;
            M22 = matrix.M22;
            M23 = matrix.M23;
            M31 = matrix.M31;
            M32 = matrix.M32;
            M33 = matrix.M33;
            OffsetX = matrix.OffsetX;
            OffsetY = matrix.OffsetY;
            OffsetZ = matrix.OffsetZ;
        }
    }
}
