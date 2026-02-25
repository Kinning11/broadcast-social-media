using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class TrendingController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;


        public TrendingController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var dbUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();



            var broadcasts = await _dbContext.Broadcasts
                .Where(b => b.Published >= DateTime.Now.AddDays(-7))

         .Include(b => b.User)
         .Include(b => b.Likes)
         .OrderByDescending(b => b.Likes.Count())
         .Take(10)
         .ToListAsync();



            var viewModel = new HomeIndexViewModel()
            {
                Broadcasts = broadcasts
            };




            return View(viewModel);
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

            return RedirectToAction("Index", "Trending");
        }
    }
}
