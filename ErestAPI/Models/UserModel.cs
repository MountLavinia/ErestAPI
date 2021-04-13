using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErestAPI.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmaillAddress { get; set; }
        public string RoleName { get; set; }
    }
}
