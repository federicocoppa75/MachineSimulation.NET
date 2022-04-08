using Machine.Data.Links;
using System.Collections.Generic;

namespace Machine.Data.MachineElements
{
    public class MachineElement
    {
        public int MachineElementID { get; set; }
        public string Name { get; set; }
        public string ModelFile { get; set; }
        public virtual Color Color { get; set; }
        public virtual Matrix Transformation { get; set; }
        public virtual ICollection<MachineElement> Children { get; protected set; } = new List<MachineElement>();
        public virtual Link LinkToParent { get; set; }
        public bool IsCollidable { get; set; }
        public int CollidableGroup { get; set; }
    }
}
