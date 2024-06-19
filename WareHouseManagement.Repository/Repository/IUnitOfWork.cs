using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();

        Task<int> CommitAsync();

        IGenericRepository<TEntity> GetRepository<TEntity>()
           where TEntity : class;
    }
}
