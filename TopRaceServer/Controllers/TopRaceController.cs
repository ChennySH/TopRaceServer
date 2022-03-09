﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopRaceServer.DTOs;
//Add the below
using TopRaceServerBL.Models;
using System.IO;

namespace TopRaceServer.Controllers
{
    [Route("TopRaceAPI")]
    [ApiController]
    public class TopRaceController : ControllerBase
    {
        #region Add connection to the db context using dependency injection
        TopRaceDBContext context;
        public TopRaceController(TopRaceDBContext context)
        {
            this.context = context;
        }
        #endregion
        [Route("Login")]
        [HttpPost]
        public User Login([FromBody] LoginDTO loginDTO)
        {
            string userNameOrEmail = loginDTO.UserNameOrEmail;
            string password = loginDTO.Password;
            User u = context.GetUser(userNameOrEmail, password);
            //Check user name and password
            if (u != null)
            {
                HttpContext.Session.SetObject("theUser", u);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
                return u;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        [Route("LogOut")]
        [HttpGet]
        public bool LogOut()
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                HttpContext.Session.SetObject("theUser", null);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                return true;
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return false;

            }
        }

        [Route("SignUp")]
        [HttpPost]
        public bool SignUp([FromBody] User user)
        {
            bool isExist = this.context.IsExist(user.UserName, user.Email);
            if (isExist)
                return false;
            else
            {
                this.context.Users.Add(user);
                this.context.SaveChanges();
                return true;
            }
        }
        [Route("IsUserNameExist")]
        [HttpPost]
        public bool IsUserNameExist([FromBody] string userName)
        {
            return this.context.IsUserNameExist(userName);
        }
        [Route("IsEmailExist")]
        [HttpPost]
        public bool IsEmailExist([FromBody] string email)
        {
            return this.context.IsEmailExist(email);
        }
        [Route("AddWin")]
        [HttpPost]
        public void AddWin([FromBody] User user)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return;
                }
                this.context.AddWin(user);
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
        }
        [Route("AddLose")]
        [HttpPost]
        public void AddLose([FromBody] User user)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return;
                }
                this.context.AddLose(user);
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
        }
        [Route("HostGame")]
        [HttpPost]
        public Game HostGame([FromBody] Game game)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                game.Status = this.context.GameStatuses.Where(s => s.Id == 1).FirstOrDefault();
                game.PrivateKey = this.context.GetPrivateKey();
                game.ChatRoom = new ChatRoom();

                this.context.Games.Update(game);
                this.context.SaveChanges();
                this.context.PlayersInGames.Update(this.context.CreatePlayerInGame(game.HostUser, true, game));
                this.context.SaveChanges();
                return game;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }
        //[Route("GetGameStatus")]
        //[HttpGet]
        //public GameStatus GetGameStatus([FromQuery] int statusId)
        //{
        //    return this.context.GameStatuses.Where(s => s.Id == statusId).FirstOrDefault();
        //}
        //[Route("GetPrivateKey")]
        //[HttpGet]
        //public string GetPrivateKey()
        //{
        //    return this.context.GetPrivateKey();
        //}
        [Route("GetGame")]
        [HttpGet]
        public Game GetGame([FromQuery] int GameID)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
               
                Game g = this.context.GetGame(GameID);
                return g;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }
        [Route("SendMessage")]
        [HttpPost]
        public bool SendMessage([FromBody] Message message)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                this.context.AddMessage(message);
                return true;
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return false;
            }
        }
        [Route("JoinPrivateGame")]
        [HttpGet]
        public Game JoinPrivateGame([FromQuery] string privateKey)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                
                Game game = this.context.GetGameFromKey(privateKey);
                if (game == null)
                {
                    return null;
                }
                foreach(PlayersInGame pl in game.PlayersInGames)
                {
                    if (pl.UserId == currentUser.Id)
                        return null;
                }
                PlayersInGame p = this.context.CreatePlayerInGame(currentUser, false, game);
                this.context.PlayersInGames.Update(p);
                this.context.SaveChanges();
                //this.context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                //this.context.PlayersInGames.Add(p);
                //this.context.Entry(p).CurrentValues.SetValues(p);
                game.LastUpdateTime = DateTime.Now;
                //this.context.Update(game);
                this.context.Entry(game).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return game;
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }
        [Route("GetAllColors")]
        [HttpGet]
        public List<Color> GetAllColors()
        {
            return this.context.Colors.ToList();
        }
        [Route("UpdatePlayer")]
        [HttpPost]
        public bool UpdatePlayer([FromBody] PlayersInGame playerInGame)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                if (!this.context.IsColorAvailable(playerInGame))
                    return false;
                //PlayersInGame pl = this.context.PlayersInGames.Where(p => p.Id == playerInGame.Id).FirstOrDefault();
                //pl.SetValues(playerInGame);
                this.context.ChangeTracker.Clear();
                this.context.Entry(playerInGame).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //var change = this.context.ChangeTracker.Entries<TopRaceServerBL.Models.Color>().Where(x => x.State == Microsoft.EntityFrameworkCore.EntityState.Modified);
                //foreach (var c in change)
                //{
                //    c.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                //}
                this.context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return false;
            }
        }
        [Route("CloseGame")]
        [HttpGet]
        public bool CloseGame ([FromQuery] int gameID)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                bool isClosed = this.context.CloseGame(gameID);
                return isClosed;
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return false;
            }

        }
        [Route("KickOutPlayer")]
        [HttpGet]

        public bool KickOut([FromQuery] int gameID, [FromQuery] int playerInGameID)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                Game game = this.context.GetGame(gameID);
                PlayersInGame pl = this.context.PlayersInGames.Where(p => p.Id == playerInGameID).FirstOrDefault();
                this.context.ChangeTracker.Clear();
                pl.IsInGame = false;
                this.context.Entry(pl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return false;
            }
        }
        [Route("StartGame")]
        [HttpGet]
        public Game StartGame(int GameID)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                Game g = this.context.GetGame(GameID);
                g.CreateGameBoard();
                this.context.Games.Update(g);
                this.context.SaveChanges();
                return g;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        [Route("Play")]
        [HttpGet]
        public Game Play(int GameID)
        {
            try
            {
                // checking if there id a user active
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                // checking in the user is in the game;
                Game game = this.context.GetGame(GameID);
                if (!this.context.IsInGame(game, currentUser.Id))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                // checking if the user requesting is the user who's turn is now
                if (game.CurrentPlayerInTurn.UserId != currentUser.Id)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                // rolling the dice - getting a random number between 1 to 6
                Random rnd = new Random();
                int rollResult = rnd.Next(1, 7);
                // the player
                PlayersInGame currentPlayerInTurn = game.CurrentPlayerInTurn;
                // getting previous position
                Position previousPos = currentPlayerInTurn.CurrentPos;
                // getting the position id after the roll
                int newPosId = previousPos.Id + rollResult;
                if (newPosId > 100)
                {
                    newPosId = 100 - (newPosId - 100);
                }
                // getting the pos itself
                Position newPos = this.context.Positions.Where(p => p.Id == newPosId).FirstOrDefault();
                // using the gameDTO in order to check if he landed on a ladder or a snake
                // and if he did to move him to the mover's end position
                GameDTO gameDTO = new GameDTO(game);
                MoversInGame[,] board = gameDTO.board;
                MoversInGame mover = board[newPos.X, newPos.Y];
                if (mover.IsLadder || mover.IsSnake)
                {
                    int finalPosId = mover.EndPosId;
                    Position finalPos = this.context.Positions.Where(p => p.Id == finalPosId).FirstOrDefault();
                    newPos = finalPos;
                    newPosId = finalPosId;
                }
                // chaning the position in the player in the DB
                currentPlayerInTurn.CurrentPosId = newPosId;
                //currentPlayerInTurn.CurrentPos = newPos;
                this.context.PlayersInGames.Update(currentPlayerInTurn);
                // only of roll result lower than 6 the turn moves otherwise extra dice!
                int nextId = currentPlayerInTurn.Id;
                if (rollResult != 6)
                {
                    // Set CurrentPlayer and previous player
                    List<PlayersInGame> lst = game.PlayersInGames.ToList();
                    do
                    {
                        if (game.CurrentPlayerInTurnId == lst[lst.Count - 1].Id)
                        {
                            nextId = lst[1].Id;
                        }
                        else
                        {
                            for (int i = 0; i < lst.Count; i++)
                            {
                                PlayersInGame pl = lst[i];
                                if (pl.Id == game.CurrentPlayerInTurnId)
                                {
                                    nextId = lst[i + 1].Id;
                                }
                            }
                        }
                    } while (this.context.PlayersInGames.Where(pl => pl.Id == nextId).FirstOrDefault().IsInGame);
                    game.CurrentPlayerInTurnId = nextId;
                    game.PreviousPlayerId = game.CurrentPlayerInTurnId;
                }
                // checking if he won
                if(newPosId == 100)
                {
                    game.WinnerId = currentPlayerInTurn.Id;
                }
                // updating the last roll result
                game.LastRollResult = rollResult;
                this.context.Games.Update(game);
                return game;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }
    }
    
}
