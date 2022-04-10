using System;
using System.Collections.Generic;
using System.Text;

namespace CollisionDectection.ViewModels
{
    internal class CollidersGroupViewModel
    {
        public int GroupId { get; set; }
        public List<ColliderViewModel> Colliders { get; private set; } = new List<ColliderViewModel>();
    }
}
