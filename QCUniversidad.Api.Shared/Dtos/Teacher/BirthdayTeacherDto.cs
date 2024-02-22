namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record BirthdayTeacherDto
{
    public Guid Id { get; set; }
    public string? Fullname { get; set; }
    public DateTime Birthday { get; set; }
    public bool IsBirthdayToday { get; set; }
}