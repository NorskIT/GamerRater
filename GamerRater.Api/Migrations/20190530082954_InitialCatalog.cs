using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamerRater.Api.Migrations
{
    public partial class InitialCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Covers",
                table => new
                {
                    id = table.Column<int>(nullable: false),
                    game = table.Column<int>(nullable: false),
                    height = table.Column<int>(nullable: false),
                    image_id = table.Column<string>(nullable: true),
                    width = table.Column<int>(nullable: false),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Covers", x => x.id); });

            migrationBuilder.CreateTable(
                "UserGroups",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_UserGroups", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Games",
                table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GameCoverId = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Cover = table.Column<int>(nullable: false),
                    Created_at = table.Column<int>(nullable: false),
                    Total_rating = table.Column<double>(nullable: false),
                    Storyline = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Popularity = table.Column<double>(nullable: false),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        "FK_Games_Covers_GameCoverId",
                        x => x.GameCoverId,
                        "Covers",
                        "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "UserHasUserGroups",
                table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    UserGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasUserGroups", x => new {x.UserId, x.UserGroupId});
                    table.ForeignKey(
                        "FK_UserHasUserGroups_UserGroups_UserGroupId",
                        x => x.UserGroupId,
                        "UserGroups",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UserHasUserGroups_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Platforms",
                table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: true),
                    Alternative_name = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Created_at = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Platform_logo = table.Column<int>(nullable: false),
                    Product_family = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Updated_at = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    GameRootId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                    table.ForeignKey(
                        "FK_Platforms_Games_GameRootId",
                        x => x.GameRootId,
                        "Games",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Ratings",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(nullable: false),
                    GameRootId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Stars = table.Column<int>(nullable: false),
                    ReviewText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        "FK_Ratings_Games_GameRootId",
                        x => x.GameRootId,
                        "Games",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Ratings_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Games_GameCoverId",
                "Games",
                "GameCoverId");

            migrationBuilder.CreateIndex(
                "IX_Platforms_GameRootId",
                "Platforms",
                "GameRootId");

            migrationBuilder.CreateIndex(
                "IX_Ratings_GameRootId",
                "Ratings",
                "GameRootId");

            migrationBuilder.CreateIndex(
                "IX_Ratings_UserId",
                "Ratings",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_UserHasUserGroups_UserGroupId",
                "UserHasUserGroups",
                "UserGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Platforms");

            migrationBuilder.DropTable(
                "Ratings");

            migrationBuilder.DropTable(
                "UserHasUserGroups");

            migrationBuilder.DropTable(
                "Games");

            migrationBuilder.DropTable(
                "UserGroups");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Covers");
        }
    }
}