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
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }

        public IList<IPanelSection> Sections => (_panelSectionsProxy != null) ? _panelSectionsProxy.Sections : null;

        public void Initialize()
        {
            _panelSectionsProxy = new PanelSectionsProxy() { Sections = this.CreateSections() };
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
                if(await this.IntersectAsync(toolActionData))
                {
                    await _panelSectionsProxy.ApplyActionAsync(toolActionData);
                }
            });
        }
    }
}
