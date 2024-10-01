using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PreferenceRepositoriy :  EfRepository<Preference>, IPreferenceRepositoriy
    {
        public PreferenceRepositoriy(DataContext dataContext) : base(dataContext) { }

        public async Task<Preference?> GetByName(string name)
        {
            return await _entitySet.FirstOrDefaultAsync(x => x.Name == name); ;
        }
    }
}
