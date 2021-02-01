using System;

namespace SupMusic.Models
{
    public class Song
    {
        public int ID { get; set; }
        public string Tags { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Path { get; set; }
    }
}
