using Demo_API.Models;

namespace Demo_API.Iservice
{
    public interface ITargetAssetService
    {
        Task<IEnumerable<TargetAsset>> GetTargetAssets();
    }
}
