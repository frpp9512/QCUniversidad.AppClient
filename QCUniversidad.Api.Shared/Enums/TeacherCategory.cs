using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

/// <summary>
/// The category of the teacher.
/// </summary>
public enum TeacherCategory
{
    // Asistente
    [Display(Name = "Asistente", Description = "Asistente", Prompt = "Asistente")]
    Assistant,

    // Asistente a tiempo parcial
    [Display(Name = "Asistente a tiempo parcial", Description = "Asistente a tiempo parcial", Prompt = "Asistente a tiempo parcial")]
    AssistantPT,

    // Asistente técnico de la docencia
    [Display(Name = "Asistente técnico de la docencia", Description = "Asistente técnico de la docencia", Prompt = "Asistente técnico de la docencia")]
    TeachingAssistant,

    // Asistente técnico de la docencia a tiempo parcial
    [Display(Name = "Asistente técnico de la docencia a tiempo parcial", Description = "Asistente técnico de la docencia a tiempo parcial", Prompt = "Asistente técnico de la docencia a tiempo parcial")]
    TeachingAssistantPT,

    // Instructor
    [Display(Name = "Instructor", Description = "Instructor", Prompt = "Instructor")]
    Instructor,

    // Instructor a tiempo parcial
    [Display(Name = "Instructor a tiempo parcial", Description = "Instructor a tiempo parcial", Prompt = "Instructor a tiempo parcial")]
    InstructorPT,

    // Profesor auxiliar
    [Display(Name = "Profesor auxiliar", Description = "Profesor auxiliar", Prompt = "Profesor auxiliar")]
    AssistantProfessor,

    // Profesor auxiliar a tiempo parcial
    [Display(Name = "Profesor auxiliar a tiempo parcial", Description = "Profesor auxiliar a tiempo parcial", Prompt = "Profesor auxiliar a tiempo parcial")]
    AssistantProfessorPT,

    // Profesor consultante auxiliar
    [Display(Name = "Profesor consultante auxiliar", Description = "Profesor consultante auxiliar", Prompt = "Profesor consultante auxiliar")]
    AssistantConsultantProfessor,

    // Profesor consultante titular
    [Display(Name = "Profesor consultante titular", Description = "Profesor consultante titular", Prompt = "Profesor consultante titular")]
    FullConsultingProfessor,

    // Profesor emérito titular
    [Display(Name = "Profesor emérito titular", Description = "Profesor emérito titular", Prompt = "Profesor emérito titular")]
    FullProfessorEmeritus,

    // Profesor titular
    [Display(Name = "Profesor titular", Description = "Profesor titular", Prompt = "Profesor titular")]
    Professor,

    // Profesor titular a tiempo parcial
    [Display(Name = "Profesor titular a tiempo parcial", Description = "Profesor titular a tiempo parcial", Prompt = "Profesor titular a tiempo parcial")]
    ProfessorPT
}