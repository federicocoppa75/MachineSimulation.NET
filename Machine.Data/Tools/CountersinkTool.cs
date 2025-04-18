﻿using Machine.Data.Interfaces.Tools;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("CountersinkTool")]
    public class CountersinkTool : Tool, IWorkData, ICountersinkTool
    {
        public double Diameter1 { get; set; }
        public double Length1 { get; set; }
        public double Diameter2 { get; set; }
        public double Length2 { get; set; }
        public double Length3 { get; set; }
        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Math.Max(Diameter1, Diameter2);
        public override double GetTotalLength() => Length1 + Length2 + Length3;

        public double GetUsefulLength() => UsefulLength;

        public double GetWorkLength() => GetTotalLength();

        public double GetWorkRadius() => GetTotalDiameter() / 2.0;
    }
}
