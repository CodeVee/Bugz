namespace Entities.DataTransferObjects
{
    public class TaskForDetailedDto : TaskForListDto
    {
        public string Creator { get; set; }
        public string Assignee { get; set; }
        public string Project { get; set; }
    }
}