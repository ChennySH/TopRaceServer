using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class User
    {
        public User()
        {
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
