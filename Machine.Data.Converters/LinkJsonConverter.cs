using Machine.Data.Converters.Helpers;
using Machine.Data.Links;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Machine.Data.Converters
{
    public class LinkJsonConverter : JsonConverter
    {
        enum LinkType
        {
            Linear,
            Pneumatic
        }

 
        public override bool CanConvert(Type objectType) => typeof(Link).IsAssignableFrom(objectType);

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            return GetLinkType(jo) switch
            {
                LinkType.Linear => CreateAndCopy<LinearLink>(jo, serializer),
                LinkType.Pneumatic => CreateAndCopy<PneumaticLink>(jo, serializer),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static Link CreateAndCopy<T>(JObject jObj, JsonSerializer serializer) where T : Link, new()
        {
            var link = new T();
            serializer.Populate(jObj.CreateReader(), link);
            return link;
        }

        //private LinkType GetLinkType(JObject jo)
        //{
        //    var linearFlags = new Flags(3);
        //    var pneumaticFlags = new Flags(5);

        //    foreach (var item in jo.Properties())
        //    {
        //        switch (item.Name)
        //        {
        //            case "Min":
        //            case "Max":
        //            case "Pos":
        //            case "min":
        //            case "max":
        //            case "pos":
        //                if (linearFlags.Add()) return LinkType.Linear;
        //                break;

        //            case "OffPos":
        //            case "OnPos":
        //            case "TOn":
        //            case "TOff":
        //            case "ToolActivator":
        //            case "offPos":
        //            case "onPos":
        //            case "tOn":
        //            case "tOff":
        //            case "toolActivator":
        //                if (pneumaticFlags.Add()) return LinkType.Pneumatic;
        //                break;

        //            default:
        //               break;
        //        }
        //    }

        //    throw new ArgumentOutOfRangeException();
        //}

        private LinkType GetLinkType(JObject jo)
        {
            const int baseProps = 4;

            switch (jo.Count - baseProps)
            {
                case 3:
                    return LinkType.Linear;

                case 5:
                    return LinkType.Pneumatic;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
