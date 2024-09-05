using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> CreateAsync(T obj, CancellationToken cancellationToken = default)
        {
            obj.Id = Guid.NewGuid();
            Data = Data.Append(obj);
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == obj.Id));
        }

        public Task<T> UpdateAsync(T obj, CancellationToken cancellationToken = default)
        {
            Data = Data.Where(x => x.Id != obj.Id);
            Data = Data.Append(obj);
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == obj.Id));
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var count = Data.Count();
            Data = Data.Where(x => x.Id != id);
            return Task.FromResult(Data.Count() < count);
        }
    }
}