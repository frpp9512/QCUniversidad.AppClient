using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Models
{
    public class FacultyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Campus { get; set; }
        public int DepartmentCount { get; set; } = 0;
        public int CareersCount { get; set; } = 0;
    }
}
