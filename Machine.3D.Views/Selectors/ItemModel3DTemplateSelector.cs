using Machine._3D.Views.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Toolholder;
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

            //if ((item is IPanelHooker) || (item is PanelHolderViewModel))
            //{
            //    it = ElementViewType.PanelHandler;
            //}
            //else if (item is IPanelViewModel)
            //{
            //    it = ElementViewType.Panel;
            //}

            //if (item is PanelHolderViewModel)
            //{
            //    it = ElementViewType.PanelHolder;
            //}
            //else if (item is IPanelHooker)
            //{
            //    it = ElementViewType.PanelHooker;
            //}
            //else if (item is PointsDistanceViewModel)
            //{
            //    it = ElementViewType.PointDistance;
            //}

            if(item is ToolViewModel tvm)
            {
                it = ElementViewType.ToolEle;
            }
            else if(item is IPanelElement pvm)
            {
                it = (pvm is IPanel) ? ElementViewType.SectionedPanel : ElementViewType.Panel;
            }
            else if(item is AngularTransmissionViewModel)
            {
                it = ElementViewType.AngularTransmission;
            }
            else if(item is ATToolholderViewModel)
            {
                it = ElementViewType.ATToolholder;
            }
            else if(item is ToolholderElementViewModel thvm)
            {
                if (thvm.LinkToParent != null)
                {
                    if (thvm.LinkToParent.Type == Data.Enums.LinkType.Linear)
                    {
                        if ((thvm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Pneumatic) &&
                            (thvm.LinkToParent.Direction == Data.Enums.LinkDirection.Z))
                        {
                            it = ElementViewType.ToolHolderEleZPneu;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                    else if (thvm.LinkToParent.Type == Data.Enums.LinkType.Rotary)
                    {
                        if (thvm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Pneumatic)
                        {
                            it = ElementViewType.ToolHolderEleRotary;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                else
                {
                    it = ElementViewType.ToolHolderEle;
                }
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
