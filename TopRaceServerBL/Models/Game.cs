using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Game
    {
        public Game()
        {
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public int PrivateKey { get; set; }
        public int HostPlayerId { get; set; }
        public int CurrentTurn { get; set; }
        public int Players { get; set; }
        public int ChatRoomId { get; set; }
        public int StatusId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
