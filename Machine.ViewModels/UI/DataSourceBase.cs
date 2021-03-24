using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public abstract class DataSourceBase : BaseViewModel, IDataSource
    {
        public abstract string Name { get; }

        private ICommand _loadMachineCommand;
        public ICommand LoadMachineCommand => _loadMachineCommand ?? (_loadMachineCommand = new RelayCommand(() => LoadMachineCommandImplementation()));

        private ICommand _loadToolingCommand;
        public ICommand LoadToolingCommand => _loadToolingCommand ?? (_loadToolingCommand = new RelayCommand(() => LoadToolingCommandImplementation()));

        protected abstract void LoadMachineCommandImplementation();
        protected abstract void LoadToolingCommandImplementation();

        public override string ToString() => Name;
    }
}
