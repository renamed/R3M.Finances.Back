using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations;

/// <inheritdoc />
public partial class ChangeToSnakeCaseNaming : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_Categories_ParentId",
            table: "Categories");

        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Categories_CategoryId",
            table: "Transactions");

        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Periods_PeriodId",
            table: "Transactions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Transactions",
            table: "Transactions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Periods",
            table: "Periods");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Categories",
            table: "Categories");

        migrationBuilder.RenameTable(
            name: "Transactions",
            newName: "transactions");

        migrationBuilder.RenameTable(
            name: "Periods",
            newName: "periods");

        migrationBuilder.RenameTable(
            name: "Categories",
            newName: "categories");

        migrationBuilder.RenameColumn(
            name: "Updated",
            table: "transactions",
            newName: "updated");

        migrationBuilder.RenameColumn(
            name: "Inserted",
            table: "transactions",
            newName: "inserted");

        migrationBuilder.RenameColumn(
            name: "Description",
            table: "transactions",
            newName: "description");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "transactions",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "PeriodId",
            table: "transactions",
            newName: "period_id");

        migrationBuilder.RenameColumn(
            name: "InvoiceValue",
            table: "transactions",
            newName: "invoice_value");

        migrationBuilder.RenameColumn(
            name: "InvoiceDate",
            table: "transactions",
            newName: "invoice_date");

        migrationBuilder.RenameColumn(
            name: "CategoryId",
            table: "transactions",
            newName: "category_id");

        migrationBuilder.RenameIndex(
            name: "IX_Transactions_PeriodId",
            table: "transactions",
            newName: "ix_transactions_period_id");

        migrationBuilder.RenameIndex(
            name: "IX_Transactions_InvoiceDate",
            table: "transactions",
            newName: "ix_transactions_invoice_date");

        migrationBuilder.RenameIndex(
            name: "IX_Transactions_CategoryId",
            table: "transactions",
            newName: "ix_transactions_category_id");

        migrationBuilder.RenameColumn(
            name: "Updated",
            table: "periods",
            newName: "updated");

        migrationBuilder.RenameColumn(
            name: "Start",
            table: "periods",
            newName: "start");

        migrationBuilder.RenameColumn(
            name: "Name",
            table: "periods",
            newName: "name");

        migrationBuilder.RenameColumn(
            name: "Inserted",
            table: "periods",
            newName: "inserted");

        migrationBuilder.RenameColumn(
            name: "End",
            table: "periods",
            newName: "end");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "periods",
            newName: "id");

        migrationBuilder.RenameIndex(
            name: "IX_Periods_Name_Start",
            table: "periods",
            newName: "ix_periods_name_start");

        migrationBuilder.RenameIndex(
            name: "IX_Periods_Name",
            table: "periods",
            newName: "ix_periods_name");

        migrationBuilder.RenameColumn(
            name: "Updated",
            table: "categories",
            newName: "updated");

        migrationBuilder.RenameColumn(
            name: "Name",
            table: "categories",
            newName: "name");

        migrationBuilder.RenameColumn(
            name: "Inserted",
            table: "categories",
            newName: "inserted");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "categories",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "TransactionType",
            table: "categories",
            newName: "transaction_type");

        migrationBuilder.RenameColumn(
            name: "ParentId",
            table: "categories",
            newName: "parent_id");

        migrationBuilder.RenameIndex(
            name: "IX_Categories_ParentId",
            table: "categories",
            newName: "ix_categories_parent_id");

        migrationBuilder.RenameIndex(
            name: "IX_Categories_Name_TransactionType",
            table: "categories",
            newName: "ix_categories_name_transaction_type");

        migrationBuilder.AddPrimaryKey(
            name: "pk_transactions",
            table: "transactions",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "pk_periods",
            table: "periods",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "pk_categories",
            table: "categories",
            column: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_categories_categories_parent_id",
            table: "categories",
            column: "parent_id",
            principalTable: "categories",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_transactions_categories_category_id",
            table: "transactions",
            column: "category_id",
            principalTable: "categories",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_transactions_periods_period_id",
            table: "transactions",
            column: "period_id",
            principalTable: "periods",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_categories_categories_parent_id",
            table: "categories");

        migrationBuilder.DropForeignKey(
            name: "fk_transactions_categories_category_id",
            table: "transactions");

        migrationBuilder.DropForeignKey(
            name: "fk_transactions_periods_period_id",
            table: "transactions");

        migrationBuilder.DropPrimaryKey(
            name: "pk_transactions",
            table: "transactions");

        migrationBuilder.DropPrimaryKey(
            name: "pk_periods",
            table: "periods");

        migrationBuilder.DropPrimaryKey(
            name: "pk_categories",
            table: "categories");

        migrationBuilder.RenameTable(
            name: "transactions",
            newName: "Transactions");

        migrationBuilder.RenameTable(
            name: "periods",
            newName: "Periods");

        migrationBuilder.RenameTable(
            name: "categories",
            newName: "Categories");

        migrationBuilder.RenameColumn(
            name: "updated",
            table: "Transactions",
            newName: "Updated");

        migrationBuilder.RenameColumn(
            name: "inserted",
            table: "Transactions",
            newName: "Inserted");

        migrationBuilder.RenameColumn(
            name: "description",
            table: "Transactions",
            newName: "Description");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Transactions",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "period_id",
            table: "Transactions",
            newName: "PeriodId");

        migrationBuilder.RenameColumn(
            name: "invoice_value",
            table: "Transactions",
            newName: "InvoiceValue");

        migrationBuilder.RenameColumn(
            name: "invoice_date",
            table: "Transactions",
            newName: "InvoiceDate");

        migrationBuilder.RenameColumn(
            name: "category_id",
            table: "Transactions",
            newName: "CategoryId");

        migrationBuilder.RenameIndex(
            name: "ix_transactions_period_id",
            table: "Transactions",
            newName: "IX_Transactions_PeriodId");

        migrationBuilder.RenameIndex(
            name: "ix_transactions_invoice_date",
            table: "Transactions",
            newName: "IX_Transactions_InvoiceDate");

        migrationBuilder.RenameIndex(
            name: "ix_transactions_category_id",
            table: "Transactions",
            newName: "IX_Transactions_CategoryId");

        migrationBuilder.RenameColumn(
            name: "updated",
            table: "Periods",
            newName: "Updated");

        migrationBuilder.RenameColumn(
            name: "start",
            table: "Periods",
            newName: "Start");

        migrationBuilder.RenameColumn(
            name: "name",
            table: "Periods",
            newName: "Name");

        migrationBuilder.RenameColumn(
            name: "inserted",
            table: "Periods",
            newName: "Inserted");

        migrationBuilder.RenameColumn(
            name: "end",
            table: "Periods",
            newName: "End");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Periods",
            newName: "Id");

        migrationBuilder.RenameIndex(
            name: "ix_periods_name_start",
            table: "Periods",
            newName: "IX_Periods_Name_Start");

        migrationBuilder.RenameIndex(
            name: "ix_periods_name",
            table: "Periods",
            newName: "IX_Periods_Name");

        migrationBuilder.RenameColumn(
            name: "updated",
            table: "Categories",
            newName: "Updated");

        migrationBuilder.RenameColumn(
            name: "name",
            table: "Categories",
            newName: "Name");

        migrationBuilder.RenameColumn(
            name: "inserted",
            table: "Categories",
            newName: "Inserted");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Categories",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "transaction_type",
            table: "Categories",
            newName: "TransactionType");

        migrationBuilder.RenameColumn(
            name: "parent_id",
            table: "Categories",
            newName: "ParentId");

        migrationBuilder.RenameIndex(
            name: "ix_categories_parent_id",
            table: "Categories",
            newName: "IX_Categories_ParentId");

        migrationBuilder.RenameIndex(
            name: "ix_categories_name_transaction_type",
            table: "Categories",
            newName: "IX_Categories_Name_TransactionType");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Transactions",
            table: "Transactions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Periods",
            table: "Periods",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Categories",
            table: "Categories",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_Categories_ParentId",
            table: "Categories",
            column: "ParentId",
            principalTable: "Categories",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Categories_CategoryId",
            table: "Transactions",
            column: "CategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Periods_PeriodId",
            table: "Transactions",
            column: "PeriodId",
            principalTable: "Periods",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
