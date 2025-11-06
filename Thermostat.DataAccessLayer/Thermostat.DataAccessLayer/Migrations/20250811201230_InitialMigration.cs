using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thermostat.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatingSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TempLow = table.Column<double>(type: "float", nullable: false),
                    TempHigh = table.Column<double>(type: "float", nullable: false),
                    HumidityAlertThreshold = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseRootId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomArea = table.Column<double>(type: "float", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Houses_HouseRootId",
                        column: x => x.HouseRootId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseRootId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Houses_HouseRootId",
                        column: x => x.HouseRootId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SensorMeasurements",
                columns: table => new
                {
                    HouseRootId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorMeasurements", x => new { x.HouseRootId, x.SensorEntityId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_SensorMeasurements_Houses_HouseRootId",
                        column: x => x.HouseRootId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SensorMeasurements_Sensors_SensorEntityId",
                        column: x => x.SensorEntityId,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RuleEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionDefinitions_Rules_RuleEntityId",
                        column: x => x.RuleEntityId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleDefinitions_Rules_RuleEntityId",
                        column: x => x.RuleEntityId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParameterValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionDefinitionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionParameters_ActionDefinitions_ActionDefinitionEntityId",
                        column: x => x.ActionDefinitionEntityId,
                        principalTable: "ActionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuleDefinitionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleParameters_RuleDefinitions_RuleDefinitionEntityId",
                        column: x => x.RuleDefinitionEntityId,
                        principalTable: "RuleDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d4"), "Default House" });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "HouseRootId", "RoomArea", "RoomName", "RoomType", "SensorType", "MeasurementUnit" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), new Guid("90a6909d-5d14-4220-b035-b60926edd2d4"), 20.0, "Living Room", "LivingRoom", "Temperature", "Celsius" },
                    { new Guid("b1c2d3e4-f5a6-7890-abcd-ef1234567890"), new Guid("90a6909d-5d14-4220-b035-b60926edd2d4"), 15.0, "Bedroom", "Bedroom", "Humidity", "Percent" }
                });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "Id", "Enabled", "HouseRootId", "Name", "SensorId" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d6"), true, new Guid("90a6909d-5d14-4220-b035-b60926edd2d4"), "Stop Heating Rule if temp >= 23°C", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.InsertData(
                table: "ActionDefinitions",
                columns: new[] { "Id", "CreatedAt", "RuleEntityId", "Type" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d5"), new DateTime(2015, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("90a6909d-5d14-4220-b035-b60926edd2d6"), "HeatingControl" });

            migrationBuilder.InsertData(
                table: "RuleDefinitions",
                columns: new[] { "Id", "RuleEntityId", "Type" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d7"), new Guid("90a6909d-5d14-4220-b035-b60926edd2d6"), "GreaterThan" });

            migrationBuilder.InsertData(
                table: "ActionParameters",
                columns: new[] { "Id", "ActionDefinitionEntityId", "ParameterName", "Type", "ParameterValue" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d7"), new Guid("90a6909d-5d14-4220-b035-b60926edd2d5"), "Command", "System.String", "Stop" });

            migrationBuilder.InsertData(
                table: "RuleParameters",
                columns: new[] { "Id", "Name", "RuleDefinitionEntityId", "Type", "Value" },
                values: new object[] { new Guid("90a6909d-5d14-4220-b035-b60926edd2d9"), "Threshold", new Guid("90a6909d-5d14-4220-b035-b60926edd2d7"), "System.Double", "23.0" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionDefinitions_RuleEntityId",
                table: "ActionDefinitions",
                column: "RuleEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionParameters_ActionDefinitionEntityId",
                table: "ActionParameters",
                column: "ActionDefinitionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionParameters_Name",
                table: "ActionParameters",
                column: "ParameterName");

            migrationBuilder.CreateIndex(
                name: "IX_RuleDefinitions_RuleEntityId",
                table: "RuleDefinitions",
                column: "RuleEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RuleParameters_RuleDefinitionEntityId",
                table: "RuleParameters",
                column: "RuleDefinitionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_HouseRootId",
                table: "Rules",
                column: "HouseRootId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_SensorId",
                table: "Rules",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorMeasurements_SensorId_Timestamp",
                table: "SensorMeasurements",
                columns: new[] { "SensorEntityId", "Timestamp" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_SensorMeasurements_Timestamp",
                table: "SensorMeasurements",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_HouseRootId",
                table: "Sensors",
                column: "HouseRootId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionParameters");

            migrationBuilder.DropTable(
                name: "HeatingSettings");

            migrationBuilder.DropTable(
                name: "RuleParameters");

            migrationBuilder.DropTable(
                name: "SensorMeasurements");

            migrationBuilder.DropTable(
                name: "ActionDefinitions");

            migrationBuilder.DropTable(
                name: "RuleDefinitions");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Houses");
        }
    }
}
