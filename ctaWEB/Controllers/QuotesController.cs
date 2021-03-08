using ctaCOMMON.Charts;
using ctaSERVICES;
using ctaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class QuotesController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shares()
        {
            return View();
        }

        public ActionResult Options()
        {
            return View();
        }

        public ActionResult Bonds()
        {
            return View();
        }

        public ActionResult Quote(int symbol_id)
        {
            Symbol model = QuotesService.GetSymbolInfo(symbol_id);

            return View(model);
        }

        public ActionResult Indexes()
        {
            return View();
        }


        [HttpPost]
        public PartialViewResult SharesSelector(int marketIndex_id)
        {
            return PartialView("~/Views/Quotes/Partial/SharesWidget.cshtml", new ctaWEB.Models.QuotesSharesModel(1, marketIndex_id));
        }

        [HttpPost]
        public PartialViewResult BondsSelector(int marketIndex_id)
        {
            return PartialView("~/Views/Quotes/Partial/BondsWidget.cshtml", new ctaWEB.Models.QuotesSharesModel(2, marketIndex_id));
        }
    }
}