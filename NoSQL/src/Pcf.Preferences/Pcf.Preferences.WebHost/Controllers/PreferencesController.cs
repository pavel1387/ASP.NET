using Microsoft.AspNetCore.Mvc;
using Pcf.Preferences.Core.Abstractions;
using Pcf.Preferences.WebHost.Models;

namespace Pcf.Preferences.WebHost.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class PreferencesController : ControllerBase
{
    private readonly ILogger<PreferencesController> _logger;
    private readonly IPreferenceRepository _preferencesRepository;


    public PreferencesController(ILogger<PreferencesController> logger, IPreferenceRepository preferencesRepository)
    {
        _logger = logger;
        _preferencesRepository = preferencesRepository;
    }

    /// <summary>
    /// Получить список предпочтений
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
    {
        var preferences = await _preferencesRepository.GetAllAsync();

        var response = preferences.Select(x => new PreferenceResponse()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Ok(response);
    }
}
