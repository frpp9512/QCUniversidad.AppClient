using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.WebClient.Services.Platform;

public static class EnumExtensions
{
    public static string GetLabel(this TeachingModality modality)
    {
        return modality switch
        {
            TeachingModality.Classroom => "Diurno",
            TeachingModality.ByMeeting => "Por encuentro",
            TeachingModality.DistanceLearning => "A distancia",
            //TeachingModality.PostgraduateDegree => "Postgrado",
            //TeachingModality.MastersDegree => "Maestría",
            //TeachingModality.PhDDegree => "Doctorado",
            _ => "No reconocido"
        };
    }
}