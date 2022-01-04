using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Homeapp.Backend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReadHouseholdIds = table.Column<string>(nullable: true),
                    ReadHouseholdGroupIds = table.Column<string>(nullable: true),
                    ReadUserIds = table.Column<string>(nullable: true),
                    EditHouseholdIds = table.Column<string>(nullable: true),
                    EditHouseholdGroupIds = table.Column<string>(nullable: true),
                    EditUserIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    HouseholdId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseholdGroups_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    StartingBalance = table.Column<double>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    SharedEntitiesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_SharedEntities_SharedEntitiesId",
                        column: x => x.SharedEntitiesId,
                        principalTable: "SharedEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHouseholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    HouseholdId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHouseholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHouseholds_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHouseholds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHouseholdGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    HouseholdGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHouseholdGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHouseholdGroups_HouseholdGroups_HouseholdGroupId",
                        column: x => x.HouseholdGroupId,
                        principalTable: "HouseholdGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHouseholdGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ExpenseCategoryId = table.Column<Guid>(nullable: true),
                    IncomeCategoryId = table.Column<Guid>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    RecurringType = table.Column<int>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    IsCleared = table.Column<bool>(nullable: false),
                    SharedEntitiesId = table.Column<int>(nullable: false),
                    SharedEntitiesId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_ExpenseCategories_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_IncomeCategories_IncomeCategoryId",
                        column: x => x.IncomeCategoryId,
                        principalTable: "IncomeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_SharedEntities_SharedEntitiesId1",
                        column: x => x.SharedEntitiesId1,
                        principalTable: "SharedEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_SharedEntitiesId",
                table: "Accounts",
                column: "SharedEntitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdGroups_HouseholdId",
                table: "HouseholdGroups",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExpenseCategoryId",
                table: "Transactions",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IncomeCategoryId",
                table: "Transactions",
                column: "IncomeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SharedEntitiesId1",
                table: "Transactions",
                column: "SharedEntitiesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholdGroups_HouseholdGroupId",
                table: "UserHouseholdGroups",
                column: "HouseholdGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholdGroups_UserId",
                table: "UserHouseholdGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholds_HouseholdId",
                table: "UserHouseholds",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholds_UserId",
                table: "UserHouseholds",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserHouseholdGroups");

            migrationBuilder.DropTable(
                name: "UserHouseholds");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "IncomeCategories");

            migrationBuilder.DropTable(
                name: "HouseholdGroups");

            migrationBuilder.DropTable(
                name: "SharedEntities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Households");
        }
    }
}
