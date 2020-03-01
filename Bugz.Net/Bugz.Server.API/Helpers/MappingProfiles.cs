using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Bugz.Server.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForDetailedDto>();
            CreateMap<User, UserForListDto>();
            CreateMap<ProjectForCreationDto, Project>();
            CreateMap<Project, ProjectForDetailedDto>();
            CreateMap<ProjectForUpdateDto, Project>();
            CreateMap<Project, ProjectForUpdateDto>();
            CreateMap<IssueForCreationDto, Issue>();
            CreateMap<Issue, IssueForDetailedDto>();
        }
    }
}