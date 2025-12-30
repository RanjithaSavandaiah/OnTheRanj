using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnTheRanj.API.Migrations;

/// <inheritdoc />
public partial class AddAssignmentEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Assignments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                ProjectCodeId = table.Column<int>(type: "INTEGER", nullable: false),
                StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Assignments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Assignments_ProjectCodes_ProjectCodeId",
                    column: x => x.ProjectCodeId,
                    principalTable: "ProjectCodes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Assignments_Users_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 1,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4980));

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 2,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4990));

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 3,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4990));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 1,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4890));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 2,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4900));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 3,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4900));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 2, 38, 35, 416, DateTimeKind.Utc).AddTicks(2670), "$2a$11$WNImVw7GbW5sXhuUip5wvOoEYt6Oqc7sQm7MmS6dx0c9on51XcvKa" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 2, 38, 35, 550, DateTimeKind.Utc).AddTicks(7570), "$2a$11$5cR0RZHDW7D7WJAvDVUqUea2gxIXHg/j.C6W8GgTIm5cuunDbTFGq" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 3,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 2, 38, 35, 677, DateTimeKind.Utc).AddTicks(4470), "$2a$11$nxthm4jJPI5bcUZSqqRVpOoCR32tDGTULXY00IA0rdLGtTMzXZWRO" });

        migrationBuilder.CreateIndex(
            name: "IX_Assignments_EmployeeId",
            table: "Assignments",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_Assignments_ProjectCodeId",
            table: "Assignments",
            column: "ProjectCodeId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Assignments");

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 1,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5430));

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 2,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5440));

        migrationBuilder.UpdateData(
            table: "ProjectAssignments",
            keyColumn: "Id",
            keyValue: 3,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5440));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 1,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5370));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 2,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5380));

        migrationBuilder.UpdateData(
            table: "ProjectCodes",
            keyColumn: "Id",
            keyValue: 3,
            column: "CreatedAt",
            value: new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(5380));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 1, 6, 22, 650, DateTimeKind.Utc).AddTicks(7430), "$2a$11$S.MwkTKi1xi.3w/FiubX2O2us5DHStmANy8XSZ7odyvFEd6P0e3J." });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 1, 6, 22, 763, DateTimeKind.Utc).AddTicks(1220), "$2a$11$U9Y0..8jt.AMn1ioslncm.UNn6Yz25YeDTLknDx1XpmoTt2fTBT76" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: 3,
            columns: new[] { "CreatedAt", "PasswordHash" },
            values: new object[] { new DateTime(2025, 12, 23, 1, 6, 22, 873, DateTimeKind.Utc).AddTicks(4980), "$2a$11$1Im3stg.91hWRGP98ZZBlOCi0MQCE7UM7xAWi3TsBHnY4FMMblq7y" });
    }
}
