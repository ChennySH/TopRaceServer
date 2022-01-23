CREATE DATABASE TopRaceDB
GO
USE TopRaceDB
CREATE TABLE "User"(
    "id" INT IDENTITY NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "Password" NVARCHAR(255) NOT NULL,
    "PhoneNumber" NVARCHAR(255) NOT NULL,
    "WinsNumber" INT NOT NULL,
    "LosesNumber" INT NOT NULL,
    "WinsStreak" INT NOT NULL,
    "ProfilePic" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "User" ADD CONSTRAINT "user_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "user_username_unique" ON
    "User"("UserName");
CREATE UNIQUE INDEX "user_email_unique" ON
    "User"("Email");
CREATE TABLE "Game"(
    "id" INT IDENTITY NOT NULL,
    "GameName" NVARCHAR(255) NOT NULL,
    "IsPrivate" BIT NOT NULL,
    "PrivateKey" NVARCHAR(255) NOT NULL,
    "HostUserID" INT NOT NULL,
    "CurrentTurn" INT NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "StatusID" INT NOT NULL,
    "LastUpdateTime" DATETIME NOT NULL
);
ALTER TABLE
    "Game" ADD CONSTRAINT "game_id_primary" PRIMARY KEY("id");
CREATE TABLE "PlayersInGame"(
    "id" INT IDENTITY NOT NULL,
    "UserID" INT NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
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
CREATE TABLE "ChatRoom"("id" INT IDENTITY NOT NULL);
ALTER TABLE
    "ChatRoom" ADD CONSTRAINT "chatroom_id_primary" PRIMARY KEY("id");
CREATE TABLE "Color"(
    "id" INT IDENTITY NOT NULL,
    "ColorName" NVARCHAR(255) NOT NULL,
    "ColorCode" NVARCHAR(255) NOT NULL,
    "PicLink" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Color" ADD CONSTRAINT "color_id_primary" PRIMARY KEY("id");
CREATE TABLE "Message"(
    "id" INT IDENTITY NOT NULL,
    "FromID" INT NOT NULL,
    "Message" NVARCHAR(255) NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "TimeSent" DATETIME NOT NULL
);
ALTER TABLE
    "Message" ADD CONSTRAINT "message_id_primary" PRIMARY KEY("id");
CREATE TABLE "GameStatus"(
    "id" INT IDENTITY NOT NULL,
    "StatusName" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "GameStatus" ADD CONSTRAINT "gamestatus_id_primary" PRIMARY KEY("id");
CREATE TABLE "Position"(
    "id" INT IDENTITY NOT NULL,
    "x" INT NOT NULL,
    "y" INT NOT NULL
);
ALTER TABLE
    "Position" ADD CONSTRAINT "position_id_primary" PRIMARY KEY("id");
CREATE TABLE "MoversInGame"(
    "id" INT IDENTITY NOT NULL,
    "GameID" INT NOT NULL,
    "StartPosID" INT NOT NULL,
    "EndPosID" INT NOT NULL,
    "IsLadder" BIT NOT NULL
);
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_userid_foreign" FOREIGN KEY("UserID") REFERENCES "User"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_hostuserid_foreign" FOREIGN KEY("HostUserID") REFERENCES "User"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "MoversInGame" ADD CONSTRAINT "moversingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
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
	GO
use TopRaceDB 
insert GameStatus (StatusName)
values ('Wait');
insert GameStatus (StatusName)
values ('On');
insert GameStatus (StatusName)
values ('InActive');
INSERT Color
VALUES ('Red', '#FF0000', 'https://www.graphicpie.com/wp-content/uploads/2020/11/red-among-us-png.png');
INSERT Color
VALUES ('Cyan', '#00FFFF', 'https://www.graphicpie.com/wp-content/uploads/2020/11/cyan-among-us-character.png');
INSERT Color
VALUES ('Lime', '#00FF00','https://www.graphicpie.com/wp-content/uploads/2020/11/among-us-green-png.png');
INSERT Color
VALUES ('Yellow', '#FFFF00', 'https://www.graphicpie.com/wp-content/uploads/2020/11/yellow-among-us.png');
INSERT [User]
VALUES('User1', 't@g', '12345678', '0500000000', 0,0,0,'https://www.eekwi.org/sites/default/files/styles/original/public/2019-11/greysquirrel.jpg?itok=xGaAUTUj');