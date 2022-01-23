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

        [Route("TestPlayer")]
        [HttpGet]
        public bool TestPlayer()
        {
            Player p = new Player() { LosesNumber = 0, WinStreak = 0, PlayerName = "tal", WinsNumber = 0, ProfilePic = " " };
            this.context.Players.Add(p);
            this.context.SaveChanges();
            return true;
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
                if (currentUser == null)
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
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
                game.StatusId = 1;
                game.PrivateKey = this.context.GetPrivateKey();
                game.ChatRoom = new ChatRoom();
                
                this.context.Games.Update(game);
                this.context.SaveChanges();
                this.context.PlayersInGames.Update(this.context.AddPlayer(game.HostPlayer, true, game));
                this.context.SaveChanges();
                return game;
            }
            catch(Exception e)
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
                User currentUser = HttpContext.Session.GetObject<User>("theUser");
                if (currentUser == null)
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
