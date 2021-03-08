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
    public class AdminMarketIndexesController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();

        // GET: AdminMarketIndex
        public ActionResult Index()
        {
            return View(db.GetMarketIndexes());
        }

        // GET: AdminHolidays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexModel model = db.GetMarketIndexes().Where(s => s.Id == id).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: AdminHolidays/Create
        public ActionResult Create()
        {
            ViewBag.market_id = new SelectList(db.GetMarkets(), "Id", "name");
            return View();
        }

        // POST: AdminHolidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,market_id")] AdminMarketIndexModel adminMarketIndexModel)
        {
            if (ModelState.IsValid)
            {
                db.SaveNewMarketIndex(adminMarketIndexModel);
                return RedirectToAction("Index");
            }

            ViewBag.market_id = new SelectList(db.GetMarkets(), "Id", "name", adminMarketIndexModel.market_id);
            return View(adminMarketIndexModel);
        }

        // GET: AdminHolidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexModel model = db.GetMarketIndexes().Where(s => s.Id == id).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.market_id = new SelectList(db.GetMarkets(), "Id", "name", model.market_id);
            return View(model);
        }

        // POST: AdminHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,market_id")] AdminMarketIndexModel adminMarketIndexModel)
        {
            if (ModelState.IsValid)
            {
                db.UpdateMarketIndex(adminMarketIndexModel);
                return RedirectToAction("Index");
            }
            ViewBag.market_id = new SelectList(db.GetMarkets(), "Id", "name", adminMarketIndexModel.market_id);
            return View(adminMarketIndexModel);
        }

        // GET: AdminHolidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketIndexModel AdminMarketIndexModel = db.GetMarketIndexes().Where(s => s.Id == id).FirstOrDefault();
            if (AdminMarketIndexModel == null)
            {
                return HttpNotFound();
            }
            return View(AdminMarketIndexModel);
        }

        // POST: AdminHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminMarketIndexModel AdminMarketIndexModel = db.GetMarketIndexes().Where(s => s.Id == id).FirstOrDefault();
            db.DeleteMarketIndex(AdminMarketIndexModel);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}