﻿using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
