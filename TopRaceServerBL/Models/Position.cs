using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class Position
    {
        public Position()
        {
            MoverEndPos = new HashSet<Mover>();
            MoverNextPos = new HashSet<Mover>();
            MoverStartPos = new HashSet<Mover>();
            PlayersInGames = new HashSet<PlayersInGame>();
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Mover> MoverEndPos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mover> MoverNextPos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mover> MoverStartPos { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
    }
}
