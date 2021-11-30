using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI;
using Machine.Views.Messages;
using Machine.Views.Messages.Links;
using Machine.Views.ViewModels.MachineElementProxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVMIF = Machine.ViewModels.Interfaces.Factories;

namespace Machine.Views.ViewModels
{
    public class SelectedViewModel : BaseElementsCollectionViewModel, IMachineStructEditor
    {
        private static int _newElementCount;

        private ElementProxyViewModel _selectedProxy;
        public ElementProxyViewModel SelectedProxy 
        {
            get => _selectedProxy;
            set
            {
                var last = _selectedProxy;

                if(Set(ref _selectedProxy, value, nameof(SelectedProxy)))
                {
                    if (last != null) last.Dispose();
                }
            }
        }

        class AddElementCommand : IAddElementCommand
        {
            public string Label { get; set; }
            public ICommand Command { get; set; }
        }

        #region IMachineStructEditor
        private IEnumerable<IAddElementCommand> _addCommands;
        public IEnumerable<IAddElementCommand> AddCommands => _addCommands ?? (_addCommands = CreateAddComands());


        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(() => DeleteCommandImpl(), () => CanExecuteDeleteCommand()));

        private IEnumerable<IAddElementCommand> _addLinkCommands;
        public IEnumerable<IAddElementCommand> AddLinkCommands => _addLinkCommands ?? (_addLinkCommands = CreateAddLinkCommands());

        private ICommand _deleteLinkCommand;
        public ICommand DeleteLinkCommand => _deleteLinkCommand ?? (_deleteLinkCommand = new RelayCommand(() => DeleteLinkCommandImpl(), () => CanExecuteDeleteLinkCommand()));
        #endregion


        public SelectedViewModel()
        {
            Kernel.SelectedChanged += OnSelectedChanged;

            Messenger.Register<UpdateStructByLinkMessage>(this, (m) => UpdateStructByLink());
        }

        #region BaseElementsCollectionViewModel abstracts implementation
        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            NotifyCanExecuteChanged();
        }

        protected override void Clear()
        {
            SelectedProxy = null;
            Kernel.Selected = null;
            NotifyCanExecuteChanged();
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            var s = Kernel.Selected;

            if(s != null)
            {
                while (s.Parent != null) s = s.Parent;

                if (elements.Contains(s))
                {
                    SelectedProxy = null;
                    Kernel.Selected = null;
                }
            }            

            NotifyCanExecuteChanged();
        }
        #endregion

        #region implementation
        private IEnumerable<IAddElementCommand> CreateAddComands()
        {
            var factories = GetInstance<MVMIF.IMachineElementFactoriesProvider>()?.Factories;
            var list = new List<IAddElementCommand>();

            foreach (var item in factories)
            {
                var isRoot = item.IsRoot;

                list.Add(new AddElementCommand()
                {
                    Label = item.Label,
                    Command = new RelayCommand(() => CreateElement(item), () => CanExecuteCreateElement(isRoot))
                });
            }

            return list;
        }

        private void CreateElement(MVMIF.IMachineElementFactory factory)
        {
            var e = factory.Create();
            var s = Kernel.Selected;

            if (string.IsNullOrWhiteSpace(e.Name)) e.Name = $"element {++_newElementCount}";

            if(s != null)
            {
                s.Children.Add(e);
                e.Parent = s;
                Kernel.Selected = e;
                ChangeSelectionState(s, false);
                ChangeSelectionState(e, true);
            }
            else
            {
                Kernel.Machines.Add(e);
                Kernel.Selected = e;
                ChangeSelectionState(e, true);
            }
        }

        private bool CanExecuteCreateElement(bool isRoot)
        {
            if(isRoot)
            {
                return (Kernel.Selected == null) && (Kernel.Machines.Count == 0);
            }
            else
            {
                return (Kernel.Selected != null);
            }
        }

        private void DeleteCommandImpl()
        {
            var selected = Kernel.Selected;

            if (selected != null)
            {
                if (selected.Parent != null)
                {
                    selected.Parent.Children.Remove(selected);
                    Kernel.Selected = selected.Parent;
                    selected.Parent = null;
                }
                else
                {
                    Kernel.Machines.Remove(selected);
                    Kernel.Selected = null;
                }
            }
        }

        private bool CanExecuteDeleteCommand() => Kernel.Selected != null;

        private void OnSelectedChanged(object sender, EventArgs e)
        {
            SelectedProxy = (Kernel.Selected != null) ? GetProxy(Kernel.Selected) : null;
            NotifyCanExecuteChanged();
        }

        private void NotifyCanExecuteChanged()
        {
            (_deleteCommand as RelayCommand)?.RaiseCanExecuteChanged();

            foreach (var item in _addCommands)
            {
                (item.Command as RelayCommand)?.RaiseCanExecuteChanged();
            }

            (_deleteLinkCommand as RelayCommand)?.RaiseCanExecuteChanged();

            foreach (var item in _addLinkCommands)
            {
                (item.Command as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private static void ChangeSelectionState(IMachineElement element, bool state)
        {
            if(element is IViewElementData ved)
            {
                ved.IsSelected = state;
            }
        }

        private static ElementProxyViewModel GetProxy(IMachineElement element)
        {
            ElementProxyViewModel proxy = null;

            if (element is IInserterElement inse) proxy = new InserterProxyViewModel(inse);
            else if (element is IInjectorElement inje) proxy = new InjectorProxyViewModel(inje);
            else if (element is IRootElement re) proxy = new RootProxyViewModel(re);
            else if (element is IColliderElement ce) proxy = new ColliderProxyViewModel(ce);
            else if (element is IPanelholderElement phe) proxy = new PanelHolderProxyViewModel(phe);
            else if (element is IToolholderElement the) proxy = new ToolHolderProxyViewModel(the);
            else proxy = new ElementProxyViewModel(element);

            return proxy;
        }

        private bool CanExecuteAddLinkCommand() => (Kernel.Selected != null) && (Kernel.Selected.LinkToParent == null) && (Kernel.Selected.Parent != null);

        private bool CanExecuteDeleteLinkCommand() => (Kernel.Selected != null) && (Kernel.Selected.LinkToParent != null);

        private void DeleteLinkCommandImpl()
        {
            var link = SelectedProxy.SetLinkProxy(null);
            NotifyCanExecuteChanged();
            Messenger.Send(new DeletedLinkMessage() { Link = link });
            UpdateStructByLink();
        }

        private IEnumerable<IAddElementCommand> CreateAddLinkCommands()
        {
            var factories = GetInstance<MVMIF.ILinkFactoriesProvider>()?.Factories;
            var list = new List<IAddElementCommand>();

            foreach (var item in factories)
            {
                list.Add(new AddElementCommand()
                {
                    Label = item.Label,
                    Command = new RelayCommand(() => CreateLink(item), () => CanExecuteAddLinkCommand())
                });
            }

            return list;
        }

        private void CreateLink(MVMIF.ILinkFactory factory)
        {
            var link = factory.Create();
            SelectedProxy.SetLinkProxy(link);
            NotifyCanExecuteChanged();
            Messenger.Send(new AddedLinkMessage() { Link = link });
            UpdateStructByLink();
        }

        private void UpdateStructByLink()
        {
            var element = Kernel.Selected;

            if((element != null) && (element.Parent != null))
            {
                Kernel.SelectedChanged -= OnSelectedChanged;
                element.Parent.Children.Remove(element);
                element.Parent.Children.Add(element);
                Kernel.Selected = element;
                Kernel.SelectedChanged += OnSelectedChanged;
            }
        }

        #endregion
    }
}
