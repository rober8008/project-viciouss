using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ctaWEB.Models;
using ctaWEB.Models.AdminModels;
using ctaSERVICES;
using ctaCOMMON.Charts;
using ctaCOMMON.DataParser;
using ctaSERVICES.Reporting;

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminStockQuotesController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();

        [HttpPost]
        public string ClearIntradiary(string market_name)
        {
            try
            {
                QuotesService.ClearIntradiaryDataByMarketID(market_name);
            }
            catch (Exception ex)
            {
                return "ERROR Deleting Intradiary:\n " + ex.Message;
            }
            return "Intradiary Deleted";
        }

        [HttpPost]
        public ActionResult ReadDataFromCSV(HttpPostedFileBase csvdatafile)
        {
            QuotesService.ReadHistoricalDataFromCSV(csvdatafile.InputStream, csvdatafile.FileName);
            return RedirectToAction("Index"); 
        }

        [AllowAnonymous]
        [HttpPost]
        public string RunDailyReportUpdate(string batch)
        {
            string result = "";

            try
            {                
                ReportData_Generator generator = new ReportData_Generator();
                generator.GenerateReportData(batch);                    
            }
            catch (Exception ex)
            {
                result += "\nError: " + ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ctaCOMMON.EmailSender.SendErrorUpdatingReport(result, DateTime.Now);
            }

            return result += "\nReport Updated";
        }

        [HttpPost]
        public string DeleteReportData()
        {
            try
            {
                ReportData_Generator generator = new ReportData_Generator();
                generator.DeleteReportData();
            }
            catch (Exception ex)
            {
                return "ERROR Deleting Report Data:\n " + ex.Message;
            }
            return "Todays Report Data Deleted";
        }

        [AllowAnonymous]
        [HttpPost]        
        public string RunIntradiaryUpdate(string market_name)
        {
            string result = "";
            //foreach (var market in MarketService.GetMarkets().Where(m => m.name.ToLower() == market_name.ToLower() || market_name.ToUpper() == "ALL"))
            //{
            //    bool error = false;
            //    int utc_offset = ConfigService.GetConfigs().Where(c => c.ConfigName == market.name + "_UTCOffset").Select(c => int.Parse(c.ConfigValue)).FirstOrDefault();
            //    result += market.name.ToUpper() + "\n" + QuotesService.ReadMarketIntradiaryData(market.name, market.Id, utc_offset, out error) + "\n\n";

            //    //Update SyncTime
            //    QuotesService.UpdateQuotesSyncNextTime(market.name, DateTime.UtcNow.AddHours(utc_offset), error);
            //}
            return result;
        }

        [HttpPost]
        public string RunHistoricalUpdate(int stockid, string startdate, string enddate)
        {
            string result = "";
            if (startdate != "" && enddate != "" && stockid != 0)
            {
                try
                {
                    Symbol symbol = QuotesService.GetSymbolInfo(stockid);
                    DateTime start = DateTime.Parse(startdate);
                    DateTime end = DateTime.Parse(enddate);
                    result += symbol.Symbol_Name.ToUpper() + "\nStartDate: " + startdate + "\nEndDate: " + enddate;
                    QuotesService.ReadHistoricalData(start, end, symbol.Symbol_Name, symbol.Symbol_ID);
                }
                catch (Exception ex)
                {
                    return result += "\nError: " + ex.Message;
                }
            }
            return result += "\nHistorical Updated";
        }

        // GET: AdminStockQuotes
        public ActionResult Index(int stockid = 0)
        {
            ViewBag.stock_id = new SelectList(db.GetStocks().Where(s => s.active && s.market_id != 1).OrderBy(s => s.symbol), "Id", "symbol_market");
            ViewBag.SyncMarketInfo = QuotesService.GetMarketsSyncTimeInfo();
            return View(db.GetStockQuotes(stockid).OrderByDescending(s => s.date_round));
        }        

        // GET: AdminStockQuotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminStockQuotesModel adminStockQuotesModel = db.GetStockQuotes().Where(s => s.Id == id).FirstOrDefault();            

            if (adminStockQuotesModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockQuotesModel);
        }

        // GET: AdminStockQuotes/Create
        public ActionResult Create()
        {
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol_market");
            return View();
        }

        // POST: AdminStockQuotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,stock_id,opening,closing,minimun,maximun,volume,date_round,adj_close")] AdminStockQuotesModel adminStockQuotesModel)
        {
            if (ModelState.IsValid)
            {
                //db.AdminStockQuotesModels.Add(adminStockQuotesModel);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol_market", adminStockQuotesModel.stock_id);
            return View(adminStockQuotesModel);
        }

        // GET: AdminStockQuotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStockQuotesModel adminStockQuotesModel = db.GetStockQuotes().Where(s => s.Id == id).FirstOrDefault();
            if (adminStockQuotesModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol_market", adminStockQuotesModel.stock_id);
            return View(adminStockQuotesModel);
        }

        // POST: AdminStockQuotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,stock_id,opening,closing,minimun,maximun,volume,date_round,adj_close")] AdminStockQuotesModel adminStockQuotesModel)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(adminStockQuotesModel).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol_market", adminStockQuotesModel.stock_id);
            return View(adminStockQuotesModel);
        }

        // GET: AdminStockQuotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStockQuotesModel adminStockQuotesModel = db.GetStockQuotes().Where(s => s.Id == id).FirstOrDefault();
            if (adminStockQuotesModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockQuotesModel);
        }

        // POST: AdminStockQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminStockQuotesModel adminStockQuotesModel = db.GetStockQuotes().Where(s => s.Id == id).FirstOrDefault();
            //db.AdminStockQuotesModels.Remove(adminStockQuotesModel);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
