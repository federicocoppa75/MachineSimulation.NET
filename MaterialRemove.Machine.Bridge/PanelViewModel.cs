using System;
using System.Collections.Generic;
using System.Text;
using MVMME = Machine.ViewModels.MachineElements;
using MaterialRemove.Interfaces;
using System.Threading.Tasks;
using MaterialRemove.ViewModels;
using MaterialRemove.ViewModels.Extensions;
using System.Linq;

namespace MaterialRemove.Machine.Bridge
{
    class PanelViewModel : MVMME.PanelViewModel, IPanel
    {
        private PanelSectionsProxy _panelSectionsProxy;

        public int NumCells { get; set; }
        public int SectionsX100mm { get; set; }
        public double CubeSize { get; set; }

        public IList<IPanelSection> Sections => (_panelSectionsProxy != null) ? _panelSectionsProxy.Sections : null;
        public IEnumerable<ISectionFace> Faces => Sections.SelectMany(s => s.Faces);
        public IEnumerable<ISectionVolume> Volumes => Sections.Select(s => s.Volume);

        public void Initialize()
        {
            _panelSectionsProxy = new PanelSectionsProxy() { Sections = this.CreateSections(CenterX, CenterY, CenterZ) };
        }

        public void ApplyAction(ToolActionData toolActionData)
        {
            if (this.Intersect(toolActionData))
            {
                _panelSectionsProxy.ApplyAction(toolActionData);
            }
        }

        public Task ApplyActionAsync(ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                if (await this.IntersectAsync(toolActionData))
                {
                    await _panelSectionsProxy.ApplyActionAsync(toolActionData);
                }
            });
        }
    }
}
