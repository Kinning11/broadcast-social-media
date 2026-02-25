using BroadcastSocialMedia.Models;

namespace BroadcastSocialMedia.ViewModels
{
    public class UsersIndexViewModel
    {
        public string? Search { get; set; } //Det vi söker på

        public List<ApplicationUser> Result { get; set; } = new List<ApplicationUser>(); // Resultatet av det vi sökt på
    }
}
