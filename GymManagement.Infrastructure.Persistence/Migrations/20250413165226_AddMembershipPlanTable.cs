using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "membership_plan_id",
                table: "memberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "membership_plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    membership_plan_type = table.Column<int>(type: "integer", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric", nullable: false),
                    yearly_discount = table.Column<decimal>(type: "numeric", nullable: false),
                    registration_fees = table.Column<decimal>(type: "numeric", nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membership_plans", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_memberships_membership_plan_id",
                table: "memberships",
                column: "membership_plan_id");

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_membership_plans_membership_plan_id",
                table: "memberships",
                column: "membership_plan_id",
                principalTable: "membership_plans",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memberships_membership_plans_membership_plan_id",
                table: "memberships");

            migrationBuilder.DropTable(
                name: "membership_plans");

            migrationBuilder.DropIndex(
                name: "IX_memberships_membership_plan_id",
                table: "memberships");

            migrationBuilder.DropColumn(
                name: "membership_plan_id",
                table: "memberships");
        }
    }
}
