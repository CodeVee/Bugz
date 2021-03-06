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
        Task<Project> GetProjectWithCollections(Guid projectId);
        Task<Project> GetProjectWithUsers(Guid projectId);
        Task<bool> ProjectTitleIsUnique(string title);
        void CreateProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(Project project);
    }
}