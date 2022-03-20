using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
    }
}
