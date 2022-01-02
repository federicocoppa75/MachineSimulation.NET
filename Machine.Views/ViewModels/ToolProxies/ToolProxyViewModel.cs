using Machine.Data.Enums;
using Machine.Data.Interfaces.Factories;
using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.Messages.Tooling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal abstract class ToolProxyViewModel : BaseViewModel
    {
        protected static int _newIdx;

        private ITool _tool;

        [Category("General")]
        [PropertyOrder(0)]
        public string ToolType => GetToolType();

        [Category("General")]
        [PropertyOrder(1)]
        public string Name
        {
            get => _tool.Name;
            set => Set(_tool.Name, value, v => _tool.Name = v, nameof(Name));
        }

        [Category("General")]
        [PropertyOrder(2)]
        public string Description 
        { 
            get => _tool.Description; 
            set => Set(_tool.Description, value, v => _tool.Description = v, nameof(Description)); 
        }

        [Category("General")]
        [PropertyOrder(3)]
        public ToolLinkType ToolLinkType 
        { 
            get => _tool.ToolLinkType; 
            set => Set(_tool.ToolLinkType, value, v => _tool.ToolLinkType = v, nameof(ToolLinkType)); 
        }

        [Category("General")]
        [PropertyOrder(4)]
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string ConeModelFile 
        { 
            get => _tool.ConeModelFile; 
            set
            {
                if(Set(_tool.ConeModelFile, value, v => _tool.ConeModelFile = v, nameof(ConeModelFile)))
                {
                    UpdateTool();
                }
            }
        }

        [Category("General")]
        [PropertyOrder(5)]
        public double TotalDiameter => GetTotalDiameter();

        [Category("General")]
        [PropertyOrder(6)]
        public double TotalLength => GetTotalLength();

        [Browsable(false)]
        public virtual bool IsAngolarTransmission => false;

        public ToolProxyViewModel(ITool tool)
        {
            _tool = tool;
        }

        public double GetTotalDiameter() => _tool.GetTotalDiameter();

        public double GetTotalLength() => _tool.GetTotalLength();

        public ITool GetTool() => _tool;

        public void Unload() => Messenger.Send(new UnloadToolMessage());

        public void Load()
        {
            if(IsAngolarTransmission)
            {
                var at = GetTool<IAngularTransmission>();

                Messenger.Send(new AngularTransmissionLoadMessage() 
                { 
                    AngularTransmission = at,
                    AppendSubSpindle = (addSubSpindle) =>
                    {
                        foreach (var item in at.GetSubspindles())
                        {
                            var tool = ((item is ISubspindleEx sse) && (sse.GetTool() != null)) ? sse.GetTool() : null;
                            addSubSpindle(item.Position(), item.Direction(), tool);
                        }
                    }
                });
            }
            else
            {
                Messenger.Send(new LoadToolMessage() { Tool = _tool });
            }
        }

        public abstract ToolProxyViewModel CreateCopy();

        protected T GetTool<T>() where T : class, ITool
        {
            return _tool as T;
        }

        protected void UpdateTool()
        {
            RisePropertyChanged(nameof(TotalDiameter));
            RisePropertyChanged(nameof(TotalLength));

            Unload();
            Load();
        }

        protected static T CreateTool<T>() where T : ITool
        {
            return Machine.ViewModels.Ioc.SimpleIoc<IToolFactory>.GetInstance().Create<T>();
        }

        protected abstract string GetToolType();

        protected bool ProcessDiameterEx(IToolDimension dimension, double distanceFromZero, double distanceFromContact, double diameter)
        {
            dimension.ContactPoint1 = new Data.Base.Point() { X = diameter / 2.0, Y = 0.0, Z = -distanceFromZero };
            dimension.ContactPoint2 = new Data.Base.Point() { X = -diameter / 2.0, Y = 0.0, Z = -distanceFromZero };
            dimension.MeasurePoint1 = new Data.Base.Point() { X = diameter / 2.0, Y = 0.0, Z = -(distanceFromZero + distanceFromContact) };
            dimension.MeasurePoint2 = new Data.Base.Point() { X = -diameter / 2.0, Y = 0.0, Z = -(distanceFromZero + distanceFromContact) };

            return true;
        }

        protected bool ProcessDiameter(IToolDimension dimension, double distanceFromZero, double distanceFromContact, double diameter)
        {
            dimension.ContactPoint1 = new Data.Base.Point() { X = diameter / 2.0, Y = 0.0, Z = -distanceFromZero };
            dimension.ContactPoint2 = new Data.Base.Point() { X = -diameter / 2.0, Y = 0.0, Z = -distanceFromZero };
            dimension.MeasurePoint1 = new Data.Base.Point() { X = diameter / 2.0, Y = diameter / 2.0 + distanceFromContact, Z = -distanceFromZero };
            dimension.MeasurePoint2 = new Data.Base.Point() { X = -diameter / 2.0, Y = diameter / 2.0 + distanceFromContact, Z = -distanceFromZero };

            return true;
        }

        protected bool ProcessLength(IToolDimension dimension, double distanceFromCenter, double distanceFromContact, double startLength, double endLength)
        {
            dimension.ContactPoint1 = new Data.Base.Point() { X = distanceFromCenter, Y = 0, Z = -startLength };
            dimension.ContactPoint2 = new Data.Base.Point() { X = distanceFromCenter, Y = 0, Z = -endLength };
            dimension.MeasurePoint1 = new Data.Base.Point() { X = distanceFromCenter + distanceFromContact, Y = 0, Z = -startLength };
            dimension.MeasurePoint2 = new Data.Base.Point() { X = distanceFromCenter + distanceFromContact, Y = 0, Z = -endLength };

            return true;
        }

        protected bool ProcessRadialDimension(IToolDimension dimension, double distanceFromCenter, double dimensionValue, double distanceFromZero, double distanceFromContact)
        {
            dimension.ContactPoint1 = new Data.Base.Point() { X = distanceFromCenter, Y = 0, Z = -distanceFromZero };
            dimension.ContactPoint2 = new Data.Base.Point() { X = distanceFromCenter + dimensionValue, Y = 0, Z = -distanceFromZero };
            dimension.MeasurePoint1 = new Data.Base.Point() { X = distanceFromCenter, Y = 0, Z = -(distanceFromZero + distanceFromContact) };
            dimension.MeasurePoint2 = new Data.Base.Point() { X = distanceFromCenter + dimensionValue, Y = 0, Z = -(distanceFromZero + distanceFromContact) };

            return true;
        }
    }
}
