using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.SchoolYear;

public record NewSchoolYearDto
{
    public int CareerYear { get; set; }
    public string Denomination { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public TeachingModality TeachingModality { get; set; }
    public Guid CareerId { get; set; }
    public Guid CurriculumId { get; set; }
}