using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnTheRanj.API.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProjectCodes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                ProjectName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                ClientName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                IsBillable = table.Column<bool>(type: "INTEGER", nullable: false),
                Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                CreatedBy = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectCodes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProjectAssignments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                ProjectCodeId = table.Column<int>(type: "INTEGER", nullable: false),
                StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectAssignments", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProjectAssignments_ProjectCodes_ProjectCodeId",
                    column: x => x.ProjectCodeId,
                    principalTable: "ProjectCodes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProjectAssignments_Users_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Timesheets",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                ProjectCodeId = table.Column<int>(type: "INTEGER", nullable: false),
                Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                HoursWorked = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                ManagerComments = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                ReviewedBy = table.Column<int>(type: "INTEGER", nullable: true),
                ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Timesheets", x => x.Id);
                table.ForeignKey(
                    name: "FK_Timesheets_ProjectCodes_ProjectCodeId",
                    column: x => x.ProjectCodeId,
                    principalTable: "ProjectCodes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Timesheets_Users_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Timesheets_Users_ReviewedBy",
                    column: x => x.ReviewedBy,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "ProjectCodes",
            columns: new[] { "Id", "ClientName", "Code", "CreatedAt", "CreatedBy", "IsBillable", "ProjectName", "Status", "UpdatedAt" },
            values: new object[,]
            {
                { 1, "TechCorp Ltd", "PROJ001", new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5370), 1, true, "E-Commerce Website", "Active", null },
                { 2, "InnovateSoft", "PROJ002", new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5380), 1, true, "Mobile App Development", "Active", null },
                { 3, "OnTheRanj", "INTERNAL001", new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5380), 1, false, "Internal Training", "Active", null }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Role" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 12, 23, 1, 6, 22, 650, DateTimeKind.Utc).AddTicks(7430), "manager@ontheranj.com", "John Manager", true, "$2a$11$S.MwkTKi1xi.3w/FiubX2O2us5DHStmANy8XSZ7odyvFEd6P0e3J.", "Manager" },
                { 2, new DateTime(2025, 12, 23, 1, 6, 22, 763, DateTimeKind.Utc).AddTicks(1220), "alice@ontheranj.com", "Alice Employee", true, "$2a$11$U9Y0..8jt.AMn1ioslncm.UNn6Yz25YeDTLknDx1XpmoTt2fTBT76", "Employee" },
                { 3, new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(4980), "bob@ontheranj.com", "Bob Employee", true, "$2a$11$1Im3stg.91hWRGP98ZZBlOCi0MQCE7UM7xAWi3TsBHnY4FMMblq7y", "Employee" }
            });

        migrationBuilder.InsertData(
            table: "ProjectAssignments",
            columns: new[] { "Id", "CreatedAt", "CreatedBy", "EmployeeId", "EndDate", "ProjectCodeId", "StartDate" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5430), 1, 2, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc) },
                { 2, new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5440), 1, 2, null, 3, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc) },
                { 3, new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5440), 1, 3, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc) }
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectAssignments_EmployeeId_ProjectCodeId",
            table: "ProjectAssignments",
            columns: new[] { "EmployeeId", "ProjectCodeId" });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectAssignments_ProjectCodeId",
            table: "ProjectAssignments",
            column: "ProjectCodeId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectCodes_Code",
            table: "ProjectCodes",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Timesheets_EmployeeId_ProjectCodeId_Date",
            table: "Timesheets",
            columns: new[] { "EmployeeId", "ProjectCodeId", "Date" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Timesheets_ProjectCodeId",
            table: "Timesheets",
            column: "ProjectCodeId");

        migrationBuilder.CreateIndex(
            name: "IX_Timesheets_ReviewedBy",
            table: "Timesheets",
            column: "ReviewedBy");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProjectAssignments");

        migrationBuilder.DropTable(
            name: "Timesheets");

        migrationBuilder.DropTable(
            name: "ProjectCodes");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
