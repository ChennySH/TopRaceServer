using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class MoversInGame
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int StartPosId { get; set; }
        public int EndPosId { get; set; }
        public bool IsLadder { get; set; }

        public virtual Position EndPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual Position StartPos { get; set; }
    }
}
