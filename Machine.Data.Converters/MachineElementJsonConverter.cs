using Machine.Data.Converters.Helpers;
using Machine.Data.MachineElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Machine.Data.Converters
{
    public class MachineElementJsonConverter : JsonConverter
    {
        enum MachineElementsTypes
        {
            MachineElement,
            ColliderElement,
            InjectorElement,
            InserterElement,
            PanelHolderElement,
            RootElement,
            ToolholderElement
        }

        public override bool CanConvert(Type objectType) => typeof(MachineElement).IsAssignableFrom(objectType);

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            return GetLinkType(jo) switch
            {
                MachineElementsTypes.ColliderElement => CreateAndCopy<ColliderElement>(jo, serializer),
                MachineElementsTypes.InjectorElement => CreateAndCopy<InjectorElement>(jo, serializer),
                MachineElementsTypes.InserterElement => CreateAndCopy<InserterElement>(jo, serializer),
                MachineElementsTypes.MachineElement => CreateAndCopy<MachineElement>(jo, serializer),
                MachineElementsTypes.PanelHolderElement => CreateAndCopy<PanelHolderElement>(jo, serializer),
                MachineElementsTypes.RootElement => CreateAndCopy<RootElement>(jo, serializer),
                MachineElementsTypes.ToolholderElement => CreateAndCopy<ToolholderElement>(jo, serializer),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static MachineElement CreateAndCopy<T>(JObject jObj, JsonSerializer serializer) where T : MachineElement, new()
        {
            var link = new T();
            serializer.Populate(jObj.CreateReader(), link);
            return link;
        }

        private MachineElementsTypes GetLinkType(JObject jo)
        {
            var colliderFlags = new Flags(2);
            var injectorFlags = new Flags(4);
            var inserterFlags = new Flags(8);
            var panelHolderFlags = new Flags(4);
            var rootElementFlags = new Flags(2);
            var toolholderFlags = new Flags(4);

            foreach (var item in jo.Properties())
            {
                switch (item.Name)
                {
                    case "Radius":
                    case "Points":
                    case "radius":
                    case "points":
                        if (colliderFlags.Add()) return MachineElementsTypes.ColliderElement;
                        break;

                    case "InserterId":
                    case "InserterColor":
                    case "Length":
                    case "LoaderLinkId":
                    case "DischargerLinkId":
                    case "Diameter":
                    case "inserterId":
                    case "inserterColor":
                    case "length":
                    case "loaderLinkId":
                    case "dischargerLinkId":
                    case "diameter":
                        injectorFlags.Add();
                        inserterFlags.Add();
                        break;

                    case "PanelHolderId":
                    case "PanelHolderName":
                    case "Corner":
                    case "panelHolderId":
                    case "panelHolderName":
                    case "corner":
                        if (panelHolderFlags.Add()) return MachineElementsTypes.PanelHolderElement;
                        break;

                    case "AssemblyName":
                    case "RootType":
                    case "assemblyName":
                    case "rootType":
                        if (rootElementFlags.Add()) return MachineElementsTypes.RootElement;
                        break;

                    case "ToolHolderId":
                    case "ToolHolderType":
                    case "toolHolderId":
                    case "toolHolderType":
                        if (toolholderFlags.Add()) return MachineElementsTypes.ToolholderElement;
                        break;

                    case "Position":
                    case "position":
                        injectorFlags.Add();
                        inserterFlags.Add();
                        if (panelHolderFlags.Add()) return MachineElementsTypes.PanelHolderElement;
                        if (toolholderFlags.Add()) return MachineElementsTypes.ToolholderElement;
                        break;


                    case "Direction":
                    case "direction":
                        injectorFlags.Add();
                        inserterFlags.Add();
                        if (toolholderFlags.Add()) return MachineElementsTypes.ToolholderElement;
                        break;

                    default:
                        break;
                }
            }

            if (inserterFlags.AllTrue())
            {
                return MachineElementsTypes.InserterElement;
            }
            else if (injectorFlags.AllTrue())
            {
                return MachineElementsTypes.InjectorElement;
            }
            else
            {
                return MachineElementsTypes.MachineElement;
            }
        }

        //private MachineElementsTypes GetLinkType(JObject jo)
        //{
        //    const int baseProps = 7;

        //    switch (jo.Count - baseProps)
        //    {
        //        case 2:
        //            return GetLinkType2(jo);

        //        case 4:
        //            return GetLinkType4(jo);

        //        case 8:
        //            return MachineElementsTypes.InserterElement;

        //        default:
        //            return MachineElementsTypes.MachineElement;
        //    }
        //}

        //private MachineElementsTypes GetLinkType2(JObject jo)
        //{
        //    foreach (var item in jo.Properties())
        //    {
        //        switch (item.Name)
        //        {
        //            case "Radius":
        //            case "Points":
        //            case "radius":
        //            case "points":
        //                return MachineElementsTypes.ColliderElement;

        //            case "AssemblyName":
        //            case "RootType":
        //            case "assemblyName":
        //            case "rootType":
        //                return MachineElementsTypes.RootElement;

        //            default:
        //                break;
        //        }
        //    }

        //    throw new ArgumentOutOfRangeException();
        //}

        //private MachineElementsTypes GetLinkType4(JObject jo)
        //{
        //    foreach (var item in jo.Properties())
        //    {
        //        switch (item.Name)
        //        {
        //            case "InserterId":
        //            case "InserterColor":
        //            case "inserterId":
        //            case "inserterColor":
        //                return MachineElementsTypes.InjectorElement;

        //            case "PanelHolderId":
        //            case "PanelHolderName":
        //            case "Corner":
        //            case "panelHolderId":
        //            case "panelHolderName":
        //            case "corner":
        //                return MachineElementsTypes.PanelHolderElement;

        //            case "ToolHolderId":
        //            case "ToolHolderType":
        //            case "toolHolderId":
        //            case "toolHolderType":
        //                return MachineElementsTypes.ToolholderElement;

        //            default:
        //                break;
        //        }
        //    }

        //    throw new ArgumentOutOfRangeException();
        //}
    }
}
