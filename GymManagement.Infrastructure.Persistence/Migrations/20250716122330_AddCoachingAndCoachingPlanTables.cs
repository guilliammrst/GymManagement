using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachingAndCoachingPlanTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "coaching-plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    coach_id = table.Column<int>(type: "integer", nullable: false),
                    club_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coaching-plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_coaching-plans_clubs_club_id",
                        column: x => x.club_id,
                        principalTable: "clubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_coaching-plans_users_coach_id",
                        column: x => x.coach_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coachings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    renew_when_expiry = table.Column<bool>(type: "boolean", nullable: false),
                    week_day = table.Column<int>(type: "integer", nullable: false),
                    hour = table.Column<int>(type: "integer", nullable: false),
                    member_id = table.Column<int>(type: "integer", nullable: false),
                    coaching_plan_id = table.Column<int>(type: "integer", nullable: false),
                    payment_detail_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coachings", x => x.id);
                    table.ForeignKey(
                        name: "FK_coachings_coaching-plans_coaching_plan_id",
                        column: x => x.coaching_plan_id,
                        principalTable: "coaching-plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coachings_payment_details_payment_detail_id",
                        column: x => x.payment_detail_id,
                        principalTable: "payment_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coachings_users_member_id",
                        column: x => x.member_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_coaching-plans_club_id",
                table: "coaching-plans",
                column: "club_id");

            migrationBuilder.CreateIndex(
                name: "IX_coaching-plans_coach_id",
                table: "coaching-plans",
                column: "coach_id");

            migrationBuilder.CreateIndex(
                name: "IX_coachings_coaching_plan_id",
                table: "coachings",
                column: "coaching_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_coachings_member_id",
                table: "coachings",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_coachings_payment_detail_id",
                table: "coachings",
                column: "payment_detail_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coachings");

            migrationBuilder.DropTable(
                name: "coaching-plans");
        }
    }
}
