using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                TransactionType = table.Column<string>(type: "text", nullable: false),
                ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                Inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_Categories_Categories_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Categories",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Periods",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Start = table.Column<DateOnly>(type: "date", nullable: false),
                End = table.Column<DateOnly>(type: "date", nullable: false),
                Name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                Inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Periods", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                InvoiceValue = table.Column<decimal>(type: "numeric", nullable: true),
                CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                PeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                Inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Transactions_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Transactions_Periods_PeriodId",
                    column: x => x.PeriodId,
                    principalTable: "Periods",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Categories_Name_TransactionType",
            table: "Categories",
            columns: new[] { "Name", "TransactionType" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Categories_ParentId",
            table: "Categories",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Periods_Name",
            table: "Periods",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Periods_Name_Start",
            table: "Periods",
            columns: new[] { "Name", "Start" });

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_CategoryId",
            table: "Transactions",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_InvoiceDate",
            table: "Transactions",
            column: "InvoiceDate",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_PeriodId",
            table: "Transactions",
            column: "PeriodId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Transactions");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Periods");
    }
}
