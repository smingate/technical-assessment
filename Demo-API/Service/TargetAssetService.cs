using Demo_API.Iservice;
using Demo_API.Models;
using Newtonsoft.Json;

namespace Demo_API.Service
{
    public class TargetAssetService : ITargetAssetService
    {
        private readonly HttpClient _httpClient;
        public TargetAssetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TargetAsset>> GetTargetAssets()
        {
            IEnumerable<TargetAsset> targetAssets = Enumerable.Empty<TargetAsset>();
            try
            {
                targetAssets = await getAssetsFromTargetApi();

                var currentDay = DateTime.Now.Day;

                if (currentDay == 3)
                {
                    targetAssets.Where(x => x != null && x.status == Status.Running.ToString()).Select(c =>
                    {
                        c.isPatchable = true;
                        return c;
                    }).ToList();
                }

                foreach (var item in targetAssets.Where(x => x != null))
                {
                    var count = 1;
                    var assetId = item.id;
                    var parentId = item.parentId;

                    while (true)
                    {
                        if (parentId != null)
                        {
                            var parentAsset = targetAssets.Where(x => x != null && x.id == parentId);
                            if (parentAsset.Any() && parentAsset.First().parentId == null)
                            {
                                break;
                            }
                            else if (parentAsset.Any() && parentAsset.First().parentId != assetId)
                            {
                                count++;
                                parentId = parentAsset.First().parentId;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            count = 0;
                            break;
                        }

                    }
                    item.parentTargetAssetCount = count;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return targetAssets;
        }

        private async Task<IEnumerable<TargetAsset>> getAssetsFromTargetApi()
        {
            var assets = new List<TargetAsset>();
            try
            {
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    assets = JsonConvert.DeserializeObject<List<TargetAsset>>(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return assets;
        }

    }
}
