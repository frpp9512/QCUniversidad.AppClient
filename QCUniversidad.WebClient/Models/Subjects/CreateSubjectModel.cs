using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.Models.Subjects;

public record CreateSubjectModel : SubjectModel
{
    public IList<DisciplineModel>? Disciplines { get; set; }

    public string? ReturnTo { get; set; }
}
