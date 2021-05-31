using Machine.Steps.ViewModels.Interfaces;
using Machine.Steps.ViewModels.Interfaces.Models;
using Machine.Steps.ViewModels.Messages;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Messages;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels
{
    public class ActionViewModel : BaseViewModel
    {
        static int _idSeed = 1;

        public int Id { get; private set; }

        public BaseAction Action { get; private set; }

        private bool _durationIsValid;
        private double _duration;

        public double Duration
        {
            get
            {
                if (!_durationIsValid) InitDuration();
                return _duration;
            }
        }

        public bool IsCompleted { get; set; }

        public ActionViewModel(BaseAction action)
        {
            Id = _idSeed++;
            Action = action;

            Messenger.Register<ActionExecutedMessage>(this, OnActionExecutedMessage);
        }

        private void InitDuration()
        {
            _duration = GetInstance<IDurationProvider>().GetDuration(Action);
            _durationIsValid = true;
        }

        public void Execute(bool notifyExecution = false) => GetInstance<IActionExecuter>().Execute(Action, notifyExecution ? Id : 0);

        public void UpdateLazy()
        {
            if ((Action is ILazyAction lazyAction) && !lazyAction.IsUpdated) lazyAction.Update();
        }

        private void OnActionExecutedMessage(ActionExecutedMessage msg)
        {
            if(msg.Id == Id)
            {
                IsCompleted = true;
                Messenger.Send(new ActionCompletedMessage());
            }
        }
    }
}
