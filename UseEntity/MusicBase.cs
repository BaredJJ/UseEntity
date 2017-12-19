namespace UseEntity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MusicBase : DbContext
    {
        public MusicBase( )
            : base("name=MusicBase")
        {
        }

        public virtual DbSet<Album> Albums
        {
            get; set;
        }
        public virtual DbSet<Artist> Artists
        {
            get; set;
        }
        public virtual DbSet<Style> Styles
        {
            get; set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>( )
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Artist>( )
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Artist>( )
                .HasMany(e => e.Albums)
                .WithRequired(e => e.Artist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Artist>( )
                .HasMany(e => e.Styles)
                .WithMany(e => e.Artists)
                .Map(m => m.ToTable("ArtistStyle").MapLeftKey("ArtistId").MapRightKey("StyleId"));

            modelBuilder.Entity<Style>( )
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
