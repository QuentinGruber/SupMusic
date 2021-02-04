using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupMusic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
            var user = _userManager.GetUserId(HttpContext.User);
            Console.WriteLine(user);
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
                    file.CopyTo(fileStream); //copy the file that was sent to this path
                }
                song.OwnerID = _userManager.GetUserId(HttpContext.User);
                _db.Song.Add(song);
                _db.SaveChanges();
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
                _db.Playlist.Add(playlist);
                _db.SaveChanges();
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
            _db.Update(modifiedPlaylist);
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
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Discover()
        {

            ViewBag.songs = _db.Song.ToList();
            return View();
        }

        public IActionResult PlaylistPlayer()
        {

            var songList = _db.Song.ToList();
            ViewBag.song = songList[0];
            return View();
        }


        public IActionResult CreatePlaylist()
        {
            ViewBag.playlists = _db.Playlist.ToList();
            return View();
        }

        public IActionResult playlists()
        {
            ViewBag.playlists = _db.Playlist.ToList();
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
