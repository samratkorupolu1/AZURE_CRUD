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


            //Manually populate the ORI table, which will be used for a master-detail relationship with Result
            //ORI ori1 = new ORI();
            //ori1.nameORI = "TX0010000";
            //ori1.county = "Anderson";

            //ORI ori2 = new ORI();
            //ori2.nameORI = "TX0010100";
            //ori2.county = "Anderson";

            //ORI ori3 = new ORI();
            //ori3.nameORI = "TX0010300";
            //ori3.county = "Anderson";

            //ORI ori4 = new ORI();
            //ori4.nameORI = "TX0020000";
            //ori4.county = "Andrews";

            //ORI ori5 = new ORI();
            //ori5.nameORI = "TX0020100";
            //ori5.county = "Andrews";

            //ORI ori6 = new ORI();
            //ori6.nameORI = "TX0030000";
            //ori6.county = "Angelina";

            //ORI ori7 = new ORI();
            //ori7.nameORI = "TX0030100";
            //ori7.county = "Angelina";

            //ORI ori8 = new ORI();
            //ori8.nameORI = "TX0030200";
            //ori8.county = "Angelina";

            //ORI ori9 = new ORI();
            //ori9.nameORI = "TX0030400";
            //ori9.county = "Angelina";

            //ORI ori10 = new ORI();
            //ori10.nameORI = "TX0031300";
            //ori10.county = "Angelina";

            //ORI ori11 = new ORI();
            //ori11.nameORI = "TX0040000";
            //ori11.county = "Aransas";

            //ORI ori12 = new ORI();
            //ori12.nameORI = "TX0040100";
            //ori12.county = "Aransas";

            //ORI ori13 = new ORI();
            //ori13.nameORI = "TX0040200";
            //ori13.county = "Aransas";

            //ORI ori14 = new ORI();
            //ori14.nameORI = "TX0050000";
            //ori14.county = "Archer";

            //ORI ori15 = new ORI();
            //ori15.nameORI = "TX0050200";
            //ori15.county = "Archer";

            //ORI ori16 = new ORI();
            //ori16.nameORI = "TX0060000";
            //ori16.county = "Armstrong";

            //ORI ori17 = new ORI();
            //ori17.nameORI = "TX0070000";
            //ori17.county = "Atascosa";

            //ORI ori18 = new ORI();
            //ori18.nameORI = "TX0070100";
            //ori18.county = "Atascosa";

            //ORI ori19 = new ORI();
            //ori19.nameORI = "TX0070200";
            //ori19.county = "Atascosa";

            //ORI ori20 = new ORI();
            //ori20.nameORI = "TX0070300";
            //ori20.county = "Atascosa";

            //dbContext.Agencies.Add(ori1);
            //dbContext.Agencies.Add(ori2);
            //dbContext.Agencies.Add(ori3);
            //dbContext.Agencies.Add(ori4);
            //dbContext.Agencies.Add(ori5);
            //dbContext.Agencies.Add(ori6);
            //dbContext.Agencies.Add(ori7);
            //dbContext.Agencies.Add(ori8);
            //dbContext.Agencies.Add(ori9);
            //dbContext.Agencies.Add(ori10);
            //dbContext.Agencies.Add(ori11);
            //dbContext.Agencies.Add(ori12);
            //dbContext.Agencies.Add(ori13);
            //dbContext.Agencies.Add(ori14);
            //dbContext.Agencies.Add(ori15);
            //dbContext.Agencies.Add(ori16);
            //dbContext.Agencies.Add(ori17);
            //dbContext.Agencies.Add(ori18);
            //dbContext.Agencies.Add(ori19);
            //dbContext.Agencies.Add(ori20);

            //dbContext.SaveChanges();

            return View(root); //root
        }

        // GET: Results
        //public ActionResult Charts()
        public async Task<ActionResult> Charts()
        {
            var offen = await dbContext.Results.Select(j => j.offense).Distinct().ToListAsync();
            var oris = await dbContext.Results.Select(j => j.ori).Distinct().ToListAsync();
            var crimes = (dbContext.Results
                //.GroupBy(j => j.offense)
                .Where(j => j.offense == "larceny")
                .Select(j => j.actual)).ToList<int>();
            //.Select(group => new {
            //    Count = group.Count()
            //});
            //var totcrimes = crimes.Select(a => a.Count).ToArray();
            //ViewBag.Data = Newtonsoft.Json.JsonConvert.SerializeObject(crimes);
            //ViewBag.ObjectName = Newtonsoft.Json.JsonConvert.SerializeObject(oris);
            //return new JsonResult(new { myOffenses = oris, myCrimes = crimes });
           

            return View(new Charts(oris, crimes));
        }
        public IActionResult aboutUs()
        {
            return View();
        }
  
        public IActionResult details(string ori)
        {
            var lstResulsts = dbContext.Results.Where(x => x.ori == ori).ToList();

            return View(lstResulsts);
        }
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

        //public IActionResult Remove(int id)
        //{
        //    Result toDelete = dbContext.Result.Find(id);
        //    // TODO alert confirmation as argument
        //    dbContext.Result.Remove(toDelete);
        //    return View("mainpage");
        //}

        //public async Task<ViewResult> DatabaseOperations()
        //{
        //    Result Result1 = new Result();
        //    Result1.ID = 1;
        //    Result1.ori = "Michael";
        //    Result1.data_year = 2019;
        //    Result1.offense = "Rape";
        //    Result1.state_abbr = "FL";
        //    Result1.actual = 21;
        //    Result1.cleared = 11;

        //    dbContext.Result.Add(Result1);

        //    dbContext.SaveChanges();

        //    return View();
        //}

        

    }
    
}

