using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeNerdFilm.Models;
using System.Net;
using System.Net.Mail;

namespace CodeNerdFilm.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        /*public ActionResult SendMail1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMail1(CodeNerdFilm.Models.Gmail model)
        {
            MailMessage mailMessage = new MailMessage(model.From, "nhatnguyenktz119@gmail.com");
            mailMessage.Subject = model.Subject;
            mailMessage.Body = model.Body;
            mailMessage.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential networkCredential = new NetworkCredential("nhatnguyenktz119@gmail.com", "mulcvehakdunxaep");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            ViewBag.Message = "Email đã đăng ký thành công!";
            return View();
        }*/

        public ActionResult LienHe1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LienHe1(CodeNerdFilm.Models.Gmail model) //CodeNerdFilm.Models.Gmail model
        {
            //MailMessage mailMessage = new MailMessage(model.From, "nhatnguyenktz119@gmail.com");
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add("codenerd.film.201@gmail.com");
            mailMessage.From = new MailAddress("khangdev.it.2001@gmail.com");
            //mailMessage.Headers = model.TextFrom;
            mailMessage.Subject = "Đăng ký nhận thông tin mới nhất từ Code Nerd";
            mailMessage.Body = model.Body;

            /*mailMessage.Subject = Add().Subject;
            mailMessage.Body = model.Body;
            mailMessage.IsBodyHtml = true;*/

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential networkCredential = new NetworkCredential("khangdev.it.2001@gmail.com", "vyrufhmowwycyfjq");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            //SendMail1();
            //SendMail1("codenerd.film.201@gmail.com", model.Subject);
            ViewBag.Message = "Email đã đăng ký thành công!";

            //SendMail1("cc", "codenerd.film.201@gmail.com"  , model.Subject, model.Body);


            return View();
        }

        public ActionResult LienHe2()
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult LienHe2(CodeNerdFilm.Models.Gmail model) //CodeNerdFilm.Models.Gmail model
        {
            if (Session["Taikhoannguoidung"] == null)
                return RedirectToAction("Login", "NguoiDung");
            else
            {
                //MailMessage mailMessage = new MailMessage(model.From, "nhatnguyenktz119@gmail.com");
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("codenerd.film.201@gmail.com");
                mailMessage.From = new MailAddress("khangdev.it.2001@gmail.com");
                //mailMessage.Headers = model.TextFrom;
                mailMessage.Subject = "Đăng ký nhận thông tin mới nhất từ Code Nerd";
                mailMessage.Body = model.Body;

                /*mailMessage.Subject = Add().Subject;
                mailMessage.Body = model.Body;
                mailMessage.IsBodyHtml = true;*/

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                NetworkCredential networkCredential = new NetworkCredential("khangdev.it.2001@gmail.com", "vyrufhmowwycyfjq");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = networkCredential;
                smtpClient.Send(mailMessage);
                //SendMail1();
                //SendMail1("codenerd.film.201@gmail.com", model.Subject);
                ViewBag.Message = "Email đã đăng ký thành công!";

                //SendMail1("cc", "codenerd.film.201@gmail.com"  , model.Subject, model.Body);


                return View();
            }
        }
    }
}