using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PromocodeRepositoriy : EfRepository<PromoCode>
    {
        public PromocodeRepositoriy(DataContext dataContext) : base(dataContext) { }
    }
}
