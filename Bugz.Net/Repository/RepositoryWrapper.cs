using System.Threading.Tasks;
using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private IProjectRepository _project;
        private IIssueRepository _issue;
        private ITaskRepository _task;
        private ICommentRepository _comment;
        private IHistoryRepository _history;
        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;

        }
        public IUserRepository User
        {
            get
            {
                if (_user == null) _user = new UserRepository(_repositoryContext);
                return _user;
            }
        }

        public IProjectRepository Project
        {
            get
            {
                if (_project == null) _project = new ProjectRepository(_repositoryContext);
                return _project;
            }
        }

        public ITaskRepository Task
        {
            get
            {
                if (_task == null) _task = new TaskRepository(_repositoryContext);
                return _task;
            }
        }

        public IIssueRepository Issue
        {
            get
            {
                if (_issue == null) _issue = new IssueRepository(_repositoryContext);
                return _issue;
            }
        }

        public IHistoryRepository History
        {
            get
            {
                if (_history == null) _history = new HistoryRepository(_repositoryContext);
                return _history;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null) _comment = new CommentRepository(_repositoryContext);
                return _comment;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _repositoryContext.SaveChangesAsync() > 0;
        }
    }
}