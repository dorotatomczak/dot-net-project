namespace WebClinic.Models
{
    public class Patient : Person
    {
        string illnessHistory { get; set; }
        string recommendedDrugs { get; set; }
    }
}
