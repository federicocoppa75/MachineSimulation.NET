using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Indicators;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages;
using Machine.ViewModels.UI;
using Machine.ViewModels.UI.Attributes;
using MVMIF = Machine.ViewModels.Interfaces.Factories;

namespace Machine.ViewModels.MachineElements
{
    [MachineStruct("Panel holder", 2)]
    public class PanelHolderElementViewModel : ElementViewModel, IPanelholderElement, IPositionIndicator
    {
        private int _panelHolderId;
        public int PanelHolderId 
        { 
            get => _panelHolderId; 
            set => Set(ref _panelHolderId, value, nameof(PanelHolderId)); 
        }

        private string _panelHolderName;
        public string PanelHolderName 
        { 
            get => _panelHolderName; 
            set => Set(ref _panelHolderName, value, nameof(PanelHolderName)); 
        }

        private Point _position;
        public virtual Point Position 
        { 
            get => _position; 
            set => Set(ref _position, value, nameof(Position)); 
        }

        private PanelLoadType _corner;
        public PanelLoadType Corner 
        { 
            get => _corner; 
            set => Set(ref _corner, value, nameof(Corner)); 
        }

        private IPanelElement _loadedPanel;
        public IPanelElement LoadedPanel 
        {
            get => _loadedPanel;
            set => Set(ref _loadedPanel, value, nameof(LoadedPanel)); 
        }

        public PanelHolderElementViewModel()
        {
            Messenger.Register<LoadPanelMessage>(this, OnLoadPanelMessage);
            Messenger.Register<UnloadPanelMessage>(this, OnUnloadPanelMessage);
        }

        private void OnUnloadPanelMessage(UnloadPanelMessage msg)
        {
            if(msg.PanelHolderId == PanelHolderId)
            {
                Children.Remove(LoadedPanel);
                LoadedPanel.Parent = null;
                if (LoadedPanel is System.IDisposable disposable) disposable.Dispose();
                LoadedPanel = null;
                msg.NotifyExecution?.Invoke(true);
            }
        }

        private void OnLoadPanelMessage(LoadPanelMessage msg)
        {
            if (msg.PanelHolderId == PanelHolderId)
            {
                var center = GetPanelCenter(msg.Length, msg.Width, msg.Height);

                GetInstance<IDispatcherHelper>().CheckBeginInvokeOnUi(() =>
                {
                    LoadedPanel = GetInstance<MVMIF.IPanelElementFactory>().Create(center.X + Position.X,
                                                                                     center.Y + Position.Y,
                                                                                     center.Z + Position.Z,
                                                                                     msg.Length,
                                                                                     msg.Width,
                                                                                     msg.Height);

                    LoadedPanel.Name = "Panel";
                    LoadedPanel.Parent = this;

                    Children.Add(LoadedPanel);
                });

                msg.NotifyExecution?.Invoke(true);
            }
        }

        protected Vector GetPanelCenter(double length, double width, double height)
        {
            Vector center;

            switch (Corner)
            {
                case PanelLoadType.Corner1:
                    center = new Vector() { X = length / 2.0, Y = width / 2.0, Z = height / 2.0 };
                    break;
                case PanelLoadType.Corner2:
                    center = new Vector() { X = -length / 2.0, Y = width / 2.0, Z = height / 2.0 };
                    break;
                case PanelLoadType.Corner3:
                    center = new Vector() { X = -length / 2.0, Y = -width / 2.0, Z = height / 2.0 };
                    break;
                case PanelLoadType.Corner4:
                    center = new Vector() { X = length / 2.0, Y = -width / 2.0, Z = height / 2.0 };
                    break;
                default:
                    center = new Vector();
                    break;
            }

            return center;
        }

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            Messenger.Unregister<LoadPanelMessage>(this);
            Messenger.Unregister<UnloadPanelMessage>(this);
            base.Dispose(disposing);
        }
        #endregion
    }
}
