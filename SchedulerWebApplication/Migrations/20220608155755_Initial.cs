using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchedulerWebApplication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InputType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OutputType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Command = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DefaultEnvironmentVariables = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Executors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executors_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalAccounts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MicrosoftAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MicrosoftAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrosoftAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MicrosoftAccounts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    EnvironmentVariables = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutorStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutorId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutorStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutorStatuses_Executors_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Executors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    FlowTaskId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flows_FlowTasks_FlowTaskId",
                        column: x => x.FlowTaskId,
                        principalTable: "FlowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flows_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingUps",
                columns: table => new
                {
                    SuccessorId = table.Column<int>(type: "int", nullable: false),
                    PredecessorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingUps", x => new { x.PredecessorId, x.SuccessorId });
                    table.ForeignKey(
                        name: "FK_StartingUps_FlowTasks_PredecessorId",
                        column: x => x.PredecessorId,
                        principalTable: "FlowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StartingUps_FlowTasks_SuccessorId",
                        column: x => x.SuccessorId,
                        principalTable: "FlowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunDate = table.Column<long>(type: "bigint", nullable: false),
                    FlowId = table.Column<int>(type: "int", nullable: false),
                    ExecutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowRuns_Executors_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Executors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowRuns_Flows_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowRunId = table.Column<int>(type: "int", nullable: false),
                    FlowTaskId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Date = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowTaskStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowTaskStatuses_FlowRuns_FlowRunId",
                        column: x => x.FlowRunId,
                        principalTable: "FlowRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowTaskStatuses_FlowTasks_FlowTaskId",
                        column: x => x.FlowTaskId,
                        principalTable: "FlowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Executors_PersonId",
                table: "Executors",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutorStatuses_ExecutorId",
                table: "ExecutorStatuses",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuns_ExecutorId",
                table: "FlowRuns",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuns_FlowId",
                table: "FlowRuns",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Flows_FlowTaskId",
                table: "Flows",
                column: "FlowTaskId",
                unique: true,
                filter: "[FlowTaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Flows_PersonId",
                table: "Flows",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowTasks_TaskId",
                table: "FlowTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowTaskStatuses_FlowRunId",
                table: "FlowTaskStatuses",
                column: "FlowRunId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowTaskStatuses_FlowTaskId",
                table: "FlowTaskStatuses",
                column: "FlowTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalAccounts_PersonId",
                table: "LocalAccounts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MicrosoftAccounts_PersonId",
                table: "MicrosoftAccounts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingUps_SuccessorId",
                table: "StartingUps",
                column: "SuccessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutorStatuses");

            migrationBuilder.DropTable(
                name: "FlowTaskStatuses");

            migrationBuilder.DropTable(
                name: "LocalAccounts");

            migrationBuilder.DropTable(
                name: "MicrosoftAccounts");

            migrationBuilder.DropTable(
                name: "StartingUps");

            migrationBuilder.DropTable(
                name: "FlowRuns");

            migrationBuilder.DropTable(
                name: "Executors");

            migrationBuilder.DropTable(
                name: "Flows");

            migrationBuilder.DropTable(
                name: "FlowTasks");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
