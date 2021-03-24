using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApp_OpenIDConnect_DotNet.Models;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //using (var reader = new StreamReader(Request.Body))
            //{
            //    var body = reader.ReadToEnd();
            //    //string logOutput = "<p>This is a test</p><br/><script>console.log(\"" + body + "\");</script>";
            //    byte[] bytes = Encoding.ASCII.GetBytes(body);
            //    ViewData["ReqBody"] = Encoding.UTF8.GetString(bytes);
            //}           

            if (TempData.ContainsKey("ReqBody"))
            {
                ViewData["ReqBody"] = TempData["LoginReqBody"] as string;
            }

            //if (TempData.ContainsKey("RespBody"))
            //{
            //    string reqBodyFromTemp = TempData["LoginRespBody"] as string;
            //    ViewData["RespBody"] = reqBodyFromTemp;
            //}

            ViewData["TestVar"] = "This is just a test for view data"; // works

            ViewData["TEST"] = TempData["TEST"] as string;
            ViewData["IDToken"] = TempData["IDToken"] as string;
            ViewData["PostTrigger"] = TempData["PostTrigger"] as string;
            ViewData["FormData"] = TempData["FormData"] as string;
            

            return View();
        }

        public async Task<IActionResult> GetIDToken()
        {
            // /Home/GetIDToken

            string idToken = await HttpContext.GetTokenAsync("id_token"); // this returns the id token

            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            //var content = await client.GetStringAsync("https://login.microsoftonline.com/0721e9db-564b-4941-ac5e-2447794ec2b3/oauth2/v2.0/authorize");

            ViewData["IDToken"] = idToken;

            return View("IDTokenRedirect");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
