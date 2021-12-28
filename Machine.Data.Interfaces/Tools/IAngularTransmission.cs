using System.Collections.Generic;

namespace Machine.Data.Interfaces.Tools
{
    public interface IAngularTransmission : ITool
    {
        string BodyModelFile { get; }

        IEnumerable<ISubspindle> GetSubspindles();
    }
}
