using System;
using CompanyManager.Domain.Shared.Contracts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedAtToEmployeeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt", 
                table: "Employees", 
                type: "timestamp with time zone", 
                nullable: false,
                defaultValue: TimeProvider.Now);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");
        }
    }
}
