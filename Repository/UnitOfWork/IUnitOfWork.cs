using Microsoft.EntityFrameworkCore.Storage;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> Complete();
        IDbContextTransaction BeginTransaction();
    }
}
