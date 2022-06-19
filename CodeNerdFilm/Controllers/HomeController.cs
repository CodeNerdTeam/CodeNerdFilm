using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeNerdFilm.Models;
using PagedList;

namespace CodeNerdFilm.Controllers
{
    public class HomeController : Controller
    {
        // Tạo đối tượng data chứa dữ liệu từ model đã tạo. 
        CodeNerdFilmDBContext data = new CodeNerdFilmDBContext();

        // Trang Index
        public ActionResult Index(int? page, string search)
        {
            // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 10;
            // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
            int pagenum = (page ?? 1);
            return View(data.Films.Where(n => n.Ten.Contains(search) || n.Tieu_De.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
        }

        // Trang trailer
        public ActionResult Trailer(int? page)
        {
            if (page == null) page = 1;
            var trailers = (from s in data.Trailers select s).OrderBy(x => x.Id);
            int pageSize = 10;
            int pageNum = page ?? 1;
            return View(trailers.ToPagedList(pageNum, pageSize));
        }

        // Trang film lẻ
        public ActionResult FilmLe(int? page)
        {
            if (page == null) page = 1;
            var films = (from s in data.Films select s).OrderBy(x => x.Chung_Film_Id == 1);
            int pageSize = 10;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // Chi tiết film
        public ActionResult ChiTietFilm(int id)
        {
            var film = from f in data.Films where f.Id == id select f;
            return View(film.SingleOrDefault());
        }

        //
        // Chi tiết trailer
        public ActionResult ChiTietTrailer(int id)
        {
            var trailers = from f in data.Trailers where f.Id == id select f;
            return View(trailers.SingleOrDefault());
        }

        //
        // Trang xem film
        public ActionResult XemFilm(int id)
        {
            var film = from f in data.Films where f.Id == id select f;
            return View(film.SingleOrDefault());
        }

        //
        // Trang xem trailer
        public ActionResult XemTrailer(int id)
        {
            var trailers = from f in data.Trailers where f.Id == id select f;
            return View(trailers.SingleOrDefault());
        }

        //
        // Trang xem trailer từ trang film
        public ActionResult Xem_Trailer(int id)
        {
            var films = from f in data.Films where f.Id == id select f;
            return View(films.SingleOrDefault());
        }
    }
}