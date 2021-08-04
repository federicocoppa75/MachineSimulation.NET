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
using Matrix3D = System.Windows.Media.Media3D.Matrix3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;
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
        public ToolDataViewModel ToolData { get; private set; } = new ToolDataViewModel() { Radius = 5.0};
        public ToolPositionViewModel ToolPosition { get; private set; } = new ToolPositionViewModel() { X= -20.0, Y = -20.0, Z = 10.0 };
        public IPanel Panel { get; set; } //= new PanelViewModel() { NumCells = 16, SectionsX100mm = 3 };

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

        private bool _viewWireFrame = false;
        public bool ViewWireFrame
        {
            get => _viewWireFrame;
            set => Set(ref _viewWireFrame, value, nameof(ViewWireFrame));
        }

        private bool _isParallel;
        public bool IsParallel
        {
            get => _isParallel;
            set => Set(ref _isParallel, value, nameof(IsParallel));
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
            if(ToolData.Radius < 50.0)
            {
                ApplyToolActionData();
                //ApplyToolConeActionData();
            }
            else
            {
                ApplyToolSectionActionData();
            }

            ToolPosition.ResetD();
            PanelPosition.ResetD();
        }

        private void ApplyToolActionData()
        {
            var tad = new ToolActionData()
            {
                Length = (float)ToolData.Length,
                Radius = (float)ToolData.Radius,
                Orientation = ToolData.Direction,
                X = (float)(ToolPosition.X - PanelPosition.X),
                Y = (float)(ToolPosition.Y - PanelPosition.Y),
                Z = (float)(ToolPosition.Z - PanelPosition.Z)
            };

            if (IsParallel)
            {
                Panel.ApplyActionAsync(tad);
            }
            else
            {
                Panel.ApplyAction(tad);
            }
        }

        private void ApplyToolSectionActionData()
        {
            var nSection = 24;                                          // numero di sezioni
            var sa = 360.0 / nSection;                                  // ampiezza angolare delle sezioni
            var sh = 5.0;                                               // altezza sezione;
            var sw = ToolData.Radius * sa * (Math.PI * 2.0) / 360.0;    // larghezza sezione
            var sl = ToolData.Length; // linghezza sezione
            var n = GetOrientation(ToolData.Direction);
            var r = GetRadial(ToolData.Direction);
            var p = new Point3D(ToolPosition.X - PanelPosition.X, 
                                ToolPosition.Y - PanelPosition.Y, 
                                ToolPosition.Z - PanelPosition.Z) + n * sl / 2.0;
            var d = new Vector3D(ToolPosition.DX - PanelPosition.DX, 
                                 ToolPosition.DY - PanelPosition.DY, 
                                 ToolPosition.DZ - PanelPosition.DZ);

            for (int i = 0; i < nSection; i++)
            {
                var matrix = Matrix3D.Identity;
                matrix.Rotate(new Quaternion(n, i * sa));
                var radial = matrix.Transform(r);
                var sc = p + radial * (ToolData.Radius - sh / 2.0);
                var tsad = new ToolSectionActionData()
                {
                    PX = (float)sc.X,
                    PY = (float)sc.Y,
                    PZ = (float)sc.Z,
                    DX = (float)radial.X,
                    DY = (float)radial.Y,
                    DZ = (float)radial.Z,
                    L = (float)sl,
                    W = (float)sw,
                    H = (float)sh,
                    FixBaseAx = ToolData.Direction
                };

                if (Vector3D.DotProduct(radial, d) < 0.0) continue;

                if (IsParallel)
                {
                    Panel.ApplyActionAsync(tsad);
                }
                else
                {
                    Panel.ApplyAction(tsad);
                }
            }
        }

        private void ApplyToolConeActionData()
        {
            var tcad = new ToolConeActionData()
            {
                Length = (float)ToolData.Length,
                MinRadius = 0.0f,
                MaxRadius = (float)ToolData.Radius,
                Orientation = ToolData.Direction,
                X = (float)(ToolPosition.X - PanelPosition.X),
                Y = (float)(ToolPosition.Y - PanelPosition.Y),
                Z = (float)(ToolPosition.Z - PanelPosition.Z)
            };

            if (IsParallel)
            {
                Panel.ApplyActionAsync(tcad);
            }
            else
            {
                Panel.ApplyAction(tcad);
            }
        }

        private Vector3D GetRadial(Orientation direction)
        {
            switch (direction)
            {
                case Orientation.XPos:
                case Orientation.XNeg:
                    return new Vector3D(0.0, 0.0, 1.0);
                case Orientation.YPos:
                case Orientation.YNeg:
                    return new Vector3D(1.0, 0.0, 0.0);
                case Orientation.ZPos:
                case Orientation.ZNeg:
                    return new Vector3D(1.0, 0.0, 0.0);
                default:
                    throw new NotImplementedException();
            }            
        }

        private Vector3D GetOrientation(Orientation direction)
        {
            switch (direction)
            {
                case Orientation.XPos:
                    return new Vector3D(-1.0, 0.0, 0.0);
                case Orientation.XNeg:
                    return new Vector3D(1.0, 0.0, 0.0);
                case Orientation.YPos:
                    return new Vector3D(0.0, -1.0, 0.0);
                case Orientation.YNeg:
                    return new Vector3D(0.0, 1.0, 0.0);
                case Orientation.ZPos:
                    return new Vector3D(0.0, 0.0, -1.0);
                case Orientation.ZNeg:
                    return new Vector3D(0.0, 0.0, 1.0);
                default:
                    throw new NotImplementedException();
            }            
        }

        private void OnToolPositionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((string.Compare(e.PropertyName, "DX") == 0) ||
                (string.Compare(e.PropertyName, "DY") == 0) ||
                (string.Compare(e.PropertyName, "DZ") == 0))
            {
                if((ToolPosition.DX != 0.0) || 
                    (ToolPosition.DY != 0.0) || 
                    (ToolPosition.DZ != 0.0))
                {
                    OnPositionChanged();
                }                
            }
        }

        private void OnPanelPositionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((string.Compare(e.PropertyName, "DX") == 0) ||
                (string.Compare(e.PropertyName, "DY") == 0) ||
                (string.Compare(e.PropertyName, "DZ") == 0))
            {
                if ((PanelPosition.DX != 0.0) ||
                    (PanelPosition.DY != 0.0) ||
                    (PanelPosition.DZ != 0.0))
                {
                    OnPositionChanged();
                }
            }
        }

        private void OnToolDataChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => UpdateToolDataData();

        private void OnPanelDataChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => UpdatePanelData();
    
        private void UpdatePanelData()
        {
            Panel = new PanelViewModel() { NumCells = 16, SectionsX100mm = 3 };

            Panel.SizeX = PanelData.SizeX;
            Panel.SizeY = PanelData.SizeY;
            Panel.SizeZ = PanelData.SizeZ;

            Panel.Initialize();

            RisePropertyChanged(nameof(Panel));
            RisePropertyChanged(nameof(Faces));
            RisePropertyChanged(nameof(Volumes));
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
