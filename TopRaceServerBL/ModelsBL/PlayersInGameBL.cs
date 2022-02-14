using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopRaceServerBL.Models
{
    public partial class PlayersInGame
    {
        public void SetValues(PlayersInGame pl)
        {
            this.ColorId = pl.ColorId;
            this.CurrentPosId = pl.CurrentPosId;
        }
    }
}
