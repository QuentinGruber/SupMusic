using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using SupMusic.Models;

namespace SupMusic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<Song> Song { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>(n =>
            {
                n.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Playlist>(n =>
            {
                n.HasKey(p => p.ID);

                n.Property(f => f.ID).ValueGeneratedOnAdd();

                n.Property(p => p.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Playlist>()
    .HasData(
        new Playlist
        {
            ID = 1,
            Tags = "party,clubbing,bringue,all night long",
            Name = "Big Fiesta/Party Playlist",
            Songs = "1,4",
            isPrivate = false
        },
        new Playlist
        {
            ID = 2,
            Tags = "relax, very relax",
            Name = "Chill",
            Songs = "5,3,2,6",
            isPrivate = false
        }
    );

            modelBuilder.Entity<Song>(n =>
            {
                n.HasKey(p => p.ID);

                n.Property(f => f.ID).ValueGeneratedOnAdd();

                n.Property(p => p.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Song>()
    .HasData(
        new Song
        {
            ID = 1,
            OwnerID = "-1",
            Tags = "party,clubbing",
            Name = "feteMan",
            Path = "/songs/fete.wav"
        },
        new Song
        {
            ID = 2,
            OwnerID = "-1",
            Tags = "egirl",
            Name = "Doja Cat",
            Path = "/songs/Doja Cat - Say So (Official Video).mp3"
        },
        new Song
        {
            ID = 3,
            OwnerID = "-1",
            Tags = "jazz,clarinet,orchestra",
            Name = "Serpent Maigre",
            Path = "/songs/serpent-maigre.wav"
        },
         new Song
         {
             ID = 4,
             OwnerID = "-1",
             Tags = "Scam,Bold man",
             Name = "Hey Hey Hey - Carlos feat Bitconnect",
             Path = "/songs/bitconnect-remix-warning-scam.mp3"
         },
         new Song
         {
             ID = 5,
             OwnerID = "-1",
             Tags = "cloud",
             Name = "The song of the great Monarch - Sylvain Durif ",
             Path = "/songs/la-chanson-du-grand-monarque-sylvain-durif-cest-moi.mp3"
         },
            new Song
            {
                ID = 6,
                OwnerID = "-1",
                Tags = "Dirty Dancing",
                Name = "I've Had The Time Of My Life",
                Path = "/songs/dirty-dancing-soundtrack-ive-had-the-time-of-my-life.mp3"
            }
    );
        }
    }
}
