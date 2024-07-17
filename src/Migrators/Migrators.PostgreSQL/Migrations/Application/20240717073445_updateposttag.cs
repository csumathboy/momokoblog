using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class updateposttag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "Posts",
                table: "PostTag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "Posts",
                table: "PostTag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Posts",
                table: "PostTag",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Posts",
                table: "PostTag",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Posts",
                table: "PostTag",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                schema: "Posts",
                table: "PostTag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "Posts",
                table: "PostTag",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
