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
            return View(data.Films.Where(n => n.Ten.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
        }

        //
        // Chi tiết film
        public ActionResult ChiTietFilm(int id)
        {
            var film = from f in data.Films where f.Id == id select f;
            return View(film.SingleOrDefault());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}