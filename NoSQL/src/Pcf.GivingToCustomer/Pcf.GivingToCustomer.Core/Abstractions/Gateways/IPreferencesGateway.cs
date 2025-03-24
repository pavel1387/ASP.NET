using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Gateways;
public interface IPreferencesGateway
{
    Task<IEnumerable<Preference>> GetRangeByIdsAsync(List<Guid> ids);

    public Task<Preference> GetByIdAsync(Guid id);
}
