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

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminStocksController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();

        // GET: AdminStocks
        public ActionResult Index()
        {
            return View(db.GetStocks(true));
        }

        // GET: AdminStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStocksModel adminStockModel = db.GetStocks().Where(s => s.Id == id).FirstOrDefault();
            if (adminStockModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockModel);
        }

        // GET: AdminStocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,symbol,name,market_id,type_id,description,active")] AdminStocksModel adminStockModel)
        {
            if (ModelState.IsValid && !db.GetStocks().Any(s => s.market_id == adminStockModel.market_id && s.symbol == adminStockModel.symbol))
            {
                db.SaveNewStock(adminStockModel);
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid Stock Data";
            return View(adminStockModel);
        }

        // GET: AdminStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStocksModel adminStockModel = db.GetStocks().Where(s => s.Id == id).FirstOrDefault();
            if (adminStockModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockModel);
        }

        // POST: AdminStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,symbol,name,market_id,type_id,description,active")] AdminStocksModel adminStockModel)
        {
            if (ModelState.IsValid && !db.GetStocks().Any(s => s.market_id == adminStockModel.market_id && s.symbol == adminStockModel.symbol && s.Id != adminStockModel.Id))
            {
                db.UpdateStock(adminStockModel);
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid Stock Data";
            return View(adminStockModel);
        }

        // GET: AdminStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStocksModel adminStockModel = db.GetStocks().Where(s => s.Id == id).FirstOrDefault();
            if (adminStockModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockModel);
        }

        // POST: AdminStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminStocksModel adminStockModel = db.GetStocks().Where(s => s.Id == id).FirstOrDefault();            
            //db.DeleteStock(adminStockModel);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
    }
}
