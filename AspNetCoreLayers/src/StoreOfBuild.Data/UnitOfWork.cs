using System.Threading.Tasks;
using StoreOfBuild.Data.Contexts;
using StoreOfBuild.Domain;

namespace StoreOfBuild.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
        {
            await this.context.SaveChangesAsync();
        }
    }
}