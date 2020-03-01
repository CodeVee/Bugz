using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly RepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public IssuesController(RepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetIssues(Guid projectId)
        {
            var issuesFromRepo = await _repository.Issue.GetIssuesForProject(projectId);

            var issuesToReturn = _mapper.Map<IEnumerable<IssueForDetailedDto>>(issuesFromRepo);

            return Ok(issuesToReturn);
        }

        [HttpGet("{issueId}")]
        public async Task<IActionResult> GetIssue(Guid projectId, Guid issueId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issuesFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issuesFromRepo == null)
                return BadRequest("Issue doesn't exist");

            if (issuesFromRepo.ProjectId != projectFromRepo.ProjectId)
                return BadRequest("Issue doesn't exist for current project");

            var issueToReturn = _mapper.Map<IssueForDetailedDto>(issuesFromRepo);

            return Ok(issueToReturn);
        }


        
    }
}