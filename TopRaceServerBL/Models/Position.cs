using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Position
    {
        public Position()
        {
            MoversInGameEndPos = new HashSet<MoversInGame>();
            MoversInGameStartPos = new HashSet<MoversInGame>();
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public virtual ICollection<MoversInGame> MoversInGameEndPos { get; set; }
        public virtual ICollection<MoversInGame> MoversInGameStartPos { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
