using Machine.ViewModels;
using Machine.ViewModels.Base.Implementation;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    internal class ToolholdersViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<IToolholderElement> Toolholders { get; private set; } = new ObservableCollection<IToolholderElement>();

        private IToolholderElement _selected;

        public IToolholderElement Selected
        {
            get => _selected; 
            set => Set(ref _selected, value, nameof(Selected));
        }


        public ToolholdersViewModel() : base()
        {
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var element in elements)
            {
                BrowseElements(element, (e) =>
                {
                    if(e is IToolholderElement th)
                    {
                        Toolholders.Add(th);
                    }
                });
            }
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var element in elements)
            {
                BrowseElements(element, (e) =>
                {
                    if (e is IToolholderElement th)
                    {
                        Toolholders.Remove(th);
                    }
                });
            }
        }

        protected override void Clear() => Toolholders.Clear();

        private void BrowseElements(IMachineElement element, Action<IMachineElement> action) 
        {
            action(element);

            if((element.Children != null) && (element.Children.Count > 0))
            {
                foreach (var child in element.Children)
                {
                    BrowseElements(child, action);
                }
            }
        }
    }
}
