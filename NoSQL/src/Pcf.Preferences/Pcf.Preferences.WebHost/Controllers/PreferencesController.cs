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

    /// <summary>
    /// Получить предпочтение по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PreferenceResponse>> GetPreferenceByIdAsync(Guid id)
    {
        var preference = await _preferencesRepository.GetByIdAsync(id);
        if (preference == null)
           return NotFound();
        
        var response = new PreferenceResponse()
        {
            Id = preference.Id,
            Name = preference.Name
        };

        return Ok(response);
    }

    /// <summary>
    /// Получить набор предпочтений по списку идентификаторов
    /// </summary>
    [HttpGet("range")]
    public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesRangeAsync([FromQuery] List<Guid> ids)
    {
        if (ids == null || !ids.Any())
            return BadRequest();
        
        var preferences = await _preferencesRepository.GetRangeByIdsAsync(ids);
        var response = preferences.Select(x => new PreferenceResponse()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Ok(response);
    }
}
