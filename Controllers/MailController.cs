using DemoTest.BL.Helper;
using DemoTest.BL.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
namespace DemoTest.Controllers
{
    public class MailController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMail(MailVM model)
        {
            try
            {

               SendMaillHelpaer.MailSender(model);
        
                return View(model);



            }catch(Exception ex)
            {
                SendMaillHelpaer.MailSender(model);
               
                return View (model);
            }
        
          
        }
    }
}
