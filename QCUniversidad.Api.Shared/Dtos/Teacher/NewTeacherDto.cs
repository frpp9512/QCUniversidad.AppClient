using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Teacher
{
    public record NewTeacherDto
    {
        public string Fullname { get; set; }
        public string? PersonalId { get; set; }
        public string? Position { get; set; }
        public TeacherCategory Category { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid[]? SelectedDisciplines { get; set; }
    }
}
