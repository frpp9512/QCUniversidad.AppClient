using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedUserProfileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "StoredUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredUsername",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileUrl",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedAt",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneInfo",
                table: "StoredUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "PreferredUsername",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "ProfileUrl",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "StoredUsers");

            migrationBuilder.DropColumn(
                name: "ZoneInfo",
                table: "StoredUsers");
        }
    }
}
