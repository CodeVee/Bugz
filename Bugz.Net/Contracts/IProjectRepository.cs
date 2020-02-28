using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        Task<IEnumerable<Project>> GetAllProjects();
        Task<Project> GetProject(Guid projectId);
        Task<bool> ProjectTitleIsUnique(string title);
        void CreateProject(Project project);
        void UpdateProject(Project project);
    }
}