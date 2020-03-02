using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Enumerations;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class IssuesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public IssuesController(IRepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetIssuesForProject(Guid projectId)
        {
            var issuesFromRepo = await _repository.Issue.GetIssuesForProject(projectId);

            var issuesToReturn = _mapper.Map<IEnumerable<IssueForDetailedDto>>(issuesFromRepo);

            return Ok(issuesToReturn);
        }

        [HttpGet("{issueId}", Name = "GetIssue")]
        public async Task<IActionResult> GetIssueForProject(Guid projectId, Guid issueId)
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


        [HttpPost]
        public async Task<IActionResult> CreateIssueForProject(Guid projectId, IssueForCreationDto issueForCreation)
        {
            if (issueForCreation.DueDate != null && issueForCreation.DueDate < DateTime.Today)
                return BadRequest("Due Date cannot be before today");  

            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issueToCreate = _mapper.Map<Issue>(issueForCreation);
            var reporterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Severity severity;
            Classification classify;
            Stage status;

            Enum.TryParse(issueForCreation.Severity, out severity);
            Enum.TryParse(issueForCreation.Classification, out classify);          
            Enum.TryParse(issueForCreation.Status, out status);

            issueToCreate.Severity = severity;
            issueToCreate.Classification = classify;
            issueToCreate.Status = status;
            issueToCreate.ProjectId = projectId;
            issueToCreate.ReporterId = reporterId;
            issueToCreate.Created = DateTime.Now;

            _repository.Issue.CreateIssue(issueToCreate);
            var saveIssue = await _repository.SaveAsync();
            if (!saveIssue)
                throw new Exception("Failed to create issue for project");

            var issueToReturn = _mapper.Map<IssueForDetailedDto>(issueToCreate);
            return CreatedAtRoute("GetIssue", 
            new { projectId = issueToCreate.ProjectId, issueId = issueToCreate.IssueId}, issueToReturn);
        }
    }
}