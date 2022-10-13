using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Subjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QCUniversidad.WebClient.Models.Planning
{
    public class EditTeachingPlanItemModel
    {
        [Required]
        [Display(Name = "Asignatura", Description = "Asignatura planificada", Prompt = "Asignatura planificada")]
        public Guid SubjectId { get; set; }

        [Required]
        [Display(Name = "Actividad", Description = "Actividad planificada", Prompt = "Tipo de activdad")]
        public TeachingActivityType Type { get; set; }

        [Required]
        [Display(Name = "Cantidad de horas", Description = "Horas planificadas", Prompt = "Cantidad de horas planificadas")]
        [Range(0, 2287.2)]
        public double HoursPlanned { get; set; }

        [Display(Name = "Grupos", Description = "Cantidad de grupos involucrados en la actividad", Prompt = "Cantidad de grupos")]
        public uint GroupsAmount { get; set; }

        [Required]
        public Guid PeriodId { get; set; }

        public PeriodModel? Period { get; set; }

        public Guid? SchoolYearId { get; set; }
        public SchoolYearModel? SchoolYear { get; set; }
        public IList<SubjectModel>? Subjects { get; set; }
    }
}
