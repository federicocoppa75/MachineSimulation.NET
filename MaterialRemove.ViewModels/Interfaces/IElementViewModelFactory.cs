using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Interfaces
{
    public interface IElementViewModelFactory
    {
        SectionFaceViewModel CreateSectionFaceViewModel();
        SectionVolumeViewModel CreateSectionVolumeViewModel();
    }
}
