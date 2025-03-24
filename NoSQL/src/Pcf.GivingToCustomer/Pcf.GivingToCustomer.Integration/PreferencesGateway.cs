using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Pcf.GivingToCustomer.Integration;
public class PreferencesGateway : IPreferencesGateway
{
    private readonly HttpClient _httpClient;

    public PreferencesGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Preference>> GetRangeByIdsAsync(List<Guid> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        var query = string.Join("&", ids.Select(id => $"ids={id}"));
        var requestUrl = $"api/v1/preferences/range?{query}";

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Preference>>();
    }
    public async Task<Preference> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/v1/preferences/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Preference>();
    }
}
