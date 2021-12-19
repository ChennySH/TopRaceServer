using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class GameStatus
    {
        public GameStatus()
        {
            Games = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
