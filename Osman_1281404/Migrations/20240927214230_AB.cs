using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osman_1281404.Migrations
{
    /// <inheritdoc />
    public partial class AB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    RestaurantType = table.Column<int>(type: "int", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "date", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.RestaurantId);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    FoodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.FoodId);
                    table.ForeignKey(
                        name: "FK_Foods_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });
            //1
            string sql = @"CREATE PROCEDURE InsertFood
                @fn NVARCHAR(60),
                @p MONEY,
                @rid INT

               AS
              INSERT INTO Foods(FoodName, Price,  RestaurantId) VALUES(@fn, @p, @rid)

              RETURN 0";
            migrationBuilder.Sql(sql);
            //2
            sql = @"CREATE PROCEDURE DeleteFoodsOfRestaurant
                @rid INT

                AS
                DELETE Foods WHERE RestaurantId=@rid

             RETURN 0";
            migrationBuilder.Sql(sql);
            //3
            sql = @"CREATE PROCEDURE DeleteRestaurant
                @rid INT

                AS
                DELETE Restaurants WHERE RestaurantId=@rid

             RETURN 0";
            migrationBuilder.Sql(sql);
            migrationBuilder.CreateIndex(
                name: "IX_Foods_RestaurantId",
                table: "Foods",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Restaurants");
            migrationBuilder.Sql("DROP PROCEDURE InsertFood");
            migrationBuilder.Sql("DROP PROCEDURE DeleteFoodsOfRestaurant");
            migrationBuilder.Sql("DROP PROCEDURE DeleteRestaurant");

        }
    }
}
