﻿using Microsoft.AspNetCore.Mvc;
using SupMusic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SupMusic.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace SupMusic.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterNewSong(IFormFile file, Song song)
        {
            try
            {
                song.Path = "/songs/" + song.Name + ".wav";
                using (var fileStream = new FileStream("./wwwroot" + song.Path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }

                song.OwnerID = _userManager.GetUserId(HttpContext.User);
                _db.Song.Add(song);
                _db.SaveChanges();
                ViewBag.song = song;
                ViewBag.resultMessage = "success";
            }
            catch (System.Exception error)
            {
                ViewBag.resultMessage = error.ToString();
            }

            return View();
        }


        [HttpPost]
        public IActionResult RegisterNewPlaylist(Playlist playlist)
        {
            try
            {
                playlist.OwnerID = _userManager.GetUserId(HttpContext.User);
                playlist.Songs = "";
                _db.Playlist.Add(playlist);
                _db.SaveChanges();
                ViewBag.playlist = playlist;
                ViewBag.resultMessage = "success";
            }
            catch (System.Exception error)
            {
                ViewBag.resultMessage = error.ToString();
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditPlaylist(Playlist modifiedPlaylist)
        {
            if (modifiedPlaylist.Songs == null) // to fix an issue where a "" can be interpreted as null
            {
                modifiedPlaylist.Songs = "";
            }

            _db.Playlist.Update(modifiedPlaylist);
            _db.SaveChanges();
            return RedirectToAction(nameof(playlists));
        }

        [HttpGet]
        public IActionResult EditPlaylist(int? PlaylistId)
        {
            var choosenPlaylist = _db.Playlist.Find(PlaylistId);
            if (choosenPlaylist == null) return NotFound();
            return View(choosenPlaylist);
        }

        [HttpGet]
        public IActionResult DeletePlaylist(int? PlaylistId)
        {
            var choosenPlaylist = _db.Playlist.Find(PlaylistId);
            if (choosenPlaylist == null) return NotFound();
            return View(choosenPlaylist);
        }

        [HttpPost]
        public IActionResult DeletePlaylistConfirmed(Playlist PlaylistToDelete)
        {
            _db.Playlist.Attach(PlaylistToDelete);
            _db.Playlist.Remove(PlaylistToDelete);
            _db.SaveChanges();
            return RedirectToAction(nameof(playlists));
        }

        [HttpGet]
        public IActionResult DeleteSong(int? SongId)
        {
            var choosenSong = _db.Song.Find(SongId);
            if (choosenSong == null) return NotFound();
            return View(choosenSong);
        }

        [HttpPost]
        public IActionResult DeleteSongConfirmed(Song SongToDelete)
        {
            System.IO.File.Delete("./wwwroot" + SongToDelete.Path);
            _db.Song.Attach(SongToDelete);
            _db.Song.Remove(SongToDelete);
            _db.SaveChanges();
            return RedirectToAction(nameof(Discover));
        }

        [HttpPost]
        public IActionResult AddSongToPlaylist(ModifyPlaylistModel request)
        {
            Playlist playlistTargeted = _db.Playlist.Find(request.PlaylistId);
            var userID = _userManager.GetUserId(HttpContext.User);
            if (playlistTargeted.OwnerID != userID)
            {
                // for security reason
                return RedirectToAction(nameof(Discover));
            }

            playlistTargeted.Songs += request.SongId.ToString() + ",";
            _db.Playlist.Update(playlistTargeted);
            _db.SaveChanges();
            return RedirectToAction(nameof(Discover));
        }

        public IActionResult RemoveSongToPlaylist(ModifyPlaylistModel request)
        {
            Playlist playlistTargeted = _db.Playlist.Find(request.PlaylistId);
            var userID = _userManager.GetUserId(HttpContext.User);
            if (playlistTargeted.OwnerID != userID)
            {
                // for security reason
                return RedirectToAction(nameof(Discover));
            }

            var listOfSongs = playlistTargeted.Songs.Split(',').ToList();
            listOfSongs.Remove(request.SongId);
            String StringOfSongs = "";
            foreach (var song in listOfSongs)
            {
                StringOfSongs += song + ",";
            }

            playlistTargeted.Songs = StringOfSongs;
            _db.Playlist.Update(playlistTargeted);
            _db.SaveChanges();
            if (playlistTargeted.Songs.Replace(",", "").Length != 0)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return RedirectToAction(nameof(playlists));
            }
        }

        public IActionResult Discover()
        {
            var userID = _userManager.GetUserId(HttpContext.User);
            ViewBag.userID = userID;
            List<Playlist> pubPlaylists = new List<Playlist>();
            List<Playlist> userPlaylists = new List<Playlist>();
            foreach (var playlist in _db.Playlist.ToList())
            {
                var songList = _db.Song.Where(song => playlist.Songs.Contains(song.ID.ToString()));
                if (playlist.isPrivate == false && songList.ToList().Count() > 0)
                {
                    pubPlaylists.Add(playlist);
                }

                if (playlist.OwnerID == userID)
                {
                    userPlaylists.Add(playlist);
                }
            }

            ViewBag.pubPlaylists = pubPlaylists;
            ViewBag.userPlaylists = userPlaylists;
            ViewBag.songs = _db.Song.ToList();
            return View();
        }

        public IActionResult PlaylistPlayer(int? PlaylistId, int? index)
        {
            var playlist = _db.Playlist.Find(PlaylistId);
            var songList = _db.Song.Where(song => playlist.Songs.Contains(song.ID.ToString()));
            var userID = _userManager.GetUserId(HttpContext.User);
            if (index >= songList.Count())
            {
                // if index is out of range
                index = 0;
            }

            ViewBag.playlist = playlist;
            ViewBag.index = index;
            ViewBag.songList = songList.ToList();
            ViewBag.userID = userID;
            if (ViewBag.songList.Count < 1)
            {
                // this issue can happen if all the songs from a playlist are deleted
                // reset the list of songs so the user can't try again to listen this playlist
                playlist.Songs = "";
                _db.Playlist.Update(playlist);
                _db.SaveChanges();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult switchTheme()
        {
            if (Global.isInDarkMode == true)
            {
                Global.isInDarkMode = false;
            }
            else
            {
                Global.isInDarkMode = true;
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult CreatePlaylist()
        {
            ViewBag.playlists = _db.Playlist.ToList();
            return View();
        }

        public IActionResult playlists()
        {
            var userID = _userManager.GetUserId(HttpContext.User);
            ViewBag.playlists = _db.Playlist.Where(playlist => playlist.OwnerID == userID).ToList();
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}