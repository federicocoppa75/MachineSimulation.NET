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
//using System.Windows.Media.Media3D;
using MVMIP = Machine.ViewModels.Interfaces.Probing;

namespace Machine._3D.Views
{
    /// <summary>
    /// Logica di interazione per MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        private GeometryModel3D _selectedModel;
        private bool _startMove;
        private SharpDX.Vector3 _startMovePoint;

        private IProbesController _probesController;
        protected IProbesController ProbesController => _probesController ?? (_probesController = Machine.ViewModels.Ioc.SimpleIoc<IProbesController>.GetInstance());

        public MachineView()
        {
            InitializeComponent();
            Machine.ViewModels.Ioc.SimpleIoc<IViewTransformationFactory>.Register<TransformationFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IColliderHelperFactory>.Register<ColliderHelperFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IProcessCaller>.Register<RenderProcessCaller>();
            Machine.ViewModels.Ioc.SimpleIoc<IToolToPanelTransformerFactory>.Register<ToolToPanelTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IInserterToSinkTransformerFactory>.Register<InserterToSinkTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMIP.IProbePointTransformerFactory>.Register<ProbePointTransformerFactory>();
            DataContext = new MainViewModel();

            var isToolEditor = Machine.ViewModels.Ioc.SimpleIoc<IApplicationInformationProvider>.GetInstance().ApplicationType == ApplicationType.ToolEditor;

            view3DX.AddHandler(Element3D.MouseDown3DEvent, new RoutedEventHandler((s, e) =>
            {
                var arg = e as HelixToolkit.Wpf.SharpDX.MouseDown3DEventArgs;

                if (isToolEditor) return;
                if (arg.HitTestResult == null) return;
                if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

                var selectedModel = arg.HitTestResult.ModelHit as GeometryModel3D;


                if (ProbesController.Active)
                {
                    var p = arg.HitTestResult.PointHit;
                    var vm = selectedModel.DataContext as MVMIP.IProbableElement;

                    vm?.AddProbePoint(new MVMIP.Point() { X = p.X, Y = p.Y, Z = p.Z });
                }
                //else if((_selectedModel != null) && ReferenceEquals(selectedModel, _selectedModel))
                //{
                //    _startMovePoint = arg.HitTestResult.PointHit;
                //    _startMove = true;
                //}
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
            }));
            //view3DX.AddHandler(Element3D.MouseUp3DEvent, new RoutedEventHandler((s, e) => 
            //{
            //    var arg = e as HelixToolkit.Wpf.SharpDX.MouseUp3DEventArgs;

            //    if (arg.HitTestResult == null) return;
            //    if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            //    _startMove = false;
            //}));
            //view3DX.AddHandler(Element3D.MouseMove3DEvent, new RoutedEventHandler((s, e) => 
            //{
            //    var arg = e as HelixToolkit.Wpf.SharpDX.MouseMove3DEventArgs;

            //    if (arg.HitTestResult == null) return;
            //    if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

            //    var p = arg.Position;
            //    var delta = arg.HitTestResult.PointHit - _startMovePoint;
            //    var d = delta;
            //}));

            FillView3DFlags(FlagsNames, (DataContext as MainViewModel).Flags);
            FillView3DOptions(OptionsNames, (DataContext as MainViewModel).Options);
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
