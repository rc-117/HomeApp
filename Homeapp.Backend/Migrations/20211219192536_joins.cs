using Microsoft.EntityFrameworkCore.Migrations;

namespace Homeapp.Backend.Migrations
{
    public partial class joins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHousehold_Households_HouseholdId",
                table: "UserHousehold");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHousehold_Users_UserId",
                table: "UserHousehold");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholdGroup_HouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholdGroup_Users_UserId",
                table: "UserHouseholdGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHouseholdGroup",
                table: "UserHouseholdGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHousehold",
                table: "UserHousehold");

            migrationBuilder.RenameTable(
                name: "UserHouseholdGroup",
                newName: "UserHouseholdGroups");

            migrationBuilder.RenameTable(
                name: "UserHousehold",
                newName: "UserHouseholds");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholdGroup_UserId",
                table: "UserHouseholdGroups",
                newName: "IX_UserHouseholdGroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholdGroup_HouseholdGroupId",
                table: "UserHouseholdGroups",
                newName: "IX_UserHouseholdGroups_HouseholdGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHousehold_UserId",
                table: "UserHouseholds",
                newName: "IX_UserHouseholds_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHousehold_HouseholdId",
                table: "UserHouseholds",
                newName: "IX_UserHouseholds_HouseholdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHouseholdGroups",
                table: "UserHouseholdGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHouseholds",
                table: "UserHouseholds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholdGroups_HouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroups",
                column: "HouseholdGroupId",
                principalTable: "HouseholdGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholdGroups_Users_UserId",
                table: "UserHouseholdGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholds_Households_HouseholdId",
                table: "UserHouseholds",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholds_Users_UserId",
                table: "UserHouseholds",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholdGroups_HouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholdGroups_Users_UserId",
                table: "UserHouseholdGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholds_Households_HouseholdId",
                table: "UserHouseholds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholds_Users_UserId",
                table: "UserHouseholds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHouseholds",
                table: "UserHouseholds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHouseholdGroups",
                table: "UserHouseholdGroups");

            migrationBuilder.RenameTable(
                name: "UserHouseholds",
                newName: "UserHousehold");

            migrationBuilder.RenameTable(
                name: "UserHouseholdGroups",
                newName: "UserHouseholdGroup");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholds_UserId",
                table: "UserHousehold",
                newName: "IX_UserHousehold_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholds_HouseholdId",
                table: "UserHousehold",
                newName: "IX_UserHousehold_HouseholdId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholdGroups_UserId",
                table: "UserHouseholdGroup",
                newName: "IX_UserHouseholdGroup_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroup",
                newName: "IX_UserHouseholdGroup_HouseholdGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHousehold",
                table: "UserHousehold",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHouseholdGroup",
                table: "UserHouseholdGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHousehold_Households_HouseholdId",
                table: "UserHousehold",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHousehold_Users_UserId",
                table: "UserHousehold",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholdGroup_HouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroup",
                column: "HouseholdGroupId",
                principalTable: "HouseholdGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholdGroup_Users_UserId",
                table: "UserHouseholdGroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
