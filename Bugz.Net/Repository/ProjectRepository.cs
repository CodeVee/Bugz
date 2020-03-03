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
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateProject(Project project)
        {
            Create(project);
        }

        public void DeleteProject(Project project)
        {
            Delete(project);
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await FindAll()
                    .OrderBy(project => project.StartDate)
                    .ToListAsync();
        }

        public async Task<Project> GetProject(Guid projectId)
        {
            return await FindByCondition(project => project.ProjectId.Equals(projectId))
                            .FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectWithCollections(Guid projectId)
        {
            return await FindByCondition(project => project.ProjectId.Equals(projectId))
                        .Include(project => project.Users)
                            .ThenInclude(userproject => userproject.User)
                        .Include(project => project.Issues)
                        .Include(project => project.Tasks)
                        .FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectWithUsers(Guid projectId)
        {
            return await FindByCondition(project => project.ProjectId.Equals(projectId))
                        .Include(project => project.Users)
                        .FirstOrDefaultAsync();
        }

        public async Task<bool> ProjectTitleIsUnique(string title)
        {
            var project = await FindByCondition(project => project.Title.Equals(title))
                                .FirstOrDefaultAsync();
            return project.Title.Equals(title);
        }

        public void UpdateProject(Project project)
        {
            Update(project);
        }
    }
}