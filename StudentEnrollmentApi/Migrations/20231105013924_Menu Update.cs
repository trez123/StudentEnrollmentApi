using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentApi.Migrations
{
    /// <inheritdoc />
    public partial class MenuUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentImageFilePath",
                table: "Students",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MealTyes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTyes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Starch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Beverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vegetable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    MenuImageFilePath = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_MealTyes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTyes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MealTypeId",
                table: "Menus",
                column: "MealTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "MealTyes");

            migrationBuilder.DropColumn(
                name: "StudentImageFilePath",
                table: "Students");
        }
    }
}
