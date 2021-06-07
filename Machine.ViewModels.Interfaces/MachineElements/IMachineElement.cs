using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IMachineElement
    {
        int MachineElementID { get; set; }
        string Name { get; set; }
        string ModelFile { get; set; }
        Color Color { get; set; }
        Matrix Transformation { get; set; }
        ICollection<IMachineElement> Children { get; }
        ILinkViewModel LinkToParent { get; set; }
        IMachineElement Parent { get; set; }
    }
}
