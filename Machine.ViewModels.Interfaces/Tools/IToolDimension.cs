using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Tools
{
    public interface IToolDimension
    {
        string PropertyName { get; set; }
        Point ContactPoint1 { get; set; }
        Point ContactPoint2 { get; set; }
        Point MeasurePoint1 { get; set; }
        Point MeasurePoint2 { get; set; }
    }
}
