using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadItem;

public class TeacherLoadViewModel
{
    public TeacherModel Teacher { get; set; }
    public IList<LoadViewItemModel> LoadItems { get; set; }
}
