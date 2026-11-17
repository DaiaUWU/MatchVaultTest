using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchVault.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchType",
                table: "Matches",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Matches",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Winner",
                table: "Matches",
                type: "TEXT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_EventId",
                table: "Matches",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Events_EventId",
                table: "Matches",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Events_EventId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_EventId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchType",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Matches");
        }
    }
}
