using CollisionDectection.Interfaces;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MVMI = Machine.ViewModels.Interfaces;
using MVMIM = Machine.ViewModels.Interfaces.MachineElements;

namespace CollisionDectection.ViewModels
{
    public class CollisionDetectionManager : BaseViewModel,  ICollisionDetectionData
    {
        private MVMI.IKernelViewModel _kernel;
        private CollisionDetectionViewModel _collisionDetection;

        private bool _enable;
        public bool Enable
        {
            get => _enable;
            set
            {
                if (Set(ref _enable, value, nameof(Enable)))
                {
                    if (_enable)
                    {
                        InitializKernel();
                        AttachCollisionDetector();
                    }
                    else
                    {
                        DetachCollisionDetector();
                    }
                }
            }
        }

        private void InitializKernel()
        {
            if(_kernel == null)
            {
                _kernel = GetInstance<MVMI.IKernelViewModel>();
                _kernel.MachinesCollectionChanged += OnKernelMachineCollectionChanged;
            }
        }

        private void OnKernelMachineCollectionChanged(object sender, EventArgs e)
        {
            if(_kernel.Machines.Count == 0)
            {
                DetachCollisionDetector();
            }
            else if(_kernel.Machines.Count == 1)
            {
                AttachCollisionDetector();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void AttachCollisionDetector()
        {
            _collisionDetection = new CollisionDetectionViewModel();

            foreach(var machine in _kernel.Machines)
            {
                AttachCollisionDetector(machine);
            }
        }

        private void AttachCollisionDetector(MVMIM.IMachineElement element)
        {
            var collider = element as MVMIM.ICollidableElement;

            if((collider != null) && (collider.IsCollidable))
            {
                var groupId = collider.CollidableGroup;

                if (!_collisionDetection.CollidersGroups.ContainsKey(groupId))
                {
                    _collisionDetection.CollidersGroups.Add(groupId, 
                        new CollidersGroupViewModel() 
                        { 
                            GroupId = groupId 
                        });
                }

                var cvm = new ColliderViewModel()
                {
                    Element = collider,
                    GroupId = groupId
                };

                cvm.PositionChanged += _collisionDetection.ColliderPositionChanged;
                _collisionDetection.CollidersGroups[groupId].Colliders.Add(cvm);
            }

            foreach (var item in element.Children)
            {
                AttachCollisionDetector(item);
            }
        }

        private void DetachCollisionDetector()
        {
            if(_collisionDetection != null)
            {
                foreach(var group in _collisionDetection.CollidersGroups.Values)
                {
                    foreach (var item in group.Colliders)
                    {
                        item.PositionChanged -= _collisionDetection.ColliderPositionChanged;
                        item.Element = null;
                    }

                    group.Colliders.Clear();
                }

                _collisionDetection.CollidersGroups.Clear();
                _collisionDetection = null;
            }
        }
    }
}
