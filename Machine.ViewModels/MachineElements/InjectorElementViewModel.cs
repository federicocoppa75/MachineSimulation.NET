using Machine.Data.Base;
using Machine.ViewModels.Insertions;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
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

            if(sink != null)
            {
                var transformer = GetInstance<IInserterToSinkTransformerFactory>().GetTransformer(sink, this);
                var position = transformer.Transform();
                var injected = new InjectedViewModel()
                {
                    Name = $"Injected({InserterId})",
                    InserterId = InserterId,
                    Color = InserterColor,
                    Position = position.Position,
                    Direction = position.Direction
                };

                sink.Children.Add(injected);
            }
        }
    }
}
