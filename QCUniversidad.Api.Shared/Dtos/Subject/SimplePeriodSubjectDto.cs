using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record SimplePeriodSubjectDto : EditPeriodSubjectDto
{
    public SubjectDto Subject { get; set; }
}
