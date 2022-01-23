using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Game
    {
        public Game()
        {
            MoversInGames = new HashSet<MoversInGame>();
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public string GameName { get; set; }
        public bool IsPrivate { get; set; }
        public string PrivateKey { get; set; }
        public int HostUserId { get; set; }
        public int CurrentTurn { get; set; }
        public int ChatRoomId { get; set; }
        public int StatusId { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual User HostUser { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual ICollection<MoversInGame> MoversInGames { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
