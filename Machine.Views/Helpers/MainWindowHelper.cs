using System.Collections.Generic;
using System.Linq;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using VMUI = Machine.ViewModels.UI;

namespace Machine.Views.Helpers
{
    public static class MainWindowHelper
    {
        public static MColor Convert(DColor color)
        {
            return new MColor()
            {
                A = color.A,
                B = color.B,
                G = color.G,
                R = color.R,
            };
        }

        public static DColor Convert(MColor color) => DColor.FromArgb(color.A, color.R, color.G, color.B);

        public static string Convert(ICollection<VMUI.IFlag> flags)
        {
            var d = flags.ToDictionary((o) => o.Name, (o) => o.Value.ToString());

            return Newtonsoft.Json.JsonConvert.SerializeObject(d, Newtonsoft.Json.Formatting.Indented);
        }

        public static string Convert(ICollection<VMUI.IOptionProvider> options)
        {
            var d = options.ToDictionary((o) => o.Name, (o) => o.ToString());

            return Newtonsoft.Json.JsonConvert.SerializeObject(d, Newtonsoft.Json.Formatting.Indented);
        }

        public static bool TryToParse(string data, ICollection<VMUI.IFlag> flags)
        {
            var result = true;
            Dictionary<string, string> dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            foreach (var item in flags)
            {
                if (dictionary.TryGetValue(item.Name, out string value))
                {
                    item.TryToParse(value);
                }
            }

            return result;
        }

        public static bool TryToParse(string data, ICollection<VMUI.IOptionProvider> options)
        {
            var result = true;
            Dictionary<string, string> dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            foreach (var item in options)
            {
                if (dictionary.TryGetValue(item.Name, out string value))
                {
                    item.TryToParse(value);
                }
            }

            return result;
        }

    }
}
