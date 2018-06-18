using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private IMailService mailService;
        private IDutchRepository _dutchRepository;
        

        public AppController(IMailService mailService, IDutchRepository dutchRepository)
        {
            this.mailService = mailService;
            this._dutchRepository = dutchRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet("contact")]// makes a shortcut appear for the tag helper link rendering
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact";
            
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                mailService.SendMail("someome@example.com", model.Subject, $"From {model.Email} Message { model.Message}");
                ViewBag.UserMessage = "Message was sent";
                ModelState.Clear();
            }
            else
            {

            }
            return View();
        }

        public IActionResult AboutUs()
        {
            ViewBag.Title = "AboutUs";
            return View();
        }
        [HttpGet("shop")]
        public IActionResult Shop()
        {
            return View(_dutchRepository.GetProducts().ToList());
        }
    }
}

