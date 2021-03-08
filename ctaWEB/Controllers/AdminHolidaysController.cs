using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ctaWEB.Models.AdminModels;
using ctaSERVICES;
using ctaCOMMON.AdminModel;

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminHolidaysController : Controller
    {
        // GET: AdminHolidays
        public ActionResult Index()
        {
            return View(HolidayService.GetHolidays().Select(s => new AdminHolidaysModel() { date = s.date, duration = s.duration, market_id = s.market_id, Id = s.Id, market_name = s.market_name }));
        }

        // GET: AdminHolidays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminHolidaysModel adminHolidaysModel = HolidayService.GetHolidays().Where(s => s.Id == id).Select(s => new AdminHolidaysModel() { date = s.date, duration = s.duration, market_id = s.market_id, Id = s.Id, market_name = s.market_name }).FirstOrDefault();
            if (adminHolidaysModel == null)
            {
                return HttpNotFound();
            }
            return View(adminHolidaysModel);
        }

        // GET: AdminHolidays/Create
        public ActionResult Create()
        {
            ViewBag.market_id = new SelectList(MarketService.GetMarkets().Select( m => new AdminMarketsModel() { Id = m.Id, name = m.name, work_hours = m.work_hours}), "Id", "name");
            return View();
        }

        // POST: AdminHolidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,date,duration,market_id")] AdminHolidaysModel adminHolidaysModel)
        {
            if (ModelState.IsValid)
            {
                HolidayService.CreateHoliday(new HolidayModel() { date = adminHolidaysModel.date, duration = adminHolidaysModel.duration, market_id = adminHolidaysModel.market_id, market_name = adminHolidaysModel.market_name });                                
                return RedirectToAction("Index");
            }

            ViewBag.market_id = new SelectList(MarketService.GetMarkets().Select(m => new AdminMarketsModel() { Id = m.Id, name = m.name, work_hours = m.work_hours }), "Id", "name", adminHolidaysModel.market_id);
            return View(adminHolidaysModel);
        }

        // GET: AdminHolidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminHolidaysModel adminHolidaysModel = HolidayService.GetHolidays().Where(s => s.Id == id).Select(s => new AdminHolidaysModel() { date = s.date, duration = s.duration, market_id = s.market_id, Id = s.Id, market_name = s.market_name }).FirstOrDefault();
            if (adminHolidaysModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.market_id = new SelectList(MarketService.GetMarkets().Select(m => new AdminMarketsModel() { Id = m.Id, name = m.name, work_hours = m.work_hours }), "Id", "name", adminHolidaysModel.market_id);
            return View(adminHolidaysModel);
        }

        // POST: AdminHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,date,duration,market_id")] AdminHolidaysModel adminHolidaysModel)
        {
            if (ModelState.IsValid)
            {
                HolidayService.UpdateHoliday(new HolidayModel() { Id = adminHolidaysModel.Id, date = adminHolidaysModel.date, duration = adminHolidaysModel.duration, market_id = adminHolidaysModel.market_id, market_name = adminHolidaysModel.market_name });                              
                return RedirectToAction("Index");
            }
            ViewBag.market_id = new SelectList(MarketService.GetMarkets().Select(m => new AdminMarketsModel() { Id = m.Id, name = m.name, work_hours = m.work_hours }), "Id", "name", adminHolidaysModel.market_id);
            return View(adminHolidaysModel);
        }

        // GET: AdminHolidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminHolidaysModel adminHolidaysModel = HolidayService.GetHolidays().Where(s => s.Id == id).Select(s => new AdminHolidaysModel() { date = s.date, duration = s.duration, market_id = s.market_id, Id = s.Id, market_name = s.market_name }).FirstOrDefault();
            if (adminHolidaysModel == null)
            {
                return HttpNotFound();
            }
            return View(adminHolidaysModel);
        }

        // POST: AdminHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolidayService.DeleteHoliday(HolidayService.GetHolidays().Where(s => s.Id == id).FirstOrDefault());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
    }
}
