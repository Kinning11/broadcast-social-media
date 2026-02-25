using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class RecommendController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public RecommendController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.GetUserAsync(User);

            var user = await _dbContext.Users
            .Include(u => u.ListeningTo)
            .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

            var recommendedUsers = await _dbContext.Users
            .Where(u => u.Id != user.Id && !user.ListeningTo
            .Select(l => l.Id)
            .Contains(u.Id))
            .Take(5)
            .ToListAsync();





            return View(recommendedUsers);
        }


    }
}
