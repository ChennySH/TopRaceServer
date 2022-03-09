using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceServerBL.Models;


namespace TopRaceServerBL.Models
{
    public partial class Game
    {
        public void CreateGameBoard()
        {
            MoversInGame[,] board = new MoversInGame[10, 10];
            MoversInGame[] ladders = new MoversInGame[8];
            MoversInGame[] snakes = new MoversInGame[8];
            Random rnd = new Random();
            for (int i = 0; i < ladders.Length; i++)
            {
                // setting the snakes and ladders
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    MoversInGame mover = new MoversInGame 
                    foreach(MoversInGame m in ladders)
                    {
                        if(m.StartPos.X == j && m.StartPos.Y == i)
                        {

                        }
                    }
                    //board[j,i] = 
                }
            }
        }
       
    }
}
