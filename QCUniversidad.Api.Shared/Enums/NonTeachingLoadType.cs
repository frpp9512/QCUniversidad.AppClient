using QCUniversidad.Api.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum NonTeachingLoadType
{
    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Consultas", Description = "Tiempo necesario para impartir consultas a los estudiantes de pregrado de las asignaturas que imparte.", AutoGenerateField = true)]
    Consultation,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Calificación de exámenes", Description = "Tiempo necesario para calificar los exámenes de las asignaturas que imparte.", AutoGenerateField = true)]
    ExamGrade,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Tribunal y revisión de tesis", Description = "Tiempo necesario para participar en tribunales y revisiones de tesis.")]
    ThesisCourtAndRevision,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Preparación de clases", Description = "Tiempo que necesita el profesor para la preparación de las clases que imparte.", AutoGenerateField = true)]
    ClassPreparation,

    /// <summary>
    /// Should be provided by base value. (Beginner, Average, Experienced)
    /// </summary>
    [Display(Name = "Cursos recibidos y superación", Description = "Tiempo necesario para la autosuperación y participación en cursos en función del nivel de experiencia del profesor.")]
    CoursesReceivedAndImprovement,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Reuniones", Description = "Tiempo necesario para participar en reuniones.", AutoGenerateField = true)]
    Meetings,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Acciones metodológicas", Description = "Tiempo necesario para la realización de acciones metodológicas.", AutoGenerateField = true)]
    MethodologicalActions,

    /// <summary>
    /// Should be provided by base value. JSON => (IntegrativeProjectDiplomants, ThesisDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de pregrado", Description = "Tiempo invertido en realizar tutorías a estudiantes de pregrado.")]
    UndergraduateTutoring,

    /// <summary>
    /// Should be provided by base value. JSON => (DiplomaOrMastersDegreeDiplomants, DoctorateDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de posgrado", Description = "Tiempo invertido en realizar tutorías a estudiantes de posgrado.")]
    GraduateTutoring,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Eventos y publicaciones", Description = "Tiempo necesario para la participación en eventos y redacción de artículos académicos.", AutoGenerateField = true)]
    EventsAndPublications,

    /// <summary>
    /// Should be provided by base value. (Participate in proyects: (yes/no))
    /// </summary>
    [Display(Name = "Participación en proyectos", Description = "Tiempo para invertir en proyectos en caso de que participe en alguno.")]
    ParticipationInProjects,

    /// <summary>
    /// Should be provided by base value. (Level of participation: (Low, Medium, High))
    /// </summary>
    [Display(Name = "Acciones de extensión universitaria", Description = "Tiempo necesario para realizar actividades de extensión universitaria en función del nivel de participación.")]
    UniversityExtensionActions,

    /// <summary>
    /// Should be provided by base value. (Load)
    /// </summary>
    [Display(Name = "Otras actividades", Description = "Tiempo para la realización de otras actividades varias.", AutoGenerateField = true)]
    OtherActivities,

    /// <summary>
    /// Should be provided by base value. (Load)
    /// </summary>
    [Display(Name = "Otras funciones", Description = "Tiempo para realizar otras funciones determinadas.")]
    OtherFunctions
}