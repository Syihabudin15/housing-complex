using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HousingComplex.Migrations
{
    /// <inheritdoc />
    public partial class solvedNameInHouseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "filesize",
                table: "m_image_house_type",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "m_house_type",
                type: "Varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "m_house_type");

            migrationBuilder.AlterColumn<string>(
                name: "filesize",
                table: "m_image_house_type",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
