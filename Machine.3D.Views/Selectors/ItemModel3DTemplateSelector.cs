using Machine._3D.Views.Enums;
using Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.Indicators;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using Machine.ViewModels.Interfaces.Tools;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Machine._3D.Views.Selectors
{
    [ContentProperty("Templates")]
    public class ItemModel3DTemplateSelector : DataTemplateSelector
    {
        public List<ItemModel3DTemplateSelectorItem> Templates { get; set; } = new List<ItemModel3DTemplateSelectorItem>();

        public ItemModel3DTemplateSelector() :base()
        {

        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dt = null;
            var it = GetItemType(item);

            foreach (var t in Templates)
            {
                if (t.When == it)
                {
                    dt = t.Then;
                    break;
                }
            }

            return dt;
        }

        private static ElementViewType GetItemType(object item)
        {
            var it = ElementViewType.Default;

            if(item is IInsertedObject)
            {
                it = ElementViewType.InsertedObj;
            }
            else if(item is IInjectedObject)
            {
                it = ElementViewType.InjectedObj;
            }
            else if(item is IToolElement tvm)
            {
                it = ElementViewType.ToolEle;
            }
            else if(item is IPanelElement pvm)
            {
                it = (pvm is IPanel) ? ElementViewType.SectionedPanel : ElementViewType.Panel;
            }
            else if(item is IAngularTransmission)
            {
                it = ElementViewType.AngularTransmission;
            }
            else if(item is IATToolholder)
            {
                it = ElementViewType.ATToolholder;
            }
            else if(item is IDebugElementViewModel)
            {
                it = ElementViewType.Debug;
            }
            else if(item is IProbePoint)
            {
                it = ElementViewType.PointProbe;
            }
            else if(item is IProbeDistance)
            {
                it = ElementViewType.DistanceProbe;
            }
            else if((item is IPositionAndDirectionIndicator) && (item is IIndicatorProxy))
            {
                it = ElementViewType.PointAndDirIndicator;
            }
            else if((item is IPositionIndicator) && (item is IIndicatorProxy))
            {
                it = ElementViewType.PointIndicator;
            }
            else if((item is IPositionsIndicator) && (item is IIndicatorProxy))
            {
                it = ElementViewType.PointsIndicator;
            }
            else if(item is IToolDimension)
            {
                it = ElementViewType.ToolDimension;
            }
            else if(item is IElementHandle)
            {
                it = ElementViewType.ElementHandle;
            }
            else if(item is IPositionHandle)
            {
                it = ElementViewType.PositionHandle;
            }
            else if(item is IRotationHandle)
            {
                it = ElementViewType.RotationHandle;
            }
            else if(item is IMachineElement evm)
            {
                if(evm.LinkToParent != null)
                {
                    if(evm.LinkToParent.Type == Data.Enums.LinkType.Linear)
                    {
                        if (evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear)
                        {
                            switch (evm.LinkToParent.Direction)
                            {
                                case Data.Enums.LinkDirection.X:
                                    it = ElementViewType.XLinearTranslateEle;
                                    break;
                                case Data.Enums.LinkDirection.Y:
                                    it = ElementViewType.YLinearTranslateEle;
                                    break;
                                case Data.Enums.LinkDirection.Z:
                                    it = ElementViewType.ZLinearTranslateEle;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else if (evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Pneumatic)
                        {
                            switch (evm.LinkToParent.Direction)
                            {
                                case Data.Enums.LinkDirection.X:
                                    it = ElementViewType.XPneumaticTranslateEle;
                                    break;
                                case Data.Enums.LinkDirection.Y:
                                    it = ElementViewType.YPneumaticTranslateEle;
                                    break;
                                case Data.Enums.LinkDirection.Z:
                                    it = ElementViewType.ZPneumaticTranslateEle;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                    else if(evm.LinkToParent.Type == Data.Enums.LinkType.Rotary)
                    {
                        if(evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear)
                        {
                            throw new NotImplementedException();
                        }
                        else if(evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Pneumatic)
                        {
                            it = ElementViewType.PneumaticRotatyEle;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            return it;
        }
    }

    [ContentProperty("Then")]
    public class ItemModel3DTemplateSelectorItem
    {
        public ElementViewType When { get; set; }
        public DataTemplate Then { get; set; }
    }
}
