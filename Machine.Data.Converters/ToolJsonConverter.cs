using Machine.Data.Converters.Helpers;
using Machine.Data.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Converters
{
    public class ToolJsonConverter : LinkJsonConverter
    {
        enum ToolTypes
        {
            SimpleTool,
            PointedTool,
            TwoSectionTool,
            CountersinkTool,
            DiskTool,
            DiskOnConeTool
        }

        public override bool CanConvert(Type objectType) => typeof(Tool).IsAssignableFrom(objectType);

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            return GetToolType(jo) switch
            {
                ToolTypes.SimpleTool => CreateAndCopy<SimpleTool>(jo, serializer),
                ToolTypes.PointedTool => CreateAndCopy<PointedTool>(jo, serializer),
                ToolTypes.TwoSectionTool => CreateAndCopy<TwoSectionTool>(jo, serializer),
                ToolTypes.CountersinkTool => CreateAndCopy<CountersinkTool>(jo, serializer),
                ToolTypes.DiskTool => CreateAndCopy<DiskTool>(jo, serializer),
                ToolTypes.DiskOnConeTool => CreateAndCopy<DiskOnConeTool>(jo, serializer),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static Tool CreateAndCopy<T>(JObject jObj, JsonSerializer serializer) where T : Tool, new()
        {
            var tool = new T();
            serializer.Populate(jObj.CreateReader(), tool);
            return tool;
        }

        private ToolTypes GetToolType(JObject jo)
        {
            var simpleFlags = new Flags(3);
            var pointedFlags = new Flags(4);
            var twoSectionFlags = new Flags(5);
            var countersinkFlags = new Flags(6);
            var diskFlags = new Flags(5);
            var diskOnConeFlags = new Flags(7);

            foreach (var item in jo.Properties())
            {
                var propName = item.Name.ToLower();
                    
                switch (propName)
                {
                    case "diameter":                    
                    case "usefullength":
                        simpleFlags.Add();
                        pointedFlags.Add();
                        diskFlags.Add();
                        countersinkFlags.Add();
                        twoSectionFlags.Add();
                        break;

                    case "length":
                        simpleFlags.Add();
                        return ToolTypes.SimpleTool;

                    case "straightlength":
                    case "coneheight":
                        pointedFlags.Add();
                        return ToolTypes.PointedTool;

                    case "cuttingradialthickness":
                    case "bodythickness":
                    case "cuttingthickness":
                    case "radialusefullength":
                        diskFlags.Add();
                        diskOnConeFlags.Add();
                        break;

                    case "postponemntlength":
                    case "postponemntdiameter":
                        diskOnConeFlags.Add();
                        return ToolTypes.DiskOnConeTool;

                    case "diameter1":
                    case "length1":
                    case "diameter2":
                    case "length2":
                        countersinkFlags.Add();
                        twoSectionFlags.Add();
                        break;

                    case "length3":
                        countersinkFlags.Add();
                        return ToolTypes.CountersinkTool;

                    default:
                        break;
                }
            }

            if (diskFlags.AllTrue()) return ToolTypes.DiskTool;
            else if (twoSectionFlags.AllTrue()) return ToolTypes.TwoSectionTool;
            else throw new ArgumentOutOfRangeException();
        }
    }
}
