using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    public class PanelSectionsProxy
    {
        protected IProgressState _stateProgressState;

        public IList<IPanelSection> Sections { get; set; }

        public PanelSectionsProxy()
        {
            _stateProgressState = Machine.ViewModels.Ioc.SimpleIoc<IProgressState>.HasInstance() ? Machine.ViewModels.Ioc.SimpleIoc<IProgressState>.GetInstance() : null;
        }

         public void ApplyAction(IPanel panel, ToolActionData toolActionData)
        {
            var ta = toolActionData.ToApplication(GetIndex());

            if(ta.Intersect(panel))
            {
                ApplyAction(ta);
            }
        }

        public Task ApplyActionAsync(IPanel panel, ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                var ta = toolActionData.ToApplication(GetIndex());

                if(await Task.Run(() => ta.Intersect(panel)))
                {
                    await ApplyActionAsync(ta);
                }
            });
        }

        public void ApplyAction(IPanel panel, ToolSectionActionData toolSectionActionData)
        {
            var ta = toolSectionActionData.ToApplication(GetIndex());

            if (ta.Intersect(panel))
            {
                ApplyAction(ta);
            }
        }

        public Task ApplyActionAsync(IPanel panel, ToolSectionActionData toolSectionActionData)
        {
            return Task.Run(async () =>
            {
                var ta = toolSectionActionData.ToApplication(GetIndex());

                if (await Task.Run(() => ta.Intersect(panel)))
                {
                    await ApplyActionAsync(ta);
                }
            });
        }

        public void ApplyAction(IPanel panel, ToolConeActionData toolConeActionData)
        {
            var ta = toolConeActionData.ToApplication(GetIndex());

            if (ta.Intersect(panel))
            {
                ApplyAction(ta);
            }
        }

        public Task ApplyActionAsync(IPanel panel, ToolConeActionData toolConeActionData)
        {
            return Task.Run(async () =>
            {
                var ta = toolConeActionData.ToApplication(GetIndex());

                if (await Task.Run(() => ta.Intersect(panel)))
                {
                    await ApplyActionAsync(ta);
                }
            });
        }

        private void ApplyAction<T>(T toolApplication) where T : g3.BoundedImplicitFunction3d, IIntersector, IIndexed
        {
 
            foreach (var section in Sections)
            {
                if (toolApplication.Intersect(section))
                {
                    foreach (var face in section.Faces)
                    {
                        if (toolApplication.Intersect(face))
                        {
                            if (face is SectionElementViewModel sfvm)
                            {
                                sfvm.ApplyAction(toolApplication);
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }

                    if (section.Volume is SectionVolumeViewModel svvm)
                    {
                        svvm.ApplyAction(toolApplication);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        private Task ApplyActionAsync<T>(T toolApplication) where T : g3.BoundedImplicitFunction3d, IIntersector, IIndexed
        {
            var tasks = new List<Task>();

            foreach (var section in Sections)
            {
                tasks.Add(Task.Run(async () =>
                {
                    if (await Task.Run(() => toolApplication.Intersect(section)))
                    {
                        var tt = new Task[]
                        {
                            ApplyActionToFacesAsync(section, toolApplication),
                            ApplyActionToVolumeAsync(section, toolApplication)
                        };

                        await Task.WhenAll(tt);
                    }
                }));
            }

            return Task.WhenAll(tasks);
        }

        private Task ApplyActionToFacesAsync<T>(IPanelSection section, T toolApplication) where T : g3.BoundedImplicitFunction3d, IIntersector
        {
            var tasks = new List<Task<bool>>();

            foreach (var face in section.Faces)
            {
                tasks.Add(Task.Run(async () =>
                {
                    if (await Task.Run(() => toolApplication.Intersect(face)))
                    {
                        if (face is SectionElementViewModel sevm)
                        {
                            await sevm.ApplyActionAsync(toolApplication);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                    return true;
                }));
            }

            return Task.WhenAll(tasks);
        }

        private Task ApplyActionToVolumeAsync<T>(IPanelSection section, T toolApplication) where T : g3.BoundedImplicitFunction3d/*, IIntersector*/, IIndexed
        {
            return Task.Run(async () =>
            {
                if (section.Volume is SectionElementViewModel sevm)
                {
                    await sevm.ApplyActionAsync(toolApplication);
                }
                else
                {
                    throw new NotImplementedException();
                }
            });
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

        private int GetIndex() => (_stateProgressState != null) ? _stateProgressState.ProgressIndex : -1;
    }
}
