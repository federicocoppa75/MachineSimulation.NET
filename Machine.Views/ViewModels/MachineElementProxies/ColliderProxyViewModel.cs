using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using MDB = Machine.Data.Base;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    [CategoryOrder("Collider", 2)]
    public class ColliderProxyViewModel : ElementProxyViewModel
    {
        [Category("Collider")]
        public ColliderType ColliderType => (_element as IColliderElement).Type;

        [Category("Collider")]
        public int PointsNumber
        {
            get => GetPointsNumber();
            set => CheckAndUpdatePoints(value);
        }

        private PointSet _points;
        [Category("Collider")]
        [ExpandableObject]
        public PointSet Points 
        {
            get => _points;
            set
            {
                if(!ReferenceEquals(_points, value))
                {
                    _points = value;
                    RaisePropertyChanged(nameof(Points));
                    RaisePropertyChanged(nameof(PointsNumber));
                }
            }
        }

        public ColliderProxyViewModel(IColliderElement element) : base(element)
        {
            Points = element.Points.Convert();
        }

        private ICollection<MDB.Point> GetPoints() => (_element as IColliderElement).Points;
        private int GetPointsNumber() => GetPoints().Count;

        private void CheckAndUpdatePoints(int value)
        {
            if(GetPointsNumber() != value)
            {
                var points = GetPoints();
                var n = points.Count;
                var max = Math.Min(n, value);
                var ppp = new List<MDB.Point>();

                for (int i = 0; i < max; i++) ppp.Add((points as IList<MDB.Point>)[i]);
                points.Clear();
                for (int i = 0; i < max; i++) points.Add(ppp[i]);

                if(max < value)
                {
                    for (int i = max; i < value; i++) points.Add(new MDB.Point());
                }

                Points = points.Convert();
            }
        }
    }
}
