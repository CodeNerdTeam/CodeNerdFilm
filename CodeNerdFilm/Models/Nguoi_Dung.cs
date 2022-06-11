namespace CodeNerdFilm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Nguoi_Dung
    {
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 3)]
        public string Ho_Ten { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "Tên ??ng nh?p ph?i ch?a ít nh?t 4 ký t?")]
        public string Ten_Dang_Nhap { get; set; }

        public string Mat_Khau { get; set; }

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15, MinimumLength = 10)]
        public string Dien_Thoai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Sinh { get; set; }

        public int Quyen { get; set; }

        public virtual Quyen Quyen1 { get; set; }
    }
}
