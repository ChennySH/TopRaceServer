using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Mover
    {
        public int Id { get; set; }
        public int StartPosId { get; set; }
        public int? NextPosId { get; set; }
        public int EndPosId { get; set; }
        public bool IsLadder { get; set; }
        public bool IsSnake { get; set; }

        public virtual Position EndPos { get; set; }
        public virtual Position NextPos { get; set; }
        public virtual Position StartPos { get; set; }
    }
}
