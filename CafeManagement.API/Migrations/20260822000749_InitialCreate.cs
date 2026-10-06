using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    SecondarySection = table.Column<int>(type: "int", nullable: true),
                    SecondaryLevel = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ThumbNailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturesUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedWaiterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBaristaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedChefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedCashierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MOrderItem",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderedUnitPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MOrderItem_Orders_MOrderId",
                        column: x => x.MOrderId,
                        principalSchema: "admin",
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOrderItem_MOrderId",
                schema: "admin",
                table: "MOrderItem",
                column: "MOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedBaristaId",
                schema: "admin",
                table: "Orders",
                column: "AssignedBaristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedCashierId",
                schema: "admin",
                table: "Orders",
                column: "AssignedCashierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedChefId",
                schema: "admin",
                table: "Orders",
                column: "AssignedChefId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedWaiterId",
                schema: "admin",
                table: "Orders",
                column: "AssignedWaiterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "MenuItems",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "MOrderItem",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Tables",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "admin");
        }
    }
}
