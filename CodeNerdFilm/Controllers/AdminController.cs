using CodeNerdFilm.Models;
using PagedList;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CodeNerd_Film.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        // Tạo đối tượng data chứa dữ liệu từ model đã tạo. 
        CodeNerdFilmDBContext data = new CodeNerdFilmDBContext();

        //
        // Trang Index
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }

        //
        // Đăng nhập tài khoản admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
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
                var ad = data.Nguoi_Dung.SingleOrDefault(n => n.Ten_Dang_Nhap == Ten_Dang_Nhap && n.Mat_Khau == Mat_Khau && n.Quyen == 1);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
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
        ///// Thực hiện các chức năng quản lý
        //

        // 1. Hiện thị danh sách film
        public ActionResult Film(int? page, string search)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 10;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Films.Where(n => n.Ten.Contains(search) || n.Tieu_De.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
            }
        }

        //
        // 2. Thêm mới 1 bộ film: Hiện thị view để thêm mới, sau đó Lưu 
        [HttpGet]
        public ActionResult ThemFilm()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Film f = new Film();

                // Get list quốc gia
                f.DSQuocGia = data.Quoc_Gia.ToList();
                // Get list thể loại
                f.DSTheLoai = data.The_Loai.ToList();
                // Get list chủng phim
                f.DSChungFilm = data.Chung_Film.ToList();
                return View(f);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemFilm(Film f, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var Hinh = collection["Hinh"];
                if (!ModelState.IsValid)
                {
                    f.DSQuocGia = data.Quoc_Gia.ToList();
                    f.DSTheLoai = data.The_Loai.ToList();
                    f.DSChungFilm = data.Chung_Film.ToList();
                    return View("ThemFilm", f);
                }
                else
                {
                    f.Hinh = Hinh.ToString();
                    //Luu vao CSDL
                    data.Films.Add(f);
                    data.SaveChanges();
                    return RedirectToAction("Film", "Admin");
                }

            }
        }

        //
        // 3. Xem chi tiết film
        public ActionResult ChiTietFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var film = from f in data.Films where f.Id == id select f;
                return View(film.SingleOrDefault());
            }
        }

        //
        // 4. Xoá film
        [HttpGet]
        public ActionResult XoaFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from f in data.Films where f.Id == id select f;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaFilm")]
        public ActionResult XacNhanXoaFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Film film = data.Films.SingleOrDefault(n => n.Id == id);
                data.Films.Remove(film);
                data.SaveChanges();
                return RedirectToAction("Film");
            }
        }

        //
        // 5. Điều chỉnh thông tin Film
        public ActionResult ChinhSuaFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Film f = data.Films.SingleOrDefault(n => n.Id == id);
                // Get list quốc gia
                f.DSQuocGia = data.Quoc_Gia.ToList();
                // Get list thể loại
                f.DSTheLoai = data.The_Loai.ToList();
                // Get list chủng phim
                f.DSChungFilm = data.Chung_Film.ToList();
                return View(f);
            }
        }
        [HttpPost, ActionName("ChinhSuaFilm")]
        public ActionResult XacNhanSuaFilm(int id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Film f = data.Films.SingleOrDefault(n => n.Id == id);
                var Hinh = collection["Hinh"];
                if (!ModelState.IsValid)
                {
                    f.DSQuocGia = data.Quoc_Gia.ToList();
                    f.DSTheLoai = data.The_Loai.ToList();
                    f.DSChungFilm = data.Chung_Film.ToList();
                    return View("ChinhSuaFilm", f);
                }
                else
                {
                    f.Hinh = Hinh.ToString();
                    //Luu vao CSDL
                    UpdateModel(f);
                    data.SaveChanges();
                    return RedirectToAction("Film");
                }
            }
        }

        //
        // 6. Hiện thị danh sách thể loại film
        public ActionResult TheLoai(int? page, string search)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.The_Loai.Where(n => n.Ten.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
            }
        }

        //
        // 7. Thêm mới 1 thể loại film: Hiện thị view để thêm mới, sau đó Lưu 
        [HttpGet]
        public ActionResult ThemTheLoaiFilm()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemTheLoaiFilm(FormCollection collection, The_Loai t)
        {
            var ten = collection["ten"];

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var check = data.The_Loai.SingleOrDefault(x => x.Ten == ten);
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorTTL"] = "Không được để trống";
                }
                else if (check != null)
                {
                    ViewData["ErrorTTL"] = "Thể loại này đã có";
                }
                else
                {
                    t.Ten = ten.ToString();

                    data.The_Loai.Add(t);
                    data.SaveChanges();
                    return RedirectToAction("TheLoai");
                }
                return this.ThemTheLoaiFilm();
            }
        }

        //
        // 8. Xoá thể loại film
        [HttpGet]
        public ActionResult XoaTheLoaiFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from t in data.The_Loai where t.Id == id select t;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaTheLoaiFilm")]
        public ActionResult XacNhanXoaTheLoai(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                The_Loai tl = data.The_Loai.SingleOrDefault(n => n.Id == id);
                data.The_Loai.Remove(tl);
                data.SaveChanges();
                return RedirectToAction("TheLoai");
            }
        }

        //
        // 9. Điều chỉnh thông tin thể loại film
        public ActionResult ChinhSuaTheLoaiFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                The_Loai t = data.The_Loai.SingleOrDefault(n => n.Id == id);
                return View(t);
            }
        }
        [HttpPost]
        public ActionResult ChinhSuaTheLoaiFilm(int id, FormCollection collection)
        {
            var ten = collection["ten"];

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                The_Loai t = data.The_Loai.SingleOrDefault(n => n.Id == id);
                var check = data.The_Loai.SingleOrDefault(x => x.Ten == ten);
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorCSTL"] = "Không được để trống";
                }
                else if (check != null)
                {
                    ViewData["ErrorCSTL"] = "Thể loại này đã có";
                }
                else
                {
                    t.Ten = ten.ToString();
                    UpdateModel(t);
                    data.SaveChanges();
                    return RedirectToAction("TheLoai");
                }
                return this.ChinhSuaTheLoaiFilm(id);
            }
        }

        //
        // 10. Hiện thị danh sách quốc gia
        public ActionResult QuocGia(int? page, string search)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Quoc_Gia.Where(n => n.Ten.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
            }
        }

        //
        // 11. Thêm mới 1 quốc gia: Hiện thị view để thêm mới, sau đó Lưu 
        [HttpGet]
        public ActionResult ThemQuocGia()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemQuocGia(FormCollection collection, Quoc_Gia q)
        {
            var ten = collection["ten"];
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var check = data.Quoc_Gia.SingleOrDefault(x => x.Ten == ten);
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorQG"] = "Không được để trống";
                }
                else if (check != null)
                {
                    ViewData["ErrorQG"] = "Quốc gia này đã tồn tại";
                }
                else
                {
                    q.Ten = ten.ToString();

                    data.Quoc_Gia.Add(q);
                    data.SaveChanges();
                    return RedirectToAction("QuocGia");
                }
                return this.ThemQuocGia();
            }
        }

        //
        // 12. Xoá quốc gia
        [HttpGet]
        public ActionResult XoaQuocGia(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from t in data.Quoc_Gia where t.Id == id select t;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaQuocGia")]
        public ActionResult XacNhanXoaQuocGia(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Quoc_Gia qg = data.Quoc_Gia.SingleOrDefault(n => n.Id == id);
                data.Quoc_Gia.Remove(qg);
                data.SaveChanges();
                return RedirectToAction("QuocGia");
            }
        }

        //
        // 13. Điều chỉnh thông tin quốc gia
        public ActionResult ChinhSuaQuocGia(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Quoc_Gia qg = data.Quoc_Gia.SingleOrDefault(n => n.Id == id);
                return View(qg);
            }
        }
        [HttpPost]
        public ActionResult ChinhSuaQuocGia(int id, FormCollection collection)
        {
            var ten = collection["ten"];

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Quoc_Gia qg = data.Quoc_Gia.SingleOrDefault(n => n.Id == id);
                var check = data.Quoc_Gia.SingleOrDefault(x => x.Ten == ten);
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorCSQG"] = "Không được để trống";
                }
                else if (check != null)
                {
                    ViewData["ErrorCSQG"] = "Quốc gia này đã tồn tại";
                }
                else
                {
                    qg.Ten = ten.ToString();
                    UpdateModel(qg);
                    data.SaveChanges();
                    return RedirectToAction("QuocGia");
                }
                return this.ChinhSuaQuocGia(id);
            }
        }

        // 14. Hiện thị danh sách trailer
        public ActionResult Trailer(int? page, string search)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 10;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Trailers.Where(n => n.Ten.Contains(search) || n.Tieu_De.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
            }
        }

        //
        // 15. Thêm mới 1 video trailer: Hiện thị view để thêm mới, sau đó Lưu 
        [HttpGet]
        public ActionResult ThemTrailer()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Trailer t = new Trailer();

                // Get list quốc gia
                t.DSQuocGia = data.Quoc_Gia.ToList();
                // Get list thể loại
                t.DSTheLoai = data.The_Loai.ToList();
                // Get list chủng phim
                t.DSChungFilm = data.Chung_Film.ToList();
                return View(t);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemTrailer(Trailer t, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var Hinh = collection["Hinh"];
                if (!ModelState.IsValid)
                {
                    t.DSQuocGia = data.Quoc_Gia.ToList();
                    t.DSTheLoai = data.The_Loai.ToList();
                    t.DSChungFilm = data.Chung_Film.ToList();
                    return View("ThemTrailer", t);
                }
                else
                {
                    t.Hinh = Hinh.ToString();
                    //Luu vao CSDL
                    data.Trailers.Add(t);
                    data.SaveChanges();
                    return RedirectToAction("Trailer", "Admin");
                }

            }
        }

        //
        // 3. Xem chi tiết trailer
        public ActionResult ChiTietTrailer(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var film = from f in data.Trailers where f.Id == id select f;
                return View(film.SingleOrDefault());
            }
        }

        //
        // 4. Xoá trailer
        [HttpGet]
        public ActionResult XoaTrailer(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var trailers = from f in data.Trailers where f.Id == id select f;
                return View(trailers.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaTrailer")]
        public ActionResult XacNhanXoaTrailer(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Trailer trailer = data.Trailers.SingleOrDefault(n => n.Id == id);
                data.Trailers.Remove(trailer);
                data.SaveChanges();
                return RedirectToAction("Trailer");
            }
        }

        //
        // 5. Điều chỉnh thông tin trailer
        public ActionResult ChinhSuaTrailer(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Trailer t = data.Trailers.SingleOrDefault(n => n.Id == id);

                // Get list quốc gia
                t.DSQuocGia = data.Quoc_Gia.ToList();
                // Get list thể loại
                t.DSTheLoai = data.The_Loai.ToList();
                // Get list chủng phim
                t.DSChungFilm = data.Chung_Film.ToList();
                return View(t);
            }
        }
        [HttpPost, ActionName("ChinhSuaTrailer")]
        public ActionResult XacNhanSuaTrailer(int id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Trailer t = data.Trailers.SingleOrDefault(n => n.Id == id);

                var Hinh = collection["Hinh"];
                if (!ModelState.IsValid)
                {
                    t.DSQuocGia = data.Quoc_Gia.ToList();
                    t.DSTheLoai = data.The_Loai.ToList();
                    t.DSChungFilm = data.Chung_Film.ToList();
                    return View("ChinhSuaTrailer", t);
                }
                else
                {
                    t.Hinh = Hinh.ToString();
                    //Lưu vào CSDL
                    UpdateModel(t);
                    data.SaveChanges();
                    return RedirectToAction("Trailer");
                }
            }
        }

        //
        // Get link ảnh
        public String ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Images/" + file.FileName));
            return "/Content/Images/" + file.FileName;
        }
    }
}