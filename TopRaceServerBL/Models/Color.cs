using System;
using System.Collections.Generic;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Color
    {
        public Color()
        {
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public string ColorName { get; set; }
        public string PicLink { get; set; }

        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
