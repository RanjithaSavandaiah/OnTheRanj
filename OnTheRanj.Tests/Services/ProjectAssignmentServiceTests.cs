
using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services.Implementations;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class ProjectAssignmentServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private ProjectAssignmentService _service;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new ProjectAssignmentService(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllProjectAssignmentsAsync_ReturnsAssignments()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetAllProjectAssignmentsAsync()).ReturnsAsync(assignments);
        var result = await _service.GetAllProjectAssignmentsAsync();
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetAllProjectAssignmentsAsync_ReturnsEmptyList_WhenNoAssignments()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetAllProjectAssignmentsAsync()).ReturnsAsync(new List<ProjectAssignment>());
        var result = await _service.GetAllProjectAssignmentsAsync();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllProjectAssignmentsAsync_ThrowsException_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetAllProjectAssignmentsAsync()).ThrowsAsync(new System.Exception("DB error"));
        Assert.ThrowsAsync<System.Exception>(async () => await _service.GetAllProjectAssignmentsAsync());
    }

    [Test]
    public async Task UpdateProjectAssignmentAsync_UpdatesFields()
    {
        var assignment = new ProjectAssignment { Id = 1, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(5), ProjectCodeId = 2, EmployeeId = 3 };
        var existing = new ProjectAssignment { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetByIdAsync(1)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.UpdateAsync(existing)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.UpdateProjectAssignmentAsync(assignment);
        Assert.That(result, Is.Not.Null);
        Assert.That(existing.StartDate, Is.EqualTo(assignment.StartDate));
        _unitOfWorkMock.Verify(u => u.ProjectAssignments.UpdateAsync(existing), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public async Task UpdateProjectAssignmentAsync_ReturnsNull_WhenNotFound()
    {
        var assignment = new ProjectAssignment { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetByIdAsync(1)).ReturnsAsync((ProjectAssignment?)null);
        var result = await _service.UpdateProjectAssignmentAsync(assignment);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AssignProjectToEmployeeAsync_Success()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync(new User { Id = 3, Role = UserRole.Employee });
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Active });
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.AddAsync(It.IsAny<ProjectAssignment>())).ReturnsAsync((ProjectAssignment p) => p);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, DateTime.Today.AddDays(5), 1);
        Assert.That(result.EmployeeId, Is.EqualTo(3));
        Assert.That(result.ProjectCodeId, Is.EqualTo(2));
    }

    [Test]
    public void AssignProjectToEmployeeAsync_Throws_WhenInvalidEmployee()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync((User?)null);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, null, 1));
    }

    [Test]
    public void AssignProjectToEmployeeAsync_Throws_WhenNotEmployeeRole()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync(new User { Id = 3, Role = UserRole.Manager });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, null, 1));
    }

    [Test]
    public void AssignProjectToEmployeeAsync_Throws_WhenInvalidProject()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync(new User { Id = 3, Role = UserRole.Employee });
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync((ProjectCode?)null);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, null, 1));
    }

    [Test]
    public void AssignProjectToEmployeeAsync_Throws_WhenInactiveProject()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync(new User { Id = 3, Role = UserRole.Employee });
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Inactive });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, null, 1));
    }

    [Test]
    public void AssignProjectToEmployeeAsync_Throws_WhenEndDateBeforeStart()
    {
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(3)).ReturnsAsync(new User { Id = 3, Role = UserRole.Employee });
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Active });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AssignProjectToEmployeeAsync(3, 2, DateTime.Today, DateTime.Today.AddDays(-1), 1));
    }

    [Test]
    public async Task RemoveProjectAssignmentAsync_DeletesAssignment()
    {
        var assignment = new ProjectAssignment { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetByIdAsync(1)).ReturnsAsync(assignment);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.DeleteAsync(assignment)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.RemoveProjectAssignmentAsync(1);
        Assert.That(result, Is.True);
        _unitOfWorkMock.Verify(u => u.ProjectAssignments.DeleteAsync(assignment), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public async Task RemoveProjectAssignmentAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetByIdAsync(1)).ReturnsAsync((ProjectAssignment?)null);
        var result = await _service.RemoveProjectAssignmentAsync(1);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetProjectAssignmentByIdAsync_ReturnsAssignment()
    {
        var assignment = new ProjectAssignment { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetByIdAsync(1)).ReturnsAsync(assignment);
        var result = await _service.GetProjectAssignmentByIdAsync(1);
        Assert.That(result, Is.EqualTo(assignment));
    }

    [Test]
    public async Task GetAssignmentsByEmployeeIdAsync_ReturnsAssignments()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetAssignmentsByEmployeeIdAsync(3)).ReturnsAsync(assignments);
        var result = await _service.GetAssignmentsByEmployeeIdAsync(3);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetAssignmentsByProjectCodeIdAsync_ReturnsAssignments()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.GetAssignmentsByProjectCodeIdAsync(2)).ReturnsAsync(assignments);
        var result = await _service.GetAssignmentsByProjectCodeIdAsync(2);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task CanEmployeeSubmitTimesheetAsync_ReturnsTrue_WhenAssignedAndActive()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(3, 2, It.IsAny<DateTime>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Active });
        var result = await _service.CanEmployeeSubmitTimesheetAsync(3, 2, DateTime.Today);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task CanEmployeeSubmitTimesheetAsync_ReturnsFalse_WhenNotAssigned()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(3, 2, It.IsAny<DateTime>())).ReturnsAsync(false);
        var result = await _service.CanEmployeeSubmitTimesheetAsync(3, 2, DateTime.Today);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CanEmployeeSubmitTimesheetAsync_ReturnsFalse_WhenProjectInactive()
    {
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(3, 2, It.IsAny<DateTime>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Inactive });
        var result = await _service.CanEmployeeSubmitTimesheetAsync(3, 2, DateTime.Today);
        Assert.That(result, Is.False);
    }
}
