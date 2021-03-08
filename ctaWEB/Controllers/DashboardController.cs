using ctaCOMMON;
using ctaCOMMON.Charts;
using ctaCOMMON.Dashboard;
using ctaCOMMON.Interface;
using ctaSERVICES;
using ctaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard        
        public ActionResult Index()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.IsFREEuser = (UserService.GetTenant_Type(username) == 1);
            DashboardModel dashboard = new DashboardModel(username, 0);
            return View(dashboard);            
        }

        public ActionResult SymbolDashboard(string username, int symbol_id, int portfolio_id)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == username)
            {
                SymbolDashboardModel symb = new SymbolDashboardModel(portfolio_id, symbol_id, username, ChartRange.ThreeMonths, CandelRange.Daily, true); //false: for now true because the quotes are needed to calculate indicator's DisplayValue.
                return View(symb);
            }
            return RedirectToAction("Logout", "User");
        }

        [HttpPost]
        public ActionResult GetSymbolByLabel(string term)
        {
            List<ISimplifiedSearchSymbolResult> data = DashboardService.GetSymbolsByLabel(term);
            return Json(data, JsonRequestBehavior.AllowGet);            
        }        

        [HttpPost]
        public PartialViewResult DashboardAddPortfolio(DashhboardAddPortfolioModel dashboard_add_portfolio)
        {
            if(System.Web.HttpContext.Current.User.Identity.Name == dashboard_add_portfolio.username)
            {
                string action_validation = ValidationService.ValidateUserProtfolioCount(dashboard_add_portfolio.username);
                if (String.IsNullOrEmpty(action_validation))
                    DashboardService.AddPortfolioToDashboard(dashboard_add_portfolio.username, dashboard_add_portfolio.Portfolio_Name);
                else
                    ViewBag.ActionValidationError = action_validation;                
            }            
            return PartialView("~/Views/Dashboard/Partial/Watchlists.cshtml", new DashboardModel(System.Web.HttpContext.Current.User.Identity.Name, 0));
        }

        [HttpPost]
        public PartialViewResult DashboardDeletePortfolio(DashboardModel dashboard_delete_portfolio, string Username)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == Username)
            {
                DashboardService.DeletePortfolioFromDashboard(Username, dashboard_delete_portfolio.Portfolio_Name);
            }
            return PartialView("~/Views/Dashboard/Partial/Watchlists.cshtml", new DashboardModel(System.Web.HttpContext.Current.User.Identity.Name, 0));
        }

        [HttpPost]
        public PartialViewResult PortfolioAddSymbol(PortfolioAddSymbolModel portfolio_add_symbol)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == portfolio_add_symbol.Username)
            {
                string action_validation = ValidationService.ValidateUserSymbolCount(portfolio_add_symbol.Username, portfolio_add_symbol.Portfolio_Id);
                if (String.IsNullOrEmpty(action_validation))
                    DashboardService.AddSymbolToPortfolio(portfolio_add_symbol.Portfolio_Id, portfolio_add_symbol.Symbol_Id);
                else
                    ViewBag.ActionValidationError = action_validation;                
            }
            return PartialView("~/Views/Dashboard/Partial/Watchlists.cshtml", new DashboardModel(System.Web.HttpContext.Current.User.Identity.Name, portfolio_add_symbol.Portfolio_Id));
        }

        public RedirectToRouteResult PortfolioDeleteSymbol(PortfolioAddSymbolModel portfolio_add_symbol)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == portfolio_add_symbol.Username)
            {
                DashboardService.DeleteSymbolFromPortfolio(portfolio_add_symbol.Portfolio_Id, portfolio_add_symbol.Symbol_Id);
            }
            
            return RedirectToAction("Index");            
        }

        public ActionResult GetUserDashboardData() 
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            UserDashboard dashboard = new UserDashboard(username);
            return Json(dashboard.UserDashboardToJSON(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSymbolDashboardData(int symbol_id, int portfolio_id) 
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            SymbolDashboard dashboard = new SymbolDashboard();
            return Json(dashboard.SymbolDashboardToJSON(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSimplifiedSymbolData(int symbol_id, int portfolio_id, string containerid)
        {
            Serie data = DashboardService.GetSymbolDataSerie(symbol_id, portfolio_id, DataSourceFieldUsed.Close);
            Serie intradiaty_data = DashboardService.GetSymbolIntradiaryDataSerie(portfolio_id, symbol_id);

            object dataformated = Serie.GetChartLineFormatedData(data);
            object intradiaryformated = Serie.GetChartLineFormatedData(intradiaty_data);
            object response = new { symbol_id = symbol_id, quotesdata = dataformated, containerid = containerid, quotesintradiary = intradiaryformated };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetSymbolContent(int symbol_id, string main_chart_type, ChartRange chartRange, CandelRange candelRange, int portfolio_id = 0)
        {
            SymbolContentModel symbol_content = new SymbolContentModel(portfolio_id, symbol_id);
            symbol_content.Symbol_Dashboard = DashboardService.GetSymbolDashboard(portfolio_id, symbol_id, chartRange, candelRange, true);            

            object data = symbol_content.GetDataInJSONFormat(main_chart_type);
            object data_series_main_chart = symbol_content.GetDataSeriesInfoForMainChart(main_chart_type);
            object data_view_main_chart = symbol_content.GetDataViewInfoForMainChartData(main_chart_type);
            object volume_chart_view = symbol_content.GetDataViewInfoForVolumeChart(main_chart_type);
            object date_range_selector_view = symbol_content.GetDataViewInfoForDateRangeSelector(main_chart_type);
            object[] data_indicators = symbol_content.GetDataInfoForChartIndicators(main_chart_type);
            object max_data_date = symbol_content.max_date.ToString("r");

            object response = new { data = data, series_main_chart = data_series_main_chart, view_main_chart = data_view_main_chart, indicators = data_indicators, date_range_selector_view = date_range_selector_view, max_data_date = max_data_date, volume_chart_view = volume_chart_view };

            var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public ActionResult AddIndicator(AddIndicatorModel indicator)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == indicator.username)
            {
                string action_validation = ValidationService.ValidateUserIndicatorCount(indicator.username, indicator.id_portfolio, indicator.id_symbol);
                if (String.IsNullOrEmpty(action_validation))                
                    DashboardService.AddIndicator(indicator.id_portfolio, indicator.id_symbol, indicator.id_indicator, indicator.param1, indicator.color1, indicator.param2, indicator.color2, indicator.param3, indicator.color3);                
                else
                    ViewBag.ActionValidationError = action_validation;
            }
            return PartialView("~/Views/Dashboard/Partial/SymbolDashboardContent.cshtml", new SymbolDashboardModel(indicator.id_portfolio, indicator.id_symbol,indicator.username, ChartRange.ThreeMonths, CandelRange.Daily, true));           
        }

        public ActionResult DeleteIndicator(int indicator_id, string username)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == username)
            {
                DashboardService.DeleteIndicator(indicator_id);
            }

            object response = new { success = true };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddShape(AddShapeModel shape)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == shape.username)
            {
                string action_validation = ValidationService.ValidateUserIndicatorCount(shape.username, shape.id_portfolio, shape.id_symbol);
                if (String.IsNullOrEmpty(action_validation))
                    DashboardService.AddShape(shape.id_portfolio, shape.id_symbol, shape.id_shape, shape.date1, shape.value1, shape.date2, shape.value2, shape.color, shape.name);
                else
                    ViewBag.ActionValidationError = action_validation;
            }
            return PartialView("~/Views/Dashboard/Partial/SymbolDashboardContent.cshtml", new SymbolDashboardModel(shape.id_portfolio, shape.id_symbol, shape.username, ChartRange.ThreeMonths, CandelRange.Daily,true));            
        }

        public ActionResult DeleteShape(int shape_id, string username)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == username)
            {
                DashboardService.DeleteShape(shape_id);
            }

            object response = new { success = true };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}