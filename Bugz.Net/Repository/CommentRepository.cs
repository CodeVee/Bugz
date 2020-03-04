using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateComment(Comment comment)
        {
            Create(comment);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForTask(Guid taskId)
        {
            return await FindByCondition(comment => comment.TaskId.Equals(taskId))
                        .Include(comment => comment.User)
                        .OrderByDescending(comment => comment.Posted)
                        .ToListAsync();
        }
    }
}