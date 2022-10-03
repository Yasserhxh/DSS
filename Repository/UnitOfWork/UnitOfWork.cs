using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
    }
}
