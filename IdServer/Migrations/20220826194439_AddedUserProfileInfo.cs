using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class AddedUserProfileInfo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTimeOffset>(
            name: "BirthDate",
            table: "StoredUsers",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        _ = migrationBuilder.AddColumn<string>(
            name: "FamilyName",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "Gender",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "GivenName",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "Locale",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "MiddleName",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "Nickname",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "PictureUrl",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "PreferredUsername",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "ProfileUrl",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "UpdatedAt",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "Website",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<string>(
            name: "ZoneInfo",
            table: "StoredUsers",
            type: "TEXT",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "BirthDate",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "FamilyName",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "Gender",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "GivenName",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "Locale",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "MiddleName",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "Name",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "Nickname",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "PictureUrl",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "PreferredUsername",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "ProfileUrl",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "Website",
            table: "StoredUsers");

        _ = migrationBuilder.DropColumn(
            name: "ZoneInfo",
            table: "StoredUsers");
    }
}
