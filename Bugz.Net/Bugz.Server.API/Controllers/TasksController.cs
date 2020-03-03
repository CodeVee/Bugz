using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
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
        public async Task<IActionResult> GetIssuesForProject(Guid projectId)
        {
            var tasksFromRepo = await _repository.Task.GetTasksForProject(projectId);

            var tasksToReturn = _mapper.Map<IEnumerable<TaskForDetailedDto>>(tasksFromRepo);

            return Ok(tasksToReturn);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetIssueForProject(Guid projectId, Guid taskId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var taskFromRepo = await _repository.Task.GetTask(taskId);
            if (taskFromRepo == null)
                return BadRequest("Task doesn't exist");

            if (taskFromRepo.ProjectId != projectFromRepo.ProjectId)
                return BadRequest("Task doesn't exist for current project");

            var taskToReturn = _mapper.Map<IssueForDetailedDto>(taskFromRepo);

            return Ok(taskToReturn);
        }
    }
}