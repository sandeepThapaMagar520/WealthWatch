using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WealthWatch.Models
{
    public class Users
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String FullName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

        public String Currency {  get; set; }
    }
}
