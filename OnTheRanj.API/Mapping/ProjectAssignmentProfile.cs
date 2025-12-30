using AutoMapper;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Entities;

namespace OnTheRanj.API.Mapping;

public class ProjectAssignmentProfile : Profile
{
    public ProjectAssignmentProfile()
    {
        CreateMap<ProjectAssignment, ProjectAssignmentDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectCode.ProjectName));
        CreateMap<ProjectAssignmentDto, ProjectAssignment>();
    }
}
