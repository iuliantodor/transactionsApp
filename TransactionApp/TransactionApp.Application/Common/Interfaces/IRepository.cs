using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Domain.Common;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Application.Common.Interfaces
{
    public interface IRepository
    {
        Task<PaginatedList<T>> GetPaginated<T>(int pageNumber, int pageSize) where T : BaseEntity;

        IQueryable<T> Get<T>() where T : BaseEntity;       

        Task<T?> GetByIdAsync<T>(Guid guid) where T : BaseEntity;

        T GetById<T>(Guid guid) where T : BaseEntity;

        Task<T> InsertAsync<T>(T value) where T : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void Update<T>(T value) where T : BaseEntity;

        void SoftDelete<T>(Guid guid) where T : BaseEntity;

        void HardDelete<T>(T value) where T : BaseEntity;
    }
}
