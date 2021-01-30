using System;

namespace SupMusic.Models
{
    public class SongModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Path { get; set; }
    }
}
