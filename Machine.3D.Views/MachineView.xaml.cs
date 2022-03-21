using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Factories;
using Machine._3D.Views.Helpers;
using Machine.ViewModels;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SWMM = System.Windows.Media.Media3D;
using MVMIP = Machine.ViewModels.Interfaces.Probing;
using MVMIPR = Machine.ViewModels.Interfaces.Providers;
using MVMIH = Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine._3D.Views
{
    /// <summary>
    /// Logica di interazione per MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        private GeometryModel3D _selectedModel;
        private bool _startMove;
        private MVMIH.IPositionHandle _positionHandle;
        private System.Windows.Media.Media3D.Matrix3D _matrix;

        private IProbesController _probesController;
        protected IProbesController ProbesController => _probesController ?? (_probesController = Machine.ViewModels.Ioc.SimpleIoc<IProbesController>.GetInstance());

        private bool _isToolEditor;

        public MachineView()
        {
            InitializeComponent();
            Machine.ViewModels.Ioc.SimpleIoc<IViewTransformationFactory>.Register<TransformationFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IColliderHelperFactory>.Register<ColliderHelperFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IProcessCaller>.Register<RenderProcessCaller>();
            Machine.ViewModels.Ioc.SimpleIoc<IToolToPanelTransformerFactory>.Register<ToolToPanelTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IInserterToSinkTransformerFactory>.Register<InserterToSinkTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMIP.IProbePointTransformerFactory>.Register<ProbePointTransformerFactory>();

            var geometryBuffer = new Implementation.Geometry3DBuffer();
            Machine.ViewModels.Ioc.SimpleIoc<Interfaces.IGeometry3DBuffer>.Register(geometryBuffer);
            Machine.ViewModels.Ioc.SimpleIoc<MVMIPR.IElementBoundingBoxProvider>.Register(geometryBuffer);
            DataContext = new MainViewModel();

            _isToolEditor = Machine.ViewModels.Ioc.SimpleIoc<IApplicationInformationProvider>.GetInstance().ApplicationType == ApplicationType.ToolEditor;

            view3DX.AddHandler(Element3D.MouseDown3DEvent, new RoutedEventHandler(OnMouseDown3DEventHandler));
            view3DX.AddHandler(Element3D.MouseUp3DEvent, new RoutedEventHandler(OnMouseUp3DEventHandler));
            view3DX.AddHandler(Element3D.MouseMove3DEvent, new RoutedEventHandler(OnMouseMove3DEventHandler));

            FillView3DFlags(FlagsNames, (DataContext as MainViewModel).Flags);
            FillView3DOptions(OptionsNames, (DataContext as MainViewModel).Options);
        }

        private void OnMouseDown3DEventHandler(object s, RoutedEventArgs e)
        {
            var arg = e as MouseDown3DEventArgs;

            if (_isToolEditor) return;
            if (arg.HitTestResult == null) return;
            if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            var selectedModel = arg.HitTestResult.ModelHit as GeometryModel3D;


            if (ProbesController.Active)
            {
                var p = arg.HitTestResult.PointHit;
                var vm = selectedModel.DataContext as MVMIP.IProbableElement;

                vm?.AddProbePoint(new MVMIP.Point() { X = p.X, Y = p.Y, Z = p.Z });
            }
            else if ((_selectedModel != null) && !ReferenceEquals(selectedModel, _selectedModel) && (selectedModel.DataContext is MVMIH.IPositionHandle ph))
            {
                _startMove = true;
                _positionHandle = ph;
                _matrix = GetTranslateTransformation(ph);

                _positionHandle.StartMove();
            }
            else
            {
                var updateSelection = true;

                if (_selectedModel != null)
                {
                    updateSelection = !ReferenceEquals(selectedModel, _selectedModel);
                    _selectedModel.IsSelected = false;
                    _selectedModel = null;
                }

                if (updateSelection)
                {
                    selectedModel.IsSelected = true;
                    _selectedModel = selectedModel;
                }
            }
        }

        private void OnMouseUp3DEventHandler(object s, RoutedEventArgs e)
        {
            if (!_startMove) return;

            var arg = e as HelixToolkit.Wpf.SharpDX.MouseUp3DEventArgs;

            if (arg.HitTestResult == null) return;
            if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            _startMove = false;
        }

        private void OnMouseMove3DEventHandler(object s, RoutedEventArgs e)
        {
            if (!_startMove) return;

            var arg = e as HelixToolkit.Wpf.SharpDX.MouseMove3DEventArgs;

            if (arg.HitTestResult == null) return;
            if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            var p = arg.Position;
            var position = arg.HitTestResult.PointHit.ToPoint3D();
            var newPosition = view3DX.UnProjectOnPlane(p, position, view3DX.Camera.LookDirection);

            if((newPosition != null) && newPosition.HasValue)
            {
                var np = newPosition.Value;
                var d = np - position;

                ProcessPositionHandleMove(d);
            }
        }

        private SWMM.Matrix3D GetTranslateTransformation(MVMIH.IPositionHandle ph)
        {
            var me = ph as IMachineElement;
            var eleToMove = me.Parent.Parent; // recupero l'elemento a cui è attaccato l'handle
            //var matrix = (eleToMove.Parent != null) ? eleToMove.Parent.GetChainTransformation() : SWMM.Matrix3D.Identity;
            var matrix = eleToMove.GetChainTransformation(/*true*/);

            matrix.OffsetX = 0;
            matrix.OffsetY = 0;
            matrix.OffsetZ = 0;

            return matrix;
        }

        private void ProcessPositionHandleMove(SWMM.Vector3D step)
        {
            SWMM.Vector3D v;

            switch (_positionHandle.Type)
            {
                case MVMIH.Type.X:
                    v = new SWMM.Vector3D(1.0, 0.0, 0.0);
                    break;
                case MVMIH.Type.Y:
                    v = new SWMM.Vector3D(0.0, 1.0, 0.0);
                    break;
                case MVMIH.Type.Z:
                    v = new SWMM.Vector3D(0.0, 0.0, 1.0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var v2 = _matrix.Transform(v);
            var dp = SWMM.Vector3D.DotProduct(step, v2);
            var v3 = v2 * dp;

            _positionHandle.Move(v3.X, v3.Y, v3.Z);
        }
        
        private void FillView3DFlags(IEnumerable<string> flagsNames, ICollection<IFlag> flags)
        {
            foreach (PropertyDescriptor item in System.ComponentModel.TypeDescriptor.GetProperties(view3DX, new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                var dpp = DependencyPropertyDescriptor.FromProperty(item);

                if((dpp != null) && flagsNames.Any(s => string.Compare(s, dpp.Name) == 0))
                //if ((dpp != null) && (string.Compare(dpp.PropertyType.FullName, typeof(bool).FullName) == 0) && !dpp.IsReadOnly)
                {
                    var vm = new FlagViewModel() { Name = dpp.Name };
                    var binding = new Binding("Value");

                    binding.Source = vm;
                    view3DX.SetBinding(dpp.DependencyProperty, binding);

                    flags.Add(vm);
                }
            }
        }

        private void FillView3DOptions(IEnumerable<string> optionNames, ICollection<IOptionProvider> options)
        {
            foreach (PropertyDescriptor item in System.ComponentModel.TypeDescriptor.GetProperties(view3DX, new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                var dpp = DependencyPropertyDescriptor.FromProperty(item);

                //if ((dpp != null) && dpp.PropertyType.IsEnum && !dpp.IsReadOnly)
                if ((dpp != null) && optionNames.Any(s => string.Compare(s, dpp.Name) == 0))
                {
                    var tOption = typeof(EnumOptionProvider<>);
                    Type[] tArgs = { dpp.PropertyType };
                    var tCostructed = tOption.MakeGenericType(tArgs);
                    IOptionProvider instance = Activator.CreateInstance(tCostructed) as IOptionProvider;
                    var binding = new Binding("Value");

                    instance.Name = dpp.Name;
                    binding.Source = instance;
                    view3DX.SetBinding(dpp.DependencyProperty, binding);

                    options.Add(instance);
                }
            }
        }

        private static string[] FlagsNames
        {
            get
            {
                string[] ss = {
                    "EnableCurrentPosition",
                    "InfiniteSpin",
                    "IsShadowMappingEnabled",
                    "IsChangeFieldOfViewEnabled",
                    "IsInertiaEnabled",
                    "IsPanEnabled",
                    "IsRotationEnabled",
                    "IsTouchRotateEnabled",
                    "IsPinchZoomEnabled",
                    "PinchZoomAtCenter",
                    "IsThreeFingerPanningEnabled",
                    "IsZoomEnabled",
                    "Orthographic",
                    "RotateAroundMouseDownPoint",
                    "ShowCameraInfo",
                    "ShowCameraTarget",
                    "ShowCoordinateSystem",
                    "ShowFrameRate",
                    "ShowTriangleCountInfo",
                    "ShowViewCube",
                    "UseDefaultGestures",
                    "IsViewCubeEdgeClicksEnabled",
                    "IsViewCubeMoverEnabled",
                    "IsCoordinateSystemMoverEnabled",
                    "ZoomAroundMouseDownPoint",
                    "ZoomExtentsWhenLoaded",
                    "FixedRotationPointEnabled",
                    "EnableMouseButtonHitTest",
                    "EnableDeferredRendering",
                    "EnableSharedModelMode",
                    "EnableSwapChainRendering",
                    "ShowFrameDetails",
                    "EnableD2DRendering",
                    "EnableAutoOctreeUpdate",
                    "IsMoveEnabled",
                    "EnableOITRendering",
                    "EnableDesignModeRendering",
                    "EnableRenderOrder",
                    "EnableSSAO",
                    "AllowUpDownRotation",
                    "AllowLeftRightRotation",
                    "BelongsToParentWindow",
                    "EnableDpiScale",
                    "IsTabStop",
                    "OverridesDefaultStyle",
                    "UseLayoutRounding",
                    "ForceCursor",
                    "AllowDrop",
                    "ClipToBounds",
                    "SnapsToDevicePixels",
                    "IsEnabled",
                    "IsHitTestVisible",
                    "Focusable",
                    "IsManipulationEnabled"
                };
                return ss;
            }
        }

        private static string[] OptionsNames
        {
            get
            {
                string[] ss = {
                    "CameraMode",
                    "CameraRotationMode",
                    "MSAA",
                    "OITWeightMode",
                    "FXAALevel",
                    "SSAOQuality",
                    "FlowDirection",
                    "Visibility",
                    //"InputMethod.PreferredImeState",
                    //"InputMethod.PreferredImeConversionMode",
                    //"InputMethod.PreferredImeSentenceMode",
                    //"RenderOptions.EdgeMode",
                    //"RenderOptions.BitmapScalingMode",
                    //"RenderOptions.ClearTypeHint",
                    //"RenderOptions.CachingHint",
                    //"VirtualizingPanel.VirtualizationMode",
                    //"VirtualizingPanel.ScrollUnit",
                    //"VirtualizingPanel.CacheLengthUnit",
                    //"AutomationProperties.IsOffscreenBehavior",
                    //"AutomationProperties.LiveSetting",
                };

                return ss;
            }
        }
    }
}
