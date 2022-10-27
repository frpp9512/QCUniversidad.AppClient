using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Subject;
public record EditPeriodSubjectDto : NewPeriodSubjectDto
{
    public Guid Id { get; set; }
}
