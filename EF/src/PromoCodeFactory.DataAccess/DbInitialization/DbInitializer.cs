using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.DbInitialization
{
    public class DbInitializer 
    {
        protected readonly DataContext _dbContext;

        public DbInitializer(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.Migrate();

            if (_dbContext.Employees.Count() == 0)
            {
                _dbContext.AddRange(FakeDataFactory.Employees);
                _dbContext.SaveChanges();
            }
            if (_dbContext.Preferences.Count() == 0)
            {
                _dbContext.AddRange(FakeDataFactory.Preferences);
                _dbContext.SaveChanges();
            }
            if (_dbContext.Customers.Count() ==0)
            {
                _dbContext.AddRange(FakeDataFactory.Customers);
                _dbContext.SaveChanges();
            }
        }
    }
}
