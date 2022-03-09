using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class PlayersInGame
    {
        public PlayersInGame()
        {
            GameCurrentPlayerInTurns = new HashSet<Game>();
            GamePreviousPlayers = new HashSet<Game>();
            GameWinners = new HashSet<Game>();
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsHost { get; set; }
        public bool IsInGame { get; set; }
        public bool DidPlayInGame { get; set; }
        public int ColorId { get; set; }
        public int ChatRoomId { get; set; }
        public int GameId { get; set; }
        public int CurrentPosId { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LastMoveTime { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Game> GameCurrentPlayerInTurns { get; set; }
        public virtual ICollection<Game> GamePreviousPlayers { get; set; }
        public virtual ICollection<Game> GameWinners { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
