using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories.Abstractions
{
    public interface IPreferenceRepositoriy : IRepository<Preference>
    {
        Task<Preference?> GetByName(string name);
    }
}
