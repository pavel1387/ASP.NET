using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EmployeeRepositoriy : EfRepository<Employee>
    {
        public EmployeeRepositoriy(DataContext dataContext) : base(dataContext) { }
    }
}
