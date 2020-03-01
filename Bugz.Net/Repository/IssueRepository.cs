using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
    {
        public IssueRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Issue>> GetAllIssues()
        {
            throw new NotImplementedException();
        }

        public Task<Issue> GetIssue(Guid issueId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Issue>> GetIssuesForProject(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}