using CodeNerdFilm.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

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
            int pagesize = 12;
            // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
            int pagenum = (page ?? 1);
            return View(data.Films.Where(n => n.Ten.Contains(search) || n.Tieu_De.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
        }

        // Trang trailer
        public ActionResult Trailer(int? page)
        {
            if (page == null) page = 1;
            var trailers = (from s in data.Trailers select s).OrderBy(x => x.Id);
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(trailers.ToPagedList(pageNum, pageSize));
        }

        // Trang film lẻ
        public ActionResult FilmLe(int? page)
        {
            if (page == null) page = 1;
            var films = (from s in data.Films select s).OrderBy(x => x.Chung_Film_Id == 1);
            int pageSize = 12;
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

        //
        // Quốc gia
        //

        // 1. Trung Quốc
        public ActionResult QGTrungQuoc(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 6).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 2. Thái Lan
        public ActionResult QGThaiLan(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 5).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 3. Mỹ
        public ActionResult QGMy(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 1).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 4. Anh
        public ActionResult QGChauAu(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 9).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        // 5. Nhật Bản
        public ActionResult QGNhatBan(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 8).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        // 6. Hàn Quốc
        public ActionResult QGHanQuoc(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.Quoc_Gia_Id == 4).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // Thể loại
        //

        // 1. Hành động
        public ActionResult TLHanhDong(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.The_Loai_Id == 1).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 2. Kinh dị
        public ActionResult TLKinhDi(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.The_Loai_Id == 3).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        // 3. Viễn tưởng
        public ActionResult TLVienTuong(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.The_Loai_Id == 8).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 4. Hoạt hình
        public ActionResult TLHoatHinh(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.The_Loai_Id == 7).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }

        //
        // 5. Cổ trang
        public ActionResult TLCoTrang(int? page)
        {
            if (page == null) page = 1;
            var films = data.Films.Where(x => x.The_Loai_Id == 10).ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(films.ToPagedList(pageNum, pageSize));
        }
    }
}