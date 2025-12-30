using OnTheRanj.API.DTOs;

namespace OnTheRanj.Tests.DTOs;

[TestFixture]
public class TimesheetDtoTests
{
    [Test]
    public void Can_Create_And_Access_All_Properties()
    {
        var now = DateTime.UtcNow;
        var dto = new TimesheetDto
        {
            Id = 1,
            EmployeeId = 2,
            ProjectCodeId = 3,
            ProjectName = "ProjectX",
            Date = now,
            HoursWorked = 8.5m,
            Description = "Worked on feature Y",
            Status = "Submitted",
            ManagerComments = "Looks good",
            CreatedAt = now,
            UpdatedAt = now.AddHours(1)
        };
        Assert.Multiple(() => {
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.EmployeeId, Is.EqualTo(2));
            Assert.That(dto.ProjectCodeId, Is.EqualTo(3));
            Assert.That(dto.ProjectName, Is.EqualTo("ProjectX"));
            Assert.That(dto.Date, Is.EqualTo(now));
            Assert.That(dto.HoursWorked, Is.EqualTo(8.5m));
            Assert.That(dto.Description, Is.EqualTo("Worked on feature Y"));
            Assert.That(dto.Status, Is.EqualTo("Submitted"));
            Assert.That(dto.ManagerComments, Is.EqualTo("Looks good"));
            Assert.That(dto.CreatedAt, Is.EqualTo(now));
            Assert.That(dto.UpdatedAt, Is.EqualTo(now.AddHours(1)));
        });
    }

    [Test]
    public void Defaults_Are_Correct()
    {
        var dto = new TimesheetDto();
        Assert.That(dto.ProjectName, Is.EqualTo(string.Empty));
        Assert.That(dto.Description, Is.EqualTo(string.Empty));
        Assert.That(dto.Status, Is.EqualTo(string.Empty));
        Assert.That(dto.ManagerComments, Is.Null);
        Assert.That(dto.UpdatedAt, Is.Null);
    }
}
