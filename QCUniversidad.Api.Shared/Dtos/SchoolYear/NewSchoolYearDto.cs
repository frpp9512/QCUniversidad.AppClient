namespace QCUniversidad.Api.Shared.Dtos.SchoolYear
{
    public record NewSchoolYearDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Current { get; set; }
    }
}
