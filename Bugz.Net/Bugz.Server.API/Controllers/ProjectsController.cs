using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public ProjectsController(IRepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projectsFromRepo = await _repository.Project.GetAllProjects();
            var projectsToReturn = _mapper.Map<IEnumerable<ProjectForListDto>>(projectsFromRepo);

            return Ok(projectsToReturn);
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var projectFromRepo = await _repository.Project.GetProjectWithCollections(projectId);

            if (projectFromRepo == null)
                return BadRequest("Project Not Found");

            var projectToReturn = _mapper.Map<ProjectForDetailedDto>(projectFromRepo);

            return Ok(projectToReturn);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateProject(ProjectForCreationDto projectForCreation)
        {
            var dateCheck = ValidateProjectDates(projectForCreation);
            if (dateCheck)
                return BadRequest("Start Date must be from today and less than End Date");

            var project = _mapper.Map<Project>(projectForCreation);
            _repository.Project.Create(project);

            var saveProject = await _repository.SaveAsync();
            if (!saveProject)
                throw new Exception($"Failed to Create Project");

            var projectToReturn = _mapper.Map<ProjectForDetailedDto>(project);
            return CreatedAtRoute("GetProject", new { projectId = projectToReturn.ProjectId }, projectToReturn);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, ProjectForUpdateDto projectForUpdate)
        {
            var dateCheck = ValidateProjectDates(projectForUpdate);
            if (dateCheck)
                return BadRequest("Start Date must be from today and less than End Date");

            var projectFromRepo = await _repository.Project.GetProject(projectId);

            if (projectFromRepo == null)
                return BadRequest("Invalid Request");

            _mapper.Map(projectForUpdate, projectFromRepo);
            _repository.Project.UpdateProject(projectFromRepo);

            var updateProject = await _repository.SaveAsync();
            if (!updateProject)
                throw new Exception("Failed to Update Project");

            return NoContent();
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Invalid Request");

            _repository.Project.DeleteProject(projectFromRepo);

            var deleteProject = await _repository.SaveAsync();
            if (!deleteProject)
                throw new Exception("Failed to Delete Project");

            return NoContent();
        }

        [HttpPost("{projectId}/assign/{email}")]
        public async Task<IActionResult> AssignUserToProject(Guid projectId, string email)
        {
            var project = await _repository.Project.GetProjectWithUsers(projectId);
            if (project == null)
                return BadRequest("Project Doesn't Exist");

            var user = await _repository.User.GetUser(email);
            if (user == null)
                return BadRequest("User doesn't exist");

            var userproject = project.Users.FirstOrDefault(up => up.UserId.Equals(user.Id));
            if (userproject != null)
                return NoContent();

            project.Users.Add(new UserProject { UserId = user.Id });
            _repository.Project.UpdateProject(project);

            var addUser = await _repository.SaveAsync();
            if (!addUser)
                return StatusCode(500);

            return NoContent();
        }

        [HttpPost("{projectId}/remove/{email}")]
        public async Task<IActionResult> RemoveUserFromProject(Guid projectId, string email)
        {
            var project = await _repository.Project.GetProjectWithUsers(projectId);
            if (project == null)
                return BadRequest("Project Doesn't Exist");

            var user = await _repository.User.GetUser(email);
            if (user == null)
                return BadRequest("User doesn't exist");

            var userproject = project.Users.FirstOrDefault(up => up.UserId.Equals(user.Id));
            _repository.Project.UpdateProject(project);
            project.Users.Remove(userproject);

            var removeUser = await _repository.SaveAsync();
            if (!removeUser)
                return StatusCode(500);

            return NoContent();
        }

        private bool ValidateProjectDates(ProjectDto projectDto)
        {
            var startDateCheck = projectDto.StartDate < DateTime.Today;
            var endDateCheck = projectDto.EndDate < projectDto.StartDate;

            return startDateCheck || endDateCheck;
        }
    }
}