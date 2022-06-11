using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeNerd_Film.Models;
using PagedList;

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

        // Logout
        public ActionResult Logout()
        {
            Session.Clear(); // xoá session
            return RedirectToAction("Login");
        }

        //
        ///// Thực hiện các chức năng quản lý
        //

        // 1. Hiện thị danh sách film
        public ActionResult Film(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 10;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Films.ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
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
                Film film = data.Films.SingleOrDefault(n => n.Id == id);
                return View(film);
            }
        }
        [HttpPost, ActionName("ChinhSuaFilm")]
        public ActionResult XacNhanSuaFilm(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Film film = data.Films.SingleOrDefault(n => n.Id == id);
                UpdateModel(film);
                data.SaveChanges();
                return RedirectToAction("Film");
            }
        }

        //
        // 6. Hiện thị danh sách thể loại film
        public ActionResult TheLoai(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.The_Loai.ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
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
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var ten = collection["ten"];
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorTTL"] = "Không được để trống";
                }
                else if (ten != null)
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
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                The_Loai t = data.The_Loai.SingleOrDefault(n => n.Id == id);
                var ten = collection["ten"];
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorCSTL"] = "Không được để trống";
                }
                else if (ten != null)
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
        public ActionResult QuocGia(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Quoc_Gia.ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
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
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var ten = collection["ten"];
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorQG"] = "Không được để trống";
                }
                else if (ten != null)
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
                var sach = from t in data.The_Loai where t.Id == id select t;
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
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Quoc_Gia qg = data.Quoc_Gia.SingleOrDefault(n => n.Id == id);
                var ten = collection["ten"];
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["ErrorCSQG"] = "Không được để trống";
                }
                else if (ten != null)
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

        //---Get link image---
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