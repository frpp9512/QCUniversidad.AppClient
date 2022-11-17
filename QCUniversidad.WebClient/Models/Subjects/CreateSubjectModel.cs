using QCUniversidad.WebClient.Models.Disciplines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Subjects;

public record CreateSubjectModel : SubjectModel
{
    public IList<DisciplineModel>? Disciplines { get; set; }

    public string? ReturnTo { get; set; }
}
