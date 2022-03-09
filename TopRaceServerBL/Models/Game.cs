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
        public int ChatRoomId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Board { get; set; }
        public int StatusId { get; set; }
        public int? WinnerId { get; set; }
        public int? CurrentPlayerInTurnId { get; set; }
        public int? PreviousPlayerId { get; set; }
        public int LastRollResult { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual PlayersInGame CurrentPlayerInTurn { get; set; }
        public virtual User HostUser { get; set; }
        public virtual PlayersInGame PreviousPlayer { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual PlayersInGame Winner { get; set; }
        public virtual ICollection<MoversInGame> MoversInGames { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
