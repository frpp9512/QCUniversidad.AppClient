namespace QCUniversidad.WebClient.Models.Teachers;
public record BirthdayTeacherModel
{
    public Guid Id { get; set; }
    public string? Fullname { get; set; }
    public DateTime Birthday { get; set; }
    public int Age
    {
        get
        {
            var diff = DateTime.Now - Birthday;
            var years = diff.TotalDays / 365;
            return (int)years;
        }
    }
    public bool IsBirthdayToday { get; set; }
}
