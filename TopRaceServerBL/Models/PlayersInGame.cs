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

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public bool IsHost { get; set; }
        public int Number { get; set; }
        public int ColorId { get; set; }
        public int ChatRoomId { get; set; }
        public int GameId { get; set; }
        public int CurrentPosId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
