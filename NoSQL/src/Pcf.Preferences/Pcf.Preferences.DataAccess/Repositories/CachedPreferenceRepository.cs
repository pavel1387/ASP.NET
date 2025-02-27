using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.Core.Abstractions;
using Pcf.Preferences.Core.Domain;
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
            return JsonSerializer.Deserialize<List<Preference>>(cachedData);

        var preferences = await _preferenceRepository.GetAllAsync();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
        };
        await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(preferences), cacheOptions);

        return preferences;
    }
}
