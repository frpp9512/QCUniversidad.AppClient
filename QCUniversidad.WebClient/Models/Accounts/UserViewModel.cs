using QCUniversidad.WebClient.Models.Departments;
using SmartB1t.Security.WebSecurity.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Accounts
{
    public class UserViewModel : User
    {
        public DepartmentModel DepartmentModel { get; set; }
    }
}
