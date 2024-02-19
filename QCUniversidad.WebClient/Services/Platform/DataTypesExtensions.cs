using QCUniversidad.WebClient.Models.Course;
using System.Security.Claims;

namespace QCUniversidad.WebClient.Services.Platform;

public static class DataTypesExtensions
{
    public static string GetCareerYearLabel(this CourseModel model) => model.CareerYear switch
    {
        1 => "1er año",
        2 => "2do año",
        3 => "3er año",
        4 => "4to año",
        5 => "5to año",
        6 => "6to año",
        7 => "7mo año",
        8 => "8vo año",
        _ => throw new NotImplementedException()
    };

    public static Guid GetFullname(this ClaimsPrincipal user) => new(user.Claims.First(c => c.Type == ClaimTypes.Name).Value);

    public static Guid GetEmail(this ClaimsPrincipal user) => new(user.Claims.First(c => c.Type == ClaimTypes.Email).Value);

    public static Guid GetDepartmentId(this ClaimsPrincipal user) => new(user.Claims.First(c => c.Type == "DepartmentId").Value);

    public static Guid GetFacultyId(this ClaimsPrincipal user) => new(user.Claims.First(c => c.Type == "FacultyId").Value);

    public static bool IsAdmin(this ClaimsPrincipal user) => user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrador");

    public static bool IsPlanner(this ClaimsPrincipal user) => user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Planificador");

    public static bool IsDepartmentManager(this ClaimsPrincipal user) => user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Jefe de departamento");
}