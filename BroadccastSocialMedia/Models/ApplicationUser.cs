using Microsoft.AspNetCore.Identity;

namespace BroadcastSocialMedia.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string? Name { get; set; }

        public  string? ProfileImagePath { get; set; }
        public ICollection<Broadcast> Broadcasts { get; set; }

        public ICollection<ApplicationUser> ListeningTo { get; set; } = new List<ApplicationUser>();

        public ICollection<ApplicationUser> ListeningBy { get; set; } = new List<ApplicationUser>();
    }
}
