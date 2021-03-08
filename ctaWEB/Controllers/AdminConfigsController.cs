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
using ctaCOMMON.AdminModel;

namespace ctaWEB.Controllers
{
    public class AdminConfigsController : Controller
    {
        // GET: AdminConfigs
        public ActionResult Index()
        {
            return View(ConfigService.GetConfigs().Select(c => new AdminConfigsModel() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }));            
        }

        // GET: AdminConfigs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminConfigsModel adminConfigsModel = ConfigService.GetConfigs().Select(c => new AdminConfigsModel() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).Where(c => c.Id == id).FirstOrDefault();
            if (adminConfigsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminConfigsModel);            
        }

        // GET: AdminConfigs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminConfigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ConfigName,ConfigValue")] AdminConfigsModel adminConfigsModel)
        {
            if (ModelState.IsValid)
            {
                ConfigService.CreateConfig(new ConfigModel() { ConfigName = adminConfigsModel.ConfigName, ConfigValue = adminConfigsModel.ConfigValue });               
                return RedirectToAction("Index");
            }

            return View(adminConfigsModel);            
        }

        // GET: AdminConfigs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminConfigsModel adminConfigsModel = ConfigService.GetConfigs().Select(c => new AdminConfigsModel() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).Where(c => c.Id == id).FirstOrDefault();
            if (adminConfigsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminConfigsModel);            
        }

        // POST: AdminConfigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ConfigName,ConfigValue")] AdminConfigsModel adminConfigsModel)
        {
            if (ModelState.IsValid)
            {
                ConfigService.UpdateConfig(new ConfigModel() { Id = adminConfigsModel.Id, ConfigName = adminConfigsModel.ConfigName, ConfigValue = adminConfigsModel.ConfigValue });
                return RedirectToAction("Index");
            }
            return View(adminConfigsModel);
        }

        // GET: AdminConfigs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminConfigsModel adminConfigsModel = ConfigService.GetConfigs().Select(c => new AdminConfigsModel() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).Where(c => c.Id == id).FirstOrDefault();
            if (adminConfigsModel == null)
            {
                return HttpNotFound();
            }
            return View(adminConfigsModel);            
        }

        // POST: AdminConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConfigModel adminConfigsModel = ConfigService.GetConfigs().Where(c => c.Id == id).FirstOrDefault();
            ConfigService.DeleteConfig(adminConfigsModel);           
            return RedirectToAction("Index");            
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            //base.Dispose(disposing);
        }
    }
}
