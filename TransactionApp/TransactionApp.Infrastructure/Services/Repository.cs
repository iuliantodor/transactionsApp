using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Domain.Common;
using TransactionApp.Infrastructure.Persistence;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Infrastructure.Services
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dataContext;
        public Repository(ApplicationDbContext applicationDbContext)
        {
            _dataContext = applicationDbContext;
        }

        public async Task<PaginatedList<T>> GetPaginated<T>(int pageNumber, int pageSize) where T : BaseEntity
        {
            var items = await _dataContext.Set<T>()
                                .AsNoTracking()
                                .Where(x => !x.IsDeleted)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return new PaginatedList<T>(items, items.Count, pageNumber, pageSize);
        }

        public async Task<T?> GetByIdAsync<T>(Guid guid) where T : BaseEntity
        {
            return await _dataContext.Set<T>()
                                .Where(x => !x.IsDeleted)
                                .FirstOrDefaultAsync(i => i.Id == guid);
        }

        public async Task<T> InsertAsync<T>(T value) where T : BaseEntity
        {
            return (await _dataContext.Set<T>().AddAsync(value)).Entity;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = await _dataContext.SaveChangesAsync(cancellationToken);
            return results;
        }

        public void Update<T>(T value) where T : BaseEntity
        {
            _dataContext.Set<T>().Update(value);
            _dataContext.Entry(value).Property(e => e.Id).IsModified = false;
        }

        public void SoftDelete<T>(Guid guid) where T : BaseEntity
        {
            var entity = GetById<T>(guid);

            if (entity == null)
            {
                throw new NotFoundException(nameof(T), guid);
            }

            entity.IsDeleted = true;

            Update(entity);
        }

        public void HardDelete<T>(T value) where T : BaseEntity
        {
            _dataContext.Set<T>().Remove(value);
        }

        public T? GetById<T>(Guid guid) where T : BaseEntity
        {
            IQueryable<T> query = _dataContext.Set<T>()
                                .Where(x => !x.IsDeleted);
            return query.FirstOrDefault(i => i.Id == guid);
        }

        public IQueryable<T> Get<T>() where T : BaseEntity
        {
            return _dataContext.Set<T>()
                                .Where(x => !x.IsDeleted);
        }
    }
}
