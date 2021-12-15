using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class ChatRoom
    {
        public ChatRoom()
        {
            Games = new HashSet<Game>();
            Messages = new HashSet<Message>();
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
