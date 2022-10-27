using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum NonTeachingLoadType
{
    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Consultas")]
    Consultation,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Calificación de exámenes")]
    ExamGrade,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Tribunal y revisión de tesis")]
    ThesisCourtAndRevision,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Preparación de clases")]
    ClassPreparation,

    /// <summary>
    /// Should be provided by base value. (Beginner, Average, Experienced)
    /// </summary>
    [Display(Name = "Cursos recividos y superación")]
    CoursesReceivedAndImprovement,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Reuniones")]
    Meetings,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Acciones metodológicas")]
    MethodologicalActions,

    /// <summary>
    /// Should be provided by base value. JSON => (IntegrativeProjectDiplomants, ThesisDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de pregrado")]
    UndergraduateTutoring,

    /// <summary>
    /// Should be provided by base value. JSON => (DiplomaOrMastersDegreeDiplomants, DoctorateDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de posgrado")]
    GraduateTutoring,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Eventos y publicaciones")]
    EventsAndPublications,

    /// <summary>
    /// Should be provided by base value. (Participate in proyects: (yes/no))
    /// </summary>
    [Display(Name = "Participación en proyectos")]
    ParticipationInProjects,

    /// <summary>
    /// Should be provided by base value. (Level of participation: (Low, Medium, High))
    /// </summary>
    [Display(Name = "Acciones de extensión universitaria")]
    UniversityExtensionActions,

    /// <summary>
    /// Should be provided by base value. (Load)
    /// </summary>
    [Display(Name = "Otras actividades")]
    OtherActivities,
}