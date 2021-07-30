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
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MvcMovie.Controllers
{
    
    public class MoviesController : Controller
    {
        

        private readonly MvcMovieContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IEmailService _emailService;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;

        public MoviesController(IConfiguration configuration, MvcMovieContext context , IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        //public async Task<IActionResult> Index()
        //{
        //    var movie  = await _context.Movie.ToListAsync();
        //    return View(movie);
        //}
        // GET: Movies
        // GET: Movies
        /*
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
        */

        ///Api Usable code for Index and getting all the details.

        public async Task<ActionResult> Index(string movieGenre, string searchString)
        {
            IEnumerable<Movie> movies = null;
            var movieGenreVM = new MovieGenreViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var result = await client.GetAsync("/api/Movies");

                if (result.IsSuccessStatusCode)
                {

                    var results = result.Content.ReadAsStringAsync().Result;
                    movies = JsonConvert.DeserializeObject<List<Movie>>(results);
                    

                }

                IQueryable<string> genreQuery = from m in movies.AsQueryable()
                                                orderby m.Genre
                                                select m.Genre;

                movies = from m in movies
                         select m;
                movies = movies.Where(s => s.IsPrivate != true);

                if (!string.IsNullOrEmpty(searchString))
                {
                    movies =  movies.Where(s => s.Title.Contains(searchString)).ToList();
                }

                if (!string.IsNullOrEmpty(movieGenre))
                {
                    movies = movies.Where(x => x.Genre == movieGenre).ToList();
                }
                movieGenreVM = new MovieGenreViewModel
                {
                    Genres = new SelectList( genreQuery.Distinct().ToList()),
                    Movies =   movies.ToList()
                };
            }
            return View(movieGenreVM);
        }


        //Api method to get movies by Id using API call
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Movie movies = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var results = await client.GetAsync($"api/Movies/{id}");
                if (results.IsSuccessStatusCode)
                {
                    var result = results.Content.ReadAsStringAsync().Result;
                    movies = JsonConvert.DeserializeObject<Movie>(result);
                }
            }
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        // GET: Movies/Details/5
        /*
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
        */
        // GET: Movies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //API CALL FOR Create a Movie

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(MovieViewModel movieViewModel)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress=  new Uri(apiBaseUrl);

                    string Uniquiefilename = Uploadfile(movieViewModel);
                    Movie movie = new Movie
                    {
                        Title = movieViewModel.Title,
                        ReleaseDate = movieViewModel.ReleaseDate,
                        Genre = movieViewModel.Genre,
                        Price = movieViewModel.Price,
                        Rating = movieViewModel.Rating,
                        CoverPhoto = "/" + Uniquiefilename,
                        IsPrivate = movieViewModel.IsPrivate


                    };
                    var response = await client.PostAsJsonAsync("api/Movies", movie);
                    if (response.IsSuccessStatusCode)
                    {
                        UserEailOptions options = new UserEailOptions
                        {
                            ToEmails = new List<string>() { "test@gmail.com" }

                        };

                        await _emailService.SendTestemail(options);
                        return RedirectToAction(nameof(Index));

                    }
                }
                

               // _context.Add(movie);
                await _context.SaveChangesAsync();

            }

           

            return View(movieViewModel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("ID,Title,ReleaseDate,Genre,Price,Rating,coverPhoto")]
        /*
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
        */

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
        /*
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
        */

        //Get method for edit calling from api 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movies = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var results = await client.GetAsync($"api/Movies/{id}");
                if (results.IsSuccessStatusCode)
                {
                    var result = results.Content.ReadAsStringAsync().Result;
                    movies = JsonConvert.DeserializeObject<Movie>(result);
                }
            }

            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }


        // POST: Movies/Edit/5---[Bind("ID,Title,ReleaseDate,Genre,Price,Rating,CoverPhoto")] 
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
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
        */
        //Api Method callig for post the edit\

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
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    var response = await client.PutAsJsonAsync($"api/movies/{id}", movie);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        /*
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
        */
        //Api method to call for calling get view of delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Movie movies = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var results = await client.GetAsync($"api/Movies/{id}");
                if (results.IsSuccessStatusCode)
                {
                    var result = results.Content.ReadAsStringAsync().Result;
                    movies = JsonConvert.DeserializeObject<Movie>(result);
                }
            }


            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        /*
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
        */

        // POST: Movies/Delete/5 using API CALL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(apiBaseUrl);
                var response = await Client.DeleteAsync($"api/Movies/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
            
        }
        

            

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }

        //api calll for private movies
        [Authorize]
        public async Task<IActionResult> PrivateMovies()
        {
            IEnumerable<Movie> movies = null;
            var movieGenreVM = new MovieGenreViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var result = await client.GetAsync("/api/Movies");

                if (result.IsSuccessStatusCode)
                {

                    var results = result.Content.ReadAsStringAsync().Result;
                    movies = JsonConvert.DeserializeObject<List<Movie>>(results);


                }
                movies = from m in _context.Movie
                         select m;
                movies = movies.Where(s => s.IsPrivate == true);
                movieGenreVM = new MovieGenreViewModel
                {
                    Movies = movies.ToList()
                };
            }
            return View(movieGenreVM);
        }

        /*
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
         */
    }
}
