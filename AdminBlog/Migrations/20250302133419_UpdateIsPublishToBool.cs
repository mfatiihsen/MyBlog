using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminBlog.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIsPublishToBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublish",
                table: "Blog",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsPublish",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
