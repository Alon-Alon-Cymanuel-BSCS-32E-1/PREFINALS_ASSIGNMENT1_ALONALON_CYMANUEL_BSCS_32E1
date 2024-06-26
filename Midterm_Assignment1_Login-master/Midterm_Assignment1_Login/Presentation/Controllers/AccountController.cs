﻿using Microsoft.AspNetCore.Mvc;
using Midterm_Assignment1_Login.Interfaces;
using Midterm_Assignment1_Login.Models;
using Midterm_Assignment1_Login.Presentation.ViewModels;
using BCrypt.Net;


namespace Midterm_Assignment1_Login.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        //LOGIN PR
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
             
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

              
                var user = new User { Username = model.Username, Password = hashedPassword };

                _userRepository.CreateUser(user);

        
                return RedirectToAction("Login");
            }

          
            return View(model);
        }




    }
}
