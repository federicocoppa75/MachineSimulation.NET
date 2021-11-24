using Machine.ViewModels;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    public class SelectedViewModel : BaseElementsCollectionViewModel
    {
        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
        }

        protected override void Clear()
        {
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
        }
    }
}
