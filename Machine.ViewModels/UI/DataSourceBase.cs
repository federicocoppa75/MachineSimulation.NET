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
        
        private ICommand _saveMachineCommand;
        public ICommand SaveMachineCommand => _saveMachineCommand ?? (_saveMachineCommand = new RelayCommand(() => SaveMachineCommandImplementation(), () => SaveMachineCommandCanExecute()));

        private ICommand _loadToolingCommand;
        public ICommand LoadToolingCommand => _loadToolingCommand ?? (_loadToolingCommand = new RelayCommand(() => LoadToolingCommandImplementation()));

        private ICommand _loadEnvironmentCommand;
        public ICommand LoadEnvironmentCommand => _loadEnvironmentCommand ?? (_loadEnvironmentCommand = new RelayCommand(() => LoadEnvironmentCommandImplementation(), () => LoadEnvironmentCommandCanExecute()));

        private ICommand _saveEnvironmentCommand;
        public ICommand SaveEnvironmentCommand => _saveEnvironmentCommand ?? (_saveEnvironmentCommand = new RelayCommand(() => SaveEnvironmentCommandImplementation(), () => SaveEnvironmentCommandCanExecute()));

        protected abstract void LoadMachineCommandImplementation();

        protected virtual void SaveMachineCommandImplementation() { }
        protected virtual bool SaveMachineCommandCanExecute() => false;

        protected abstract void LoadToolingCommandImplementation();

        public override string ToString() => Name;

        protected virtual void LoadEnvironmentCommandImplementation() { }

        protected virtual bool LoadEnvironmentCommandCanExecute() => false;

        protected virtual void SaveEnvironmentCommandImplementation() { }

        protected virtual bool SaveEnvironmentCommandCanExecute() => false;
    }
}
