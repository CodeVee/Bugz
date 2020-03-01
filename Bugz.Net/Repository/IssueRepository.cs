using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
    {
        public IssueRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateIssue(Issue issue)
        {
            Create(issue);
        }

        public async Task<IEnumerable<Issue>> GetAllIssues()
        {
            return await FindAll().OrderBy(issue => issue.Created)
                        .ToListAsync();
        }

        public async Task<Issue> GetIssue(Guid issueId)
        {
            return await FindByCondition(issue => issue.IssueId.Equals(issueId))
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesForProject(Guid projectId)
        {
            return await FindByCondition(issue => issue.ProjectId.Equals(projectId))
                        .ToListAsync();
        }
    }
}