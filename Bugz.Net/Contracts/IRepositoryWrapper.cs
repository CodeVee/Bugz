using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IProjectRepository Project { get; }
        ITaskRepository Task { get; }
        IIssueRepository Issue { get; }
        IHistoryRepository History { get; }
        ICommentRepository Comment { get; }
        Task<bool> SaveAsync();
    }
}