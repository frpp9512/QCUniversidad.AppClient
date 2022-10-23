using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Represents a person that teach a set of subjects from a determined discipline.
/// </summary>
public record TeacherModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The full name of the teacher.
    /// </summary>
    public string? Fullname { get; set; }

    /// <summary>
    /// The identity card number.
    /// </summary>
    [MaxLength(11), MinLength(11)]
    public string? PersonalId { get; set; }

    /// <summary>
    /// The position that occupies in the university.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The category of the teacher.
    /// </summary>
    public TeacherCategory Category { get; set; }

    /// <summary>
    /// The type of contract of the teacher.
    /// </summary>
    public TeacherContractType ContractType { get; set; }

    /// <summary>
    /// The email address of the teacher.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The id department which the teacher belongs to.
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// The department which the teacher belongs to.
    /// </summary>
    public DepartmentModel? Department { get; set; }

    /// <summary>
    /// The disciplines whose the teacher can teach.
    /// </summary>
    public IList<TeacherDiscipline>? TeacherDisciplines { get; set; }

    /// <summary>
    /// The load items assigned to the teacher.
    /// </summary>
    public IList<LoadItemModel>? LoadItems { get; set; }

    /// <summary>
    /// Defines if the teacher is active for generate load distribution.
    /// </summary>
    public bool Active { get; set; } = true;
}