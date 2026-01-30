using Microsoft.AspNetCore.Mvc;
using SaravananPortfolio.Models;
using System.Net;
using System.Net.Mail;

namespace SaravananPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Skills() => View();
        public IActionResult Projects() => View();
        public IActionResult PortfolioProject() => View();

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                SendEmail(model);
                ViewBag.Success = "Message sent successfully!";
            }
            return View();
        }

        private void SendEmail(ContactViewModel model)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var password = _configuration["EmailSettings:AppPassword"];

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "New Contact Message",
                Body =
                    $"Name: {model.Name}\n" +
                    $"Email: {model.Email}\n\n" +
                    $"Message:\n{model.Message}",
                IsBodyHtml = false
            };

            mail.To.Add(fromEmail);

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };
            smtp.Send(mail);
        }

        public IActionResult ViewResume()
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "files",
                "Developer_Resume.pdf"
            );

            return PhysicalFile(filePath, "application/pdf");
        }

        public IActionResult DownloadResume()
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "files",
                "Developer_Resume.pdf"
            );

            return PhysicalFile(
                filePath,
                "application/pdf",
                "Developer_Resume.pdf"
            );
        }

    }
}
