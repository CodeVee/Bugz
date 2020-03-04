using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsForTask(Guid taskId);
        Task<IEnumerable<Comment>> GetCommentsForIssue(Guid issueId);
        void CreateComment(Comment comment);
    }
}