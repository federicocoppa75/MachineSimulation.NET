using HelixToolkit.Wpf.SharpDX;
using Machine.ViewModels.Base;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;

namespace MaterialRemove.Test.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public Camera Camera { get; protected set; }
        public IEffectsManager EffectsManager { get; protected set; }
        public Vector3D DirectionalLightDirection { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }
        public Vector3D UpDirection { set; get; } = new Vector3D(0, 1, 0);
        public PhongMaterial ToolMaterial => PhongMaterials.Blue;
        public PhongMaterial PanelMaterial => PhongMaterials.Orange;

        private Geometry3D _toolGeometry;
        public Geometry3D ToolGeometry 
        { 
            get => _toolGeometry; 
            set => Set(ref _toolGeometry, value, nameof(ToolGeometry)); 
        }
        private Geometry3D _panelGeometry;
        public Geometry3D PanelGeometry 
        { 
            get => _panelGeometry; 
            set => Set(ref _panelGeometry, value, nameof(PanelGeometry)); 
        }
        public IEnumerable<ISectionFace> Faces => Panel.Sections.SelectMany(s => s.Faces);
        public IEnumerable<ISectionVolume> Volumes => Panel.Sections.Select(s => s.Volume);

        public PanelDataViewModel PanelData { get; private set; } = new PanelDataViewModel();
        public PanelPositionViewModel PanelPosition { get; private set; } = new PanelPositionViewModel();
        public ToolDataViewModel ToolData { get; private set; } = new ToolDataViewModel();
        public ToolPositionViewModel ToolPosition { get; private set; } = new ToolPositionViewModel();
        public IPanel Panel { get; set; } = new PanelViewModel() { NumCells = 16, SectionsX100mm = 3 };

        private bool _isFacesVisible = true;
        public bool IsFacesVisible
        {
            get => _isFacesVisible;
            set => Set(ref _isFacesVisible, value, nameof(IsFacesVisible));
        }

        private bool _isVolumesVisible = true;
        public bool IsVolumesVisible
        {
            get => _isVolumesVisible;
            set => Set(ref _isVolumesVisible, value, nameof(IsVolumesVisible));
        }

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            // camera setup
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(-800, -800, 500),
                LookDirection = new Vector3D(8, 8, -5),
                UpDirection = new Vector3D(0, 0, 1),
                FarPlaneDistance = 5000000
            };

            // setup lighting            
            AmbientLightColor = Colors.DimGray;
            DirectionalLightColor = Colors.White;
            DirectionalLightDirection = new Vector3D(-2, -2, -5);

            UpdatePanelData();
            ToolGeometry = GetToolGeometry();
            PanelData.PropertyChanged += OnPanelDataChanged;
            ToolData.PropertyChanged += OnToolDataChanged;
            PanelPosition.PropertyChanged += OnPanelPositionChanged;
            ToolPosition.PropertyChanged += OnToolPositionChanged;
        }

        private void OnPositionChanged()
        {
            Panel.ApplyAction(new ToolActionData()
            {
                Length = (float)ToolData.Length,
                Radius = (float)ToolData.Radius,
                Orientation = ToolData.Direction,
                X = (float)(ToolPosition.X - PanelPosition.X),
                Y = (float)(ToolPosition.Y - PanelPosition.Y),
                Z = (float)(ToolPosition.Z - PanelPosition.Z)
            });
        }

        private void OnToolPositionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => OnPositionChanged();

        private void OnPanelPositionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => OnPositionChanged();

        private void OnToolDataChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => UpdateToolDataData();

        private void OnPanelDataChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => UpdatePanelData();
    
        private void UpdatePanelData()
        {
            Panel.SizeX = PanelData.SizeX;
            Panel.SizeY = PanelData.SizeY;
            Panel.SizeZ = PanelData.SizeZ;

            Panel.Initialize();
        }

        private void UpdateToolDataData() => ToolGeometry = GetToolGeometry();

        private Geometry3D GetToolGeometry()
        {
            var builder = new MeshBuilder();
            SharpDX.Vector3 v;

            switch (ToolData.Direction)
            {
                case Orientation.ZNeg:
                    v = new SharpDX.Vector3(0.0f, 0.0f, (float)ToolData.Length);
                    break;
                case Orientation.ZPos:
                    v = new SharpDX.Vector3(0.0f, 0.0f, (float)ToolData.Length * -1.0f);
                    break;
                case Orientation.XPos:
                    v = new SharpDX.Vector3((float)ToolData.Length * -1.0f, 0.0f, 0.0f);
                    break;
                case Orientation.XNeg:
                    v = new SharpDX.Vector3((float)ToolData.Length, 0.0f, 0.0f);
                    break;
                case Orientation.YPos:
                    v = new SharpDX.Vector3(0.0f, (float)ToolData.Length * -1.0f, 0.0f);
                    break;
                case Orientation.YNeg:
                    v = new SharpDX.Vector3(0.0f, (float)ToolData.Length, 0.0f);
                    break;
                default:
                    throw new ArgumentException();
            }

            for (int i = 0; i < ToolData.Repetition; i++)
            {
                var dx = new SharpDX.Vector3((float)ToolData.StepX * i, (float)ToolData.StepY * i, 0.0f);
                builder.AddCylinder(dx, dx + v, ToolData.Radius * 2.0, 32);
            }
            

            return builder.ToMesh();
        }

        private Geometry3D GetPanelGeometry()
        {
            var builder = new MeshBuilder();

            builder.AddBox(new SharpDX.Vector3(), PanelData.SizeX, PanelData.SizeY, PanelData.SizeZ);

            return builder.ToMesh();
        }
    }
}
