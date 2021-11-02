using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIP = Machine.ViewModels.Interfaces.Probing;

namespace MaterialRemove.ViewModels._3D
{
    public class ElementViewModelFactory : IElementViewModelFactory
    {
        public ViewModels.SectionFaceViewModel CreateSectionFaceViewModel() => new SectionFaceViewModel();

        public ViewModels.SectionVolumeViewModel CreateSectionVolumeViewModel() => new SectionVolumeViewModel();
    }
}
