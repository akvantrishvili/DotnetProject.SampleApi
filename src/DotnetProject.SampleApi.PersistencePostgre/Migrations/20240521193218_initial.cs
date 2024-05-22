using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotnetProject.SampleApi.PersistencePostgre.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActualAddress_City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActualAddress_ZipCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ActualAddress_Address1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ActualAddress_Address2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    LegalAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LegalAddress_City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LegalAddress_ZipCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LegalAddress_Address1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    LegalAddress_Address2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    DocumentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PersonalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOfExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityDocuments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityDocuments_CustomerId",
                table: "IdentityDocuments",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityDocuments");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
