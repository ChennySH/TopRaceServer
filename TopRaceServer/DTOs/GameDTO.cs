using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TopRaceServerBL.Models;

namespace TopRaceServer.DTOs
{
    public class GameDTO
    {
        public DateTime lastUpdated { get; set; }
        public MoversInGame[,] board { get; set; }
        public User host { get; set; }
        public List<PlayersInGame> playersInGame { get; set; }
        public PlayersInGame winner { get; set; }
        public PlayersInGame currentPlayer { get; set; }
        public ChatRoom gameChatRoom { get; set; }
        public GameDTO(Game game)
        {
            lastUpdated = game.LastUpdateTime;
            host = game.HostUser;
            playersInGame = game.PlayersInGames.ToList();
            winner = game.Winner;
            currentPlayer = game.CurrentPlayerInTurn;
            gameChatRoom = game.ChatRoom;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                PropertyNameCaseInsensitive = true
            };
            board = JsonSerializer.Deserialize<MoversInGame[,]>(game.Board, options);
        }
    }
}
