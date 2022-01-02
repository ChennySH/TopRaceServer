﻿using Microsoft.EntityFrameworkCore;
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
            User u = this.Users.Include(uc => uc.Player).Where(uc => uc.UserName == userNameOrEmail && uc.Password == password).FirstOrDefault();
            if (u != null)
                return u;
            else
                return this.Users.Include(uc => uc.Player).Where(uc => uc.Email == userNameOrEmail && uc.Password == password).FirstOrDefault();
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
            }while(this.IsKeyInUse(privateKey));
            return privateKey;
        }
        public bool IsKeyInUse(string key)
        {
            foreach (Game g in this.Games) 
            {
                if (g.StatusId != 2 && g.PrivateKey == key)
                    return true;
            }
            return false;
        }
        public PlayersInGame AddPlayer(Player player, bool isHost, Game game)
        {
            PlayersInGame playerInGame = new PlayersInGame
            {
                Player = player,
                IsHost = isHost,
                Number = game.PlayersInGames.Count(),
                Color = GetColor(game),
                CurrentPos = GetPosition(0, 0)
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
            return pos;
        }
    }
}
