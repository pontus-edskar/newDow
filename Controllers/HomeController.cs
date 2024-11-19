using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieRate.Models;

namespace MovieRate.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private static List<RateMovie> movieRatings = new List<RateMovie>
    {
        //Create some rated movie so list is not empty when user starts site
        new RateMovie { Title = "The Matrix", Genre = "Sci-Fi", Rate = 8.6m },
        new RateMovie { Title = "Fight Club", Genre = "Action", Rate = 9.5m },
        new RateMovie { Title = "The Godfather", Genre = "Drama", Rate = 8.3m },
        new RateMovie { Title = "Truman Show", Genre = "Comedy", Rate = 8.4m },
        new RateMovie { Title = "Chernobyl", Genre = "Documentary", Rate = 7.9m },
    };

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RateAMovie()
    {
        //list of genres to choose for the movie
        List<String> genre = new List<string>()
        {
            "Horror",
            "Sci-Fi",
            "Romance",
            "Comedy",
            "Fantasy",
            "Drama",
            "Action",
            "Musical",
            "Animation",
            "Documentary"
        };

        ViewBag.Genre = new SelectList(genre);
        return View();
    }

    [HttpPost]
    public IActionResult SubmitRating(RateMovie model)
    {
        if(ModelState.IsValid)
        {
            movieRatings.Add(model);

            //Store Ratingcount in cookie
            HttpContext.Response.Cookies.Append("RatingsCount", movieRatings.Count.ToString(), new CookieOptions
            {
                //lifetime of 30 mintues after being created
                Expires = DateTimeOffset.Now.AddMinutes(30)
            });

            return RedirectToAction("MovieRates");
        }
        //Om lista är ogilltig returnera användaren
        List<String> genre = new List<string>()
        {
            "Horror",
            "Sci-Fi",
            "Romance",
            "Comedy",
            "Fantasy",
            "Drama",
            "Action",
            "Musical",
            "Animation",
            "Documentary"
        };
        ViewBag.Genre = new SelectList(genre);
        return View("RateAMovie", model);
    }
    
    public IActionResult MovieRates()
    {
        //Get ratings count from cookie
        var ratingsCount = HttpContext.Request.Cookies["RatingsCount"];

        ViewBag.RatingsCount = ratingsCount ?? "0";
        //Sort list by rating
        var sortedMovies = movieRatings.OrderByDescending(m => m.Rate).ToList();
        return View(sortedMovies);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult EditMovie(string title)
    {
        var movie = movieRatings.FirstOrDefault(m => m.Title == title);
        if(movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    [HttpPost]
    public IActionResult SubmitEdit(RateMovie model)
    {
        if(ModelState.IsValid)
        {
            var movie = movieRatings.FirstOrDefault(m => m.Title == model.Title);
            if(movie != null)
            {
                movie.Rate = model.Rate;
            }
            return RedirectToAction("MovieRates");
        }
        return View("EditMovie", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
