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
    "LastUpdateTime" DATETIME NOT NULL,
    "UpdatesCounter" INT NOT NULL,
    "MovesCounter" INT NOT NULL,
    "Board" TEXT NOT NULL,
    "StatusID" INT NOT NULL,
    "WinnerID" INT NULL,
    "CurrentPlayerInTurnID" INT NULL,
    "PreviousPlayerID" INT NULL,
    "LastRollResult" INT NOT NULL
);
ALTER TABLE
    "Game" ADD CONSTRAINT "game_id_primary" PRIMARY KEY("id");
CREATE TABLE "PlayersInGame"(
    "id" INT IDENTITY NOT NULL,
    "UserID" INT NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "ProfilePic" NVARCHAR(255) NOT NULL,
    "IsHost" BIT NOT NULL,
    "IsInGame" BIT NOT NULL,
    "DidPlayInGame" BIT NOT NULL,
    "ColorID" INT NOT NULL,
    "ChatRoomID" INT NOT NULL,
    "GameID" INT NOT NULL,
    "CurrentPosID" INT NOT NULL,
    "EnterTime" DATETIME NOT NULL,
    "LastMoveTime" DATETIME NOT NULL
);
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_id_primary" PRIMARY KEY("id");
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
    "GameID" INT NOT NULL,
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
CREATE TABLE "Mover"(
    "id" INT IDENTITY NOT NULL,
    "StartPosID" INT NOT NULL,
    "NextPosID" INT NULL,
    "EndPosID" INT NOT NULL,
    "IsLadder" BIT NOT NULL,
    "IsSnake" BIT NOT NULL
);
ALTER TABLE
    "Mover" ADD CONSTRAINT "mover_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_userid_foreign" FOREIGN KEY("UserID") REFERENCES "User"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_gameid_foreign" FOREIGN KEY("GameID") REFERENCES "Game"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_winnerid_foreign" FOREIGN KEY("WinnerID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_currentplayerinturnid_foreign" FOREIGN KEY("CurrentPlayerInTurnID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_previousplayerid_foreign" FOREIGN KEY("PreviousPlayerID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "Message" ADD CONSTRAINT "message_fromid_foreign" FOREIGN KEY("FromID") REFERENCES "PlayersInGame"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_currentposid_foreign" FOREIGN KEY("CurrentPosID") REFERENCES "Position"("id");
ALTER TABLE
    "PlayersInGame" ADD CONSTRAINT "playersingame_colorid_foreign" FOREIGN KEY("ColorID") REFERENCES "Color"("id");
ALTER TABLE
    "Game" ADD CONSTRAINT "game_statusid_foreign" FOREIGN KEY("StatusID") REFERENCES "GameStatus"("id");
ALTER TABLE
    "Mover" ADD CONSTRAINT "mover_startposid_foreign" FOREIGN KEY("StartPosID") REFERENCES "Position"("id");
ALTER TABLE
    "Mover" ADD CONSTRAINT "mover_endposid_foreign" FOREIGN KEY("EndPosID") REFERENCES "Position"("id");
ALTER TABLE
    "Mover" ADD CONSTRAINT "mover_nextposid_foreign" FOREIGN KEY("NextPosID") REFERENCES "Position"("id");
GO
use TopRaceDB 
insert GameStatus (StatusName)
values ('Wait');
insert GameStatus (StatusName)
values ('On');
insert GameStatus (StatusName)
values ('InActive');
INSERT Color
VALUES ('Red', '#FF0000', 'RedCrewmate.png');
INSERT Color
VALUES ('Blue', '#0000FF', 'BlueCrewmate.png');
INSERT Color
VALUES ('Green', '#008000', 'GreenCrewmate.png');
INSERT Color
VALUES ('Pink', '#FF1493', 'PinkCrewmate.png');
INSERT Color
VALUES ('Orange', '#FFA500', 'OrangeCrewmate.png');
INSERT Color
VALUES ('Yellow', '#FFFF00', 'YellowCrewmate.png');
INSERT Color
VALUES ('Black', '#000000', 'BlackCrewmate.png');
INSERT Color
VALUES ('White', '#FFFFFF', 'WhiteCrewmate.png');
INSERT Color
VALUES ('Purple', '#800080', 'PurpleCrewmate.png');
INSERT Color
VALUES ('Brown', '#A52A2A', 'BrownCrewmate.png');
INSERT Color
VALUES ('Cyan', '#00FFFF', 'CyanCrewmate.png');
INSERT Color
VALUES ('Lime', '#00FF00','LimeCrewmate.png');
INSERT [User]
VALUES('User1', 't@g', '12345678', '0500000000', 0,0,0,'https://www.eekwi.org/sites/default/files/styles/original/public/2019-11/greysquirrel.jpg?itok=xGaAUTUj');
INSERT [User]
VALUES('User2', 't@g2', '12345678', '0500000000', 0,0,0,'https://static01.nyt.com/images/2021/09/21/science/09tb-butterflies-1/09tb-butterflies-1-mobileMasterAt3x.jpg');
INSERT [User]
VALUES('User3', 't@g3', '12345678', '0500000000', 0,0,0,'https://ip.index.hr/remote/indexnew.s3.index.hr/989fd20a-b4d3-4f81-b1a3-0d4bdae4b0f4-46962987_l%20(1).jpg');
INSERT [User]
VALUES('User4', 't@g4', '12345678', '0500000000', 0,0,0,'https://www.google.com/url?sa=i&url=https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCamel&psig=AOvVaw3cE2Prj_gT4yLOczU7n1RB&ust=1651680062453000&source=images&cd=vfe&ved=0CAwQjRxqFwoTCODQ4fHZw_cCFQAAAAAdAAAAABAD');
INSERT [Position](x,y)--1--
Values(0,0);
INSERT [Position](x,y)--2--
Values(1,0);
 INSERT [Position](x,y)--3--
Values(2,0);
 INSERT [Position](x,y)--4--
Values(3,0);
 INSERT [Position](x,y)--5--
Values(4,0);
 INSERT [Position](x,y)--6--
Values(5,0);
 INSERT [Position](x,y)--7--
Values(6,0);
 INSERT [Position](x,y)--8--
Values(7,0);
 INSERT [Position](x,y)--9--
Values(8,0);
 INSERT [Position](x,y)--10--
Values(9,0);
 INSERT [Position](x,y)--11--
Values(9,1);
 INSERT [Position](x,y)--12--
Values(8,1);
 INSERT [Position](x,y)--13--
Values(7,1);
 INSERT [Position](x,y)--14--
Values(6,1);
 INSERT [Position](x,y)--15--
Values(5,1);
 INSERT [Position](x,y)--16--
Values(4,1);
 INSERT [Position](x,y)--17--
Values(3,1);
 INSERT [Position](x,y)--18--
Values(2,1);
 INSERT [Position](x,y)--19--
Values(1,1);
 INSERT [Position](x,y)--20--
Values(0,1);
 INSERT [Position](x,y)--21--
Values(0,2);
 INSERT [Position](x,y)--22--
Values(1,2);
 INSERT [Position](x,y)--23--
Values(2,2);
 INSERT [Position](x,y)--24--
Values(3,2);
 INSERT [Position](x,y)--25--
Values(4,2);
 INSERT [Position](x,y)--26--
Values(5,2);
 INSERT [Position](x,y)--27--
Values(6,2);
 INSERT [Position](x,y)--28--
Values(7,2);
 INSERT [Position](x,y)--29--
Values(8,2);
 INSERT [Position](x,y)--30--
Values(9,2);
 INSERT [Position](x,y)--31--
Values(9,3);
 INSERT [Position](x,y)--32--
Values(8,3);
 INSERT [Position](x,y)--33--
Values(7,3);
 INSERT [Position](x,y)--34--
Values(6,3);
 INSERT [Position](x,y)--35--
Values(5,3);
 INSERT [Position](x,y)--36--
Values(4,3);
 INSERT [Position](x,y)--37--
Values(3,3);
 INSERT [Position](x,y)--38--
Values(2,3);
 INSERT [Position](x,y)--39--
Values(1,3);
 INSERT [Position](x,y)--40-
Values(0,3);
 INSERT [Position](x,y)--41--
Values(0,4);
 INSERT [Position](x,y)--42--
Values(1,4);
 INSERT [Position](x,y)--43--
Values(2,4);
 INSERT [Position](x,y)--44--
Values(3,4);
 INSERT [Position](x,y)--45--
Values(4,4);
 INSERT [Position](x,y)--46--
Values(5,4);
 INSERT [Position](x,y)--47--
Values(6,4);
 INSERT [Position](x,y)--48-
Values(7,4);
 INSERT [Position](x,y)--49--
Values(8,4);
 INSERT [Position](x,y)--50--
Values(9,4);
 INSERT [Position](x,y)--51--
Values(9,5);
 INSERT [Position](x,y)--52--
Values(8,5);
 INSERT [Position](x,y)--53--
Values(7,5);
 INSERT [Position](x,y)--54--
Values(6,5);
 INSERT [Position](x,y)--55--
Values(5,5);
 INSERT [Position](x,y)--56--
Values(4,5);
 INSERT [Position](x,y)--57--
Values(3,5);
 INSERT [Position](x,y)--58--
Values(2,5);
 INSERT [Position](x,y)--59--
Values(1,5);
 INSERT [Position](x,y)--60--
Values(0,5);
 INSERT [Position](x,y)--61--
Values(0,6);
 INSERT [Position](x,y)--62--
Values(1,6);
 INSERT [Position](x,y)--63--
Values(2,6);
 INSERT [Position](x,y)--64--
Values(3,6);
 INSERT [Position](x,y)--65--
Values(4,6);
 INSERT [Position](x,y)--66--
Values(5,6);
 INSERT [Position](x,y)--67--
Values(6,6);
 INSERT [Position](x,y)--68--
Values(7,6);
 INSERT [Position](x,y)--69--
Values(8,6);
 INSERT [Position](x,y)--70--
Values(9,6);
 INSERT [Position](x,y)--71--
Values(9,7);
 INSERT [Position](x,y)--72--
Values(8,7);
 INSERT [Position](x,y)--73--
Values(7,7);
 INSERT [Position](x,y)--74--
Values(6,7);
 INSERT [Position](x,y)--75--
Values(5,7);
 INSERT [Position](x,y)--76--
Values(4,7);
 INSERT [Position](x,y)--77--
Values(3,7);
 INSERT [Position](x,y)--78--
Values(2,7);
 INSERT [Position](x,y)--79--
Values(1,7);
 INSERT [Position](x,y)--80--
Values(0,7);
 INSERT [Position](x,y)--81--
Values(0,8);
 INSERT [Position](x,y)--82--
Values(1,8);
 INSERT [Position](x,y)--83--
Values(2,8);
 INSERT [Position](x,y)--84--
Values(3,8);
 INSERT [Position](x,y)--85--
Values(4,8);
 INSERT [Position](x,y)--86--
Values(5,8);
 INSERT [Position](x,y)--87--
Values(6,8);
 INSERT [Position](x,y)--88-
Values(7,8);
 INSERT [Position](x,y)--89--
Values(8,8);
 INSERT [Position](x,y)--90--
Values(9,8);
 INSERT [Position](x,y)--91--
Values(9,9);
 INSERT [Position](x,y)--92--
Values(8,9);
 INSERT [Position](x,y)--93--
Values(7,9);
 INSERT [Position](x,y)--94--
Values(6,9);
 INSERT [Position](x,y)--95--
Values(5,9);
 INSERT [Position](x,y)--96--
Values(4,9);
 INSERT [Position](x,y)--97--
Values(3,9);
 INSERT [Position](x,y)--98--
Values(2,9);
 INSERT [Position](x,y)--99--
Values(1,9);
 INSERT [Position](x,y)--100--
Values(0,9);  
-- scaffold-dbcontext "Server=localhost\sqlexpress;Database=TopRaceDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models –force