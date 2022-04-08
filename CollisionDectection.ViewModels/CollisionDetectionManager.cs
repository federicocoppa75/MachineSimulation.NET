using CollisionDectection.Interfaces;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollisionDectection.ViewModels
{
    public class CollisionDetectionManager : BaseViewModel,  ICollisionDetectionData
    {
        private bool _enable;
        public bool Enable
        {
            get => _enable;
            set
            {
                if (Set(ref _enable, value, nameof(Enable)))
                {
                    if (_enable) AttachCollisionDetector();
                    else DetachCollisionDetector();
                }
            }
        }

        private void AttachCollisionDetector()
        {
        }

        private void DetachCollisionDetector()
        {
        }
    }
}
