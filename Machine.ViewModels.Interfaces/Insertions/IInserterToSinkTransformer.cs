using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Insertions
{
    public struct InsertPosition
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }
    }

    public interface IInserterToSinkTransformer
    {
        InsertPosition Transform();
    }
}
