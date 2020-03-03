using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Task = Entities.Models.Task;

namespace Contracts
{
    public interface ITaskRepository : IRepositoryBase<Task>
    {
        Task<IEnumerable<Task>> GetAllTasks();
        Task<IEnumerable<Task>> GetTasksForProject(Guid projectId);
        Task<Task> GetTask(Guid taskId);
        void CreateTask(Task task);
    }
}