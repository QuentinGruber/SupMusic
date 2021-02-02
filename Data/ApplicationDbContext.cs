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
                // clé primaire
                n.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Playlist>(n =>
            {
                // clé primaire
                n.HasKey(p => p.ID);

                // id auto-increment non null
                n.Property(f => f.ID).ValueGeneratedOnAdd();

                // taille maxi
                n.Property(p => p.Name).HasMaxLength(64);

                // clé étrangère (song id)
                /*   n.HasMany<Song>()
                   .WithOne()
                   .HasForeignKey(fk => fk.ID);
                   */
            });

            modelBuilder.Entity<Playlist>()
    .HasData(
        new Playlist
        {
            ID = 1,
            Tags = "fete, clubbing",
            Name = "Playlist de test public",
            Songs = new int[] { 1, 2 },
            isPrivate = true
        },
        new Playlist
        {
            ID = 2,
            Tags = "fete, clubbing",
            Name = "Playlist de test privé",
            Songs = new int[] { 1 },
            isPrivate = false
        }
    );

            modelBuilder.Entity<Song>(n =>
            {
                // clé primaire
                n.HasKey(p => p.ID);

                // id auto_increment non null
                n.Property(f => f.ID).ValueGeneratedOnAdd();

                // taille maxi
                n.Property(p => p.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Song>()
    .HasData(
        new Song
        {
            ID = 1,
            Tags = "fete, clubbing",
            Name = "feteMan",
            Path = "/songs/fete.wav",
            Duration = 69
        },
        new Song
        {
            ID = 2,
            Tags = "pas, fou, egirl",
            Name = "Doja Cat",
            Path = "/songs/Doja Cat - Say So (Official Video).mp3",
            Duration = 69
        }
    );
        }
    }
}
