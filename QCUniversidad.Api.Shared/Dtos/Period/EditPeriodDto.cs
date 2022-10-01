using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Period;

public record EditPeriodDto : NewPeriodDto
{
    public Guid Id { get; set; }
}
