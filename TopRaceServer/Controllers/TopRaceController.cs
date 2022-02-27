using Microsoft.AspNetCore.Http;
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
                return this.context.GetGame(GameID);
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
        [Route("KickOut")]
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
                if(currentUser.Id != game.HostUserId)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
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
    }
}
