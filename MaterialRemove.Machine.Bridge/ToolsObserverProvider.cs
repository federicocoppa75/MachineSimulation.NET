using Machine.ViewModels.Interfaces.Tools;

namespace MaterialRemove.Machine.Bridge
{
    public class ToolsObserverProvider : IToolObserverProvider
    {
        public IToolsObserver Observer { get; set; }
    }
}
