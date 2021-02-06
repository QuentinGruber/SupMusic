using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace SupMusic.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Songs { get; set; }
        public Boolean isPrivate { get; set; }
    }
}
