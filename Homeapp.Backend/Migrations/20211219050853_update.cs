using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Homeapp.Backend.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdGroups_Households_HouseholdId",
                table: "HouseholdGroups");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "HouseholdId",
                table: "HouseholdGroups",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdGroups_Households_HouseholdId",
                table: "HouseholdGroups",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdGroups_Households_HouseholdId",
                table: "HouseholdGroups");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "HouseholdId",
                table: "HouseholdGroups",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdGroups_Households_HouseholdId",
                table: "HouseholdGroups",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
