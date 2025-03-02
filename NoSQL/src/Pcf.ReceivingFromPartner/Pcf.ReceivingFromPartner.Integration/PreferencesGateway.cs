using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration;
public class PreferencesGateway : IPreferencesGateway
{
    private readonly HttpClient _httpClient;

    public PreferencesGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Preference> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/v1/preferences/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Preference>();
    }


}
