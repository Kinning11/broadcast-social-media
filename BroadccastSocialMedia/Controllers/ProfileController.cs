using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BroadcastSocialMedia.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
             _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);


            var viewModel = new ProfileIndexViewModel()
            {
                Name = user.Name ?? "" ,
                ProfileImagePath = user.ProfileImagePath
           };



            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileIndexViewModel viewModel, IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User); // Hämta användaren
            user.Name = viewModel.Name; //Spara till användaren "user"

            if (image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                var filePath = Path.Combine(uploadsFolder, image.FileName);



                using (var stream = new FileStream(filePath, FileMode.Create)) //Skapar en tom fil
                {
                    await image.CopyToAsync(stream); // Kopierar datan till den tomma filen
                }

                user.ProfileImagePath = "/images/profiles/" + image.FileName; // Uppdaterar prop i AppUser


            }

            await _userManager.UpdateAsync(user);


            return RedirectToAction("Index");

        }


    }
}
