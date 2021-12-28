using Machine.Data.Interfaces.Tools;
using Machine.Data.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class Subspindle : ISubspindle
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }
        public virtual string ToolName { get; set; }

        public void GetDirection(out double x, out double y, out double z)
        {
            x = Direction.X; 
            y = Direction.Y; 
            z = Direction.Z;
        }

        public void GetPosition(out double x, out double y, out double z)
        {
            x = Position.X;
            y = Position.Y;
            z = Position.Z;
        }

        public void SetDirection(double x, double y, double z)
        {
            Direction.X = x;
            Direction.Y = y;
            Direction.Z = z;
        }

        public void SetPosition(double x, double y, double z)
        {
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
        }
    }
}
