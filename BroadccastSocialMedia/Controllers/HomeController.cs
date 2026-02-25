using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BroadcastSocialMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var user1 = await _dbContext.Users
           .Include(u => u.ListeningTo)
           .FirstOrDefaultAsync(u => u.Id == user.Id);

            var broadcasts = await _dbContext.Broadcasts
         .Include(b => b.User)
         .Include(l => l.Likes)
         .OrderByDescending(b => b.Published)
         .ToListAsync();



            var recommended = await _dbContext.Users
            .Where(u => u.Id != user.Id && !user.ListeningTo
            .Select(l => l.Id)
            .Contains(u.Id))
            .Take(5)
            .ToListAsync();

            var viewModel = new HomePageViewModel()
            {

                Broadcasts = broadcasts,
                RecommendedUsers = recommended
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]

        public async Task<IActionResult> Broadcast(HomeBroadcastViewModel viewModel, IFormFile image)
        {


            var user = await _userManager.GetUserAsync(User); //Hämtar användaren som vi är inloggade på genom DI
            string? imagepath = null;

            if (image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "broadcasts");
                var filePath = Path.Combine(uploadsFolder, image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imagepath = "/images/broadcasts/" + image.FileName;
            }
            var broadcast = new Broadcast() //Skapa en ny broadcast
            {
                Message = viewModel.Message, //Definera props, Id kommer populera automatiskt från databasen, published får redan från DateTime.Now;, User hämtar vi genom DI (_userManager)
                User = user,
                ImagePath = imagepath,



            };

            _dbContext.Broadcasts.Add(broadcast);

            await _dbContext.SaveChangesAsync();

            return Redirect("/");



        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> BroadcastLiked(int BroadcastId)
        {
            var user = await _userManager.GetUserAsync(User);

            var likedPost = await _dbContext.Likes.Where(l => l.UserId == user.Id && l.BroadcastId == BroadcastId).FirstOrDefaultAsync(); // Är liken gjord av den inloggade användaren och gäller den här liken detta inlägget?

            if (likedPost != null)
            {
                _dbContext.Likes.Remove(likedPost);
            }
            else
            {
                _dbContext.Likes.Add(new Like()
                {
                    UserId = user.Id,
                    BroadcastId = BroadcastId
                });
            }

            await _dbContext.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int BroadcastId)
        {
            var userId = _userManager.GetUserId(User);

            var broadcast = await _dbContext.Broadcasts
                .Include(b => b.Likes)
                .FirstOrDefaultAsync(b => b.Id == BroadcastId);

            if (broadcast == null)
                return Json(new { success = false });

            var existingLike = broadcast.Likes
                .FirstOrDefault(l => l.UserId == userId);

            if (existingLike != null)
            {
                _dbContext.Likes.Remove(existingLike);
            }
            else
            {
                _dbContext.Likes.Add(new Like
                {
                    BroadcastId = BroadcastId,
                    UserId = userId
                });
            }

            await _dbContext.SaveChangesAsync();

            return Json(new { success = true });
        }

    }
}
