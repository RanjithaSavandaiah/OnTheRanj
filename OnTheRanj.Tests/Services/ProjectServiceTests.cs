using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services.Implementations;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class ProjectServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private ProjectService _service;

    [Test]
    public void DeleteProjectCodeAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ThrowsAsync(new System.Exception("DB error"));
        Assert.ThrowsAsync<System.Exception>(async () => await _service.DeleteProjectCodeAsync(1));
    }

    [Test]
    public void GetActiveProjectCodesAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetActiveProjectCodesAsync()).ThrowsAsync(new System.Exception("DB error"));
        Assert.ThrowsAsync<System.Exception>(async () => await _service.GetActiveProjectCodesAsync());
    }

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new ProjectService(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllProjectCodesAsync_ReturnsProjectCodes()
    {
        var codes = new List<ProjectCode> { new ProjectCode { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetAllAsync()).ReturnsAsync(codes);
        var result = await _service.GetAllProjectCodesAsync();
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetAllProjectCodesAsync_ReturnsEmptyList_WhenNoCodes()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetAllAsync()).ReturnsAsync(new List<ProjectCode>());
        var result = await _service.GetAllProjectCodesAsync();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllProjectCodesAsync_ThrowsException_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetAllAsync()).ThrowsAsync(new System.Exception("DB error"));
        Assert.ThrowsAsync<System.Exception>(async () => await _service.GetAllProjectCodesAsync());
    }

    [Test]
    public async Task CreateProjectCodeAsync_Success()
    {
        var code = new ProjectCode { Id = 1, Code = "P001" };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByCodeAsync("P001")).ReturnsAsync((ProjectCode?)null);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.AddAsync(code)).ReturnsAsync(code);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.CreateProjectCodeAsync(code, 99);
        Assert.That(result, Is.EqualTo(code));
        Assert.That(result.Status, Is.EqualTo(Core.Enums.ProjectStatus.Active));
        _unitOfWorkMock.Verify(u => u.ProjectCodes.AddAsync(code), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public void CreateProjectCodeAsync_Throws_WhenDuplicateCode()
    {
        var code = new ProjectCode { Id = 1, Code = "P001" };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByCodeAsync("P001")).ReturnsAsync(new ProjectCode { Id = 2, Code = "P001" });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateProjectCodeAsync(code, 99));
    }

    [Test]
    public async Task UpdateProjectCodeAsync_Success()
    {
        var code = new ProjectCode { Id = 1, Code = "P001", ProjectName = "A", ClientName = "C", IsBillable = true, Status = Core.Enums.ProjectStatus.Active };
        var existing = new ProjectCode { Id = 1, Code = "P001" };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.UpdateAsync(existing)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.UpdateProjectCodeAsync(code);
        Assert.That(result.ProjectName, Is.EqualTo("A"));
        _unitOfWorkMock.Verify(u => u.ProjectCodes.UpdateAsync(existing), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public void UpdateProjectCodeAsync_Throws_WhenNotFound()
    {
        var code = new ProjectCode { Id = 1, Code = "P001" };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync((ProjectCode?)null);
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.UpdateProjectCodeAsync(code));
    }

    [Test]
    public void UpdateProjectCodeAsync_Throws_WhenDuplicateCode()
    {
        var code = new ProjectCode { Id = 1, Code = "P002" };
        var existing = new ProjectCode { Id = 1, Code = "P001" };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByCodeAsync("P002")).ReturnsAsync(new ProjectCode { Id = 2, Code = "P002" });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateProjectCodeAsync(code));
    }

    [Test]
    public async Task DeleteProjectCodeAsync_DeletesProject()
    {
        var code = new ProjectCode { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync(code);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.DeleteAsync(code)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var result = await _service.DeleteProjectCodeAsync(1);
        Assert.That(result, Is.True);
        _unitOfWorkMock.Verify(u => u.ProjectCodes.DeleteAsync(code), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public async Task DeleteProjectCodeAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync((ProjectCode?)null);
        var result = await _service.DeleteProjectCodeAsync(1);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetActiveProjectCodesAsync_ReturnsActive()
    {
        var codes = new List<ProjectCode> { new ProjectCode { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetActiveProjectCodesAsync()).ReturnsAsync(codes);
        var result = await _service.GetActiveProjectCodesAsync();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetProjectCodeByIdAsync_ReturnsProject()
    {
        var code = new ProjectCode { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync(code);
        var result = await _service.GetProjectCodeByIdAsync(1);
        Assert.That(result, Is.EqualTo(code));
    }

    [Test]
    public async Task GetProjectCodeByIdAsync_ReturnsNull_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(1)).ReturnsAsync((ProjectCode?)null);
        var result = await _service.GetProjectCodeByIdAsync(1);
        Assert.That(result, Is.Null);
    }
}
