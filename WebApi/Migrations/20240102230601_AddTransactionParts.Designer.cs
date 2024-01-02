﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi.Context;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(FinancesContext))]
    [Migration("20240102230601_AddTransactionParts")]
    partial class AddTransactionParts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApi.Model.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted");

                    b.Property<bool>("IsEssential")
                        .HasColumnType("boolean")
                        .HasColumnName("is_essential");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("transaction_type");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_categories_parent_id");

                    b.HasIndex("Name", "TransactionType")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_name_transaction_type");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("WebApi.Model.FinancialGoal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<decimal>("Goal")
                        .HasColumnType("numeric")
                        .HasColumnName("goal");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_id");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_financial_goals");

                    b.HasIndex("PeriodId")
                        .HasDatabaseName("ix_financial_goals_period_id");

                    b.HasIndex("CategoryId", "PeriodId")
                        .HasDatabaseName("ix_financial_goals_category_id_period_id");

                    b.ToTable("financial_goals", (string)null);
                });

            modelBuilder.Entity("WebApi.Model.Period", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("End")
                        .HasColumnType("date")
                        .HasColumnName("end");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("name");

                    b.Property<DateOnly>("Start")
                        .HasColumnType("date")
                        .HasColumnName("start");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_periods");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_periods_name");

                    b.HasIndex("Name", "Start")
                        .HasDatabaseName("ix_periods_name_start");

                    b.ToTable("periods", (string)null);
                });

            modelBuilder.Entity("WebApi.Model.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("description");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted");

                    b.Property<DateOnly>("InvoiceDate")
                        .HasColumnType("date")
                        .HasColumnName("invoice_date");

                    b.Property<decimal?>("InvoiceValue")
                        .HasColumnType("numeric")
                        .HasColumnName("invoice_value");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_id");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_transactions_category_id");

                    b.HasIndex("InvoiceDate")
                        .IsDescending()
                        .HasDatabaseName("ix_transactions_invoice_date");

                    b.HasIndex("PeriodId")
                        .HasDatabaseName("ix_transactions_period_id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("WebApi.Model.TransactionPart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("description");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.Property<Guid?>("TransactionId1")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id1");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_transaction_parts");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_transaction_parts_category_id");

                    b.HasIndex("TransactionId")
                        .HasDatabaseName("ix_transaction_parts_transaction_id");

                    b.HasIndex("TransactionId1")
                        .HasDatabaseName("ix_transaction_parts_transaction_id1");

                    b.ToTable("transaction_parts", (string)null);
                });

            modelBuilder.Entity("WebApi.Model.Category", b =>
                {
                    b.HasOne("WebApi.Model.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_categories_categories_parent_id");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("WebApi.Model.FinancialGoal", b =>
                {
                    b.HasOne("WebApi.Model.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_financial_goals_categories_category_id");

                    b.HasOne("WebApi.Model.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_financial_goals_periods_period_id");

                    b.Navigation("Category");

                    b.Navigation("Period");
                });

            modelBuilder.Entity("WebApi.Model.Transaction", b =>
                {
                    b.HasOne("WebApi.Model.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_categories_category_id");

                    b.HasOne("WebApi.Model.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_periods_period_id");

                    b.Navigation("Category");

                    b.Navigation("Period");
                });

            modelBuilder.Entity("WebApi.Model.TransactionPart", b =>
                {
                    b.HasOne("WebApi.Model.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_transaction_parts_categories_category_id");

                    b.HasOne("WebApi.Model.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transaction_parts_transactions_transaction_id");

                    b.HasOne("WebApi.Model.Transaction", null)
                        .WithMany("Parts")
                        .HasForeignKey("TransactionId1")
                        .HasConstraintName("fk_transaction_parts_transactions_transaction_id1");

                    b.Navigation("Category");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("WebApi.Model.Transaction", b =>
                {
                    b.Navigation("Parts");
                });
#pragma warning restore 612, 618
        }
    }
}
