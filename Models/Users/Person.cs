namespace WebClinic.Models
{
    enum Gender
    {
        AsOriginSex,
        AsCurrentSex,
        NotRelatedToSex
    }

    enum Sex
    {
        Male,
        Female,
        ItsComplicated
    }

    public class Person
    {
        string fullname { get; set; }
        int age { get; set; }
        Sex sex { get; set; }
        Gender gender { get; set; }
    }
}
