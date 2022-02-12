using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class PlayersInGame
    {
        public PlayersInGame()
        {
            Messages = new HashSet<Message>();
        }
        public PlayersInGame(PlayersInGame playersInGame)
        {
            Messages = new HashSet<Message>();
            Id = playersInGame.Id;
            UserId = playersInGame.UserId;
            UserName = playersInGame.UserName;
            ProfilePic = playersInGame.ProfilePic;
            IsHost = playersInGame.IsHost;
            ColorId = playersInGame.ColorId;
            ChatRoomId = playersInGame.ChatRoomId;
            GameId = playersInGame.GameId;
            CurrentPosId = playersInGame.CurrentPosId;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsHost { get; set; }
        public int ColorId { get; set; }
        public int ChatRoomId { get; set; }
        public int GameId { get; set; }
        public int CurrentPosId { get; set; }
        public DateTime LastMoveTime { get; set; }
        public DateTime EnterTime { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
