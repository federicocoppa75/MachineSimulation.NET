using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Insertions
{
    public interface IInserterToSinkTransformerFactory
    {
        IInserterToSinkTransformer GetTransformer(IInsertionsSink sink, IInjectorElement injector);
    }
}
