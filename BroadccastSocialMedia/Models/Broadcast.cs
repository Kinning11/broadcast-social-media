namespace BroadcastSocialMedia.Models
{
    public class Broadcast
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string? ImagePath { get; set; }

        public DateTime Published { get; set; } = DateTime.Now;

        
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Like> Likes { get; set; }
    }

}
