using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectIO.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacilityTypes",
                columns: table => new
                {
                    typeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTypes", x => x.typeId);
                });

            migrationBuilder.CreateTable(
                name: "PassTypes",
                columns: table => new
                {
                    passTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    passTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    passTypeDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassTypes", x => x.passTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SportsCenters",
                columns: table => new
                {
                    centerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    centerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    centerStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    centerStreetNumber = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    centerCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    centerState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    centerZip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsCenters", x => x.centerId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessions",
                columns: table => new
                {
                    sessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sessionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sessionDuration = table.Column<int>(type: "int", nullable: false),
                    sessionCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessions", x => x.sessionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "WorkerFunctions",
                columns: table => new
                {
                    functionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    functionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerFunctions", x => x.functionId);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTrainingSessions",
                columns: table => new
                {
                    workerTrainingSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workerId = table.Column<int>(type: "int", nullable: false),
                    sessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTrainingSessions", x => x.workerTrainingSessionId);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    facilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeId = table.Column<int>(type: "int", nullable: false),
                    centerId = table.Column<int>(type: "int", nullable: false),
                    facilityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isChangingRoomAvailable = table.Column<bool>(type: "bit", nullable: false),
                    isEquipmentAvailable = table.Column<bool>(type: "bit", nullable: false),
                    promoStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.facilityId);
                    table.ForeignKey(
                        name: "FK_Facilities_FacilityTypes_typeId",
                        column: x => x.typeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "typeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facilities_SportsCenters_centerId",
                        column: x => x.centerId,
                        principalTable: "SportsCenters",
                        principalColumn: "centerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passes",
                columns: table => new
                {
                    passId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    passTypeId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    passEntriesLeft = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passes", x => x.passId);
                    table.ForeignKey(
                        name: "FK_Passes_PassTypes_passTypeId",
                        column: x => x.passTypeId,
                        principalTable: "PassTypes",
                        principalColumn: "passTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Passes_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginCredentials",
                columns: table => new
                {
                    loginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginCredentials", x => x.loginId);
                    table.ForeignKey(
                        name: "FK_UserLoginCredentials_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    workerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    functionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.workerId);
                    table.ForeignKey(
                        name: "FK_Workers_WorkerFunctions_functionId",
                        column: x => x.functionId,
                        principalTable: "WorkerFunctions",
                        principalColumn: "functionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    facilityId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    reservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reservationStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.reservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Facilities_facilityId",
                        column: x => x.facilityId,
                        principalTable: "Facilities",
                        principalColumn: "facilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerLoginCredentials",
                columns: table => new
                {
                    loginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerLoginCredentials", x => x.loginId);
                    table.ForeignKey(
                        name: "FK_WorkerLoginCredentials_Workers_workerId",
                        column: x => x.workerId,
                        principalTable: "Workers",
                        principalColumn: "workerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_centerId",
                table: "Facilities",
                column: "centerId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_typeId",
                table: "Facilities",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "IX_Passes_passTypeId",
                table: "Passes",
                column: "passTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Passes_userId",
                table: "Passes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_facilityId",
                table: "Reservations",
                column: "facilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_userId",
                table: "Reservations",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginCredentials_userId",
                table: "UserLoginCredentials",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerLoginCredentials_workerId",
                table: "WorkerLoginCredentials",
                column: "workerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_functionId",
                table: "Workers",
                column: "functionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passes");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "TrainingSessions");

            migrationBuilder.DropTable(
                name: "UserLoginCredentials");

            migrationBuilder.DropTable(
                name: "WorkerLoginCredentials");

            migrationBuilder.DropTable(
                name: "WorkerTrainingSessions");

            migrationBuilder.DropTable(
                name: "PassTypes");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "FacilityTypes");

            migrationBuilder.DropTable(
                name: "SportsCenters");

            migrationBuilder.DropTable(
                name: "WorkerFunctions");
        }
    }
}
