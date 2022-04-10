using System;
using System.Collections.Generic;
using System.Text;

namespace CollisionDectection.ViewModels
{
    internal class CollisionDetectionViewModel
    {
        public Dictionary<int, CollidersGroupViewModel> CollidersGroups { get; private set; } = new Dictionary<int, CollidersGroupViewModel>();

        public void ColliderPositionChanged(object sender, EventArgs e)
        {
            var collider = sender as ColliderViewModel;
            var groupId = collider.GroupId;
            var id = collider.Id;
        }
    }
}
