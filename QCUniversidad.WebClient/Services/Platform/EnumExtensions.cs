using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.WebClient.Services.Platform;

public static class EnumExtensions
{
    public static string GetLabel(this TeachingModality modality) => modality switch
    {
        TeachingModality.Classroom => "Diurno",
        TeachingModality.ByMeeting => "Por encuentro",
        TeachingModality.DistanceLearning => "A distancia",
        TeachingModality.PostgraduateCourse => "Posgrado",
        _ => "No reconocido"
    };
}