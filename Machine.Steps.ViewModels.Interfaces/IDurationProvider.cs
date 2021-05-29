using MachineSteps.Models.Actions;

namespace Machine.Steps.ViewModels.Interfaces
{
    public interface IDurationProvider
    {
        double GetDuration(BaseAction action);
    }
}
