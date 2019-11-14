namespace WebClinic.Models.Users
{
    public class Patient : AppUser
    {
        public string IllnessHistory { get; set; }
        public string RecommendedDrugs { get; set; }
    }
}
