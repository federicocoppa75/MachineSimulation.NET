using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Base;
using Machine.Views.Messages;
using Machine.Views.Messages.ToolsEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString() => $"{X}; {Y}; {Z}";
    }

    internal interface ISubSpindleObserver
    {
        void NotifySubSpindleChanged();
    }

    internal class ATSubSpindleProxyViewModel : BaseViewModel
    {
        ISubSpindleObserver _observer;
        ISubspindle _subSpindle;

        [PropertyOrder(1)]
        [Editor(typeof(PropertyGridToolSelectionCombo), typeof(PropertyGridToolSelectionCombo))]
        public string ToolName
        {
            get => _subSpindle.ToolName;
            set
            {
                if(string.Compare(_subSpindle.ToolName, value) != 0)
                {
                    var ss = _subSpindle as ISubspindleEx;
                    var t = GetTool(value);

                    ss.SetTool(t);
                    RisePropertyChanged(nameof(ToolName));
                    Update();
                }
            }
        }

        [PropertyOrder(2)]
        [ExpandableObject]
        public Vector Position
        {
            get
            {
                _subSpindle.GetPosition(out double x, out double y, out double z);
                return new Vector() { X = x, Y = y, Z = z };
            }
            set
            {
                if (SetVector(Position, value, v => _subSpindle.SetPosition(v.X, v.Y, v.Z), nameof(Position))) Update();
            }
        }

        [PropertyOrder(3)]
        [ExpandableObject]
        public Vector Direction
        {
            get
            {
                _subSpindle.GetDirection(out double x, out double y, out double z);
                return new Vector() { X = x, Y = y, Z = z };
            }
            set
            {
                if (SetVector(Direction, value, v => _subSpindle.SetDirection(v.X, v.Y, v.Z), nameof(Direction))) Update();
            }
        }

        public ATSubSpindleProxyViewModel(ISubspindle subSpindle, ISubSpindleObserver observer)
        {
            _subSpindle = subSpindle;
            _observer = observer;

            Messenger.Register<ToolDeletedMessage>(this, OnToolDeletedMessage);
        }

        override public string ToString() => _subSpindle.ToolName;

        private void Update() => _observer?.NotifySubSpindleChanged();

        private bool SetVector(Vector v1, Vector v2, Action<Vector> updateAction, string changedPropertyName)
        {
            bool changed = false;

            if((v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Z != v2.Z))
            {
                updateAction?.Invoke(v2);
                RisePropertyChanged(changedPropertyName);
                changed = true;
            }

            return changed;
        }

        private ITool GetTool(string toolName)
        {
            ITool tool = null;

            Messenger.Send(new ToolsRequestMessage()
            {
                SetTools = (tools) =>
                {
                    var t = tools.FirstOrDefault(o => string.Compare(o.Name, toolName) == 0);

                    if (t != null) tool = t;
                }
            });

            return tool;
        }

        private void OnToolDeletedMessage(ToolDeletedMessage msg)
        {
            if(!string.IsNullOrEmpty(ToolName) && (string.Compare(ToolName, msg.ToolName) == 0))
            {
                ToolName = null;
            }
        }
    }
}
