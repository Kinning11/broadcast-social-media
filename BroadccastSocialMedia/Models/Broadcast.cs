namespace BroadcastSocialMedia.Models
{
    public class Broadcast
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string? ImagePath { get; set; }

        public DateTime Published { get; set; } = DateTime.Now;

        // Foreign key
        public string UserId { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }

}
