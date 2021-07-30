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

        public Task ApplyActionAsync(ToolActionData toolActionData)
        {
            var tasks = new List<Task>();

            foreach (var section in Sections)
            {
                tasks.Add(Task.Run(async () =>
                {
                    if (await section.IntersectAsync(toolActionData))
                    {
                        await section.ApplyActionAsync(toolActionData);
                    }
                }));
            }

            return Task.WhenAll(tasks);
        }

        internal void ApplyAction(ToolSectionApplication toolSectionApplication)
        {
            foreach (var section in Sections)
            {
                if(toolSectionApplication.Intersect(section.GetBound()))
                {
                    section.ApplyAction(toolSectionApplication);
                }
            }
        }

        internal Task ApplyActionAsync(ToolSectionApplication toolSectionApplication)
        {
            var tasks = new List<Task>();

            foreach (var section in Sections)
            {
                tasks.Add(Task.Run(() =>
                {
                    if (toolSectionApplication.Intersect(section.GetBound()))
                    {
                        section.ApplyAction(toolSectionApplication);
                    }
                }));
            }

            return Task.WhenAll(tasks);
        }

        public void RemoveData(int index)
        {
            foreach (var section in Sections) section.RemoveAction(index);
        }

        public Task RemoveActionAsync(int index)
        {
            var tasks = new List<Task>();

            foreach (var section in Sections)
            {
                tasks.Add(section.RemoveActionAsync(index));
            }

            return Task.WhenAll(tasks);
        }
    }
}
