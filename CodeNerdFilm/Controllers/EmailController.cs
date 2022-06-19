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
        public ActionResult SendMail1()
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

            NetworkCredential networkCredential = new NetworkCredential("nhatnguyenktz119@gmail.com", "gmailpassword");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            ViewBag.Message = "Mail đã được gửi thành công!";
            return View();
        }
    }
}