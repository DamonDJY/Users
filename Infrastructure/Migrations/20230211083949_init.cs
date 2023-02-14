using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_UserLoginHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhoneNumberRegionCode = table.Column<int>(name: "PhoneNumber_RegionCode", type: "int", unicode: false, maxLength: 5, nullable: false),
                    PhoneNumberNumber = table.Column<string>(name: "PhoneNumber_Number", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserLoginHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumberRegionCode = table.Column<int>(name: "PhoneNumber_RegionCode", type: "int", unicode: false, maxLength: 5, nullable: false),
                    PhoneNumberNumber = table.Column<string>(name: "PhoneNumber_Number", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    passwordHash = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_UserAcessFails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LookEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailCount = table.Column<int>(type: "int", nullable: false),
                    isLookOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserAcessFails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserAcessFails_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_UserAcessFails_UserId",
                table: "T_UserAcessFails",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_UserAcessFails");

            migrationBuilder.DropTable(
                name: "T_UserLoginHistories");

            migrationBuilder.DropTable(
                name: "T_Users");
        }
    }
}
