using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.Core.Abstractions;

public interface IPreferenceRepository
{
    Task<List<Preference>> GetAllAsync();
    Task<Preference> GetByIdAsync(Guid id);
    Task<IEnumerable<Preference>> GetRangeByIdsAsync(List<Guid> ids);
}