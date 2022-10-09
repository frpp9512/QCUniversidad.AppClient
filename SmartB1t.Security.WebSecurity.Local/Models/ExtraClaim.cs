using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartB1t.Security.WebSecurity.Local.Models
{
    public class ExtraClaim
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}