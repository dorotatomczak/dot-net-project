namespace WebClinic.Models
{
    public class Patient : Person
    {
        public string IllnessHistory { get; set; }
        public string RecommendedDrugs { get; set; }
    }
}
