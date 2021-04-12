using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class AngularTransmission : Tool
    {
        public string BodyModelFile { get; set; }
        public virtual ICollection<Subspindle> Subspindles { get; protected set; } = new List<Subspindle>();

        public override double GetTotalDiameter() => -1.0;

        public override double GetTotalLength() => -1.0;
    }
}
