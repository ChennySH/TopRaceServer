using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TopRaceServerBL.Models
{
    public partial class TopRaceDBContext : DbContext
    {
        public TopRaceDBContext()
        {
        }

        public TopRaceDBContext(DbContextOptions<TopRaceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameStatus> GameStatuses { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MoversInGame> MoversInGames { get; set; }
        public virtual DbSet<PlayersInGame> PlayersInGames { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=TopRaceDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.ToTable("ChatRoom");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ColorCode)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ColorName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PicLink)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Board)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ChatRoomId).HasColumnName("ChatRoomID");

                entity.Property(e => e.CurrentPlayerInTurnId).HasColumnName("CurrentPlayerInTurnID");

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.HostUserId).HasColumnName("HostUserID");

                entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");

                entity.Property(e => e.PreviousPlayerId).HasColumnName("PreviousPlayerID");

                entity.Property(e => e.PrivateKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.WinnerId).HasColumnName("WinnerID");

                entity.HasOne(d => d.ChatRoom)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.ChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_chatroomid_foreign");

                entity.HasOne(d => d.CurrentPlayerInTurn)
                    .WithMany(p => p.GameCurrentPlayerInTurns)
                    .HasForeignKey(d => d.CurrentPlayerInTurnId)
                    .HasConstraintName("game_currentplayerinturnid_foreign");

                entity.HasOne(d => d.HostUser)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.HostUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_hostuserid_foreign");

                entity.HasOne(d => d.PreviousPlayer)
                    .WithMany(p => p.GamePreviousPlayers)
                    .HasForeignKey(d => d.PreviousPlayerId)
                    .HasConstraintName("game_previousplayerid_foreign");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_statusid_foreign");

                entity.HasOne(d => d.Winner)
                    .WithMany(p => p.GameWinners)
                    .HasForeignKey(d => d.WinnerId)
                    .HasConstraintName("game_winnerid_foreign");
            });

            modelBuilder.Entity<GameStatus>(entity =>
            {
                entity.ToTable("GameStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatRoomId).HasColumnName("ChatRoomID");

                entity.Property(e => e.FromId).HasColumnName("FromID");

                entity.Property(e => e.Message1)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Message");

                entity.Property(e => e.TimeSent).HasColumnType("datetime");

                entity.HasOne(d => d.ChatRoom)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_chatroomid_foreign");

                entity.HasOne(d => d.From)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.FromId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_fromid_foreign");
            });

            modelBuilder.Entity<MoversInGame>(entity =>
            {
                entity.ToTable("MoversInGame");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndPosId).HasColumnName("EndPosID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.NextPosId).HasColumnName("NextPosID");

                entity.Property(e => e.StartPosId).HasColumnName("StartPosID");

                entity.HasOne(d => d.EndPos)
                    .WithMany(p => p.MoversInGameEndPos)
                    .HasForeignKey(d => d.EndPosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("moversingame_endposid_foreign");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.MoversInGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("moversingame_gameid_foreign");

                entity.HasOne(d => d.NextPos)
                    .WithMany(p => p.MoversInGameNextPos)
                    .HasForeignKey(d => d.NextPosId)
                    .HasConstraintName("moversingame_nextposid_foreign");

                entity.HasOne(d => d.StartPos)
                    .WithMany(p => p.MoversInGameStartPos)
                    .HasForeignKey(d => d.StartPosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("moversingame_startposid_foreign");
            });

            modelBuilder.Entity<PlayersInGame>(entity =>
            {
                entity.ToTable("PlayersInGame");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatRoomId).HasColumnName("ChatRoomID");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.CurrentPosId).HasColumnName("CurrentPosID");

                entity.Property(e => e.EnterTime).HasColumnType("datetime");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.LastMoveTime).HasColumnType("datetime");

                entity.Property(e => e.ProfilePic)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.ChatRoom)
                    .WithMany(p => p.PlayersInGames)
                    .HasForeignKey(d => d.ChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersingame_chatroomid_foreign");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.PlayersInGames)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersingame_colorid_foreign");

                entity.HasOne(d => d.CurrentPos)
                    .WithMany(p => p.PlayersInGames)
                    .HasForeignKey(d => d.CurrentPosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersingame_currentposid_foreign");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.PlayersInGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersingame_gameid_foreign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PlayersInGames)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersingame_userid_foreign");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.X).HasColumnName("x");

                entity.Property(e => e.Y).HasColumnName("y");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "user_email_unique")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "user_username_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProfilePic)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
