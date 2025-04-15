using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "ExternalCustomers",
                newName: "Phone");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExternalProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ExternalCustomers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExternalCustomers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ExternalBranches",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExternalBranches",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExternalProducts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ExternalCustomers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExternalCustomers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExternalBranches");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "ExternalCustomers",
                newName: "ContactInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ExternalBranches",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);
        }
    }
}
