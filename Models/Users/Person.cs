namespace WebClinic.Models
{

    public enum Sex
    {
        Male,
        Female,
        ItsComplicated
    }

    public class Person
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
    }
}
