using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rus_Carmen_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
     name: "Authors",
     columns: table => new
     {
         ID = table.Column<int>(type: "int", nullable: false)
             .Annotation("SqlServer:Identity", "1, 1"),
         FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
         LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
     },
     constraints: table =>
     {
         table.PrimaryKey("PK_Authors", x => x.ID);
     });

            migrationBuilder.AddColumn<int>(
                name: "AuthorsID",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
    INSERT INTO Authors (FirstName, LastName)
    SELECT DISTINCT
        LEFT(LTRIM(RTRIM(Author)),
             CHARINDEX(' ', LTRIM(RTRIM(Author)) + ' ') - 1),
        LTRIM(SUBSTRING(LTRIM(RTRIM(Author)),
             CHARINDEX(' ', LTRIM(RTRIM(Author)) + ' ') + 1,
             LEN(Author)))
    FROM Book
    WHERE Author IS NOT NULL AND LTRIM(RTRIM(Author)) <> '';

    UPDATE b
    SET b.AuthorsID = a.ID
    FROM Book b
    INNER JOIN Authors a
        ON b.Author = a.FirstName + ' ' + a.LastName;
");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorsID",
                table: "Book",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorsID",
                table: "Book",
                column: "AuthorsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Authors_AuthorsID",
                table: "Book",
                column: "AuthorsID",
                principalTable: "Authors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Book");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Authors_AuthorsID",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorsID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AuthorsID",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
