using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class SongsController : Controller
    {
        public IActionResult Index()
        {
            return View(Songs.GetSongs());
        }

        public IActionResult SongDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Songs group = Songs.GetsongsByID(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        [HttpGet]
        public IActionResult AddSongs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSongs([Bind] Songs songs)
        {
            if (ModelState.IsValid)
            {
                if (Songs.AddSongs(songs) > 0)
                {
                    return RedirectToAction("Index","Songs");
                }
            }
            return View(songs);
        }

        [HttpGet]
        public IActionResult Editsong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Songs group = Songs.GetsongsByID(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        [HttpPost]
        public IActionResult Editsong(int id, [Bind] Songs songs)
        {
            if (id != songs.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                Songs.UpdateSongList(songs);
                return RedirectToAction("Index","Songs");
            }
            return View(songs);
        }


        public IActionResult DeleteSong(int id)
        {
            Songs group = Songs.GetsongsByID(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }



        [HttpPost]
        public IActionResult DeleteSong(int id, Songs songs)
        {
            if (Songs.DeleteSong(id) > 0)
            {
                return RedirectToAction("Index","Songs");
            }
            return View(songs);
        }
    }
}
