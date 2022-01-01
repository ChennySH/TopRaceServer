using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopRaceServerBL.DTOs;
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
                user.Player = new Player
                {
                    PlayerName = user.UserName,
                    WinsNumber = 0,
                    LosesNumber = 0,
                    WinStreak = 0,
                    ProfilePic = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
                };
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
        public void AddWin([FromBody] UserDTO userDTO)
        {
            this.context.AddWin(userDTO);
        }
        [Route("AddLose")]
        [HttpPost]
        public void AddLose([FromBody] UserDTO userDTO)
        {
            this.context.AddLose(userDTO);
        }
        [Route("HostGame")]
        [HttpPost]
        public bool HostGame([FromBody] Game game)
        {
            this.context.Games.Add(game);
            this.context.SaveChanges();
            return true;
        }
        [Route("GetGameStatus")]
        [HttpGet]
        public GameStatus GetGameStatus([FromQuery] int statusId)
        {
            return this.context.GameStatuses.Where(s => s.Id == statusId).FirstOrDefault();
        }
        [Route("GetPrivateKey")]
        [HttpGet]
        public string GetPrivateKey([FromQuery] int statusId)
        {
            return this.context.GetPrivateKey();
        }
    }
}
