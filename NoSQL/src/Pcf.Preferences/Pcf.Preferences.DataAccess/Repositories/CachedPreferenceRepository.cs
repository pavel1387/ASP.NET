using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.Core.Abstractions;
using Pcf.Preferences.Core.Domain;
using System.Diagnostics;
using System.Text.Json;

namespace Pcf.Preferences.DataAccess.Repositories;
public class CachedPreferenceRepository : IPreferenceRepository
{
    private readonly IDistributedCache _cache;
    private readonly IPreferenceRepository _preferenceRepository;
    private const string CacheKey = "preferences";

    public CachedPreferenceRepository(IDistributedCache cache, IPreferenceRepository preferenceRepository)
    {
        _cache = cache;
        _preferenceRepository = preferenceRepository;
    }

    public async Task<List<Preference>> GetAllAsync()
    {
        var cachedData = await _cache.GetStringAsync(CacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            Debug.WriteLine("get from cache");
            return JsonSerializer.Deserialize<List<Preference>>(cachedData);
        }
         
        var preferences = await _preferenceRepository.GetAllAsync();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        };
        await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(preferences), cacheOptions);

        Debug.WriteLine("get from db");
        return preferences;
    }

    public async Task<Preference> GetByIdAsync(Guid id)
    {
        var preferences = await GetAllAsync();
        return preferences.FirstOrDefault(p => p.Id == id);
    }

    public async Task<IEnumerable<Preference>> GetRangeByIdsAsync(List<Guid> ids)
    {
        var preferences = await GetAllAsync();
        return preferences.Where(p => ids.Contains(p.Id));
    }
}
