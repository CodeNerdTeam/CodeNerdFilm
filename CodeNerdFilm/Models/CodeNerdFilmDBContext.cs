using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CodeNerdFilm.Models
{
    public partial class CodeNerdFilmDBContext : DbContext
    {
        public CodeNerdFilmDBContext()
            : base("name=CodeNerdFilmDBContext")
        {
        }

        public virtual DbSet<Chung_Film> Chung_Film { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Lien_He> Lien_He { get; set; }
        public virtual DbSet<Nguoi_Dung> Nguoi_Dung { get; set; }
        public virtual DbSet<Quoc_Gia> Quoc_Gia { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<The_Loai> The_Loai { get; set; }
        public virtual DbSet<Trailer> Trailers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chung_Film>()
                .HasMany(e => e.Films)
                .WithRequired(e => e.Chung_Film)
                .HasForeignKey(e => e.Chung_Film_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chung_Film>()
                .HasMany(e => e.Trailers)
                .WithRequired(e => e.Chung_Film)
                .HasForeignKey(e => e.Chung_Film_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Nguoi_Dung>()
                .Property(e => e.Dien_Thoai)
                .IsUnicode(false);

            modelBuilder.Entity<Quoc_Gia>()
                .HasMany(e => e.Films)
                .WithRequired(e => e.Quoc_Gia)
                .HasForeignKey(e => e.Quoc_Gia_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quoc_Gia>()
                .HasMany(e => e.Trailers)
                .WithRequired(e => e.Quoc_Gia)
                .HasForeignKey(e => e.Quoc_Gia_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quyen>()
                .HasMany(e => e.Nguoi_Dung)
                .WithRequired(e => e.Quyen1)
                .HasForeignKey(e => e.Quyen)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<The_Loai>()
                .HasMany(e => e.Films)
                .WithRequired(e => e.The_Loai)
                .HasForeignKey(e => e.The_Loai_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<The_Loai>()
                .HasMany(e => e.Trailers)
                .WithRequired(e => e.The_Loai)
                .HasForeignKey(e => e.The_Loai_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
