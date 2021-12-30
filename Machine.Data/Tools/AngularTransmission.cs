using Machine.Data.Interfaces.Tools;
using System.Collections.Generic;

namespace Machine.Data.Tools
{
    public class AngularTransmission : Tool, IAngularTransmission
    {
        public string BodyModelFile { get; set; }
        public virtual ICollection<Subspindle> Subspindles { get; protected set; } = new List<Subspindle>();

        public override double GetTotalDiameter() => -1.0;

        public override double GetTotalLength() => -1.0;

        public IEnumerable<ISubspindle> GetSubspindles() => Subspindles;

        public void SetSubSpindlesNumber(int subspindlesNumber)
        {
            Subspindles.Clear();

            for (int i = 0; i < subspindlesNumber; i++) Subspindles.Add(new SubspindleEx() { Direction = new MachineElements.Vector(), Position = new MachineElements.Point() });
        }
    }
}
