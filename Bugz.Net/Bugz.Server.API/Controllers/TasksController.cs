using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public TasksController(IRepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetTasksForProject(Guid projectId)
        {
            var tasksFromRepo = await _repository.Task.GetTasksForProject(projectId);

            var tasksToReturn = _mapper.Map<IEnumerable<TaskForDetailedDto>>(tasksFromRepo);

            return Ok(tasksToReturn);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTaskForProject(Guid projectId, Guid taskId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var taskFromRepo = await _repository.Task.GetTask(taskId);
            if (taskFromRepo == null)
                return BadRequest("Task doesn't exist");

            if (taskFromRepo.ProjectId != projectFromRepo.ProjectId)
                return BadRequest("Task doesn't exist for current project");

            var taskToReturn = _mapper.Map<TaskForDetailedDto>(taskFromRepo);

            return Ok(taskToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskForProject(Guid projectId, TaskForCreationDto taskForCreation)
        {
            if (taskForCreation.StartDate < DateTime.Today)
                return BadRequest("Start Date cannot be before today");

            if (taskForCreation.EndDate < taskForCreation.StartDate)
                return BadRequest("End Date cannot be before Start Date");

            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            Status status;
            Priority priority;
            Completion percentage;

            Enum.TryParse(taskForCreation.Status, out status);
            Enum.TryParse(taskForCreation.Percentage, out percentage);
            Enum.TryParse(taskForCreation.Priority, out priority);

            var taskToCreate = _mapper.Map<Entities.Models.Task>(taskForCreation);
            var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            taskToCreate.Priority = priority;
            taskToCreate.Percentage = percentage;
            taskToCreate.Status = status;
            taskToCreate.ProjectId = projectId;
            taskToCreate.CreatorId = creatorId;

            _repository.Task.CreateTask(taskToCreate);
            var saveTask = await _repository.SaveAsync();
            if (!saveTask)
                throw new Exception("Failed to create task for project");

            var taskToReturn = _mapper.Map<TaskForDetailedDto>(taskToCreate);
            return CreatedAtRoute("GetTask",
            new { projectId = taskToCreate.ProjectId, taskId = taskToCreate.TaskId }, taskToReturn);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskForProject(Guid projectId, Guid taskId, TaskForUpdateDto taskForUpdate)
        {
            if (taskForUpdate.StartDate < DateTime.Today)
                return BadRequest("Start Date cannot be before today");

            if (taskForUpdate.EndDate < taskForUpdate.StartDate)
                return BadRequest("End Date cannot be before Start Date");

            var projectFromRepo = await _repository.Project.GetProjectWithUsers(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var taskFromRepo = await _repository.Task.GetTask(taskId);
            if (taskFromRepo == null)
                return BadRequest("Task does not exist");

            if (taskForUpdate.AssigneeEmail != null)
            {
                var user = await _repository.User.GetUser(taskForUpdate.AssigneeEmail);
                if (user == null)
                    return BadRequest("User doesn't exist");

                var userOnProject = projectFromRepo.Users.FirstOrDefault(up => up.UserId.Equals(user.Id));
                if (userOnProject == null)
                    return BadRequest("User not on project");

                taskFromRepo.AssigneeId = user.Id;
            }

            Status status;
            Priority priority;
            Completion percentage;

            Enum.TryParse(taskForUpdate.Status, out status);
            Enum.TryParse(taskForUpdate.Percentage, out percentage);
            Enum.TryParse(taskForUpdate.Priority, out priority);

            _mapper.Map(taskForUpdate, taskFromRepo);

            taskFromRepo.Status = status;
            taskFromRepo.Percentage = percentage;
            taskFromRepo.Priority = priority;

            _repository.Task.UpdateTask(taskFromRepo);
            var updateTask = await _repository.SaveAsync();
            if (!updateTask)
                throw new Exception("Failed to update task");

            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var taskFromRepo = await _repository.Task.GetTask(taskId);
            if (taskFromRepo == null)
                return BadRequest("Task doesn't exist");

            _repository.Task.DeleteTask(taskFromRepo);

            var deleteTask = await _repository.SaveAsync();
            if (!deleteTask)
                throw new Exception("Failed to Delete Task");

            return NoContent();
        }
    }
}