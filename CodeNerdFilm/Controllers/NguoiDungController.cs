using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeNerdFilm.Models;
using Scrypt;
using PagedList;

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
                int pagesize = 10;
                // Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
                int pagenum = (page ?? 1);
                return View(data.Films.Where(n => n.Ten.Contains(search) || search == null).ToList().OrderByDescending(n => n.Id).ToPagedList(pagenum, pagesize));
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

            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var matkhaunhaplai = collection["matkhaunhaplai"];
            var email = collection["email"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);

            var check = data.Nguoi_Dung.FirstOrDefault(s => s.Ten_Dang_Nhap == _user.Ten_Dang_Nhap);
            if (check == null)
            {
                if (!matkhau.Equals(matkhaunhaplai))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và nhập lại mật khẩu phải giống nhau";
                }
                _user.Ho_Ten = hoten;
                _user.Ten_Dang_Nhap = tendangnhap;
                _user.Mat_Khau = encoder.Encode(matkhau);
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
    }
}