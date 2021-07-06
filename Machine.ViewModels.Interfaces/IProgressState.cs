using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public enum ProgressDirection
    {
        Farward,
        Back
    }

    public interface IProgressState
    {
        ProgressDirection ProgressDirection { get; }
        int ProgressIndex { get; }

        event EventHandler<int> ProgressIndexChanged;
    }
}
