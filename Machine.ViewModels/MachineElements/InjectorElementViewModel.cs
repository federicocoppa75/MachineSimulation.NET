using Machine.Data.Base;
using Machine.ViewModels.Insertions;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages;
using Machine.ViewModels.UI;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    [MachineStruct("Injector", 8)]
    public class InjectorElementViewModel : InjectorBaseElementViewModel, IInjectorElement
    {
        public InjectorElementViewModel()
        {
            Messenger.Register<InjectMessage>(this, OnInjectMessage);
        }

        private void OnInjectMessage(InjectMessage msg)
        {
            if(msg.InjectorId == InserterId)
            {
                ExecuteInjection();
            }
        }

        private void ExecuteInjection()
        {
            var sink = GetInstance<IInsertionsSinkProvider>().InsertionsSink;
            var sps = GetInstance<IProgressState>();
            var exe = (sps != null) ? (sps.ProgressDirection == ProgressDirection.Farward) : true;

            if((sink != null) && exe)
            {
                var transformer = GetInstance<IInserterToSinkTransformerFactory>().GetTransformer(sink, this);
                var position = transformer.Transform();
                var injected = new InjectedViewModel()
                {
                    Name = $"Injected({InserterId})",
                    InserterId = InserterId,
                    Color = InserterColor,
                    Position = position.Position,
                    Direction = position.Direction,
                    Index = (sps != null) ? sps.ProgressIndex : -1,
                    Parent = sink
                };

                GetInstance<IDispatcherHelper>().CheckBeginInvokeOnUi(() =>
                {
                    sink.Children.Add(injected);
                });                
            }
        }

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            Messenger.Unregister<InjectMessage>(this);
            base.Dispose(disposing);
        }
        #endregion
    }
}
