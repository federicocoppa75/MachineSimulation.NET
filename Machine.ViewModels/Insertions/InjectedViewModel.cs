﻿using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Insertions
{
    public class InjectedViewModel : ElementViewModel, IInjectedObject
    {
        public int InserterId { get; set; }
        public Point Position { get; set; }
        public Vector Direction { get; set; }
    }
}