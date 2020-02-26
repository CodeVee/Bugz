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

            return Ok(projectFromRepo);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateProject(ProjectForCreationDto projectForCreation)
        {
            var startDateCheck = DateTime.Now < projectForCreation.StartDate;

            if (startDateCheck)
                return BadRequest("StartDate can't before Today");

            var endDateCheck = projectForCreation.EndDate < projectForCreation.StartDate;

            if (endDateCheck)
                return BadRequest("Start Date can't be less than End Date");
                
            var project = _mapper.Map<Project>(projectForCreation);

            _repository.Project.Create(project);

            if(await _repository.SaveAsync())
            {
                var projectToReturn = _mapper.Map<ProjectForDetailedDto>(project);
                return CreatedAtRoute("GetProject", new {projectId = projectToReturn.ProjectId }, projectToReturn);
            }

            throw new Exception($"Failed to Create Project");
        }
    }
}