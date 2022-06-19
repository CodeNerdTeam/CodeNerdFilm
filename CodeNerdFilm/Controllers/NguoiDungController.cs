using CodeNerdFilm.Models;
using PagedList;
using Scrypt;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CodeNerdFilm.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung

        // Tạo đối tượng data chứa dữ liệu từ model đã tạo. 
        CodeNerdFilmDBContext data = new CodeNerdFilmDBContext();

        //
        // Trang Index sau khi user đã đăng nhập
        public ActionResult Index(int? page, string search)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 12;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Films.Where(n => n.Ten.Contains(search) || n.Tieu_De.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
            }
        }

        // Trang trailer
        public ActionResult Trailer(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var trailers = (from s in data.Trailers select s).OrderBy(x => x.Id);
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(trailers.ToPagedList(pageNum, pageSize));
            }
        }

        // Trang film lẻ
        public ActionResult FilmLe(int? page, int? chungfilm)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Chung_Film_Id == 1).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //GET: Đăng ký
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //POST: Đăng ký (nhận dữ liệu từ trang Register và thực hiện việc tạo mới dữ liệu)
        [HttpPost]
        public ActionResult Register(Nguoi_Dung _user, FormCollection collection)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            var hoten = collection["Ho_Ten"];
            var tendangnhap = collection["Ten_Dang_Nhap"];
            var matkhau = collection["Mat_Khau"];
            var email = collection["Email"];
            var dienthoai = collection["Dien_Thoai"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["Ngay_Sinh"]);

            var check = data.Nguoi_Dung.FirstOrDefault(s => s.Ten_Dang_Nhap == _user.Ten_Dang_Nhap);
            if (check == null)
            {
                _user.Ho_Ten = hoten;
                _user.Ten_Dang_Nhap = tendangnhap;
                _user.Mat_Khau = matkhau;
                _user.Email = email;
                _user.Dien_Thoai = dienthoai;
                _user.Ngay_Sinh = DateTime.Parse(ngaysinh);
                _user.Quyen = 2;
                data.Nguoi_Dung.Add(_user);
                data.SaveChanges();
                return RedirectToAction("Index", "NguoiDung");
            }
            else
            {
                ViewData["ErrorTDN"] = "Tên đăng nhập đã tồn tại";
                return this.Register();
            }
        }

        //
        // Đăng nhập tài khoản người dùng
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection f)
        {
            var Ten_Dang_Nhap = f["Ten_Dang_Nhap"];
            var Mat_Khau = f["Mat_Khau"];

            if (String.IsNullOrEmpty(Ten_Dang_Nhap))
                ViewData["ErrorTDN"] = "Vui lòng nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(Mat_Khau))
                ViewData["ErrorMK"] = "Vui lòng nhập mật khẩu";
            else
            {
                var user = data.Nguoi_Dung.SingleOrDefault(n => n.Ten_Dang_Nhap == Ten_Dang_Nhap && n.Mat_Khau == Mat_Khau && n.Quyen == 2);
                if (user != null)
                {

                    Session["Taikhoannguoidung"] = user;
                    Session["TenNguoiDung"] = user.Ho_Ten;
                    Session["Id"] = user.Id;
                    return RedirectToAction("Index", "NguoiDung");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            }
            return View();
        }

        //
        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear(); // xoá session
            return RedirectToAction("Login");
        }

        //
        // Chi tiết film
        public ActionResult ChiTietFilm(int id)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                var film = from f in data.Films where f.Id == id select f;
                return View(film.SingleOrDefault());
            }
        }

        //
        // Chi tiết trailer
        public ActionResult ChiTietTrailer(int id)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                var trailers = from f in data.Trailers where f.Id == id select f;
                return View(trailers.SingleOrDefault());
            }
        }

        //
        // Quốc gia
        //

        // 1. Trung Quốc
        public ActionResult QGTrungQuoc(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 6).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 2. Thái Lan
        public ActionResult QGThaiLan(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 5).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 3. Mỹ
        public ActionResult QGMy(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 1).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 4. Anh
        public ActionResult QGChauAu(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 9).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        // 5. Nhật Bản
        public ActionResult QGNhatBan(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 8).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        // 6. Hàn Quốc
        public ActionResult QGHanQuoc(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.Quoc_Gia_Id == 4).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }


        //
        // Thể loại
        //

        // 1. Hành động
        public ActionResult TLHanhDong(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.The_Loai_Id == 1).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 2. Kinh dị
        public ActionResult TLKinhDi(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.The_Loai_Id == 3).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        // 3. Viễn tưởng
        public ActionResult TLVienTuong(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.The_Loai_Id == 8).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 4. Hoạt hình
        public ActionResult TLHoatHinh(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.The_Loai_Id == 7).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // 5. Cổ trang
        public ActionResult TLCoTrang(int? page)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                if (page == null) page = 1;
                var films = data.Films.Where(x => x.The_Loai_Id == 10).ToList();
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(films.ToPagedList(pageNum, pageSize));
            }
        }

        //
        // Trang xem film
        public ActionResult XemFilm(int id)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                var film = from f in data.Films where f.Id == id select f;
                return View(film.SingleOrDefault());
            }
        }

        //
        // Trang xem trailer
        public ActionResult XemTrailer(int id)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                var trailers = from f in data.Trailers where f.Id == id select f;
                return View(trailers.SingleOrDefault());
            }
        }

        //
        // Trang xem trailer từ trang film
        public ActionResult Xem_Trailer(int id)
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                var films = from f in data.Films where f.Id == id select f;
                return View(films.SingleOrDefault());
            }
        }

        //
        // Trang thông tin người dùng
        public ActionResult ThongTinNguoiDung()
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                Nguoi_Dung user = (Nguoi_Dung)Session["Taikhoannguoidung"];
                return View(user);
            }
        }


    }
}