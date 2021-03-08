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
    public class AdminStockTypesController : Controller
    {
        private ctaWEBAdminContext db = new ctaWEBAdminContext();

        // GET: AdminStockTypes
        public ActionResult Index()
        {
            return View(db.GetStockTypes());
        }

        // GET: AdminStockTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStockTypesModel adminStockTypesModel = db.GetStockTypes().Where(st => st.Id == id).FirstOrDefault();
            if (adminStockTypesModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockTypesModel);
        }

        // GET: AdminStockTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminStockTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name")] AdminStockTypesModel adminStockTypesModel)
        {
            if (ModelState.IsValid)
            {
                db.SaveNewStockType(adminStockTypesModel);                
                return RedirectToAction("Index");
            }

            return View(adminStockTypesModel);
        }

        // GET: AdminStockTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStockTypesModel adminStockTypesModel = db.GetStockTypes().Where(st => st.Id == id).FirstOrDefault();
            if (adminStockTypesModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockTypesModel);
        }

        // POST: AdminStockTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name")] AdminStockTypesModel adminStockTypesModel)
        {
            if (ModelState.IsValid)
            {
                db.UpdateStockType(adminStockTypesModel);
                return RedirectToAction("Index");
            }
            return View(adminStockTypesModel);
        }

        // GET: AdminStockTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminStockTypesModel adminStockTypesModel = db.GetStockTypes().Where(st => st.Id == id).FirstOrDefault();
            if (adminStockTypesModel == null)
            {
                return HttpNotFound();
            }
            return View(adminStockTypesModel);
        }

        // POST: AdminStockTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminStockTypesModel adminStockTypesModel = db.GetStockTypes().Where(st => st.Id == id).FirstOrDefault();
            //db.DeleteStockType(adminStockTypesModel);            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
    }
}
