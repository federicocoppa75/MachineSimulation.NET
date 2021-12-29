using Machine.Data.Enums;
using Machine.Data.Interfaces.Factories;
using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Base;
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

        public ToolProxyViewModel(ITool tool)
        {
            _tool = tool;
        }

        public double GetTotalDiameter() => _tool.GetTotalDiameter();

        public double GetTotalLength() => _tool.GetTotalLength();

        public ITool GetTool() => _tool;

        protected T GetTool<T>() where T : class, ITool
        {
            return _tool as T;
        }

        protected void UpdateTool()
        {
            RisePropertyChanged(nameof(TotalDiameter));
            RisePropertyChanged(nameof(TotalLength));

            Messenger.Send(new UnloadToolMessage());
            Messenger.Send(new LoadToolMessage() { Tool = _tool });
        }

        protected static T CreateTool<T>() where T : ITool
        {
            return Machine.ViewModels.Ioc.SimpleIoc<IToolFactory>.GetInstance().Create<T>();
        }

        protected abstract string GetToolType();
    }
}
