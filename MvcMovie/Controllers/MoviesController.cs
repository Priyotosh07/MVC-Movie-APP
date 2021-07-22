using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Controllers;
using MvcMovie.ViewModels;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    
    public class MoviesController : Controller
    {
        

        private readonly MvcMovieContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IEmailService _emailService;
        

        public MoviesController(MvcMovieContext context , IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
           
        }

        //public async Task<IActionResult> Index()
        //{
        //    var movie  = await _context.Movie.ToListAsync();
        //    return View(movie);
        //}
        // GET: Movies
        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;
            movies = movies.Where(s => s.IsPrivate != true);

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        /*
        public async Task<IActionResult> Index(string searchString)
        {
            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            return View(movies);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }
        */

        // GET: Movies/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("ID,Title,ReleaseDate,Genre,Price,Rating,coverPhoto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(  MovieViewModel movieViewModel)
        {

            if (ModelState.IsValid)
            {
                string Uniquiefilename = Uploadfile(movieViewModel);
                Movie movie = new Movie
                {
                    Title=movieViewModel.Title,
                    ReleaseDate=movieViewModel.ReleaseDate,
                    Genre=movieViewModel.Genre,
                    Price=movieViewModel.Price,
                    Rating=movieViewModel.Rating,
                    CoverPhoto="/"+ Uniquiefilename,
                    IsPrivate=movieViewModel.IsPrivate


                };
                
                _context.Add(movie);
                 await _context.SaveChangesAsync();
                
            }
            UserEailOptions options = new UserEailOptions
            {
                ToEmails = new List<string>() { "test@gmail.com" }

            };

            await _emailService.SendTestemail(options);
            return RedirectToAction(nameof(Index));

            //return View(movieViewModel);
        }

        private string Uploadfile(MovieViewModel movieViewModel)
        {
            string Uniquefilename = null;

            if (movieViewModel.coverPhoto != null)
            {
                string Folder = "MovieCoverPic/Cover/";
                Uniquefilename = Guid.NewGuid().ToString() + '_' + movieViewModel.coverPhoto.FileName;
                

                string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Uniquefilename);
                using (var fileStream = new FileStream(ServerFolder, FileMode.Create,FileAccess.ReadWrite))
                {
                     movieViewModel.coverPhoto.CopyTo(fileStream);
                }
                


            }
            return Uniquefilename;

        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5---[Bind("ID,Title,ReleaseDate,Genre,Price,Rating,CoverPhoto")] 
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }

        [Authorize]
        public async Task<IActionResult> PrivateMovies()
        {
            var movies = from m in _context.Movie
                         select m;
            movies = movies.Where(s => s.IsPrivate == true);
            var movieGenreVM = new MovieGenreViewModel
            {
                Movies = await movies.ToListAsync()
            };
            return View(movieGenreVM);
        }
    }
}
