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
            CreateMap<Issue, IssueForDetailedDto>()
                .ForMember(dest => dest.Project, opt => 
                {
                    opt.MapFrom(src => src.Project.Title);
                })
                .ForMember(dest => dest.Reporter, opt => 
                {
                    opt.MapFrom(src => src.Reporter.FirstName);
                })
                .ForMember(dest => dest.Assignee, opt =>
                {
                    opt.MapFrom(src => src.Assignee.FirstName);
                });
        }
    }
}