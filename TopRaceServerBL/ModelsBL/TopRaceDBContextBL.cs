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
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            PlayersInGame playerInGame = new PlayersInGame
            {
                ChatRoomId = game.ChatRoomId,
                UserId = user.Id,
                UserName = user.UserName,
                ProfilePic = user.ProfilePic,
                IsHost = isHost,
                IsInGame = true,
                DidPlayInGame = false,
                EnterTime = DateTime.Now,
                ColorId = GetColorID(game),
                CurrentPosId = GetPositionID(0, 0),
                LastMoveTime = DateTime.Now,
                GameId = game.Id
            };
            return playerInGame;
        }
        public int GetColorID(Game game)
        {
            foreach (Color color in this.Colors)
            {
                bool isInUse = false;
                foreach (PlayersInGame p in game.PlayersInGames)
                {
                    if (p.ColorId == color.Id)
                        isInUse = true;
                }
                if (!isInUse)
                    return color.Id;
            }
            return 0;
        }
        public int GetPositionID(int x, int y)
        {
            Position p = this.Positions.Where(p => p.X == x && p.Y == y).FirstOrDefault();
            if (p != null)
                return p.Id;
            Position pos = new Position
            {
                X = x,
                Y = y
            };
            this.Positions.Add(pos);
            SaveChanges();
            return pos.Id;
        }
        public Game GetGame(int GameID)
        {
            Game g = this.Games.Include(gm => gm.Status).Include(gm => gm.ChatRoom).ThenInclude(ch=>ch.Messages).ThenInclude(m => m.From).ThenInclude(p => p.Color).Include(gm => gm.PlayersInGames).ThenInclude(pl => pl.Color).Include(gm=>gm.HostUser).Where(gm => gm.Id == GameID).FirstOrDefault();

            return g;
        }
        public Game GetGameFromKey(string privateKey)
        {
            Game g = this.Games.Include(gm => gm.Status).Include(gm => gm.ChatRoom).ThenInclude(ch => ch.Messages).ThenInclude(m=>m.From).ThenInclude(p => p.Color).Include(gm => gm.PlayersInGames).ThenInclude(pl => pl.Color).Include(gm => gm.HostUser).Where(gm => gm.PrivateKey == privateKey && gm.StatusId == 1 && gm.PlayersInGames.Count < 4).FirstOrDefault();

            return g;
        }
        public void AddMessage(Message message)
        {
            // ChatRoom chatRoom = this.ChatRooms.Where(c => c.Id == message.ChatRoom.Id).FirstOrDefault();
            //chatRoom.Messages.Add(message);
            this.Messages.Update(message);
            var change = this.ChangeTracker.Entries<TopRaceServerBL.Models.Color>().Where(x => x.State == Microsoft.EntityFrameworkCore.EntityState.Modified);
            foreach (var c in change)
            {
                c.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            }
            SaveChanges();
        }
        public bool IsColorAvailable(PlayersInGame player)
        {
            Game game = this.Games.Include(g => g.PlayersInGames).Where(g => g.Id == player.GameId).FirstOrDefault();
            if (game == null)
                return false;
            foreach(PlayersInGame pl in game.PlayersInGames)
            {
                if (pl.Id != player.Id && pl.ColorId == player.Color.Id)
                    return false;
            }
            return true;
        }
        public bool CloseGame (int gameID)
        {
            try
            {
                Game g = this.Games.Where(g => g.Id == gameID).FirstOrDefault();
                g.StatusId = 3;
                this.Entry(g).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.SaveChanges();
                foreach (PlayersInGame pl in g.PlayersInGames)
                {
                    pl.IsInGame = false;
                    this.Entry(pl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    this.SaveChanges();
                }

                //this.Update(g);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool KickOut(int GameID, int PlayerID)
        {

        }
    }
    
}
