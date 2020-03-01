using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IIssueRepository : IRepositoryBase<Issue>
    {
        Task<IEnumerable<Issue>> GetAllIssues();
        Task<IEnumerable<Issue>> GetIssuesForProject(Guid projectId);
        Task<Issue> GetIssue(Guid issueId);
        void CreateIssue(Issue issue);
    }
}