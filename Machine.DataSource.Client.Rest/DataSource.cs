using Machine.Data;
using Machine.Data.Converters;
using Machine.ViewModels;
using Machine.ViewModels.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using MD = Machine.Data.MachineElements;
using MDTooling = Machine.Data.Toolings;
using MDTools = Machine.Data.Tools;
using System.Threading.Tasks;
using Machine.ViewModels.Messages.Tooling;
using Machine.Data.Extensions.ViewModels;

namespace Machine.DataSource.Client.Rest
{
    public class DataSource : DataSourceBase, IDataSource, INameProvider
    {
        private IKernelViewModel _kernel;
        public IKernelViewModel Kernel => _kernel ?? (_kernel = GetInstance<IKernelViewModel>());

        public override string Name => "Client.REST";

        protected override async void LoadMachineCommandImplementation()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Machine");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<MachineInfo>>();
                    var machineInfo = SelectMachine(content);

                    if(machineInfo != null)
                    {
                        var rr = await client.GetAsync($"https://localhost:44306/api/Machine/{machineInfo.MachineElementID}");

                        if (rr.IsSuccessStatusCode)
                        {
                            var m = await rr.Content.ReadAsAsync<MD.MachineElement>(new[]
                            {
                                new JsonMediaTypeFormatter()
                                {
                                    SerializerSettings = new JsonSerializerSettings()
                                    {
                                        Converters = new List<JsonConverter>()
                                                    {
                                                        new LinkJsonConverter(),
                                                        new MachineElementJsonConverter()
                                                    },
                                        NullValueHandling = NullValueHandling.Ignore,
                                    }
                                }
                            });

                            if (m != null) Kernel.Machines.Add(m.ToViewModel());
                        }
                    }
                }
            }
        }

        protected override async void LoadToolingCommandImplementation()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Tooling");

                if (response.IsSuccessStatusCode)
                {
                    var dlg = ViewModels.Ioc.SimpleIoc<IListDialog>.GetInstance();
                    var content = await response.Content.ReadAsAsync<IEnumerable<MDTooling.Tooling>>();

                    dlg.Title = "Select tooling";
                    dlg.Options = content.Select(m => m.Name);

                    if(dlg.ShowDialog())
                    {
                        var tooling = content.FirstOrDefault(t => string.Compare(t.Name, dlg.Selection) == 0);

                        if (tooling != null)
                        {
                            var toolSet = await LoadTools(tooling.Tools);

                            if (toolSet != null) SetTooling(tooling, toolSet);
                        }
                    }
                }
            }
        }

        private async Task<MDTools.ToolSet> LoadTools(string toolSetName)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44306/api/Tools");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsAsync<IEnumerable<MDTools.ToolsetInfo>>();
                    var toolSetInfo = content.FirstOrDefault(t => string.Compare(t.Name, toolSetName) == 0);
                    
                    if(toolSetInfo != null)
                    {
                        var rr = await client.GetAsync($"https://localhost:44306/api/Tools/{toolSetInfo.ToolSetID}");

                        if (rr.IsSuccessStatusCode)
                        {
                            var toolSet = await rr.Content.ReadAsAsync<MDTools.ToolSet>(new[]
                            {
                                new JsonMediaTypeFormatter()
                                {
                                    SerializerSettings = new JsonSerializerSettings()
                                    {
                                        Converters = new List<JsonConverter>()
                                        {
                                            new ToolJsonConverter()
                                        },
                                        NullValueHandling = NullValueHandling.Ignore
                                    }
                                }
                            });

                            return toolSet;
                        }
                    }
                }
            }

            return null;
        }

        private void SetTooling(MDTooling.Tooling tooling, MDTools.ToolSet toolset)
        {
            foreach (var item in tooling.Units)
            {
                var t = toolset.Tools.FirstOrDefault(e => string.Compare(e.Name, item.ToolName) == 0);

                if (t != null)
                {
                    Messenger.Send(new LoadToolMessage() { ToolHolder = item.ToolHolderId, Tool = t });
                }
            }
        }

        private MachineInfo SelectMachine(IEnumerable<MachineInfo> content)
        {
            var options = content.Select(m => m.Name);
            var dlg = ViewModels.Ioc.SimpleIoc<IListDialog>.GetInstance();

            dlg.Title = "Select machine";
            dlg.Options = options;

            if(dlg.ShowDialog())
            {
                return content.First(m => string.Compare(m.Name, dlg.Selection) == 0);
            }
            else
            {
                return null;
            }
        }
    }
}
