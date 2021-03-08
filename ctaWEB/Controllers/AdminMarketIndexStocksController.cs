using ctaWEB.Models;
using ctaWEB.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminMarketIndexStocksController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();

        // GET: AdminMarketIndex
        public ActionResult Index()
        {
            return View(db.GetMarketIndexStocks());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexStockModel model = db.GetMarketIndexStocks().Where(s => s.Id == id).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.marketindex_id = new SelectList(db.GetMarketIndexes(), "Id", "name");
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol");
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,marketindex_id,stock_id")] AdminMarketIndexStockModel AdminMarketIndexStockModel)
        {
            if (ModelState.IsValid)
            {
                db.SaveNewMarketIndexStock(AdminMarketIndexStockModel);
                return RedirectToAction("Index");
            }

            ViewBag.marketindex_id = new SelectList(db.GetMarketIndexes(), "Id", "name", AdminMarketIndexStockModel.marketindex_id);
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol", AdminMarketIndexStockModel.stock_id);
            return View(AdminMarketIndexStockModel);
        }

        // GET: AdminHolidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexStockModel model = db.GetMarketIndexStocks().Where(s => s.Id == id).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.marketindex_id = new SelectList(db.GetMarketIndexes(), "Id", "name", model.marketindex_id);
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol", model.stock_id);
            return View(model);
        }

        // POST: AdminHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,marketindex_id,stock_id")] AdminMarketIndexStockModel AdminMarketIndexStockModel)
        {
            if (ModelState.IsValid)
            {
                db.UpdateMarketIndexStock(AdminMarketIndexStockModel);
                return RedirectToAction("Index");
            }
            ViewBag.marketindex_id = new SelectList(db.GetMarketIndexes(), "Id", "name", AdminMarketIndexStockModel.marketindex_id);
            ViewBag.stock_id = new SelectList(db.GetStocks(), "Id", "symbol", AdminMarketIndexStockModel.stock_id);
            return View(AdminMarketIndexStockModel);
        }

        // GET: AdminHolidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexStockModel AdminMarketIndexStockModel = db.GetMarketIndexStocks().Where(s => s.Id == id).FirstOrDefault();
            if (AdminMarketIndexStockModel == null)
            {
                return HttpNotFound();
            }
            return View(AdminMarketIndexStockModel);
        }

        // POST: AdminHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminMarketIndexStockModel AdminMarketIndexStockModel = db.GetMarketIndexStocks().Where(s => s.Id == id).FirstOrDefault();
            db.DeleteMarketIndexStock(AdminMarketIndexStockModel);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}