using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            XmlDocument doc = new XmlDocument();
            List<Amendments> Amendments = new List<Amendments>();
            WebClient proxy = new WebClient();
            string serviceurl = string.Format("https://budget.lis.virginia.gov/BudgetPortalWebService/Budget.Amendments.1.ListOfYearSessionAmendment/");
            byte[] _data = proxy.DownloadData(serviceurl);
            Stream _mem = new MemoryStream(_data);
            var reader = new StreamReader(_mem);
            var result = reader.ReadToEnd();
            XDocument Response = XDocument.Parse(result);
            //DataSet testdataset = new DataSet();
            //testdataset.ReadXml(result);
            XNamespace aw = "http://schemas.datacontract.org/2004/07";
            var Descendants = Response.Descendants("{http://schemas.datacontract.org/2004/07/}AmendmentsYearSessionBillWebService");
            var elemts = Response.Descendants("{http://schemas.datacontract.org/2004/07/}YearsSessionsBillAmendments");
            var model = new List<Amendments>();
            var query = from data in Response.Descendants("{http://schemas.datacontract.org/2004/07/}YearsSessionsBillAmendments")


                        select new Amendments
                        {
                            year = (string)data.Element("{http://schemas.datacontract.org/2004/07/}Year"),
                            SessionType = (string)data.Element("{http://schemas.datacontract.org/2004/07/}SessionType"),
                            SessionDesc = (string)data.Element("{http://schemas.datacontract.org/2004/07/}SessionDesc")
                        };
            //var csFiles = Response.Descendants("{http://schemas.datacontract.org/2004/07/}YearsSessionsBillAmendments")
            //     .Select(f => (string)f.Element("{http://schemas.datacontract.org/2004/07/}Year"))
            //     .ToList();
             Amendments = query.ToList();
            return View(Amendments);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}