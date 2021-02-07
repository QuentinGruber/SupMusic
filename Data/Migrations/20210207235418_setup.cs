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
                    { 1, "Big Fiesta/Party Playlist", null, "1,4", "party,clubbing,bringue,all night long", false },
                    { 2, "Chill", null, "5,3,2,6", "relax, very relax", false }
                });

            migrationBuilder.InsertData(
                table: "Song",
                columns: new[] { "ID", "Name", "OwnerID", "Path", "Tags" },
                values: new object[,]
                {
                    { 1, "feteMan", "-1", "/songs/fete.wav", "party,clubbing" },
                    { 2, "Doja Cat", "-1", "/songs/Doja Cat - Say So (Official Video).mp3", "egirl" },
                    { 3, "Serpent Maigre", "-1", "/songs/serpent-maigre.wav", "jazz,clarinet,orchestra" },
                    { 4, "Hey Hey Hey - Carlos feat Bitconnect", "-1", "/songs/bitconnect-remix-warning-scam.mp3", "Scam,Bold man" },
                    { 5, "The song of the great Monarch - Sylvain Durif ", "-1", "/songs/la-chanson-du-grand-monarque-sylvain-durif-cest-moi.mp3", "cloud" },
                    { 6, "I've Had The Time Of My Life", "-1", "/songs/dirty-dancing-soundtrack-ive-had-the-time-of-my-life.mp3", "Dirty Dancing" }
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
