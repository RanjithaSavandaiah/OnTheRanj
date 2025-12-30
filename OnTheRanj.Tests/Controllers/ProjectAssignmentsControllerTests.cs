using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnTheRanj.API.Controllers;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Tests.Controllers;

[TestFixture]
public class ProjectAssignmentsControllerTests
{
    [Test]
    public async Task Create_SecondDtoNullCheck_ExplicitBranch()
    {
        // Arrange: Unset DOTNET_RUNNING_IN_TEST, set up user principal, call Create(null)
        var oldEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
        try
        {
            var controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
            var claims = new[] { new System.Security.Claims.Claim("userId", "42") };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal }
            };
            var result = await controller.Create(null) as BadRequestObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo("Assignment data is required."));
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
        }
    }

    [Test]
    public async Task Create_Covers_NameIdentifierClaim_Branch()
    {
        // Arrange: Unset DOTNET_RUNNING_IN_TEST, set up user principal with claim type ending with 'nameidentifier'
        var oldEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
        try
        {
            var controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
            var claims = new[] { new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "99") };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal }
            };
            var created = new ProjectAssignment { Id = 1 };
            var resultDto = new ProjectAssignmentDto { Id = 1 };
            _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 99)).ReturnsAsync(created);
            _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(created)).Returns(resultDto);
            var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
            var result = await controller.Create(dto) as CreatedAtActionResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(((ProjectAssignmentDto)result!.Value!).Id, Is.EqualTo(1));
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
        }
    }
    [Test]
    public async Task Create_InitializesHttpContext_WhenNull()
    {
        // Arrange
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        var created = new ProjectAssignment { Id = 1 };
        var resultDto = new ProjectAssignmentDto { Id = 1 };
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(1, 2, It.IsAny<DateTime>(), null, 1)).ReturnsAsync(created);
        _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(created)).Returns(resultDto);
        var controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
        // ControllerContext.HttpContext is null by default
        controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
        // Act
        var result = await controller.Create(dto) as CreatedAtActionResult;
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(((ProjectAssignmentDto)result!.Value!).Id, Is.EqualTo(1));
    }

    [Test]
    public async Task Create_SecondDtoNullCheck_IsCovered()
    {
        // Arrange: Simulate a user principal so isTest = false and clear DOTNET_RUNNING_IN_TEST
        var oldEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
        try
        {
            var controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
            var claims = new[] { new System.Security.Claims.Claim("sub", "42") };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal }
            };
            // Act
            var result = await controller.Create(null) as BadRequestObjectResult;
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo("Assignment data is required."));
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
        }
    }
    [Test]
    public async Task Create_ReturnsBadRequest_WhenServiceReturnsNull()
    {
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>())).ReturnsAsync((ProjectAssignment)null!);
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        var result = await _controller.Create(dto) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo("Assignment could not be created."));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenMapperReturnsNull()
    {
        var created = new ProjectAssignment { Id = 1 };
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>())).ReturnsAsync(created);
        _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(created)).Returns((ProjectAssignmentDto)null!);
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        var result = await _controller.Create(dto) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo("Mapping failed."));
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenUpdateReturnsNull()
    {
        var assignment = new ProjectAssignment { Id = 1 };
        _serviceMock.Setup(s => s.GetProjectAssignmentByIdAsync(1)).ReturnsAsync(assignment);
        _serviceMock.Setup(s => s.UpdateProjectAssignmentAsync(assignment)).ReturnsAsync((ProjectAssignment)null!);
        var result = await _controller.Update(1, new UpdateAssignmentDto());
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsStatus500_WhenExceptionThrown()
    {
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>())).ThrowsAsync(new Exception("fail"));
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        var result = await _controller.Create(dto) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
    }
    [Test]
    public async Task Create_ReturnsBadRequest_WhenDtoIsNull()
    {
        var result = await _controller.Create(null) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Create_Returns500_WhenServiceIsNull()
    {
        var controller = new ProjectAssignmentsController(null, _mapperMock.Object);
        var result = await controller.Create(new CreateAssignmentDto());
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objResult = result as ObjectResult;
        Assert.That(objResult!.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task Create_Returns500_WhenMapperIsNull()
    {
        var controller = new ProjectAssignmentsController(_serviceMock.Object, null);
        var result = await controller.Create(new CreateAssignmentDto());
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objResult = result as ObjectResult;
        Assert.That(objResult!.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task Create_HandlesMissingClaim_UsesFallbackCreatedBy()
    {
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        var created = new ProjectAssignment { Id = 1 };
        var resultDto = new ProjectAssignmentDto { Id = 1 };
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(1, 2, It.IsAny<DateTime>(), null, 1)).ReturnsAsync(created);
        _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(created)).Returns(resultDto);
        var controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
        controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() // No claims
        };
        var result = await controller.Create(dto) as CreatedAtActionResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((ProjectAssignmentDto)result!.Value!).Id, Is.EqualTo(1));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenServiceThrowsInvalidOperation()
    {
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<int>())).ThrowsAsync(new InvalidOperationException("fail"));
        var result = await _controller.Create(dto) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Create_Returns500_WhenServiceThrowsException()
    {
        var dto = new CreateAssignmentDto { EmployeeId = 1, ProjectCodeId = 2, StartDate = DateTime.Today };
        _serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<int>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.Create(dto) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
    }
    private Mock<IProjectAssignmentService> _serviceMock;
    private Mock<IMapper> _mapperMock;
    private ProjectAssignmentsController _controller;
    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IProjectAssignmentService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new ProjectAssignmentsController(_serviceMock.Object, _mapperMock.Object);
        // Set up fake user with a claim for all tests
        var claims = new[] { new System.Security.Claims.Claim("sub", "42") };
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal }
        };
    }

    [Test]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var serviceMock = new Mock<IProjectAssignmentService>();
        var mapperMock = new Mock<IMapper>();
        var controller = new ProjectAssignmentsController(serviceMock.Object, mapperMock.Object);
        var dto = new CreateAssignmentDto
        {
            EmployeeId = 123,
            ProjectCodeId = 456,
            StartDate = System.DateTime.UtcNow,
            EndDate = System.DateTime.UtcNow.AddDays(7)
        };
        var created = new ProjectAssignment { Id = 1 };
        var resultDto = new ProjectAssignmentDto { Id = 1 };
        serviceMock.Setup(s => s.AssignProjectToEmployeeAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<System.DateTime>(),
            It.IsAny<System.DateTime?>(),
            It.IsAny<int>())).ReturnsAsync(created);
        mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(created)).Returns(resultDto);

        var claims = new[] { new System.Security.Claims.Claim("sub", "42") };
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var result = await controller.Create(dto) as CreatedAtActionResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.ActionName, Is.EqualTo("GetById"));
        Assert.That(((ProjectAssignmentDto)result.Value!).Id, Is.EqualTo(1));
    }

    [Test]
    public async Task Update_ReturnsOk_WhenFound()
    {
        var dto = new UpdateAssignmentDto();
        var assignment = new ProjectAssignment { Id = 1 };
        var updated = new ProjectAssignment { Id = 1 };
        var resultDto = new ProjectAssignmentDto { Id = 1 };
        _serviceMock.Setup(s => s.GetProjectAssignmentByIdAsync(1)).ReturnsAsync(assignment);
        _serviceMock.Setup(s => s.UpdateProjectAssignmentAsync(assignment)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(updated)).Returns(resultDto);
        var result = await _controller.Update(1, dto) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((ProjectAssignmentDto)result!.Value!).Id, Is.EqualTo(1));
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetProjectAssignmentByIdAsync(1)).ReturnsAsync((ProjectAssignment?)null!);
        var result = await _controller.Update(1, new UpdateAssignmentDto());
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ReturnsOk_WhenDeleted()
    {
        _serviceMock.Setup(s => s.RemoveProjectAssignmentAsync(1)).ReturnsAsync(true);
        var result = await _controller.Delete(1);
        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.RemoveProjectAssignmentAsync(1)).ReturnsAsync(false);
        var result = await _controller.Delete(1);
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var assignment = new ProjectAssignment { Id = 1 };
        var dto = new ProjectAssignmentDto { Id = 1 };
        _serviceMock.Setup(s => s.GetProjectAssignmentByIdAsync(1)).ReturnsAsync(assignment);
        _mapperMock.Setup(m => m.Map<ProjectAssignmentDto>(assignment)).Returns(dto);
        var result = await _controller.GetById(1) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((ProjectAssignmentDto)result!.Value!).Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetProjectAssignmentByIdAsync(1)).ReturnsAsync((ProjectAssignment?)null!);
        var result = await _controller.GetById(1);
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAll_ReturnsOk()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        var dtos = new List<ProjectAssignmentDto> { new ProjectAssignmentDto { Id = 1 } };
        _serviceMock.Setup(s => s.GetAllProjectAssignmentsAsync()).ReturnsAsync(assignments);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProjectAssignmentDto>>(assignments)).Returns(dtos);
        var result = await _controller.GetAll() as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((IEnumerable<ProjectAssignmentDto>)result!.Value!), Is.EquivalentTo(dtos));
    }

    [Test]
    public async Task GetByEmployee_ReturnsOk()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        var dtos = new List<ProjectAssignmentDto> { new ProjectAssignmentDto { Id = 1 } };
        _serviceMock.Setup(s => s.GetAssignmentsByEmployeeIdAsync(1)).ReturnsAsync(assignments);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProjectAssignmentDto>>(assignments)).Returns(dtos);
        var result = await _controller.GetByEmployee(1) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((IEnumerable<ProjectAssignmentDto>)result!.Value!), Is.EquivalentTo(dtos));
    }

    [Test]
    public async Task GetByProject_ReturnsOk()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        var dtos = new List<ProjectAssignmentDto> { new ProjectAssignmentDto { Id = 1 } };
        _serviceMock.Setup(s => s.GetAssignmentsByProjectCodeIdAsync(1)).ReturnsAsync(assignments);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProjectAssignmentDto>>(assignments)).Returns(dtos);
        var result = await _controller.GetByProject(1) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((IEnumerable<ProjectAssignmentDto>)result!.Value!), Is.EquivalentTo(dtos));
    }

    [Test]
    public async Task GetActiveAssignmentsByEmployee_ReturnsOk()
    {
        var assignments = new List<ProjectAssignment> { new ProjectAssignment { Id = 1 } };
        var dtos = new List<ProjectAssignmentDto> { new ProjectAssignmentDto { Id = 1 } };
        _serviceMock.Setup(s => s.GetActiveEmployeeAssignmentsAsync(1)).ReturnsAsync(assignments);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProjectAssignmentDto>>(assignments)).Returns(dtos);
        var result = await _controller.GetActiveAssignmentsByEmployee(1) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((IEnumerable<ProjectAssignmentDto>)result!.Value!), Is.EquivalentTo(dtos));
    }
}
