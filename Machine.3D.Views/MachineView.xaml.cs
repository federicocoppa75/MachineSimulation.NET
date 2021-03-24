using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Factories;
using Machine.ViewModels;
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

namespace Machine._3D.Views
{
    /// <summary>
    /// Logica di interazione per MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        private GeometryModel3D _selectedModel;

        public MachineView()
        {
            InitializeComponent();
            Machine.ViewModels.Ioc.SimpleIoc<IViewTransformationFactory>.Register<TransformationFactory>();
            DataContext = new MainViewModel();

            view3DX.AddHandler(Element3D.MouseDown3DEvent, new RoutedEventHandler((s, e) =>
            {
                var arg = e as HelixToolkit.Wpf.SharpDX.MouseDown3DEventArgs;

                if (arg.HitTestResult == null) return;
                if ((arg.OriginalInputEventArgs is MouseButtonEventArgs mbeArg) && (mbeArg.ChangedButton != MouseButton.Left)) return;

                //if (machineViewModel.EnableSelectionByView)
                {
                    var selectedModel = arg.HitTestResult.ModelHit as GeometryModel3D;
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
                //else if (machineViewModel.AddProbePoint)
                //{
                //    var selectedModel = arg.HitTestResult.ModelHit as GeometryModel3D;
                //    var point = arg.HitTestResult.PointHit.ToPoint3D();
                //    var vm = selectedModel.DataContext as IProbableElementViewModel;

                //    vm?.AddProbePoint(point);
                //}

            }));

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
