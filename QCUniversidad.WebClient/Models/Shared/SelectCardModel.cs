namespace QCUniversidad.WebClient.Models.Shared;

public class SelectCardModel
{
    public string? Id { get; set; } = null;
    public string? GroupName { get; set; } = null;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
    public bool Selected { get; set; }
}
