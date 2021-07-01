using Machine.ViewModels.Base;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    class InjectorViewModel : BaseViewModel
    {
        public int Id { get; set; }

        private ICommand _inject;
        public ICommand Inject => _inject ?? (_inject = new RelayCommand(() => InjectImpl()));

        private void InjectImpl() => Messenger.Send(new InjectMessage() { InjectorId = Id });
    }
}
