using BroadcastSocialMedia.Models;

namespace BroadcastSocialMedia.ViewModels
{
    public class HomePageViewModel
    {
        public List<Broadcast> Broadcasts { get; set; } = new();
        public List<ApplicationUser> RecommendedUsers { get; set; } = new();
    }
}
