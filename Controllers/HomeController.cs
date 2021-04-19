using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Assignment4; 
//using Assignment4.Models;
using static Assignment4.Models.EF_Models;
using Assignment4.Models;
using Assignment4.DataAccess;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Assignment4
{
    public class HomeController : Controller
    {
        HttpClient httpClient;



        static string BASE_URL = "https://api.usa.gov/crime/fbi/sapi";
        static string API_KEY = "iiHnOKfno2Mgkt5AynpvPpUQTEyxE77jo1RU8PIv"; //Add your API key here inside ""



        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.



        public IActionResult mainpage()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



            string NATIONAL_PARK_API_PATH = BASE_URL + "/api/summarized/state/TX/burglary/2009/2019";
            string parksData = "";



            Rootobject root = null;



            httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);



            try
            {
                HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH).GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }



                if (!parksData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    root = JsonConvert.DeserializeObject<Rootobject>(parksData);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }



            return View(root);
        }


        
        public IActionResult aboutUs()
        {
            return View();
        }
        public IActionResult create()
        {
            return View();
        }
        public IActionResult update()
        {
            return View();
        }
        public IActionResult delete()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        //public IActionResult Explore()
        //{
        //    return View();
        //}

        //public IActionResult RegisteredUsers()
        //{
        //    IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
        //    return View(allUsers);
        //}

        //public IActionResult DeleteUser(string email)
        //{
        //    SignUp user = applicationDbContext.SignUp.Find(email);
        //    if (user != null)
        //    {
        //        applicationDbContext.SignUp.Remove(user);
        //        applicationDbContext.SaveChanges();
        //    }
        //    IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
        //    return View("RegisteredUsers",allUsers);
        //}

        //public IActionResult UpdateUser(string email)
        //{
        //    ViewBag.email = email;
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult UpdateUser(SignUp userChanges)
        //{
        //    var user = applicationDbContext.Attach(userChanges);
        //    user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    applicationDbContext.SaveChanges();
        //    IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
        //    return View("RegisteredUsers", allUsers);
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

    }
    
}

