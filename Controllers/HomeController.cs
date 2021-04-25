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

        public ApplicationDbContext dbContext;
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

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

            ////To populate our database we inserted four different offenses into the API string:
            ////aggravated-assault, burglary, larceny, and violent-crime.
            string NATIONAL_PARK_API_PATH = BASE_URL + "/api/summarized/state/TX/violent-crime/2009/2019";
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

                if (!dbContext.Results.Where(_ => true).Any())
                {
                    foreach (Result x in root.results)
                    {
                        dbContext.Results.Add(x);
                    }
                    dbContext.SaveChanges();
                }


            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }


            

            return View(root); //root
        }

        // GET: Results
        public async Task<ActionResult> Charts()
        {
            var offen = await dbContext.Results.Select(j => j.offense).Distinct().ToListAsync();
            var oris = await dbContext.Results.Select(j => j.ori).Distinct().ToListAsync();
            var crimes = (dbContext.Results
                
                .Where(j => j.offense == "larceny")
                .Select(j => j.actual)).ToList<int>();
            
            return View(new Charts(oris, crimes));
        }
        public IActionResult aboutUs()
        {
            return View();
        }

        //Details function
        public IActionResult details(string ori)
        {
            var lstResulsts = dbContext.Results.Where(x => x.ori == ori).ToList();

            return View(lstResulsts);
        }
        //Update Function
        public IActionResult update(int ID)
        {
            var currResult = dbContext.Results.Where(x => x.ID == ID).FirstOrDefault();
            return View(currResult);
        }

        [HttpPost]
        public IActionResult update(Result result)
        {
            dbContext.Results.Update(result);
            int count = dbContext.SaveChanges();
            
            return RedirectToAction("details","Home",new { ori = result.ori });
        }

        //Delete function
        public IActionResult delete(int ID)
        {
            var currResult = dbContext.Results.Where(x => x.ID == ID).FirstOrDefault();
            dbContext.Results.Remove(currResult);
            int count = dbContext.SaveChanges();
            if (count > 0)
            {
                return View(1);
            }
            
            return View(0);
        }
        //Index function
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult masterdetail()
        {
            //We need to access the Agencies table here and return in to the masterdetail View
            var agencis = dbContext.Agencies.ToList();
            List<ORI> lstORI = new List<ORI>();
            foreach (var agency in agencis)
            {
                ORI oRI = new ORI();
                oRI.county = agency.county;
                oRI.nameORI = agency.nameORI;
                lstORI.Add(oRI);
            }
            return View(lstORI);
        }
                

    }
    
}

