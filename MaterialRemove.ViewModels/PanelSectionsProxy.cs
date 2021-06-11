using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    public class PanelSectionsProxy
    {
        public IList<IPanelSection> Sections { get; set; }

        public void ApplyAction(ToolActionData toolActionData)
        {
            foreach (var section in Sections)
            {
                if(section.Intersect(toolActionData))
                {
                    section.ApplyAction(toolActionData);
                }
            }
        }
    }
}
