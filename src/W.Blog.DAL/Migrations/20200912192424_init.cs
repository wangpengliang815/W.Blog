using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace W.Blog.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false, comment: "类别名称"),
                    ParentId = table.Column<string>(maxLength: 100, nullable: false, comment: "类别父级Id"),
                    Order = table.Column<int>(nullable: false, comment: "排序字段")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Category", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Category");
        }
    }
}
