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
using Machine.ViewModels.Interfaces;

namespace MaterialRemove.Machine.Bridge
{
    class PanelViewModel : MVMME.PanelViewModel, IPanel, MVMI.IToolsObserver
    {
        private PanelSectionsProxy _panelSectionsProxy;
        private ToolsObserver _toolsObserver;
        private IProgressState _stepsProgressState;

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
            _stepsProgressState = GetInstance<IProgressState>();

            if(_stepsProgressState != null) _stepsProgressState.ProgressIndexChanged += OnProgressIndexChanged;
        }

        public void ApplyAction(ToolActionData toolActionData)
        {
            if ((_stepsProgressState != null) && (_stepsProgressState.ProgressDirection == ProgressDirection.Back)) return;

            if (this.Intersect(toolActionData))
            {
                _panelSectionsProxy.ApplyAction(toolActionData);
            }
        }

        public Task ApplyActionAsync(ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                if ((_stepsProgressState != null) && (_stepsProgressState.ProgressDirection == ProgressDirection.Back)) return;

                if (await this.IntersectAsync(toolActionData))
                {
                    await _panelSectionsProxy.ApplyActionAsync(toolActionData);
                }
            });
        }

        private void OnProgressIndexChanged(object sender, int e)
        {
            if(_stepsProgressState.ProgressDirection == ProgressDirection.Back)
            {
                //_panelSectionsProxy.RemoveData(e);
                _panelSectionsProxy.RemoveActionAsync(e);
                RemoveIndexedChildrenAsyn(e);
            }
        }

        private void RemoveIndexedChildren(int index)
        {
            var items = new List<IMachineElement>();

            foreach (var item in Children)
            {
                if((item is IIndexed idx) && (idx.Index == index)) items.Add(item);
            }

            if (items.Count() > 0)
            {
                foreach (var item in items)
                {
                    MVMIoc.SimpleIoc<IDispatcherHelper>.GetInstance().CheckBeginInvokeOnUi(() =>
                    {
                        Children.Remove(item);
                    });
                }
            }
        }

        private Task RemoveIndexedChildrenAsyn(int index)
        {
            return Task.Run(() => RemoveIndexedChildren(index));
        }

        public void Register(IToolElement tool) => _toolsObserver.Register(tool);

        public void Unregister(IToolElement tool) => _toolsObserver.Unregister(tool);

        protected override void Dispose(bool disposing)
        {
            (GetInstance<MVMI.IToolObserverProvider>() as ToolsObserverProvider).Observer = null;
            if (_toolsObserver != null) _toolsObserver.Dispose();
            if (_stepsProgressState != null) _stepsProgressState.ProgressIndexChanged -= OnProgressIndexChanged;
            _stepsProgressState = null;

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
