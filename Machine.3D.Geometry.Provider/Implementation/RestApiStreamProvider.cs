using Machine._3D.Geometry.Provider.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Machine._3D.Geometry.Provider.Implementation
{
    public class RestApiStreamProvider : IStreamProvider
    {
        private Dictionary<string, int> _infos;

        public Stream GetStream(string name)
        {
            byte[] data = null;
            var mres = new ManualResetEventSlim();

            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var id = await GetIdAsync(name);

                    if(id >= 0)
                    {
                        var response = await client.GetAsync($"https://localhost:44306/api/Models/{id}");

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsAsync<Mesh.Data.Mesh>();

                            data = content.Data;
                        }
                    }
                }

                mres.Set();
            });

            mres.Wait();
            mres.Reset();

            return (data != null) ? new MemoryStream(data) : null;
        }

        private Task<int> GetIdAsync(string name)
        {
            return Task.Run(async () =>
            {
                if (_infos == null) _infos = await GetInfosAsync();

                var modelName = Filter(name);

                return _infos.TryGetValue(modelName, out int id) ? id : -1;
            });
        }

        private string Filter(string name)
        {
            if(name.Contains('.') || name.Contains('\\'))
            {
                return Path.GetFileNameWithoutExtension(name);
            }
            else
            {
                return name;
            }
        }

        private async Task<Dictionary<string, int>> GetInfosAsync()
        {
            Dictionary<string, int> dictionary = null;

            await Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:44306/api/Models");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsAsync<IEnumerable<Mesh.Data.MeshInfo>>();

                        dictionary = content.ToDictionary(e => e.Name, e => e.MeshID);
                    }
                }
            });

            return dictionary;
        }
    }
}
