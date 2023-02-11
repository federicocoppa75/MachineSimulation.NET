using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDB = Machine.Data.Base;
using SWM = System.Windows.Media;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    static class ElementProxyHelper
    {
        public static Vector Convert(this MDB.Point point) => (point != null) ? new Vector() { X = point.X, Y = point.Y, Z = point.Z } : new Vector();
        public static Vector Convert(this MDB.Vector vector) => (vector != null) ? new Vector() { X = vector.X, Y = vector.Y, Z = vector.Z } : new Vector();
        public static MDB.Point Convert(this Vector vector) => new MDB.Point() { X = vector.X, Y = vector.Y, Z = vector.Z };
        public static MDB.Vector ConvertV(this Vector vector) => new MDB.Vector() { X = vector.X, Y = vector.Y, Z = vector.Z };
        public static SWM.Color Convert(this MDB.Color color) => (color != null) ? SWM.Color.FromArgb(color.A, color.R, color.G, color.B) : SWM.Colors.WhiteSmoke;
        public static MDB.Color Convert(this SWM.Color color) => new MDB.Color() { A = color.A, R = color.R, G = color.G, B = color.B };
        
        public static PointSet Convert(this ICollection<MDB.Point> points)
        {
            switch (points.Count)
            {
                case 1: return new PointSet1(points as IList<MDB.Point>);
                case 2: return new PointSet2(points as IList<MDB.Point>);
                case 3: return new PointSet3(points as IList<MDB.Point>);
                case 4: return new PointSet4(points as IList<MDB.Point>);
                case 5: return new PointSet5(points as IList<MDB.Point>);
                case 6: return new PointSet6(points as IList<MDB.Point>);
                case 7: return new PointSet7(points as IList<MDB.Point>);
                case 8: return new PointSet8(points as IList<MDB.Point>);
                default: return new PointSet1(points as IList<MDB.Point>);
            }
        }
    }
}
