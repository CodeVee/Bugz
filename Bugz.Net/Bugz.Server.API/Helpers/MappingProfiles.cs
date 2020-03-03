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
            CreateMap<Project, ProjectForListDto>();
            CreateMap<ProjectForUpdateDto, Project>();
            CreateMap<Project, ProjectForUpdateDto>();
            CreateMap<IssueForCreationDto, Issue>();
            CreateMap<Issue, IssueForListDto>();
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
            CreateMap<IssueForUpdateDto, Issue>();
            CreateMap<Issue, IssueForUpdateDto>();
            CreateMap<UserProject, UserForListDto>()
                .ForMember(dest => dest.FirstName, opt =>
                {
                    opt.MapFrom(src => src.User.FirstName);
                })
                .ForMember(dest => dest.LastName, opt =>
                {
                    opt.MapFrom(src => src.User.LastName);
                })
                .ForMember(dest => dest.Email, opt =>
                {
                    opt.MapFrom(src => src.User.Email);
                })
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.User.Id);
                });
            CreateMap<TaskForCreationDto, Task>();
            CreateMap<Task, TaskForDetailedDto>()
                .ForMember(dest => dest.Project, opt =>
                {
                    opt.MapFrom(src => src.Project.Title);
                })
                .ForMember(dest => dest.Creator, opt =>
                {
                    opt.MapFrom(src => src.Creator.FirstName);
                })
                .ForMember(dest => dest.Assignee, opt =>
                {
                    opt.MapFrom(src => src.Assignee.FirstName);
                });
            CreateMap<TaskForUpdateDto, Task>();
            CreateMap<Task, TaskForUpdateDto>();
        }
    }
}