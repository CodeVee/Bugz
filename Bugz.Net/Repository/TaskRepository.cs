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
    public class TaskRepository : RepositoryBase<Entities.Models.Task>, ITaskRepository
    {
        public TaskRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateTask(Entities.Models.Task task)
        {
            Create(task);
        }

        public async Task<IEnumerable<Entities.Models.Task>> GetAllTasks()
        {
            return await FindAll().OrderBy(task => task.StartDate)
                        .Include(task => task.Creator)
                        .Include(task => task.Assignee)
                        .Include(task => task.Project)
                        .ToListAsync();
        }

        public async Task<Entities.Models.Task> GetTask(Guid taskId)
        {
            return await FindByCondition(task => task.TaskId.Equals(taskId))
                        .Include(task => task.Creator)
                        .Include(task => task.Assignee)
                        .Include(task => task.Project)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entities.Models.Task>> GetTasksForProject(Guid projectId)
        {
            return await FindByCondition(task => task.ProjectId.Equals(projectId))
                        .Include(task => task.Creator)
                        .Include(task => task.Assignee)
                        .Include(task => task.Project)
                        .ToListAsync();
        }

        public void UpdateTask(Entities.Models.Task task)
        {
            Update(task);
        }
    }
}