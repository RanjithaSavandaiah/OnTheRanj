
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services.Implementations;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class TimesheetApprovalServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private TimesheetApprovalService _service;
    private Mock<Core.Interfaces.Strategies.ITimesheetApprovalStrategy> _strategyMock;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _strategyMock = new Mock<Core.Interfaces.Strategies.ITimesheetApprovalStrategy>();
        _service = new TimesheetApprovalService(_unitOfWorkMock.Object, _strategyMock.Object);
    }

    [Test]
    public async Task GetPendingTimesheetsAsync_ReturnsTimesheets()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1, Status = TimesheetStatus.Submitted } };
        var asyncQueryable = new TestAsyncEnumerable<Timesheet>(timesheets);
        _unitOfWorkMock.Setup(u => u.Timesheets.Query()).Returns(() => asyncQueryable.AsQueryable());
        var result = await _service.GetPendingTimesheetsAsync();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetPendingTimesheetsAsync_ReturnsEmpty_WhenNoPending()
    {
        var asyncQueryable = new TestAsyncEnumerable<Timesheet>(new List<Timesheet>());
        _unitOfWorkMock.Setup(u => u.Timesheets.Query()).Returns(() => asyncQueryable.AsQueryable());
        var result = await _service.GetPendingTimesheetsAsync();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPendingTimesheetsAsync_ThrowsException_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.Query()).Throws(new System.Exception("DB error"));
        Assert.ThrowsAsync<System.Exception>(async () => await _service.GetPendingTimesheetsAsync());
    }

    [Test]
    public async Task ApproveTimesheetAsync_Success()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        _unitOfWorkMock.Setup(u => u.Timesheets.UpdateAsync(timesheet)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        _strategyMock.Setup(s => s.CanApprove(timesheet)).Returns(true);
        var result = await _service.ApproveTimesheetAsync(1, 99);
        Assert.That(result, Is.True);
        Assert.That(timesheet.Status, Is.EqualTo(TimesheetStatus.Approved));
    }

    [Test]
    public async Task ApproveTimesheetAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var result = await _service.ApproveTimesheetAsync(1, 99);
        Assert.That(result, Is.False);
    }

    [Test]
    public void ApproveTimesheetAsync_Throws_WhenStrategyDisallows()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Draft };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        _strategyMock.Setup(s => s.CanApprove(timesheet)).Returns(false);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.ApproveTimesheetAsync(1, 99));
    }

    [Test]
    public async Task RejectTimesheetAsync_Success()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        _unitOfWorkMock.Setup(u => u.Timesheets.UpdateAsync(timesheet)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        _strategyMock.Setup(s => s.CanReject(timesheet, "Reason")).Returns(true);
        var result = await _service.RejectTimesheetAsync(1, 99, "Reason");
        Assert.That(result, Is.True);
        Assert.That(timesheet.Status, Is.EqualTo(TimesheetStatus.Rejected));
        Assert.That(timesheet.ManagerComments, Is.EqualTo("Reason"));
    }

    [Test]
    public async Task RejectTimesheetAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var result = await _service.RejectTimesheetAsync(1, 99, "Reason");
        Assert.That(result, Is.False);
    }

    [Test]
    public void RejectTimesheetAsync_Throws_WhenStrategyDisallows()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        _strategyMock.Setup(s => s.CanReject(timesheet, "")).Returns(false);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.RejectTimesheetAsync(1, 99, ""));
    }

    [Test]
    public async Task GetTimesheetsByStatusAsync_ReturnsTimesheets()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1, Status = TimesheetStatus.Submitted } };
        var asyncQueryable = new TestAsyncEnumerable<Timesheet>(timesheets);
        _unitOfWorkMock.Setup(u => u.Timesheets.Query()).Returns(() => asyncQueryable.AsQueryable());
        var result = await _service.GetTimesheetsByStatusAsync(TimesheetStatus.Submitted);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetTimesheetsByStatusAsync_ReturnsEmpty_WhenNoMatch()
    {
        var asyncQueryable = new TestAsyncEnumerable<Timesheet>(new List<Timesheet>());
        _unitOfWorkMock.Setup(u => u.Timesheets.Query()).Returns(() => asyncQueryable.AsQueryable());
        var result = await _service.GetTimesheetsByStatusAsync(TimesheetStatus.Submitted);
        Assert.That(result, Is.Empty);
    }
}

// Helper for async queryable
public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}
public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    public TestAsyncEnumerator(IEnumerator<T> inner) { _inner = inner; }
    public T Current => _inner.Current;
    public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
}
public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;
    public TestAsyncQueryProvider(IQueryProvider inner) { _inner = inner; }
    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);
    public object Execute(Expression expression) => _inner.Execute(expression)!;
    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        // For EF Core test compatibility, TResult is usually Task<T> or ValueTask<T>
        var result = Execute<TResult>(expression);
        return result;
    }
}
