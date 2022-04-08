using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface ICollidableElement : IMachineElement
    {
        /// <summary>
        /// true if the element could be collided
        /// </summary>
        bool IsCollidable { get; set; }

        /// <summary>
        /// index of group of elements that can not collide between them  
        /// but could collide with elements of other groups
        /// </summary>
        int CollidableGroup { get; set; }

        /// <summary>
        /// true if element is collided
        /// </summary>
        public bool IsCollided { get; set; }
    }
}
