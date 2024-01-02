using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transaction_parts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction_parts", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_parts_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_transaction_parts_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transaction_parts_transactions_transaction_id1",
                        column: x => x.transaction_id1,
                        principalTable: "transactions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_transaction_parts_category_id",
                table: "transaction_parts",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_parts_transaction_id",
                table: "transaction_parts",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_parts_transaction_id1",
                table: "transaction_parts",
                column: "transaction_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_parts");
        }
    }
}
