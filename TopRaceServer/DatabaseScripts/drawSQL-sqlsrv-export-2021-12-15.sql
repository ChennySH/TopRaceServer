CREATE TABLE "User"(
    "id" INT NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "Password" NVARCHAR(255) NOT NULL,
    "PhoneNumber" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "User" ADD CONSTRAINT "user_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "user_email_unique" ON
    "User"("Email");
CREATE TABLE "Player"(
    "id" INT NOT NULL,
    "UserID" INT NOT NULL,
    "WinsNumber" INT NOT NULL,
    "LosesNumber" INT NOT NULL,
    "WinStreak" INT NOT NULL,
    "ProfilePic" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Player" ADD CONSTRAINT "player_id_primary" PRIMARY KEY("id");
CREATE TABLE "Game"(
    "id" INT NOT NULL,
    "IsPrivate" BIT NOT NULL,
    "PrivateKey" INT NOT NULL,
    "HostPlayerID" INT NOT NULL,
    "CurrentTurn" INT NOT NULL,
    "Players" INT NOT NULL,
    "ChatRoomID" INT NOT NULL
);
ALTER TABLE
    "Game" ADD CONSTRAINT "game_id_primary" PRIMARY KEY("id");
CREATE TABLE "PlayersInGame"(
    "id" INT NOT NULL,
    "PlayerID" INT NOT NULL,
    "IsHost" BIT NOT NULL,
    "Number" INT NOT NULL,
    "ColorID" INT NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "GameID" INT NOT NULL
);
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_id_primary" PRIMARY KEY("id");
CREATE TABLE "ChatRoom"("id" INT NOT NULL);
ALTER TABLE
    "ChatRoom" ADD CONSTRAINT "chatroom_id_primary" PRIMARY KEY("id");
CREATE TABLE "Color"(
    "id" INT NOT NULL,
    "ColorName" NVARCHAR(255) NOT NULL,
    "PicLink" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Color" ADD CONSTRAINT "color_id_primary" PRIMARY KEY("id");
CREATE TABLE "Message"(
    "id" INT NOT NULL,
    "FromID" INT NOT NULL,
    "Message" NVARCHAR(255) NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "TimeSent" DATETIME NOT NULL
);
ALTER TABLE
    "Message" ADD CONSTRAINT "message_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "Player" ADD CONSTRAINT "player_userid_foreign" FOREIGN KEY("UserID") REFERENCES "User"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_playerid_foreign" FOREIGN KEY("PlayerID") REFERENCES "Player"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_fromid_foreign" FOREIGN KEY("FromID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_chatroomid_foreign" FOREIGN KEY("ChatRoomID") REFERENCES "ChatRoom"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_colorid_foreign" FOREIGN KEY("ColorID") REFERENCES "Color"("id");