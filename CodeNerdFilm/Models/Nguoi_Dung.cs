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

        [StringLength(10)]
        public string Ho_Ten { get; set; }

        [Required]
        [StringLength(256)]
        public string Ten_Dang_Nhap { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string Mat_Khau { get; set; }

        [Required]
        [StringLength(256)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [StringLength(15)]
        public string Dien_Thoai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Sinh { get; set; }

        public int Quyen { get; set; }

        public virtual Quyen Quyen1 { get; set; }
    }
}
