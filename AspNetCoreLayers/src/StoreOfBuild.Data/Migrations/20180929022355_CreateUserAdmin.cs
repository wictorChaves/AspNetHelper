using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using StoreOfBuild.Data.Contexts;

namespace StoreOfBuild.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180929022355_CreateUserAdmin")]
    public partial class CreateUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into [dbo].[AspNetRoles] values ('BD2AF962-6AB3-4D28-A902-0A8511750AB1', null, 'Admin', 'Admin');
                                insert into [dbo].[AspNetRoles] values ('813F9DB8-8601-4C82-BCE3-D122BC3E3AB9', null, 'Manager', 'Manager');
                                insert into [dbo].[AspNetRoles] values ('F2493BA0-B8CA-4E97-A882-DEA90B9E1728', null, 'Operation', 'Operation');");

            //@Axsd12 
            migrationBuilder.Sql(@"insert into [AspNetUsers] values
                ('6d9a6ca2-9d24-4ca2-ad4b-6265a818d7d4','wictor','wictor','admin@admin.com','admin@admin.com',0,'AQAAAAEAACcQAAAAEIIWoviAu641wICvbFTecu/e8tUNiQXxYQ9JaEUXLYmdcSrSS6OnOmJg1U6kxQgGbQ==', null,null,'31995331955','31995331955',0,null,0,0)");

            migrationBuilder.Sql(@"insert into [AspNetUserRoles] values ('6d9a6ca2-9d24-4ca2-ad4b-6265a818d7d4', 'BD2AF962-6AB3-4D28-A902-0A8511750AB1')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
