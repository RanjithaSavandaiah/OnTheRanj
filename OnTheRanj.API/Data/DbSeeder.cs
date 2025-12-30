using OnTheRanj.Core.Entities;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.API.Data;

public static class DbSeeder
{
    public static void SeedUsers(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    FullName = "John Manager",
                    Email = "manager@ontheranj.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                    Role = "Manager",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Alice Employee",
                    Email = "alice@ontheranj.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"),
                    Role = "Employee",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Bob Employee",
                    Email = "bob@ontheranj.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"),
                    Role = "Employee",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
            context.SaveChanges();
        }
    }

    public static void SeedTimesheets(ApplicationDbContext context)
    {
        if (!context.Timesheets.Any())
        {
            // Seed a few timesheets for Alice (EmployeeId=2) and Bob (EmployeeId=3)
            context.Timesheets.AddRange(
                new Timesheet
                {
                    EmployeeId = 2,
                    ProjectCodeId = 1,
                    Date = new DateTime(2025, 12, 15),
                    HoursWorked = 8,
                    Description = "Worked on frontend features",
                    Status = "Submitted",
                    CreatedAt = new DateTime(2025, 12, 15)
                },
                new Timesheet
                {
                    EmployeeId = 2,
                    ProjectCodeId = 4,
                    Date = new DateTime(2025, 12, 16),
                    HoursWorked = 6,
                    Description = "Internal training session",
                    Status = "Submitted",
                    CreatedAt = new DateTime(2025, 12, 16)
                },
                new Timesheet
                {
                    EmployeeId = 3,
                    ProjectCodeId = 2,
                    Date = new DateTime(2025, 12, 15),
                    HoursWorked = 7,
                    Description = "API integration",
                    Status = "Submitted",
                    CreatedAt = new DateTime(2025, 12, 15)
                },
                new Timesheet
                {
                    EmployeeId = 3,
                    ProjectCodeId = 2,
                    Date = new DateTime(2025, 12, 16),
                    HoursWorked = 8,
                    Description = "Bug fixes and testing",
                    Status = "Draft",
                    CreatedAt = new DateTime(2025, 12, 16)
                },
                new Timesheet
                {
                    EmployeeId = 2,
                    ProjectCodeId = 1,
                    Date = new DateTime(2025, 12, 17),
                    HoursWorked = 8,
                    Description = "UI improvements",
                    Status = "Draft",
                    CreatedAt = new DateTime(2025, 12, 17)
                }
            );
            context.SaveChanges();
        }
    }
}
