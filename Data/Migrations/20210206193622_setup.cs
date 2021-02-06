using Microsoft.EntityFrameworkCore.Migrations;

namespace SupMusic.Data.Migrations
{
    public partial class setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Songs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isPrivate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Song",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Playlist",
                columns: new[] { "ID", "Name", "OwnerID", "Songs", "Tags", "isPrivate" },
                values: new object[,]
                {
                    { 1, "Playlist de test public", null, "1,2", "fete, clubbing", true },
                    { 2, "Playlist de test privé", null, "1,2", "fete, clubbing", false }
                });

            migrationBuilder.InsertData(
                table: "Song",
                columns: new[] { "ID", "Duration", "Name", "OwnerID", "Path", "Tags" },
                values: new object[,]
                {
                    { 1, 69, "feteMan", "-1", "/songs/fete.wav", "fete, clubbing" },
                    { 2, 69, "Doja Cat", "-1", "/songs/Doja Cat - Say So (Official Video).mp3", "pas, fou, egirl" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Song");
        }
    }
}
