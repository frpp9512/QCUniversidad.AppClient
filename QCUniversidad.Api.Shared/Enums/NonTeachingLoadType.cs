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
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    Consultation,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Calificación de exámenes", Description = "Tiempo necesario para calificar los exámenes de las asignaturas que imparte.", AutoGenerateField = true)]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    ExamGrade,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Tribunal y revisión de tesis (Pregrado)", Description = "Tiempo necesario para participar en tribunales y revisiones de tesis.", AutoGenerateField = true)]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    ThesisCourtAndRevision,

    /// <summary>
    /// Should be provided by base value. JSON => (MastersAndDiplomantsThesisCourts, ThesisDiplomantsThesisCourts)
    /// </summary>
    [Display(Name = "Tribunal y revisión de tesis (Posgrado)", Description = "Tiempo necesario para participar en tribunales y revisiones de tesis.")]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    PostgraduateThesisCourtAndRevision,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Recalculate]
    [Display(Name = "Preparación de clases", Description = "Tiempo que necesita el profesor para la preparación de las clases que imparte.", AutoGenerateField = true)]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    ClassPreparation,

    /// <summary>
    /// Should be provided by base value. (Beginner, Average, Experienced)
    /// </summary>
    [Display(Name = "Cursos recibidos y superación", Description = "Tiempo necesario para la autosuperación y participación en cursos en función del nivel de experiencia del profesor.")]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    CoursesReceivedAndImprovement,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Reuniones", Description = "Tiempo necesario para participar en reuniones.", AutoGenerateField = true)]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    Meetings,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Acciones metodológicas", Description = "Tiempo necesario para la realización de acciones metodológicas.", AutoGenerateField = true)]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    MethodologicalActions,

    /// <summary>
    /// Should be provided by base value. JSON => (IntegrativeProjectDiplomants, ThesisDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de pregrado", Description = "Tiempo invertido en realizar tutorías a estudiantes de pregrado, tanto de Proyectos Integradores y Tesis de grado.")]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    UndergraduateTutoring,

    /// <summary>
    /// Should be provided by base value. JSON => (DiplomaOrMastersDegreeDiplomants, DoctorateDiplomants)
    /// </summary>
    [Display(Name = "Tutoría de posgrado", Description = "Tiempo invertido en realizar tutorías a estudiantes de posgrado, tanto de dipomados, maestrías y doctorados.")]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    GraduateTutoring,

    /// <summary>
    /// Should be auto generated.
    /// </summary>
    [Display(Name = "Eventos y publicaciones", Description = "Tiempo necesario para la participación en eventos y redacción de artículos académicos.", AutoGenerateField = true)]
    [LoadCategory(Category = "Research", PromptName = "Investigación", Description = "Proceso clave de investigación.")]
    EventsAndPublications,

    /// <summary>
    /// Should be provided by base value. (Participate in proyects: (yes/no))
    /// </summary>
    [Display(Name = "Participación en proyectos", Description = "Tiempo para invertir en proyectos en caso de que participe en alguno.")]
    [LoadCategory(Category = "Research", PromptName = "Investigación", Description = "Proceso clave de investigación.")]
    ParticipationInProjects,

    /// <summary>
    /// Should be provided by base value. (Level of participation: (Low, Medium, High))
    /// </summary>
    [Display(Name = "Acciones de extensión universitaria", Description = "Liderazgo, participación, apoyo, contribución y promoción en la realización de proyectos extensionistas dirigidos al desarrollo sociocultural comunitario y actividades extracurriculares para el desarrollo cultural integral de los estudiantes.")]
    [LoadCategory(Category = "UniversityExtension", PromptName = "Extensión universitaria", Description = "Proceso clave de extensión universitaria.")]
    UniversityExtensionActions,

    /// <summary>
    /// Should be provided by base value. (Load)
    /// </summary>
    [Display(Name = "Otras actividades", Description = "Tiempo para la realización de otras actividades varias.", AutoGenerateField = true)]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    OtherActivities,

    /// <summary>
    /// Should be provided by base value. (Type: HeadTeacherOfTheYear, YearCoordinator, Teacher)
    /// </summary>
    [Display(Name = "Trabajo educativo", Description = "Tiempo necesario para realizar actividades de trabajo educativo con los estudiantes bajo su radio de acción.")]
    [LoadCategory(Category = "Formation", PromptName = "Formación", Description = "Proceso clave de Formación académica.")]
    EducationalWork,

    /// <summary>
    /// Should be provided by base value. (Type: CUMDirector, FUMDirector, HeadOfDepartment, CareerCoordinator, HeadOfDiscipline)
    /// </summary>
    [Display(Name = "Responsabilidades administrativas", Description = "Responsabilidades adquiridas por las funciones y obligaciones administrativas del cargo que ocupa.")]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    AdministrativeResponsibilities,

    /// <summary>
    /// Should be provided by base value. (Type: SyndicalBaseRepresentative, SyndicalGeneralRepresentative, UJCBaseRepresentative, UJCComiteeRepresentative, PCCBaseRepresentative, PCCComiteeRepresentative)
    /// </summary>
    [Display(Name = "Responsabilidades políticas y sindicales", Description = "Responsabilidades sobre funciones en el sindicato, UJC o el PCC.")]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    SyndicalAndPoliticalResposabilities,

    /// <summary>
    /// Should be provided by base value. (Type: AreaProcessRepresentative, InstitutionProcessRepresentative)
    /// </summary>
    [Display(Name = "Responsabilidades de procesos", Description = "Resposabilidades que se adquieren en función de cumplir objetivos del área o del centro.")]
    [LoadCategory(Category = "Others", PromptName = "Otras actividades", Description = "Otras actividades")]
    ProcessResponsibilities
}