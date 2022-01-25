using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceServerBL.Models;


namespace TopRaceServerBL.Models
{
    public partial class TopRaceDBContext : DbContext
    {
        public User GetUser(string userNameOrEmail, string password)
        {
            User u = this.Users.Where(uc => uc.UserName == userNameOrEmail && uc.Password == password).FirstOrDefault();
            if (u != null)
                return u;
            else
                return this.Users.Where(uc => uc.Email == userNameOrEmail && uc.Password == password).FirstOrDefault();
        }
        public bool IsExist(string userName, string email)
        {
            User u = this.Users.Where(uc => uc.UserName == userName || uc.Email == email).FirstOrDefault();
            bool isExist = (u != null);
            return isExist;
        }
        public bool IsUserNameExist(string userName)
        {
            User u = this.Users.Where(uc => uc.UserName == userName).FirstOrDefault();
            bool isExist = (u != null);
            return isExist;
        }
        public bool IsEmailExist(string email)
        {
            User u = this.Users.Where(uc => uc.Email == email).FirstOrDefault();
            bool isExist = (u != null);
            return isExist;
        }
        public void AddWin(User user)
        {
            user.WinsNumber++;
            user.WinsStreak++;
        }
        public void AddLose(User user)
        {
            user.LosesNumber++;
            user.WinsStreak = 0;
        }

        public string GetPrivateKey()
        {
            List<char> lst = new List<char>();
            for (int i = '0'; i < '9' + 1; i++)
            {
                lst.Add((char)i);
            }
            for (int i = 'A'; i < 'Z' + 1; i++)
            {
                if (i != 'O')
                    lst.Add((char)i);
            }
            string privateKey = "";
            do
            {
                privateKey = "";
                for (int i = 0; i < 4; i++)
                {
                    Random rnd = new Random();
                    privateKey += lst[rnd.Next(0, lst.Count())];
                }
            } while (this.IsKeyInUse(privateKey));
            return privateKey;
        }
        public bool IsKeyInUse(string key)
        {
            foreach (Game g in this.Games)
            {
                if (g.StatusId != 3 && g.PrivateKey == key)
                    return true;
            }
            return false;
        }
        public PlayersInGame CreatePlayerInGame(User user, bool isHost, Game game)
        {
            PlayersInGame playerInGame = new PlayersInGame
            {
                ChatRoom = game.ChatRoom,
                UserId = user.Id,
                UserName = user.UserName,
                ProfilePic = user.ProfilePic,
                IsHost = isHost,
                Number = game.PlayersInGames.Count(),
                Color = GetColor(game),
                CurrentPos = GetPosition(0, 0),
                LastMoveTime = DateTime.Now,
                GameId = game.Id
            };
            return playerInGame;
        }
        public Color GetColor(Game game)
        {
            foreach (Color color in Colors)
            {
                bool isInUse = false;
                foreach (PlayersInGame p in game.PlayersInGames)
                {
                    if (p.Color == color)
                        isInUse = true;
                }
                if (!isInUse)
                    return color;
            }
            return null;
        }
        public Position GetPosition(int x, int y)
        {
            Position p = this.Positions.Where(p => p.X == x && p.Y == y).FirstOrDefault();
            if (p != null)
                return p;
            Position pos = new Position
            {
                X = x,
                Y = y
            };
            this.Positions.Add(pos);
            SaveChanges();
            return pos;
        }
        public Game GetGame(int GameID)
        {
            Game g = this.Games.Include(gm => gm.ChatRoom).Include(gm => gm.PlayersInGames).Where(gm => gm.Id == GameID).FirstOrDefault();

            return g;
        }
        public void AddMessage(Message message)
        {
            ChatRoom chatRoom = this.ChatRooms.Where(c => c.Id == message.ChatRoomId).FirstOrDefault();
            chatRoom.Messages.Add(message);
            SaveChanges();
        }
    }
}
