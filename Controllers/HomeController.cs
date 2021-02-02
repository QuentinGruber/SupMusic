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

namespace SupMusic.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
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
            Console.WriteLine(file);
            try
            {
                song.Path = "/songs/" + song.Name + ".wav";
                using (var fileStream = new FileStream("./wwwroot" + song.Path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream); //copy the file that was sent to this path
                }
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
        public IActionResult RegisterNewPlaylist(IFormFile file, Playlist playlist)
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Discover()
        {

            ViewBag.songs = _db.Song.ToList();
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
