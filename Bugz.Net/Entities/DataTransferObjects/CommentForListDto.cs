using System;

namespace Entities.DataTransferObjects
{
    public class CommentForListDto
    {
        public string Message { get; set; }
        public string CommentBy { get; set; }
        public DateTime Posted { get; set; }
    }
}