using AgileTrackingSystem.Data;
using AgileTrackingSystem.Services;
using AgileTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AgileTrackingSystem.Controllers.App
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDBRepository _dBRepository;
        
        public AppController(IMailService mailService, IDBRepository dBRepository)
        {
            _mailService = mailService;
            _dBRepository = dBRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {            
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("test@address.com", model.Subject, model.Message);
                ViewBag.UserMessage = "Mail sent.";
                ModelState.Clear();
            }
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        
        public IActionResult Shop()
        {
            var results = _dBRepository.GetAllProducts();

            return View(results);
        }
    }
}