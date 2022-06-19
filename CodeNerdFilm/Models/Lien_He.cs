namespace CodeNerdFilm.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Lien_He
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Noi_Dung { get; set; }

        [StringLength(256)]
        public string Email { get; set; }
    }
}
