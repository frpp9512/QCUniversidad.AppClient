using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Teachers;
public record BirthdayTeacherModel
{
    public Guid Id { get; set; }
    public string Fullname { get; set; }
    public DateTime Birthday { get; set; }
    public bool IsBirthdayToday { get; set; }
}
