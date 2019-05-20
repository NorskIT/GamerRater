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
                name: "GameCover",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    game = table.Column<int>(nullable: false),
                    height = table.Column<int>(nullable: false),
                    image_id = table.Column<string>(nullable: true),
                    width = table.Column<int>(nullable: false),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCover", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    GameCoverid = table.Column<int>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Cover = table.Column<int>(nullable: false),
                    Created_at = table.Column<int>(nullable: false),
                    Total_rating = table.Column<double>(nullable: false),
                    Storyline = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Popularity = table.Column<double>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Updated_at = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    GameCoverid1 = table.Column<int>(nullable: true),
                    PlatformId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.id);
                    table.ForeignKey(
                        name: "FK_Games_GameCover_GameCoverid",
                        column: x => x.GameCoverid,
                        principalTable: "GameCover",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_GameCover_GameCoverid1",
                        column: x => x.GameCoverid1,
                        principalTable: "GameCover",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(nullable: false),
                    Gameid = table.Column<int>(nullable: true),
                    UserId1 = table.Column<int>(nullable: true),
                    Stars = table.Column<int>(nullable: false),
                    Review = table.Column<string>(nullable: true),
                    GameRootid = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Games_GameRootid",
                        column: x => x.GameRootid,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Games_Gameid",
                        column: x => x.Gameid,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameCoverid",
                table: "Games",
                column: "GameCoverid");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameCoverid1",
                table: "Games",
                column: "GameCoverid1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlatformId",
                table: "Games",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_GameRootid",
                table: "Ratings",
                column: "GameRootid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Gameid",
                table: "Ratings",
                column: "Gameid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId1",
                table: "Ratings",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameCover");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
