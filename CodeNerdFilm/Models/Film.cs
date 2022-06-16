namespace CodeNerdFilm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Film")]
    public partial class Film
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Trang_Thai { get; set; }

        [StringLength(100)]
        public string Tieu_De { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        public int Quoc_Gia_Id { get; set; }

        [StringLength(50)]
        public string Nam_San_Xuat { get; set; }

        [StringLength(50)]
        public string Thoi_Luong { get; set; }

        public int The_Loai_Id { get; set; }

        public string Noi_Dung { get; set; }

        [StringLength(250)]
        public string Dao_Dien { get; set; }

        [StringLength(250)]
        public string Dien_Vien { get; set; }

        [StringLength(50)]
        public string Cong_Ty_San_Xuat { get; set; }

        public int? Danh_Gia { get; set; }

        [Required]
        [StringLength(250)]
        public string Hinh { get; set; }

        public string Link_Film { get; set; }

        public string Link_Trailer { get; set; }

        public int Chung_Film_Id { get; set; }

        public virtual Chung_Film Chung_Film { get; set; }

        public virtual Quoc_Gia Quoc_Gia { get; set; }

        public virtual The_Loai The_Loai { get; set; }

        // add list th? lo?i
        public List<The_Loai> DSTheLoai = new List<The_Loai>();

        // add list qu?c gia
        public List<Quoc_Gia> DSQuocGia = new List<Quoc_Gia>();

        // add list ch?nng film
        public List<Chung_Film> DSChungFilm = new List<Chung_Film>();
    }
}
