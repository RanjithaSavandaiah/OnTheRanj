using OnTheRanj.API.DTOs;

namespace OnTheRanj.Tests.DTOs;

public class UpdateProjectRequestTests
{
    [Test]
    public void Default_Constructor_Sets_Defaults()
    {
        var dto = new UpdateProjectRequest();
        Assert.That(dto.Code, Is.EqualTo(""));
        Assert.That(dto.ProjectName, Is.EqualTo(""));
        Assert.That(dto.ClientName, Is.EqualTo(""));
        Assert.That(dto.IsBillable, Is.False);
        Assert.That(dto.Status, Is.EqualTo("Active"));
    }

    [Test]
    public void Can_Set_And_Get_Properties()
    {
        var dto = new UpdateProjectRequest
        {
            Code = "P001",
            ProjectName = "Test Project",
            ClientName = "Test Client",
            IsBillable = true,
            Status = "Inactive"
        };
        Assert.That(dto.Code, Is.EqualTo("P001"));
        Assert.That(dto.ProjectName, Is.EqualTo("Test Project"));
        Assert.That(dto.ClientName, Is.EqualTo("Test Client"));
        Assert.That(dto.IsBillable, Is.True);
        Assert.That(dto.Status, Is.EqualTo("Inactive"));
    }
}
