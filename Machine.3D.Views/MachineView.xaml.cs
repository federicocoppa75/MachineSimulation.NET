using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Factories;
using Machine._3D.Views.Helpers;
using Machine.ViewModels;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.Tools;
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
using Machine._3D.Views.Implementation;
using Machine.ViewModels.Interfaces.Factories;

namespace Machine._3D.Views
{
    /// <summary>
    /// Logica di interazione per MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        private GeometryModel3D _selectedModel;
        private MoveEventsHandler _moveEventsHandler;
        private RotateEventHandler _rotateEventHandler;

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
            Machine.ViewModels.Ioc.SimpleIoc<MVMIH.IElementRotatorFactory>.Register<ElementRotatorFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IPanelWireframe>.Register<ViewModels.PanelWireframeViewModel>();

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

            Machine.ViewModels.Ioc.SimpleIoc<IViewExportController>.Register(new MeshViewExportController(view3DX));
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
                _moveEventsHandler = new MoveEventsHandler(ph);
            }
            else if ((_selectedModel != null) && !ReferenceEquals(selectedModel, _selectedModel) && (selectedModel.DataContext is MVMIH.IRotationHandle rh))
            {
                _rotateEventHandler = new RotateEventHandler(rh);
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
            _moveEventsHandler = null;
            _rotateEventHandler = null;
        }

        private void OnMouseMove3DEventHandler(object s, RoutedEventArgs e)
        {
            if(_moveEventsHandler != null) OnMouseMove3DEventHandlerForMove(s, e);
            else if(_rotateEventHandler != null) OnMouseMove3DEventHandlerForRotate(s, e);
        }

        private void OnMouseMove3DEventHandlerForMove(object s, RoutedEventArgs e)
        {
            var arg = e as MouseMove3DEventArgs;

            if (arg.HitTestResult == null) return;
            if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            var p = arg.Position;
            var position = arg.HitTestResult.PointHit.ToPoint3D();
            var newPosition = view3DX.UnProjectOnPlane(p, position, view3DX.Camera.LookDirection);

            if((newPosition != null) && newPosition.HasValue)
            {
                var np = newPosition.Value;
                var d = np - position;

                _moveEventsHandler?.Move(d); ;
            }
        }

        private void OnMouseMove3DEventHandlerForRotate(object s, RoutedEventArgs e)
        {
            var arg = e as MouseMove3DEventArgs;

            if (arg.HitTestResult == null) return;
            if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            var p = arg.Position;
            var position = arg.HitTestResult.PointHit.ToPoint3D();
            var newPosition = view3DX.UnProjectOnPlane(p, position, view3DX.Camera.LookDirection);

            if ((newPosition != null) && newPosition.HasValue)
            {
                var np = newPosition.Value;
                var d = np - position;

                _rotateEventHandler?.Rotate(np, position);
            }
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

    class MoveEventsHandler
    {
        private MVMIH.IPositionHandle _positionHandle;
        private SWMM.Matrix3D _matrix;

        public  MoveEventsHandler(MVMIH.IPositionHandle positionHandle)
        {
            _positionHandle = positionHandle;
            _matrix = GetTranslateTransformation(positionHandle);

            _positionHandle.StartMove();
        }

        public void Move(SWMM.Vector3D step)
        {
            SWMM.Vector3D v = GetMoveDirection(_positionHandle);
            var v2 = _matrix.Transform(v);
            var dp = SWMM.Vector3D.DotProduct(step, v2);
            var v3 = v2 * dp;

            _positionHandle.Move(v3.X, v3.Y, v3.Z);
        }

        private SWMM.Vector3D GetMoveDirection(MVMIH.IPositionHandle positionHandle)
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

            return v;
        }

        private SWMM.Matrix3D GetTranslateTransformation(MVMIH.IPositionHandle ph)
        {
            var me = ph as IMachineElement;
            var eleToMove = me.Parent.Parent; 
            var matrix = eleToMove.GetChainTransformation();

            matrix.OffsetX = 0;
            matrix.OffsetY = 0;
            matrix.OffsetZ = 0;

            return matrix;
        }
    }

    class RotateEventHandler
    {
        private MVMIH.IRotationHandle _rotationHandle;
        private SWMM.Matrix3D _matrix;
        private SWMM.Matrix3D _eleRotMatrix;
        private SWMM.Point3D _rotationCenter;
        private SWMM.Vector3D _rotationDirection;


        public RotateEventHandler(MVMIH.IRotationHandle rotationHandle)
        {
            _rotationHandle = rotationHandle;
            _matrix = GetTranslateTransformation(rotationHandle);
            _eleRotMatrix = Converters.StaticTransformationConverter.Convert((rotationHandle as IMachineElement).Parent.Parent.Transformation);
            _rotationDirection = _eleRotMatrix.Transform(GetRotationDirection(rotationHandle));
            _rotationCenter = _eleRotMatrix.Transform(GetRotationCenter((rotationHandle as IMachineElement).Parent.Parent));

            _rotationHandle.StartRotate();
        }

        public void Rotate(SWMM.Point3D newPosition, SWMM.Point3D oldPosition)
        {
            var me = _rotationHandle as IMachineElement;
            var m = new SWMM.Matrix3D(_matrix.M11, _matrix.M12, _matrix.M13, _matrix.M14, _matrix.M21, _matrix.M22, _matrix.M23, _matrix.M24, _matrix.M31, _matrix.M32, _matrix.M33, _matrix.M34, _matrix.OffsetX, _matrix.OffsetY, _matrix.OffsetZ, _matrix.M44);

            m.Invert();

            var p1 = m.Transform(oldPosition);
            var p2 = m.Transform(newPosition);
            var v1 = GetOrtoComponent(p1 - _rotationCenter, _rotationDirection);
            var v2 = GetOrtoComponent(p2 - _rotationCenter, _rotationDirection);
            var a = SWMM.Vector3D.AngleBetween(v1, v2);

            _rotationHandle.Rotate(a);
        }

        private SWMM.Matrix3D GetTranslateTransformation(MVMIH.IRotationHandle rh)
        {
            var me = rh as IMachineElement;
            var eleToMove = me.Parent.Parent;
            var matrix = eleToMove.GetChainTransformation();

            return matrix;
        }

        private SWMM.Vector3D GetRotationDirection(MVMIH.IRotationHandle rotationHandle)
        {
            SWMM.Vector3D n;

            switch (rotationHandle.Type)
            {
                case MVMIH.Type.X:
                    n = new SWMM.Vector3D(1.0, 0.0, 0.0);
                    break;
                case MVMIH.Type.Y:
                    n = new SWMM.Vector3D(0.0, 1.0, 0.0);
                    break;
                case MVMIH.Type.Z:
                    n = new SWMM.Vector3D(0.0, 0.0, 1.0);
                    break;
                default:
                    break;
            }

            return n;
        }

        private SWMM.Point3D GetRotationCenter(IMachineElement element)
        {
            Machine.ViewModels.Ioc.SimpleIoc<MVMIPR.IElementBoundingBoxProvider>
                .GetInstance()
                .GetBoundingBox(element,
                                out double minX,
                                out double minY,
                                out double minZ,
                                out double maxX,
                                out double maxY,
                                out double maxZ);

            var bb = new SharpDX.BoundingBox(new SharpDX.Vector3((float)minX, (float)minY, (float)minZ), new SharpDX.Vector3((float)maxX, (float)maxY, (float)maxZ));

            return bb.Center.ToPoint3D();
        }

        private SWMM.Vector3D GetOrtoComponent(SWMM.Vector3D v, SWMM.Vector3D n)
        {
            var s = SWMM.Vector3D.DotProduct(v, n);

            return v - n * s;
        }
    }
}
