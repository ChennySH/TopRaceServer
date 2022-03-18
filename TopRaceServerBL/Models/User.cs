using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class User
    {
        public User()
        {
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int WinsNumber { get; set; }
        public int LosesNumber { get; set; }
        public int WinsStreak { get; set; }
        public string ProfilePic { get; set; }

        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
