using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record EditCourseDto : NewCourseDto
{
    public Guid Id { get; set; }
}
