CREATE DATABASE TopRaceDB
GO
USE TopRaceDB
CREATE TABLE "User"(
    "id" INT identity NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "Password" NVARCHAR(255) NOT NULL,
    "PhoneNumber" NVARCHAR(255) NOT NULL,
    "PlayerID" INT NULL
);
ALTER TABLE
    "User" ADD CONSTRAINT "user_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "user_username_unique" ON
    "User"("UserName");
CREATE UNIQUE INDEX "user_email_unique" ON
    "User"("Email");
CREATE TABLE "Player"(
    "id" INT identity NOT NULL,
    "PlayerName" NVARCHAR(255) NOT NULL,
    "WinsNumber" INT NOT NULL,
    "LosesNumber" INT NOT NULL,
    "WinStreak" INT NOT NULL,
    "ProfilePic" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Player" ADD CONSTRAINT "player_id_primary" PRIMARY KEY("id");
CREATE TABLE "Game"(
    "id" INT identity NOT NULL,
    "GameName" NVARCHAR(255) NOT NULL,
    "IsPrivate" BIT NOT NULL,
    "PrivateKey" NVARCHAR(255) NOT NULL,
    "HostPlayerID" INT NOT NULL,
    "CurrentTurn" INT NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "StatusID" INT NOT NULL,
    "LastUpdateTime" DATETIME NOT NULL
);
ALTER TABLE
    "Game" ADD CONSTRAINT "game_id_primary" PRIMARY KEY("id");
CREATE TABLE "PlayersInGame"(
    "id" INT identity NOT NULL,
    "PlayerID" INT NOT NULL,
    "IsHost" BIT NOT NULL,
    "Number" INT NOT NULL,
    "ColorID" INT NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "GameID" INT NOT NULL,
    "CurrentPosID" INT NOT NULL,
    "LastMoveTime" DATETIME NOT NULL
);
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_id_primary" PRIMARY KEY("id");
CREATE TABLE "ChatRoom"("id" INT identity NOT NULL);
ALTER TABLE
    "ChatRoom" ADD CONSTRAINT "chatroom_id_primary" PRIMARY KEY("id");
CREATE TABLE "Color"(
    "id" INT identity NOT NULL,
    "ColorName" NVARCHAR(255) NOT NULL,
    "PicLink" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Color" ADD CONSTRAINT "color_id_primary" PRIMARY KEY("id");
CREATE TABLE "Message"(
    "id" INT identity NOT NULL,
    "FromID" INT NOT NULL,
    "Message" NVARCHAR(255) NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "TimeSent" DATETIME NOT NULL
);
ALTER TABLE
    "Message" ADD CONSTRAINT "message_id_primary" PRIMARY KEY("id");
CREATE TABLE "GameStatus"(
    "id" INT identity NOT NULL,
    "StatusName" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "GameStatus" ADD CONSTRAINT "gamestatus_id_primary" PRIMARY KEY("id");
CREATE TABLE "Position"(
    "id" INT identity NOT NULL,
    "x" INT NOT NULL,
    "y" INT NOT NULL
);
ALTER TABLE
    "Position" ADD CONSTRAINT "position_id_primary" PRIMARY KEY("id");
CREATE TABLE "MoversInGame"(
    "id" INT identity NOT NULL,
    "GameID" INT NOT NULL,
    "StartPosID" INT NOT NULL,
    "EndPosID" INT NOT NULL,
    "IsLadder" BIT NOT NULL
);
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "User" ADD CONSTRAINT "user_playerid_foreign" FOREIGN KEY("PlayerID") REFERENCES "Player"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_playerid_foreign" FOREIGN KEY("PlayerID") REFERENCES "Player"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_hostplayerid_foreign" FOREIGN KEY("HostPlayerID") REFERENCES "Player"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_fromid_foreign" FOREIGN KEY("FromID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_currentposid_foreign" FOREIGN KEY("CurrentPosID") REFERENCES "Position"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_colorid_foreign" FOREIGN KEY("ColorID") REFERENCES "Color"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_statusid_foreign" FOREIGN KEY("StatusID") REFERENCES "GameStatus"("id");
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_startposid_foreign" FOREIGN KEY("StartPosID") REFERENCES "Position"("id");
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_endposid_foreign" FOREIGN KEY("EndPosID") REFERENCES "Position"("id");
	    go
use TopRaceDB 
insert GameStatus (StatusName)
values ('Wait');
insert GameStatus (StatusName)
values ('On');
insert GameStatus (StatusName)
values ('InActive');
