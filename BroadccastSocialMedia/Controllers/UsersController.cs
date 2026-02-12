using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BroadcastSocialMedia.Controllers
{
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager )
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(UsersIndexViewModel viewModel)
        {
            if (viewModel.Search != null) 
            {
                var users = await _dbContext.Users.Where(u => u.Name.Contains(viewModel.Search))
                .ToListAsync();

                viewModel.Result= users;
            }
            
            return View(viewModel);
        }

        [Route("/Users/{id}")]
        public async Task<IActionResult> ShowUser(string id)
        {
            var broadcasts = await _dbContext.Broadcasts.Where(b => b.User.Id == id).OrderByDescending(b => b.Published).ToListAsync();
            var user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            var viewModel = new UsersShowUserViewModel()
            {
                Broadcasts = broadcasts,
                User = user
            };

            return View(viewModel);
        }

        [HttpPost, Route("/Users/Listen")]

        public async Task<IActionResult> ListenToUser(UsersListenToUserViewModel viewModel)
        {
            var loggedInUser = await _userManager.GetUserAsync(User); // Inloggade användaren
            var userToListenTo = await _dbContext.Users.Where(u => u.Id == viewModel.UserId).FirstOrDefaultAsync(); // Användaren som vi är inne på

            loggedInUser.ListeningTo.Add(userToListenTo); // Lägg till i listan

            await  _userManager.UpdateAsync(loggedInUser);
            await _dbContext.SaveChangesAsync();


            return RedirectToAction("Index", "Home");

        }
    }
}
