using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexFromFinancialGoalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_financial_goals_category_id_period_id",
                table: "financial_goals");

            migrationBuilder.CreateIndex(
                name: "ix_financial_goals_category_id_period_id",
                table: "financial_goals",
                columns: new[] { "category_id", "period_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_financial_goals_category_id_period_id",
                table: "financial_goals");

            migrationBuilder.CreateIndex(
                name: "ix_financial_goals_category_id_period_id",
                table: "financial_goals",
                columns: new[] { "category_id", "period_id" },
                unique: true);
        }
    }
}
