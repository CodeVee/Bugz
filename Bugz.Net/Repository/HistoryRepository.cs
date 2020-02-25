using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class HistoryRepository : RepositoryBase<History>, IHistoryRepository
    {
        public HistoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}