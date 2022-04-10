using System;
using System.Collections.Generic;
using System.Text;
using MVMIM = Machine.ViewModels.Interfaces.MachineElements;
using MVMB = Machine.ViewModels.Base;
using MVMIL = Machine.ViewModels.Interfaces.Links;

namespace CollisionDectection.ViewModels
{
    internal class ColliderViewModel : MVMB.BaseViewModel
    {
        private static int _seedId;

        public int Id { get; private set; }
        public int GroupId { get; set; }

        private MVMIM.ICollidableElement _element;
        public MVMIM.ICollidableElement Element 
        { 
            get => _element; 
            set
            {
                var last = _element;

                if(Set(ref _element, value, nameof(Element)))
                {
                    if (last != null) Detach(last);
                    if (_element != null) Attach(_element);
                }
            }
        }

        public event EventHandler PositionChanged;

        public ColliderViewModel()
        {
            Id = _seedId++;
        }

        private void Attach(MVMIM.ICollidableElement element)
        {
            IterateElementChain(element, (link) => link.ValueChanged += OnLinkValueChanged);
        }

        private void Detach(MVMIM.ICollidableElement element)
        {
            IterateElementChain(element, (link) => link.ValueChanged -= OnLinkValueChanged);
        }

        private void IterateElementChain(MVMIM.IMachineElement element, Action<MVMIL.ILinkViewModel> action)
        {
            MVMIM.IMachineElement e = element;

            while (e != null)
            {
                if(e.LinkToParent != null)
                {
                    action(e.LinkToParent);
                }

                e = e.Parent;
            }
        }

        private void OnLinkValueChanged(object sender, double e)
        {
            if(PositionChanged != null) PositionChanged(this, new EventArgs());
        }
    }
}
