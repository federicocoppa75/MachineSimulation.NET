using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using MDB = Machine.Data.Base;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    public abstract class PointSet
    {
        protected IList<MDB.Point> _points;

        public PointSet(IList<MDB.Point> points)
        {
            _points = points;
        }

        protected Vector GetAt(int index) => _points[index].Convert();
        protected void SetAt(Vector v, int index)
        {
            _points.RemoveAt(index);
            _points.Insert(index, v.Convert());
        }
    }

    public class PointSet1 : PointSet
    {
        [ExpandableObject]
        public Vector Point1 
        { 
            get => GetAt(0); 
            set => SetAt(value, 0); 
        }

        public PointSet1(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet2 : PointSet1
    {
        [ExpandableObject]
        public Vector Point2
        {
            get => GetAt(1);
            set => SetAt(value, 1);
        }

        public PointSet2(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet3 : PointSet2
    {
        [ExpandableObject]
        public Vector Point3
        {
            get => GetAt(2);
            set => SetAt(value, 2);
        }

        public PointSet3(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet4 : PointSet3
    {
        [ExpandableObject]
        public Vector Point4
        {
            get => GetAt(3);
            set => SetAt(value, 3);
        }

        public PointSet4(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet5 : PointSet4
    {
        [ExpandableObject]
        public Vector Point5
        {
            get => GetAt(4);
            set => SetAt(value, 4);
        }

        public PointSet5(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet6 : PointSet5
    {
        [ExpandableObject]
        public Vector Point6
        {
            get => GetAt(5);
            set => SetAt(value, 5);
        }

        public PointSet6(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet7 : PointSet6
    {
        [ExpandableObject]
        public Vector Point7
        {
            get => GetAt(6);
            set => SetAt(value, 6);
        }

        public PointSet7(IList<MDB.Point> points) : base(points) { }
    }

    public class PointSet8 : PointSet7
    {
        [ExpandableObject]
        public Vector Point8
        {
            get => GetAt(7);
            set => SetAt(value, 7);
        }

        public PointSet8(IList<MDB.Point> points) : base(points) { }
    }
}
