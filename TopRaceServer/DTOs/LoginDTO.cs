using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopRaceServer.DTO
{
    public class LoginDTO
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
