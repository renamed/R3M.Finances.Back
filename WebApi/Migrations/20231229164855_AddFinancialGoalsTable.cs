using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialGoalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "financial_goals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    goal = table.Column<decimal>(type: "numeric", nullable: false),
                    inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_financial_goals", x => x.id);
                    table.ForeignKey(
                        name: "fk_financial_goals_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_financial_goals_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_financial_goals_category_id_period_id",
                table: "financial_goals",
                columns: new[] { "category_id", "period_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_financial_goals_period_id",
                table: "financial_goals",
                column: "period_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_goals");
        }
    }
}
