using System;
using System.Collections.Generic;
using System.Linq;
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

            var issueFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issueFromRepo == null)
                return BadRequest("Issue doesn't exist");

            if (issueFromRepo.ProjectId != projectFromRepo.ProjectId)
                return BadRequest("Issue doesn't exist for current project");

            var issueToReturn = _mapper.Map<IssueForDetailedDto>(issueFromRepo);

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

            Severity severity;
            Classification classify;
            Stage status;

            Enum.TryParse(issueForCreation.Severity, out severity);
            Enum.TryParse(issueForCreation.Classification, out classify);
            Enum.TryParse(issueForCreation.Status, out status);

            var issueToCreate = _mapper.Map<Issue>(issueForCreation);
            var reporterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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
            new { projectId = issueToCreate.ProjectId, issueId = issueToCreate.IssueId }, issueToReturn);
        }

        [HttpPut("{issueId}")]
        public async Task<IActionResult> UpdateIssueForProject(Guid projectId, Guid issueId, IssueForUpdateDto issueForUpdate)
        {
            if (issueForUpdate.DueDate != null && issueForUpdate.DueDate < DateTime.Today)
                return BadRequest("Due Date cannot be before today");

            var projectFromRepo = await _repository.Project.GetProjectWithUsers(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issueFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issueFromRepo == null)
                return BadRequest("Issue doesn't exist");

            if (issueForUpdate.AssigneeEmail != null)
            {
                var user = await _repository.User.GetUser(issueForUpdate.AssigneeEmail);
                if (user == null)
                    return BadRequest("User doesn't exist");

                var userOnProject = projectFromRepo.Users.FirstOrDefault(up => up.UserId.Equals(user.Id));
                if (userOnProject == null)
                    return BadRequest("User not on project");

                issueFromRepo.AssigneeId = user.Id;
            }

            Severity severity;
            Classification classify;
            Stage status;

            Enum.TryParse(issueForUpdate.Severity, out severity);
            Enum.TryParse(issueForUpdate.Classification, out classify);
            Enum.TryParse(issueForUpdate.Status, out status);

            _mapper.Map(issueForUpdate, issueFromRepo);

            issueFromRepo.Severity = severity;
            issueFromRepo.Classification = classify;
            issueFromRepo.Status = status;

            _repository.Issue.UpdateIssue(issueFromRepo);
            var updateIssue = await _repository.SaveAsync();
            if (!updateIssue)
                throw new Exception("Failed to update issue for project");

            return NoContent();
        }

        [HttpDelete("{issueId}")]
        public async Task<IActionResult> DeleteIssue(Guid projectId, Guid issueId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issueFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issueFromRepo == null)
                return BadRequest("Issue doesn't exist");

            _repository.Issue.DeleteIssue(issueFromRepo);

            var deleteIssue = await _repository.SaveAsync();
            if (!deleteIssue)
                throw new Exception("Failed to Delete Issue");

            return NoContent();
        }

        [HttpGet("{issueId}/comments")]
        public async Task<IActionResult> GetCommentsForTask(Guid projectId, Guid issueId)
        {
            var projectFromRepo = await _repository.Project.GetProject(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issueFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issueFromRepo == null)
                return BadRequest("Issue doesn't exist");

            var comments = await _repository.Comment.GetCommentsForIssue(issueId);
            var commentsToReturn = _mapper.Map<IEnumerable<CommentForListDto>>(comments);

            return Ok(commentsToReturn);
        }

        [HttpPost("{issueId}/comments")]
        public async Task<IActionResult> CreateCommentForTask(Guid projectId, Guid issueId, CommentForCreationDto commentForCreation)
        {
            var projectFromRepo = await _repository.Project.GetProjectWithUsers(projectId);
            if (projectFromRepo == null)
                return BadRequest("Project doesn't exist");

            var issueFromRepo = await _repository.Issue.GetIssue(issueId);
            if (issueFromRepo == null)
                return BadRequest("Issue doesn't exist");

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userOnProject = projectFromRepo.Users.FirstOrDefault(up => up.UserId.Equals(userId));
            if (userOnProject == null)
                return BadRequest("User not on project");

            var commentToCreate = _mapper.Map<Comment>(commentForCreation);
            commentToCreate.Posted = DateTime.Now;
            commentToCreate.UserId = userId;
            commentToCreate.IssueId = issueId;

            _repository.Comment.CreateComment(commentToCreate);
            var saveComment = await _repository.SaveAsync();
            if (!saveComment)
                throw new Exception("Failed to Create Comment");

            return NoContent();
        }
    }
}