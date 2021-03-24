using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using AxisAngleRotation3D = System.Windows.Media.Media3D.AxisAngleRotation3D;
using Point3D = System.Windows.Media.Media3D.Point3D;
using RotateTransform3D = System.Windows.Media.Media3D.RotateTransform3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Transform3DGroup = System.Windows.Media.Media3D.Transform3DGroup;
using TranslateTransform3D = System.Windows.Media.Media3D.TranslateTransform3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Color = System.Windows.Media.Color;
using Plane = SharpDX.Plane;
using Vector3 = SharpDX.Vector3;
using Colors = System.Windows.Media.Colors;
using Color4 = SharpDX.Color4;
using System.Collections.Generic;
using System.Windows.Input;
using Machine.ViewModels.Base;
using Machine.ViewModels;
using Machine.ViewModels.UI;
using System.Collections.ObjectModel;
using Machine._3D.Views.ViewModels.Lights;
using M3DVE = Machine._3D.Views.Enums;
using System;
using System.Linq;
using Machine._3D.Views.ViewModels;
using Machine._3D.Views.Interfaces;

namespace Machine._3D.Views
{
    class MainViewModel : BaseViewModel, IPeropertiesProvider
    {
        public IKernelViewModel Kernel { get; private set; }


        //private Camera camera;
        public Camera Camera { get; protected set; }
        //{
        //    get
        //    {
        //        return camera;
        //    }

        //    protected set
        //    {
        //        SetValue(ref camera, value, "Camera");
        //        CameraModel = value is PerspectiveCamera
        //                               ? Perspective
        //                               : value is OrthographicCamera ? Orthographic : null;
        //    }
        //}
        //private IEffectsManager effectsManager;
        public IEffectsManager EffectsManager { get; protected set; }
        //{
        //    get { return effectsManager; }
        //    protected set
        //    {
        //        SetValue(ref effectsManager, value);
        //    }
        //}

        public MeshGeometry3D Model { get; private set; }
        public MeshGeometry3D Model2 { private set; get; }
        public LineGeometry3D Lines { get; private set; }
        public LineGeometry3D Grid { get; private set; }

        public PhongMaterial Material1 { get; private set; }
        public PhongMaterial Material2 { get; private set; }
        public PhongMaterial Material3 { get; private set; }
        public Color GridColor { get; private set; }

        public Transform3D Model1Transform { get; set; }
        public Transform3D Model2Transform { get; set; }
        public Transform3D Model3Transform { get; set; }
        public Transform3D GridTransform { get; set; }

        public Vector3D DirectionalLightDirection { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }

        public Element3D Target { set; get; }
        public Vector3 CenterOffset { set; get; }

        #region View settings

        public FlagViewModel Flag { get; set; } = new FlagViewModel() { Name = "ShowFrameRate" };

        public ICollection<IFlag> Flags { get; private set; } = new ObservableCollection<IFlag>();
        public ICollection<IOptionProvider> Options { get; private set; } = new ObservableCollection<IOptionProvider>();

        #region light
        private M3DVE.LightType _lightType;
        public M3DVE.LightType LightType
        {
            get => _lightType;
            set
            {
                if (Set(ref _lightType, value, nameof(LightType)))
                {
                    SetLights();
                }
            }
        }

        public IEnumerable<M3DVE.LightType> LightTypes => Enum.GetValues(typeof(M3DVE.LightType)).Cast<M3DVE.LightType>();

        public ObservableCollection<AmbientLightViewModel> AmbientLights { get; set; } = new ObservableCollection<AmbientLightViewModel>();
        public ObservableCollection<DirectionalLightViewModel> DirectionalLights { get; set; } = new ObservableCollection<DirectionalLightViewModel>();
        public ObservableCollection<DirectionalLightViewModel> DirectionalOrientedByCameraLights { get; set; } = new ObservableCollection<DirectionalLightViewModel>();
        public ObservableCollection<SpotLightViewModel> SpotLights { get; set; } = new ObservableCollection<SpotLightViewModel>();
        #endregion

        public BackgroundColor BackgroudColor { get; set; } = new BackgroundColor() { Start = Colors.LightGray, Stop = Colors.LightCyan };

        #endregion

        public MainViewModel()
        {
            Machine.ViewModels.Ioc.SimpleIoc<IPeropertiesProvider>.Register(this);
            Machine.ViewModels.Ioc.SimpleIoc<IOptionProvider<M3DVE.LightType>>.Register(new EnumOptionProxy<M3DVE.LightType>(() => LightTypes, () => LightType, (v) => LightType = v));
            Machine.ViewModels.Ioc.SimpleIoc<IBackgroundColor>.Register(BackgroudColor);
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();

            EffectsManager = new DefaultEffectsManager();

            //this.Title = "Manipulator Demo";
            //this.SubTitle = null;

            // camera setup
            //this.Camera = new OrthographicCamera { Position = new Point3D(0, 0, 30), LookDirection = new Vector3D(0, 0, -5), UpDirection = new Vector3D(0, 1, 0) };
            this.Camera = new PerspectiveCamera() { Position = new Point3D(0, 0, 3000), LookDirection = new Vector3D(0, 0, -200), UpDirection = new Vector3D(0, 1, 0), FarPlaneDistance = 10000, NearPlaneDistance = 1 };

            // setup lighting            
            this.AmbientLightColor = Colors.DimGray;
            this.DirectionalLightColor = Colors.White;
            this.DirectionalLightDirection = new Vector3D(-2, -5, -2);

            // floor plane grid
            this.Grid = LineBuilder.GenerateGrid();
            this.GridColor = Colors.Black;
            this.GridTransform = new TranslateTransform3D(-5, -1, -5);

            // scene model3d
            var b1 = new MeshBuilder();
            b1.AddSphere(new Vector3(0, 0, 0), 0.5);
            b1.AddBox(new Vector3(0, 0, 0), 1, 0.5, 1.5, BoxFaces.All);
            this.Model = b1.ToMeshGeometry3D();
            //var m1 = Load3ds("suzanne.3ds");
            //this.Model2 = m1[0].Geometry as MeshGeometry3D;
            //Manully set an offset for test
            //for (int i = 0; i < Model2.Positions.Count; ++i)
            //{
            //    Model2.Positions[i] = Model2.Positions[i] + new Vector3(2, 3, 4);
            //}
            //Model2.UpdateBounds();

            // lines model3d
            var e1 = new LineBuilder();
            e1.AddBox(new Vector3(0, 0, 0), 1, 0.5, 1.5);
            this.Lines = e1.ToLineGeometry3D();

            // model trafos
            this.Model1Transform = new TranslateTransform3D(0, 0, 0);
            this.Model2Transform = new TranslateTransform3D(-3, 0, 0);
            this.Model3Transform = new TranslateTransform3D(+3, 0, 0);

            // model materials
            this.Material1 = PhongMaterials.Orange;
            this.Material2 = PhongMaterials.Orange;
            this.Material3 = PhongMaterials.Red;

        }

        private void SetLights()
        {
            switch (_lightType)
            {
                case M3DVE.LightType.Default:
                    SetDefaultLights();
                    break;
                case M3DVE.LightType.Sun:
                    SetSunLighs();
                    break;
                case M3DVE.LightType.Spot:
                    SetSpotLight();
                    break;
                case M3DVE.LightType.Default2:
                    SetDefaultLights2();
                    break;
                case M3DVE.LightType.Default3:
                    SetDefaultLights3();
                    break;
                default:
                    break;
            }
        }

        private void ClearLights()
        {
            DirectionalOrientedByCameraLights.Clear();
            AmbientLights.Clear();
            DirectionalLights.Clear();
            SpotLights.Clear();
        }

        private void SetDefaultLights()
        {
            ClearLights();

            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(60, 60, 60), Direction = new Vector3D(0.1, 1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(105, 105, 105), Direction = new Vector3D(-1, -1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(128, 128, 128), Direction = new Vector3D(1, -1, -0.1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(50, 50, 50), Direction = new Vector3D(-1, -1, -1) });
            AmbientLights.Add(new AmbientLightViewModel() { Color = Color.FromRgb(30, 30, 30) });
        }

        private void SetDefaultLights2()
        {
            ClearLights();

            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(60, 60, 60), Direction = new Vector3D(0.1, -1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(105, 105, 105), Direction = new Vector3D(-1, 1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(128, 120, 128), Direction = new Vector3D(1, 1, -0.1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(50, 50, 50), Direction = new Vector3D(-1, 1, -1) });
            AmbientLights.Add(new AmbientLightViewModel() { Color = Color.FromRgb(30, 30, 30) });
        }

        private void SetDefaultLights3()
        {
            ClearLights();

            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(60, 60, 60), Direction = new Vector3D(0.1, 1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(105, 105, 105), Direction = new Vector3D(-1, -1, -1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(128, 128, 128), Direction = new Vector3D(1, -1, 1) });
            DirectionalLights.Add(new DirectionalLightViewModel() { Color = Color.FromRgb(50, 50, 50), Direction = new Vector3D(-1, -1, 1) });
            AmbientLights.Add(new AmbientLightViewModel() { Color = Color.FromRgb(30, 30, 30) });
        }

        private void SetSunLighs()
        {
            ClearLights();

            DirectionalOrientedByCameraLights.Add(new DirectionalLightViewModel() { Color = System.Windows.Media.Colors.WhiteSmoke });
            AmbientLights.Add(new AmbientLightViewModel() { Color = Color.FromRgb(22, 22, 22) });

        }


        private void SetSpotLight()
        {
            ClearLights();

            SpotLights.Add(new SpotLightViewModel() { Color = System.Windows.Media.Colors.WhiteSmoke });
            AmbientLights.Add(new AmbientLightViewModel() { Color = System.Windows.Media.Colors.Black });
        }
    }
}
