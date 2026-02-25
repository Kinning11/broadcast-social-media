using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search)
        {
            var viewModel = new UsersIndexViewModel
            {
                Search = search
            };

            if (!string.IsNullOrEmpty(search))
            {
                viewModel.Result = await _dbContext.Users
                    .Where(u => u.Name.Contains(search))
                    .ToListAsync();
            }

            return View(viewModel);
        }
        [Route("/Users/{id}")]
        public async Task<IActionResult> ShowUser(string id)
        {
            var loggedInUserId = _userManager.GetUserId(User);

            bool isFollowing = false;

            if (loggedInUserId != null)
            {
                isFollowing = await _dbContext.Users
                    .Where(u => u.Id == loggedInUserId)
                    .SelectMany(u => u.ListeningTo)
                    .AnyAsync(u => u.Id == id);
            }


            var broadcasts = await _dbContext.Broadcasts
                .Where(b => b.User.Id == id)
                .Include(b => b.Likes)
                .OrderByDescending(b => b.Published)
                .ToListAsync();
            var user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            var viewModel = new UsersShowUserViewModel()
            {
                Broadcasts = broadcasts,
                User = user,
                IsFollowing = isFollowing,

            };

            return View(viewModel);
        }

        [HttpPost, Route("/Users/Listen")]
        public async Task<IActionResult> ListenToUser(UsersListenToUserViewModel viewModel)
        {
            var loggedInUser = await _userManager.Users
                .Include(u => u.ListeningTo)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var userToListenTo = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == viewModel.UserId);

            if (!loggedInUser.ListeningTo.Any(u => u.Id == viewModel.UserId))
            {
                loggedInUser.ListeningTo.Add(userToListenTo);
                await _dbContext.SaveChangesAsync();
            }

            return Json(new { success = true });
        }


        [HttpPost]
        [Route("/Users/UnListen")]
        public async Task<IActionResult> UnListenToUser(string userId)
        {
            var loggedInUser = await _userManager.Users
                .Include(u => u.ListeningTo)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var userToRemove = loggedInUser.ListeningTo
                .FirstOrDefault(u => u.Id == userId);

            if (userToRemove != null)
            {
                loggedInUser.ListeningTo.Remove(userToRemove);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("ShowUser", new { id = userId });
        }




    }
}
