using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Models.LoadItem;

public class TeacherLoadViewModel
{
    public required TeacherModel Teacher { get; set; }
    public required IList<LoadViewItemModel> LoadItems { get; set; }
}
