using System;
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
            //using (var reader = new StreamReader(Request.Body))
            //{
            //    var body = reader.ReadToEnd();
            //    byte[] bytes = Encoding.ASCII.GetBytes(body);
            //    TempData["LoginReqBody"] = Encoding.UTF8.GetString(bytes);
            //}

            // get response body?
            //using (var reader = new StreamReader(Response.Body))
            //{
            //    var body = reader.ReadToEnd();
            //    byte[] bytes = Encoding.ASCII.GetBytes(body);
            //    TempData["LoginRespBody"] = Encoding.UTF8.GetString(bytes);
            //}

            //TempData.Add("TEST", "Test from Sign in action");
            //TempData.Add("IDToken", Request.Form["id_token"]);
            TempData["TEST"] = "This is a test from sign in action";
            TempData["IDToken"] = Request.Form["id_token"];
            
            TempData.Keep("TEST");

            var redirect = "https://google.com/" + Request.Form["id_token"];

            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirect },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Post([FromForm] string formData)
        {
            TempData["PostTrigger"] = "Tiggered by POST";
            // could this get the form data?
            TempData["FormData"] = Json(formData);

            TempData.Keep("PostTrigger");

            return Json(formData);
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
