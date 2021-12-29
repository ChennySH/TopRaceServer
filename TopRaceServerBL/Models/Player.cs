using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Player
    {
        public Player()
        {
            Games = new HashSet<Game>();
            PlayersInGames = new HashSet<PlayersInGame>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int WinsNumber { get; set; }
        public int LosesNumber { get; set; }
        public int WinStreak { get; set; }
        public string ProfilePic { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
