using AutoMapper;
using OnTheRanj.API.DTOs;
using OnTheRanj.API.Mapping;
using OnTheRanj.Core.Entities;

namespace OnTheRanj.Tests.Mapping;

public class ProjectAssignmentProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProjectAssignmentProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Test]
    public void ProjectAssignment_To_Dto_Maps_All_Fields()
    {
		    var entity = new ProjectAssignment
		    {
		        Id = 1,
		        Employee = new User { Id = 2, FullName = "John Doe" },
		        ProjectCode = new ProjectCode { Id = 3, ProjectName = "Test Project" },
		        StartDate = DateTime.Today,
		        EndDate = DateTime.Today.AddDays(5)
		    };

        var dto = _mapper.Map<ProjectAssignmentDto>(entity);

        Assert.That(dto.Id, Is.EqualTo(entity.Id));
        Assert.That(dto.EmployeeName, Is.EqualTo(entity.Employee.FullName));
        Assert.That(dto.ProjectName, Is.EqualTo(entity.ProjectCode.ProjectName));
        Assert.That(dto.StartDate, Is.EqualTo(entity.StartDate));
        Assert.That(dto.EndDate, Is.EqualTo(entity.EndDate));
    }

    [Test]
    public void Dto_To_ProjectAssignment_Maps_All_Fields()
    {
        var dto = new ProjectAssignmentDto
        {
            Id = 1,
            EmployeeName = "Jane Smith",
            ProjectName = "Another Project",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7)
        };

        var entity = _mapper.Map<ProjectAssignment>(dto);

        Assert.That(entity.Id, Is.EqualTo(dto.Id));
        Assert.That(entity.StartDate, Is.EqualTo(dto.StartDate));
        Assert.That(entity.EndDate, Is.EqualTo(dto.EndDate));
        // EmployeeName and ProjectName are not mapped back to navigation properties
    }
}
