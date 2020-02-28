using System;
using System.Collections.Generic;
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
            var projectsToReturn = _mapper.Map<IEnumerable<ProjectForDetailedDto>>(projectsFromRepo);

            return Ok(projectsToReturn);
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);

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
            if(!saveProject)
                throw new Exception($"Failed to Create Project");

            var projectToReturn = _mapper.Map<ProjectForDetailedDto>(project);
            return CreatedAtRoute("GetProject", new {projectId = projectToReturn.ProjectId }, projectToReturn);   
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
            if(!updateProject)
                throw new Exception($"Failed to Create Project");

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