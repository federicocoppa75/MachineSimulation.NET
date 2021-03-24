using Machine.ViewModels.Base;
using Machine.ViewModels.Messages;
using System.Windows.Input;

namespace Machine.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }

        private ICommand _unloadToolingCommand;
        public ICommand UnloadToolingCommand => _unloadToolingCommand ?? (_unloadToolingCommand = new RelayCommand(() => Messenger.Send(new UnloadAllToolMessage())));

        public MainViewModel()
        {
            Kernel = GetInstance<IKernelViewModel>();
        }
    }
}
