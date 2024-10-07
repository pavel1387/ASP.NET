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

            var tables = _dbContext.Model.GetEntityTypes()
                        .Select(t => t.GetTableName())
                        .ToList();

            if (!tables.Contains("Employees"))
            {
                _dbContext.AddRange(FakeDataFactory.Employees);
                _dbContext.SaveChanges();
            }
            if (!tables.Contains("Preferences"))
            {
                _dbContext.AddRange(FakeDataFactory.Preferences);
                _dbContext.SaveChanges();
            }
            if (!tables.Contains("Customers"))
            {
                _dbContext.AddRange(FakeDataFactory.Customers);
                _dbContext.SaveChanges();
            }
        }
    }
}
