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
                this.context.Players.Add(user.Player);         
                this.context.SaveChanges();
                user.PlayerId = user.Player.Id;
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
                if (!currentUser.Equals(user))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return;
                }
                this.context.AddWin(user);
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }
        }
        [Route("AddLose")]
        [HttpPost]
        public void AddLose([FromBody] User user)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (!currentUser.Equals(user))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return;
                }   
                this.context.AddLose(user);
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }
        }
        [Route("HostGame")]
        [HttpPost]
        public Game HostGame([FromBody] Game game)
        {
            try
            {
                Player host = game.HostPlayer;
                User u = this.context.Users.Where(us => us.Player == host).FirstOrDefault();
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (!currentUser.Equals(u))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                game.Status = this.context.GameStatuses.Where(s => s.Id == 1).FirstOrDefault();
                game.PrivateKey = this.context.GetPrivateKey();
                game.ChatRoom = new ChatRoom();
                game.PlayersInGames.Add(this.context.AddPlayer(game.HostPlayer, true, game));
                this.context.Games.Add(game);
                this.context.SaveChanges();
                return game;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
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
        [HttpPost]
        public Game GetGame([FromQuery] int GameID, [FromBody] User user)
        {
            try
            {
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (!currentUser.Equals(user))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                return this.context.GetGame(GameID);
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }
        }
        [Route("SendMessage")]
        [HttpPost]
        public bool SendMessage([FromBody] Message message)
        {
            try
            {
                PlayersInGame p = message.From;
                Player player = p.Player;
                User u = this.context.Users.Where(us => us.Player == player).FirstOrDefault();
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if(!currentUser.Equals(u))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                this.context.AddMessage(message);
                return true;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return false;
            }
        }
    }
}
