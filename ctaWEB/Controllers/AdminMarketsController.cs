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
    public class AdminMarketsController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();
        // GET: AdminMarkets
        public ActionResult Index()
        {
            return View(db.GetMarkets());
        }

        // GET: AdminMarkets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketsModel adminMarketsModel = db.GetMarkets().Where(m => m.Id == id).FirstOrDefault();
            if (adminMarketsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminMarketsModel);
        }

        // GET: AdminMarkets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminMarkets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,work_hours")] AdminMarketsModel adminMarketsModel)
        {
            if (ModelState.IsValid)
            {
                db.SaveNewMarket(adminMarketsModel);                
                return RedirectToAction("Index");
            }

            return View(adminMarketsModel);
        }

        // GET: AdminMarkets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketsModel adminMarketsModel = db.GetMarkets().Where(m => m.Id == id).FirstOrDefault();
            if (adminMarketsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminMarketsModel);
        }

        // POST: AdminMarkets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,work_hours")] AdminMarketsModel adminMarketsModel)
        {
            if (ModelState.IsValid)
            {
                db.UpdateMarket(adminMarketsModel);                
                return RedirectToAction("Index");
            }
            return View(adminMarketsModel);
        }

        // GET: AdminMarkets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMarketsModel adminMarketsModel = db.GetMarkets().Where(m => m.Id == id).FirstOrDefault();
            if (adminMarketsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminMarketsModel);
        }

        // POST: AdminMarkets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminMarketsModel adminMarketsModel = db.GetMarkets().Where(m => m.Id == id).FirstOrDefault();
            //db.DeleteMarket(adminMarketsModel);            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
    }
}
