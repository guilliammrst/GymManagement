using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDetailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "payment_detail_id",
                table: "memberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "payment_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_details", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_memberships_payment_detail_id",
                table: "memberships",
                column: "payment_detail_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_payment_details_payment_detail_id",
                table: "memberships",
                column: "payment_detail_id",
                principalTable: "payment_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memberships_payment_details_payment_detail_id",
                table: "memberships");

            migrationBuilder.DropTable(
                name: "payment_details");

            migrationBuilder.DropIndex(
                name: "IX_memberships_payment_detail_id",
                table: "memberships");

            migrationBuilder.DropColumn(
                name: "payment_detail_id",
                table: "memberships");
        }
    }
}
