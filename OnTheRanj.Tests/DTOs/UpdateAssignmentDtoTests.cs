using OnTheRanj.API.DTOs;

namespace OnTheRanj.Tests.DTOs;

[TestFixture]
public class UpdateAssignmentDtoTests
{
    [Test]
    public void Can_Create_And_Access_All_Properties()
    {
        var now = DateTime.Today;
        var dto = new UpdateAssignmentDto
        {
            Id = 42,
            EndDate = now
        };
        Assert.That(dto.Id, Is.EqualTo(42));
        Assert.That(dto.EndDate, Is.EqualTo(now));
    }

    [Test]
    public void EndDate_Can_Be_Null()
    {
        var dto = new UpdateAssignmentDto { Id = 5, EndDate = null };
        Assert.That(dto.Id, Is.EqualTo(5));
        Assert.That(dto.EndDate, Is.Null);
    }
}
