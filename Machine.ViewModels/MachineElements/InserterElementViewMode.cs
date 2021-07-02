using Machine.ViewModels.Insertions;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class InserterElementViewModel : InjectorBaseElementViewModel, IInserterElement
    {
        public double Diameter { get; set; }
        public double Length { get; set; }

        private int _loaderLinkId;
        public int LoaderLinkId 
        { 
            get => _loaderLinkId; 
            set
            {
                if(Set(ref _loaderLinkId, value, nameof(LoaderLinkId)) && (_loaderLinkId != 0))
                {
                    Messenger.Send(new GetLinkMessage() 
                    { 
                        Id = _loaderLinkId,
                        SetLink = (link) =>
                        {
                            (link as IPneumaticLinkViewModel).StateChangeCompleted += OnLoaderLinkStateChanged;
                        }
                    });
                }
            }
        }

        private int _dischargerLinkId;
        public int DischargerLinkId 
        { 
            get => _dischargerLinkId; 
            set
            {
                if (Set(ref _dischargerLinkId, value, nameof(DischargerLinkId)) && (_dischargerLinkId != 0))
                {
                    Messenger.Send(new GetLinkMessage()
                    {
                        Id = _dischargerLinkId,
                        SetLink = (link) =>
                        {
                            (link as IPneumaticLinkViewModel).StateChangeCompleted += OnDischargerLinkStateChanged;
                        }
                    });
                }
            }
        }

        private void OnLoaderLinkStateChanged(object sender, bool e)
        {
            if(e)
            {
                Children.Add(new InsertedViewModel()
                {
                    Name = $"Inserted({InserterId})",
                    InserterId = InserterId,
                    Position = Position,
                    Direction = Direction,
                    Length = Length,
                    Diameter = Diameter,
                    Color = InserterColor
                });
            }
        }

        private void OnDischargerLinkStateChanged(object sender, bool e)
        {
            if(e && (Children.Count > 0))
            {
                var sink = GetInstance<IInsertionsSinkProvider>().InsertionsSink;

                if (sink != null)
                {
                    var transformer = GetInstance<IInserterToSinkTransformerFactory>().GetTransformer(sink, this);
                    var position = transformer.Transform();
                    var inserted = Children.First() as InsertedViewModel;

                    Children.Remove(inserted);
                    inserted.Position = position.Position;
                    inserted.Direction = position.Direction;
                    sink.Children.Add(inserted);
                }
            }
        }
    }
}
