using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeNerdFilm.Models;
using Scrypt;

namespace CodeNerdFilm.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung

        // Tạo đối tượng data chứa dữ liệu từ model đã tạo. 
        CodeNerdFilmDBContext data = new CodeNerdFilmDBContext();

        public ActionResult Index()
        {
            return View();
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
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorTDN"] = "Tên đăng nhập đã tồn tại";
                return this.Register();
            }
        }
    }
}