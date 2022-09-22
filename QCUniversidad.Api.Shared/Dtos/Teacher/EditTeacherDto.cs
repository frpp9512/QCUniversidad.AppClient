using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Teacher
{
    public record EditTeacherDto : NewTeacherDto
    {
        public Guid Id { get; set; }
    }
}
