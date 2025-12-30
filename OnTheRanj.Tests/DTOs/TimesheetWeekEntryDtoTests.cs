using OnTheRanj.API.DTOs;

namespace OnTheRanj.Tests.DTOs;

[TestFixture]
public class TimesheetWeekEntryDtoTests
{
    [Test]
    public void Can_Create_And_Access_All_Properties()
    {
        var now = DateTime.Today;
        var dto = new TimesheetWeekEntryDto
        {
            EmployeeId = 1,
            ProjectCodeId = 2,
            Date = now,
            HoursWorked = 7.5m,
            Description = "Worked on bugfixes"
        };
        Assert.Multiple(() => {
            Assert.That(dto.EmployeeId, Is.EqualTo(1));
            Assert.That(dto.ProjectCodeId, Is.EqualTo(2));
            Assert.That(dto.Date, Is.EqualTo(now));
            Assert.That(dto.HoursWorked, Is.EqualTo(7.5m));
            Assert.That(dto.Description, Is.EqualTo("Worked on bugfixes"));
        });
    }

    [Test]
    public void Defaults_Are_Correct()
    {
        var dto = new TimesheetWeekEntryDto();
        Assert.That(dto.Description, Is.EqualTo(string.Empty));
    }
}
