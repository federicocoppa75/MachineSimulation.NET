using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class ColliderElementViewModel : ElementViewModel
    {
        public ColliderType Type { get; set; }
        public double Radius { get; set; }
        public virtual ICollection<Point> Points { get; set; } = new List<Point>();
    }
}
