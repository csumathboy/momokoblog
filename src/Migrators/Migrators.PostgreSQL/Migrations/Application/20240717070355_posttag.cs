using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class posttag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tag_TagsId",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_TagsId",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                schema: "Posts",
                table: "PostTag",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                schema: "Posts",
                table: "PostTag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "Id",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                schema: "Posts",
                table: "PostTag",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                schema: "Posts",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tag_TagId",
                schema: "Posts",
                table: "PostTag",
                column: "TagId",
                principalSchema: "Posts",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tag_TagId",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_TagId",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "TagId",
                schema: "Posts",
                table: "PostTag");

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
                name: "Id",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "Posts",
                table: "PostTag");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Posts",
                table: "PostTag",
                newName: "TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                schema: "Posts",
                table: "PostTag",
                columns: new[] { "PostId", "TagsId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                schema: "Posts",
                table: "PostTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tag_TagsId",
                schema: "Posts",
                table: "PostTag",
                column: "TagsId",
                principalSchema: "Posts",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
