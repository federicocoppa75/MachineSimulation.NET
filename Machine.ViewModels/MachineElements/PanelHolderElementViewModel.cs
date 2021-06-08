using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Messages;
using Machine.ViewModels.UI;

namespace Machine.ViewModels.MachineElements
{
    public class PanelHolderElementViewModel : ElementViewModel
    {
        public int PanelHolderId { get; set; }
        public string PanelHolderName { get; set; }
        public virtual Vector Position { get; set; }
        public PanelLoadType Corner { get; set; }

        private PanelViewModel _loadedPanel;
        public PanelViewModel LoadedPanel 
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
                    LoadedPanel = new PanelViewModel()
                    {
                        Name = "Panel",
                        Parent = this,
                        CenterX = center.X + Position.X,
                        CenterY = center.Y + Position.Y,
                        CenterZ = center.Z + Position.Z,
                        SizeX = msg.Length,
                        SizeY = msg.Width,
                        SizeZ = msg.Height
                    };

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
    }
}
