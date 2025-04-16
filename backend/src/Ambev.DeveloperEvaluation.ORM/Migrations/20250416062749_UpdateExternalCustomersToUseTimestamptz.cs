using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExternalCustomersToUseTimestamptz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ExternalCustomers",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ExternalCustomers",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.Sql(@"
        ALTER TABLE ""ExternalCustomers""
        ALTER COLUMN ""CreatedAt"" TYPE timestamptz USING ""CreatedAt"" AT TIME ZONE 'UTC',
        ALTER COLUMN ""UpdatedAt"" TYPE timestamptz USING ""UpdatedAt"" AT TIME ZONE 'UTC';
    ");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
