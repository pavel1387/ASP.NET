namespace Pcf.Preferences.WebHost.Models;

public record PreferenceResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

