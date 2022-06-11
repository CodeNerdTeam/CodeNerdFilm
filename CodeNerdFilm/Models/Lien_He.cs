namespace CodeNerdFilm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lien_He
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Noi_Dung { get; set; }

        [StringLength(256)]
        public string Email { get; set; }
    }
}
