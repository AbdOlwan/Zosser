using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL_OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class EditCoulturInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoriesTranslations_CategoryID",
                table: "CategoriesTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Culture",
                table: "CategoriesTranslations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category_Name",
                table: "CategoriesTranslations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesTranslations_CategoryID_Culture",
                table: "CategoriesTranslations",
                columns: new[] { "CategoryID", "Culture" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoriesTranslations_CategoryID_Culture",
                table: "CategoriesTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Culture",
                table: "CategoriesTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Category_Name",
                table: "CategoriesTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesTranslations_CategoryID",
                table: "CategoriesTranslations",
                column: "CategoryID");
        }
    }
}
