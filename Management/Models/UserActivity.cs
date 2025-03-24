namespace Management.Models
{
    public class UserActivity
    {
        public string? CreatedById { get; set; }
        public DateTime CreatedON { get; set; }

        public string? ModifiedById { get; set; }
        public DateTime ModifiedON { get; set; }
    }
}
