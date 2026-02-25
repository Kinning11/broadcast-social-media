namespace BroadcastSocialMedia.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Användaren som gillar inlägget (key)
        public ApplicationUser User { get; set; } // Kopplingen till användaren, Navigation property
        public int BroadcastId { get; set; } // Key till inlägget
        public Broadcast Broadcast { get; set; } // Kopplingen till inlägget, Navigation property
    }
}
