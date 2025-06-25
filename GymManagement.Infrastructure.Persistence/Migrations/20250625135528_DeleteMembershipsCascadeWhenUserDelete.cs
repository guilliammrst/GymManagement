using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMembershipsCascadeWhenUserDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memberships_users_member_id",
                table: "memberships");

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_users_member_id",
                table: "memberships",
                column: "member_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memberships_users_member_id",
                table: "memberships");

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_users_member_id",
                table: "memberships",
                column: "member_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
