using System.Collections.Generic;

namespace Machine.Data.Interfaces.Tools
{
    public interface IAngularTransmission : ITool
    {
        string BodyModelFile { get; set; }

        IEnumerable<ISubspindle> GetSubspindles();
        void SetSubSpindlesNumber(int subspindlesNumber);
    }
}
