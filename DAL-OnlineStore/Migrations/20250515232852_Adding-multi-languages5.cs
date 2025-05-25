using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL_OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class Addingmultilanguages5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTranslations_Brands_BrandId",
                table: "BrandTranslations");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "BrandTranslations",
                newName: "Brand_ID");

            migrationBuilder.RenameIndex(
                name: "IX_BrandTranslations_BrandId",
                table: "BrandTranslations",
                newName: "IX_BrandTranslations_Brand_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTranslations_Brands_Brand_ID",
                table: "BrandTranslations",
                column: "Brand_ID",
                principalTable: "Brands",
                principalColumn: "Brand_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTranslations_Brands_Brand_ID",
                table: "BrandTranslations");

            migrationBuilder.RenameColumn(
                name: "Brand_ID",
                table: "BrandTranslations",
                newName: "BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandTranslations_Brand_ID",
                table: "BrandTranslations",
                newName: "IX_BrandTranslations_BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTranslations_Brands_BrandId",
                table: "BrandTranslations",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Brand_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
