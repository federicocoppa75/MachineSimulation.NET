using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IStepsController
    {
        bool AutoStepOver { get; set; }
        bool DynamicTransition { get; set; }
        bool MaterialRemoval { get; set; }
        bool MultiChannel { get; set; }
        //ICommand ExportPanelCommand { get; }
        string FileOpened { get; }
        ICommand LoadStepsCommand { get; }
        ICommand UnloadStepsCommand { get; }
    }
}