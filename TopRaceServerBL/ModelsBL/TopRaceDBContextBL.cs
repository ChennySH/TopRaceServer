using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceServerBL.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


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
                if (game.PlayersInGames != null)
                { 
                    foreach (PlayersInGame p in game.PlayersInGames)
                    {
                        if (p.ColorId == color.Id && p.IsInGame)
                            isInUse = true;
                    }
                }
                if (!isInUse)
                    return color.Id;
            }
            return 0;
        }
        public int GetPositionID(int x, int y)
        {
            Position p = this.Positions.Where(p => p.X == x && p.Y == y).FirstOrDefault();
            return p.Id;
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
        public Position GetPositionByID(int id)
        {
            return this.Positions.Where(p => p.Id == id).FirstOrDefault();
        }
        public Game GetGame(int GameID)
        {
            Game g = this.Games.Include(gm => gm.Status).Include(gm => gm.Messages).ThenInclude(m => m.From).ThenInclude(p => p.Color).Include(gm => gm.PlayersInGames).ThenInclude(pl => pl.Color).Where(gm => gm.Id == GameID).FirstOrDefault();
            return g;
        }
        public Game GetGameFromKey(string privateKey)
        {
            Game g = this.Games.Include(gm => gm.Status).Include(gm => gm.Messages).ThenInclude(m => m.From).ThenInclude(p => p.Color).Include(gm => gm.PlayersInGames).ThenInclude(pl => pl.Color).Where(gm => gm.PrivateKey == privateKey && gm.StatusId == 1 && GetPlayersNumber(privateKey) < 4).FirstOrDefault();

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
                if (pl.Id != player.Id && pl.IsInGame && pl.ColorId == player.ColorId)
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
        public bool IsInGame(Game game, int UserID)
        {
            foreach(PlayersInGame pl in game.PlayersInGames)
            {
                if (pl.UserId == UserID)
                    return true;
            }
            return false;
        }
        public Mover[][] CreateGameBoard()
        {
            Mover[,] board = new Mover[10, 10];
            List<int> freePositions = new List<int>();
            List<int> moversPosition = new List<int>();
            List<int> group1 = new List<int>();
            List<int> group2 = new List<int>();
            List<int> group3 = new List<int>();
            List<int> group4 = new List<int>();
            List<Mover> snakes = new List<Mover>();
            List<Mover> ladders = new List<Mover>();
            // deviding the board to 4 different groups
            for (int i = 2; i < 100; i++)
            {
                int decedes = i / 10;
                int digits = i % 10;
                if (i <= 50)
                {
                    if ((decedes % 2 == 0 && digits <= 5) || ((decedes % 2 == 1 && digits > 5)))
                    {
                        group1.Add(i);
                    }
                    else
                    {
                        group2.Add(i);
                    }
                }
                else
                {
                    if ((decedes % 2 == 0 && digits <= 5) || ((decedes % 2 == 1 && digits > 5)))
                    {
                        group4.Add(i);
                    }
                    else
                    {
                        group3.Add(i);
                    }
                }
            }
            Random rnd = new Random();
            for (int i = 0; i < 16; i++)//getting 16 random free positions on group 1
            {
                int num = group1[rnd.Next(0, group1.Count)];
                freePositions.Add(num);
                group1.Remove(num);
            }
            for (int i = 0; i < 17; i++)//getting 17 random free positions on group 2
            {
                int num = group2[rnd.Next(0, group2.Count)];
                freePositions.Add(num);
                group2.Remove(num);
            }
            for (int i = 0; i < 17; i++)//getting 17 random free positions on group 3
            {
                int num = group3[rnd.Next(0, group3.Count)];
                freePositions.Add(num);
                group3.Remove(num);
            }
            int mustSnakePos = rnd.Next(97, 100);// setting a position between 97 - 99 where there must to be a snake
            for (int i = 0; i < 16; i++)//getting 16 random free positions on group 4
            {
                int num = group4[rnd.Next(0, group4.Count)];
                if (num == mustSnakePos)
                {
                    while (num == mustSnakePos)// if the snake pos is chosen you pick another one instead
                    {
                        num = group4[rnd.Next(0, group4.Count)];
                    }
                }
                freePositions.Add(num);
                group4.Remove(num);
            }
            foreach (int n in group1)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group2)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group3)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group4)
            {
                moversPosition.Add(n);
            }
            moversPosition.Sort();
            //setting the snakes

            //setting the top snake
            int topIndex = moversPosition.IndexOf(mustSnakePos);//getting the index
            int topSnakeEndPos = moversPosition[rnd.Next(0, topIndex)];//setting the endPos by getting an index loser than the start index
            Mover topSnake = new Mover
            {
                StartPosId = mustSnakePos,
                EndPosId = topSnakeEndPos,
                IsLadder = false,
                IsSnake = true
            };
            //adding the snake
            snakes.Add(topSnake);
            moversPosition.Remove(mustSnakePos);
            moversPosition.Remove(topSnakeEndPos);
            for (int i = 0; i < 7; i++)
            {
                int startIndex = rnd.Next(2, moversPosition.Count);// setting a start index which can't be the first advalible pos idnex
                int endIndex = rnd.Next(0, startIndex);// seting a end index lower than the start index
                // getting the pos ids
                int startPosID = moversPosition[startIndex];
                int endPosId = moversPosition[endIndex];
                // creating the snake
                Mover snake = new Mover
                {
                    StartPosId = startPosID,
                    EndPosId = endPosId,
                    IsLadder = false,
                    IsSnake = true,
                };
                // adding the snake
                snakes.Add(snake);
                // removing the taken positions
                moversPosition.Remove(startPosID);
                moversPosition.Remove(endPosId);
            }
            // setting the ladders
            for (int i = 0; i < 8; i++)
            {
                int startIndex = rnd.Next(0, moversPosition.Count - 1);// setting a start index which can't be the last advalible pos idnex
                int endIndex = rnd.Next(startIndex + 1, moversPosition.Count);// seting a end index higher than the start index
                // getting the pos ids
                int startPosID = moversPosition[startIndex];
                int endPosId = moversPosition[endIndex];
                // creating the ladder
                Mover ladder = new Mover
                {
                    StartPosId = startPosID,
                    EndPosId = endPosId,
                    IsLadder = true,
                    IsSnake = false,
                };
                // adding the ladder
                ladders.Add(ladder);
                // removing the taken positions
                moversPosition.Remove(startPosID);
                moversPosition.Remove(endPosId);
            }
            foreach (Mover l in ladders)
            {
                l.StartPos = this.Positions.Where(p => p.Id == l.StartPosId).FirstOrDefault();
                l.NextPos = this.Positions.Where(p => p.Id == l.NextPosId).FirstOrDefault();
                l.EndPos = this.Positions.Where(p => p.Id == l.EndPosId).FirstOrDefault();
            }
            foreach (Mover s in snakes)
            {
                s.StartPos = this.Positions.Where(p => p.Id == s.StartPosId).FirstOrDefault();
                s.NextPos = this.Positions.Where(p => p.Id == s.NextPosId).FirstOrDefault();
                s.EndPos = this.Positions.Where(p => p.Id == s.EndPosId).FirstOrDefault();
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Mover mover = new Mover
                    {
                        StartPosId = this.GetPositionID(j, i),
                        NextPosId = this.GetPositionID(j, i) + 1,
                        EndPosId = this.GetPositionID(j, i) + 1
                    };
                    if (i == 9 && j == 0)
                    {
                        mover = new Mover
                        {
                            StartPosId = this.GetPositionID(j, i),
                            NextPosId = this.GetPositionID(j, i),
                            EndPosId = this.GetPositionID(j, i)
                        };
                    }
                    foreach (Mover m in ladders)
                    {
                        if (m.StartPos.X == j && m.StartPos.Y == i)
                        {
                            mover = m;                     
                        }
                    }
                    foreach (Mover m in snakes)
                    {
                        if (m.StartPos.X == j && m.StartPos.Y == i)
                        {
                            mover = m;
                        }
                    }
                    board[j, i] = mover;
                }
            }
            foreach(Mover m in board)
            {
                this.Movers.Update(m);
                this.SaveChanges();
            }
            return ToJaggedArray<Mover>(board);

        }

        public static T[][] ToJaggedArray<T>(T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = 0; i < numberOfRows; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = 0; j < numberOfColumns; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i + rowsFirstIndex, j + columnsFirstIndex];
                }
            }
            return jaggedArray;
        }

        public static T[,] ToMatrix<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            T[,] twoDimensionalArray = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDimensionalArray;
        }

        
        public int GetPlayersNumber(string privateKey)
        {
            Game g = this.Games.Include(gm => gm.PlayersInGames).Where(gm => gm.PrivateKey == privateKey).FirstOrDefault();
            int counter = 0;
            foreach (PlayersInGame pl in g.PlayersInGames) 
            {
                if (pl.IsInGame)
                {
                    counter++;
                }
            }
            return counter;
        }
        public int GetPlayersNumber(int gameID)
        {
            Game g = this.Games.Include(gm => gm.PlayersInGames).Where(gm => gm.Id == gameID).FirstOrDefault();
            int counter = 0;
            foreach (PlayersInGame pl in g.PlayersInGames)
            {
                if (pl.IsInGame)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    
}
