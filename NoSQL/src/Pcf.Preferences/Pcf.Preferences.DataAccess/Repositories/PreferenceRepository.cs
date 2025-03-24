using Microsoft.EntityFrameworkCore;
using Pcf.Preferences.Core.Abstractions;
using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.DataAccess.Repositories;

public class PreferenceRepository : IPreferenceRepository
{
    private readonly DataContext _dataContext;

    public PreferenceRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Preference>> GetAllAsync()
    {
        var entities = await _dataContext.Set<Preference>().ToListAsync();

        return entities;
    }

    public async Task<Preference> GetByIdAsync(Guid id)
    {
        var entity = await _dataContext.Set<Preference>().FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<IEnumerable<Preference>> GetRangeByIdsAsync(List<Guid> ids)
    {
        var entities = await _dataContext.Set<Preference>().Where(x => ids.Contains(x.Id)).ToListAsync();
        return entities;
    }
}