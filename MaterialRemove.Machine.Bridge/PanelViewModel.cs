using System;
using System.Collections.Generic;
using System.Text;
using MVMME = Machine.ViewModels.MachineElements;
using MaterialRemove.Interfaces;
using System.Threading.Tasks;
using MaterialRemove.ViewModels;
using MaterialRemove.ViewModels.Extensions;
using System.Linq;
using MVMI = Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.Data.Base;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.UI;
using MVMIoc = Machine.ViewModels.Ioc;

namespace MaterialRemove.Machine.Bridge
{
    class PanelViewModel : MVMME.PanelViewModel, IPanel, MVMI.IToolsObserver
    {
        private PanelSectionsProxy _panelSectionsProxy;
        private ToolsObserver _toolsObserver;

        public int NumCells { get; set; }
        public int SectionsX100mm { get; set; }
        public double CubeSize { get; set; }

        public IList<IPanelSection> Sections => (_panelSectionsProxy != null) ? _panelSectionsProxy.Sections : null;
        public IEnumerable<ISectionFace> Faces => Sections.SelectMany(s => s.Faces);
        public IEnumerable<ISectionVolume> Volumes => Sections.Select(s => s.Volume);

        public void Initialize()
        {
            _panelSectionsProxy = new PanelSectionsProxy() { Sections = this.CreateSections() };
            _toolsObserver = new ToolsObserver(this);
            (GetInstance<MVMI.IToolObserverProvider>() as ToolsObserverProvider).Observer = this;
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

        public void Register(IToolElement tool) => _toolsObserver.Register(tool);

        public void Unregister(IToolElement tool) => _toolsObserver.Unregister(tool);

        protected override void Dispose(bool disposing)
        {
            (GetInstance<MVMI.IToolObserverProvider>() as ToolsObserverProvider).Observer = null;
            if (_toolsObserver != null) _toolsObserver.Dispose();

            base.Dispose(disposing);
        }

        private void Trace(ToolActionData toolActionData)
        {
            MVMIoc.SimpleIoc<IDispatcherHelper>.GetInstance().CheckBeginInvokeOnUi(() =>
            {
                Children.Add(new DebugElementViewModel(toolActionData.X, toolActionData.Y, toolActionData.Z));
            });
        }
    }
}
