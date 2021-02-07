using System;

namespace SupMusic.Models
{
    public class AddPlaylistModel
    {
        public int PlaylistId { get; set; }
        public string UserId { get; set; }
        public string SongId { get; set; }
    }
}
