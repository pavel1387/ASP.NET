using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PreferenceRepositoriy : EfRepository<Preference>
    {
        public PreferenceRepositoriy(DataContext dataContext) : base(dataContext) { }
    }
}
