using Microsoft.EntityFrameworkCore.Migrations;

namespace SchedulerWebApplication.Migrations
{
    public partial class FlowTaskStatusEntityAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FlowRunId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FlowTaskId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    StatusCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    Date = table.Column<long>(type: "NUMBER(19)", nullable: false)
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
                name: "IX_FlowTaskStatuses_FlowRunId",
                table: "FlowTaskStatuses",
                column: "FlowRunId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowTaskStatuses_FlowTaskId",
                table: "FlowTaskStatuses",
                column: "FlowTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowTaskStatuses");
        }
    }
}
