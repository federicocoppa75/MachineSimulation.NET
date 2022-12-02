using Machine.ViewModels.Insertions;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.UI;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    [MachineStruct("Inserter", 9)]
    public class InserterElementViewModel : InjectorBaseElementViewModel, IInserterElement
    {
        private double _diameter;
        public double Diameter 
        { 
            get => _diameter; 
            set => Set(ref _diameter, value, nameof(Diameter)); 
        }

        private double _length;
        public double Length 
        { 
            get => _length; 
            set => Set(ref _length, value, nameof(Length)); 
        }

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
                GetInstance<IDispatcherHelper>().CheckBeginInvokeOnUi(() =>
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
                });
            }
        }

        private void OnDischargerLinkStateChanged(object sender, bool e)
        {
            if(e && (Children.Count > 0))
            {
                var sink = GetInstance<IInsertionsSinkProvider>().InsertionsSink;
                var sps = GetInstance<IProgressState>();
                var exe = (sps != null) ? (sps.ProgressDirection == ProgressDirection.Farward) : true;

                if ((sink != null) && exe)
                {
                    var transformer = GetInstance<IInserterToSinkTransformerFactory>().GetTransformer(sink, this);
                    var position = transformer.Transform();

                    GetInstance<IDispatcherHelper>().CheckBeginInvokeOnUi(() =>
                    {
                        sink.Children.Add(new InsertedViewModel()
                        {
                            Name = $"Inserted({InserterId})",
                            InserterId = InserterId,
                            Position = position.Position,
                            Direction = position.Direction,
                            Length = Length,
                            Diameter = Diameter,
                            Color = InserterColor,
                            Index = (sps != null) ? sps.ProgressIndex : -1,
                            Parent = sink
                        });
                    });                    
                }

                Children.Clear();
            }
        }
    }
}
