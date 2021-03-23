﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            // get request body?
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                //string logOutput = "<p>This is a test</p><br/><script>console.log(\"" + body + "\");</script>";
                byte[] bytes = Encoding.ASCII.GetBytes(body);
                TempData["LoginReqBody"] = Encoding.UTF8.GetString(bytes);
            }

            // get response body?
            using (var reader = new StreamReader(Response.Body))
            {
                var body = reader.ReadToEnd();
                //string logOutput = "<p>This is a test</p><br/><script>console.log(\"" + body + "\");</script>";
                byte[] bytes = Encoding.ASCII.GetBytes(body);
                TempData["LoginRespBody"] = Encoding.UTF8.GetString(bytes);
            }

            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
