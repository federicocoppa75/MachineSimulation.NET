using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    public class PanelViewModel : IPanel
    {
        private PanelSectionsProxy _panelSectionsProxy;
        
        public int NumCells { get; set; }
        public int SectionsX100mm { get; set; }
        public double CubeSize { get; set; }
        public double FilterMargin { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }

        public IList<IPanelSection> Sections => (_panelSectionsProxy != null) ? _panelSectionsProxy.Sections : null;

        public void Initialize()
        {
            _panelSectionsProxy = new PanelSectionsProxy() { Sections = this.CreateSections() };
        }

        public void ApplyAction(ToolActionData toolActionData) => _panelSectionsProxy.ApplyAction(this, toolActionData);

        public Task ApplyActionAsync(ToolActionData toolActionData) => _panelSectionsProxy.ApplyActionAsync(this, toolActionData);

        public void ApplyAction(ToolSectionActionData toolSectionActionData) => _panelSectionsProxy.ApplyAction(this, toolSectionActionData);

        public Task ApplyActionAsync(ToolSectionActionData toolSectionActionData) => _panelSectionsProxy.ApplyActionAsync(this, toolSectionActionData);

        public void ApplyAction(ToolConeActionData toolConeActionData) => _panelSectionsProxy.ApplyAction(this, toolConeActionData);

        public Task ApplyActionAsync(ToolConeActionData toolConeActionData) => _panelSectionsProxy.ApplyActionAsync(this, toolConeActionData);
    }
}
