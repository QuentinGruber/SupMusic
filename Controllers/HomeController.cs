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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Discover()
        {
            return View();
        }

        public IActionResult playlists()
        {

            /* TODO: clean ça
            var song = new SongModel();
            // song.ID = 69;
            song.Name = "coucou";
            //   song.CategoryID = 69;
            song.Duration = 69;
            _db.Song.Add(song);

            _db.SaveChanges();
            */
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
