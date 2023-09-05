namespace QCUniversidad.WebClient.Models.Configuration;

public class IdentityServerConfiguration
{
    public required string Address { get; set; }
    public required string ClientId { get; set; }
    public required string Secret { get; set; }
    public required string Scope { get; set; }
}
