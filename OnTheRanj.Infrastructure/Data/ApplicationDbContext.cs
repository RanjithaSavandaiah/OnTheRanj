using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;

namespace OnTheRanj.Infrastructure.Data;

/// <summary>
/// Database context for OnTheRanj application
/// Manages entity configurations and database connections
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Users DbSet
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// ProjectCodes DbSet
    /// </summary>
    public DbSet<ProjectCode> ProjectCodes { get; set; } = null!;

    /// <summary>
    /// ProjectAssignments DbSet
    /// </summary>
    public DbSet<ProjectAssignment> ProjectAssignments { get; set; } = null!;

    /// <summary>
    /// Timesheets DbSet
    /// </summary>
    public DbSet<Timesheet> Timesheets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Configure relationships
            entity.HasMany(e => e.ProjectAssignments)
                .WithOne(pa => pa.Employee)
                .HasForeignKey(pa => pa.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Timesheets)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProjectCode entity
        modelBuilder.Entity<ProjectCode>(entity =>
        {
            entity.ToTable("ProjectCodes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Configure relationships
            entity.HasMany(e => e.ProjectAssignments)
                .WithOne(pa => pa.ProjectCode)
                .HasForeignKey(pa => pa.ProjectCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Timesheets)
                .WithOne(t => t.ProjectCode)
                .HasForeignKey(t => t.ProjectCodeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProjectAssignment entity
        modelBuilder.Entity<ProjectAssignment>(entity =>
        {
            entity.ToTable("ProjectAssignments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Create index for efficient querying
            entity.HasIndex(e => new { e.EmployeeId, e.ProjectCodeId });
        });

        // Configure Timesheet entity
        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.ToTable("Timesheets");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.HoursWorked).IsRequired().HasPrecision(5, 2);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ManagerComments).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Create unique index to prevent duplicate entries
            entity.HasIndex(e => new { e.EmployeeId, e.ProjectCodeId, e.Date }).IsUnique();

            // Configure relationship with reviewer
            entity.HasOne(e => e.Reviewer)
                .WithMany()
                .HasForeignKey(e => e.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Manager user
        // User seeding moved to runtime for proper password hashing

        // Seed data removed for SQLite compatibility. Use external SQL script for seeding.
    }
}
