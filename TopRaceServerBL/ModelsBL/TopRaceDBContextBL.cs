using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceServerBL.Models;
using TopRaceServerBL.DTOs;

namespace TopRaceServerBL.Models
{
    public partial class TopRaceDBContext : DbContext
    {
        public User GetUser(string userNameOrEmail, string password)
        {
            User u = this.Users.Include(uc => uc.Player).ThenInclude(p => p.PlayersInGames).Where(uc => uc.UserName == userNameOrEmail && uc.Password == password).FirstOrDefault();
            if (u != null)
                return u;
            else
                return this.Users.Include(uc => uc.Player).ThenInclude(p => p.PlayersInGames).Where(uc => uc.Email == userNameOrEmail && uc.Password == password).FirstOrDefault();
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
        public void AddWin(UserDTO userDTO)
        {
            User user = this.Users.Include(uc => uc.Player).Where(uc => uc.Email == userDTO.Email && uc.UserName == userDTO.UserName).FirstOrDefault();
            Player player = user.Player;
            player.WinsNumber++;
            player.WinStreak++;
        }
        public void AddLose(UserDTO userDTO)
        {
            User user = this.Users.Include(uc => uc.Player).Where(uc => uc.Email == userDTO.Email && uc.UserName == userDTO.UserName).FirstOrDefault();
            Player player = user.Player;
            player.LosesNumber++;
            player.WinStreak = 0;
        }
    }
}
