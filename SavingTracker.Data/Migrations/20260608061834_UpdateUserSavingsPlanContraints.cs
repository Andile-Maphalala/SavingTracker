using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSavingsPlanContraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavingsPlans_AspNetUsers_UserId",
                table: "UserSavingsPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavingsPlans_SavingsPlan_SavingsPlanId",
                table: "UserSavingsPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavingsPlans_AspNetUsers_UserId",
                table: "UserSavingsPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavingsPlans_SavingsPlan_SavingsPlanId",
                table: "UserSavingsPlans",
                column: "SavingsPlanId",
                principalTable: "SavingsPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavingsPlans_AspNetUsers_UserId",
                table: "UserSavingsPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavingsPlans_SavingsPlan_SavingsPlanId",
                table: "UserSavingsPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavingsPlans_AspNetUsers_UserId",
                table: "UserSavingsPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavingsPlans_SavingsPlan_SavingsPlanId",
                table: "UserSavingsPlans",
                column: "SavingsPlanId",
                principalTable: "SavingsPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
