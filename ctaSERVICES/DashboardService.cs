using ctaDATAMODEL;
using ctaCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.Dashboard;
using ctaCOMMON.Charts;
using ctaCOMMON.Interface;
using ctaCOMMON.Indicator;

namespace ctaSERVICES
{
    public static class DashboardService
    {
        private static UserDashboard DashBoardCache;
        private static SymbolDashboard SymbolDashboardCache;

        public static List<ISimplifiedSearchSymbolResult> GetSymbolsByLabel(string label)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                var match_symbols = entities.Stocks.Where(s => s.symbol.StartsWith(label) && s.active.Value);

                List<ISimplifiedSearchSymbolResult> result = new List<ISimplifiedSearchSymbolResult>();
                foreach (var item in match_symbols)
                {
                    SearchResultSymbol symbol = new SearchResultSymbol() { Symbol_Id = item.Id, Symbol_Name = item.symbol, Market_Id = item.market_id, Market_Name = item.Market.name };
                    result.Add(symbol);
                }

                entities.Database.Connection.Close();
                return result;
            }
        }

        public static Dictionary<string, List<DashboardReport>> GetReports()
        {
            Dictionary<string, List<DashboardReport>> result = new Dictionary<string, List<DashboardReport>>();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = (entities.Reports.Where(r => r.Report_Type.active && r.active).Select(r => new DashboardReport() { Id = r.Id, active = r.active, description = r.description, title = r.title, typename = r.Report_Type.name, url = r.url })).GroupBy(r => r.typename).Where(l => l.Count() > 0).ToDictionary(r => r.Key, l => l.ToList<DashboardReport>());                    

                return result;
            }
        }

        public static List<DashboardReport> GetReports(string reportType)
        {
            Dictionary<string, List<DashboardReport>> result = new Dictionary<string, List<DashboardReport>>();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                return entities.Reports.Where(r => r.Report_Type.name == reportType && r.active).Select(r => new DashboardReport() { Id = r.Id, active = r.active, description = r.description, title = r.title, typename = r.Report_Type.name, url = r.url }).ToList<DashboardReport>();
            }
        }

        public static UserDashboard GetDashboard(string username)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                UserDashboard result = new UserDashboard(username);
                entities.Database.Connection.Open();

                int user_id = UserService.GetUserId(username);
                string user_type = entities.Tenants.Where(t => t.Id == user_id).Select(t => t.Tenant_Type.Name).First();
                var user_portfolios = entities.Portfolios.Where(ptfolio => ptfolio.user_id == user_id);
                
                foreach (var portfolio in user_portfolios)
                {
                    DashboardItem dashboard_item = new DashboardItem() { Portfolio = portfolio.name, Symbols = new List<ctaCOMMON.Charts.Symbol>(), Portfolio_Id = portfolio.Id };
                    foreach (var stock in portfolio.Portfolio_Stock)
                    {
                        List<Candel> quotes = new List<Candel>();
                        Candel today_candel = null;
                        foreach (var quote in stock.Stock.Stock_Quote.Where(s => s.date_round < DateTime.Now.AddDays(-1) || user_type != "FREE").OrderBy(itm => itm.date_round))
                        {
                            Candel candel = new Candel() { Date = quote.date_round, Open = quote.opening, Close = quote.closing, Minimun = quote.minimun, Maximun = quote.maximun, Volume = (double)quote.volume };
                            quotes.Add(candel);
                            today_candel = candel;
                        }
                        SymbolIntradiaryInfo symbolIntradiaryInfo = QuotesService.GetSymbolIntradiaryInfo(stock.stock_id);
                        if (today_candel != null)
                        {
                            symbolIntradiaryInfo.Volume = today_candel.Volume;
                            symbolIntradiaryInfo.Maximun = today_candel.Maximun;
                            symbolIntradiaryInfo.Minimun = today_candel.Minimun;
                        }

                        ctaCOMMON.Charts.Symbol dashboard_item_symbol = new ctaCOMMON.Charts.Symbol() { Symbol_ID = stock.stock_id, Symbol_Name = stock.Stock.symbol, Symbol_Company_Name = stock.Stock.name, Symbol_Market_ID = stock.Stock.Market.Id, Symbol_Market = stock.Stock.Market.name, Quotes = quotes.OrderBy(q => q.Date).ToList(), Intradiary_Info = symbolIntradiaryInfo };
                        dashboard_item.Symbols.Add(dashboard_item_symbol);
                    }

                    result.AddDashboardItem(dashboard_item);
                }

                entities.Database.Connection.Close();

                DashBoardCache = result;
                return result;
            }
        }

        public static Serie GetSymbolDataSerie(int stock_id, int portfolio_id, DataSourceFieldUsed data_name)
        {
            List<Candel> quotes = new List<Candel>();

            if (DashBoardCache != null && false)
            {
                var oneYearAgo_Date = DateTime.Now.AddYears(-1);

                var symbol = DashBoardCache.DashboardItems.Where(x => x.Portfolio_Id == portfolio_id)
                                                          .SelectMany(x => x.Symbols)
                                                          .Where(x => x.Symbol_ID == stock_id)
                                                          .FirstOrDefault();

                quotes = symbol.Quotes.Where(x => x.Date > oneYearAgo_Date).ToList();
            }
            else
            {
                quotes = QuotesService.GetSymbolQuotes(stock_id, ChartRange.Year, CandelRange.Weekly, "FREE");                
            }

            Serie result = Candel.GetDataSerie(quotes, data_name, true);
            return result;
        }

        public static Serie GetSymbolIntradiaryDataSerie(int portfolio_id, int symbol_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                string user_type = (portfolio_id == 0) ? "FREE" : entities.Portfolios.Where(p => p.Id == portfolio_id).Select(p => p.Tenant.Tenant_Type.Name).First();
                if (user_type == "FREE")
                    return new Serie();

                var symbol_quotes = entities.Stock_Quote_Intradiary.Where(sq => sq.stock_id == symbol_id).OrderBy(sq => sq.datetime);
                List<Candel> quotes = symbol_quotes.Select(q => new Candel()
                                                    {
                                                        Date = q.datetime,
                                                        Open = q.opening,
                                                        Close = q.opening,
                                                        Maximun = q.last_trade_price,
                                                        Minimun = q.last_trade_price,
                                                        Volume = (double)q.last_trade_size,
                                                        Visible = true
                                                    }).ToList<Candel>();
                
                Serie result = Candel.GetDataSerie(quotes, DataSourceFieldUsed.Close, true);                

                entities.Database.Connection.Close();
                return result;
            }
        }        

        public static SymbolDashboard GetSymbolDashboard(int portfolio_id, int symbol_id, ChartRange chartRange, CandelRange candelRange, bool withQuotes)
        {
            bool indicatorUpdate, shapeUpdate;

            if (SymbolDashboardCache != null && SymbolDashboardCache.Portfolio_Id == portfolio_id && SymbolDashboardCache.Symbol_Id == symbol_id && false)
            {
                var indicators = DashboardService.GetSymbolIndicators(SymbolDashboardCache.Symbol, SymbolDashboardCache.Indicators.Count(), withQuotes, candelRange, out indicatorUpdate);
                var shapes = DashboardService.GetSymbolShapes(SymbolDashboardCache.Symbol, SymbolDashboardCache.Shapes.Count(), withQuotes, out shapeUpdate);

                SymbolDashboardCache.Indicators = indicatorUpdate ? indicators : SymbolDashboardCache.Indicators;
                SymbolDashboardCache.Shapes = shapeUpdate ? shapes : SymbolDashboardCache.Shapes;

                return SymbolDashboardCache;
            }
            else
            {
                SymbolDashboard symbol_dashboard = new SymbolDashboard() { Portfolio_Id = portfolio_id, Symbol_Id = symbol_id };

                symbol_dashboard.Symbol = DashboardService.GetSymbol(portfolio_id, symbol_id, chartRange, candelRange, withQuotes);
                symbol_dashboard.Indicators = DashboardService.GetSymbolIndicators(symbol_dashboard.Symbol, -1, withQuotes, candelRange, out indicatorUpdate);
                symbol_dashboard.Shapes = DashboardService.GetSymbolShapes(symbol_dashboard.Symbol, -1, withQuotes, out shapeUpdate);

                SymbolDashboardCache = symbol_dashboard;

                return symbol_dashboard;
            }
        }

        private static List<Shape_Indicator> GetSymbolShapes(ctaCOMMON.Charts.Symbol symbol, int shapesCount, bool initializeFromDatabase, out bool shapeUpdate)
        {
            List<Shape_Indicator> shapes = new List<Shape_Indicator>();
            shapeUpdate = false;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                var shapes_entities = entities.Portfolio_Stock_Shape.Where(pss => pss.portfolio_id == symbol.Portfolio_ID && pss.stock_id == symbol.Symbol_ID);

                if (shapes_entities.Count() != shapesCount)
                {
                    shapeUpdate = true;
                    foreach (var shape_entitie in shapes_entities)
                    {
                        Shape_Indicator shape = Shape_Indicator_Builder.BuildShapeInstance(shape_entitie.Shape.name);
                        shape.ID = shape_entitie.Id;
                        shape.Color = shape_entitie.color;
                        shape.Name = shape_entitie.name;
                        shape.End_Date = shape_entitie.date2.Value;
                        shape.End_Value = shape_entitie.value2.Value;
                        shape.Start_Date = shape_entitie.date1;
                        shape.Start_Value = shape_entitie.value1;
                        shape.Data_Source = symbol.Quotes;
                        shape.ApplyFormula();
                        shapes.Add(shape);
                    }
                }

                entities.Database.Connection.Close();
            }
            return shapes;
        }

        private static List<Chart_Indicator> GetSymbolIndicators(ctaCOMMON.Charts.Symbol symbol, int indicatorsCount, bool initializeFromDatabase, CandelRange candelRange, out bool indicatorUpdate)
        {
            List<Chart_Indicator> indicators = new List<Chart_Indicator>();
            indicatorUpdate = false;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var indicators_entities = entities.Portfolio_Stock_Indicator.Where(psi => psi.portfolio_id == symbol.Portfolio_ID && psi.stock_id == symbol.Symbol_ID);

                if (indicators_entities.Count() != indicatorsCount)
                {
                    indicatorUpdate = true;
                    foreach (var indicator_entitie in indicators_entities)
                    {
                        Chart_Indicator indicator = Chart_Indicator_Builder.BuildIndicatorInstance(indicator_entitie.Indicator.name);
                        indicator.ID = indicator_entitie.Id;
                        if (initializeFromDatabase)
                        {
                            indicator.InitializeFromDataBaseValues(symbol.Quotes, indicator_entitie.param1, indicator_entitie.color1, indicator_entitie.param2, indicator_entitie.color2, indicator_entitie.param3, indicator_entitie.color3, candelRange);
                        }
                        indicators.Add(indicator);
                    }
                }

                entities.Database.Connection.Close();
            }

            return indicators;
        }

        private static ctaCOMMON.Charts.Symbol GetSymbol(int portfolio_id, int symbol_id, ChartRange chartRange, CandelRange candelRange, bool withQuotes)
        {
            if (DashBoardCache != null && DashBoardCache.DashboardItems.Count > 0 && false)
            {
                var symbol = DashBoardCache.DashboardItems.Where(x => x.Portfolio_Id == portfolio_id)
                                                          .SelectMany(x => x.Symbols)
                                                          .Where(x => x.Symbol_ID == symbol_id)
                                                          .FirstOrDefault();

                return symbol;
            }
            else
            {
                ctaCOMMON.Charts.Symbol symbol = new ctaCOMMON.Charts.Symbol();

                using (ctaDBEntities entities = new ctaDBEntities())
                {
                    entities.Database.Connection.Open();
                    string user_type = (portfolio_id == 0) ? "FREE" : entities.Portfolios.Where(p => p.Id == portfolio_id).Select(p => p.Tenant.Tenant_Type.Name).First();
                    var stock_entity = entities.Stocks.Where(s => s.Id == symbol_id).First();

                    symbol.Portfolio_ID = portfolio_id;
                    symbol.Symbol_ID = stock_entity.Id;
                    symbol.Symbol_Name = stock_entity.symbol;
                    symbol.Symbol_Company_Name = stock_entity.name;
                    symbol.Symbol_Market_ID = stock_entity.market_id;
                    symbol.Symbol_Market = stock_entity.Market.name;
                    symbol.Intradiary_Info = QuotesService.GetSymbolIntradiaryInfo(symbol_id);
                    if (withQuotes)
                    {
                        symbol.Quotes = QuotesService.GetSymbolQuotes(symbol.Symbol_ID, chartRange, candelRange, user_type);
                    }

                    entities.Database.Connection.Close();
                }

                return symbol;
            }
        }
        
        public static void AddIndicator(int portfolioID, int symbolID, int indicatorID, string param1, string color1, string param2, string color2, string param3, string color3)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                Portfolio_Stock_Indicator psi = new Portfolio_Stock_Indicator();
                psi.portfolio_id = portfolioID;
                psi.indicator_id = indicatorID;
                psi.stock_id = symbolID;
                psi.param1 = (param1 == null) ? String.Empty : param1;
                psi.color1 = (color1 == null) ? String.Empty : color1;
                psi.param2 = (param2 == null) ? String.Empty : param2;
                psi.color2 = (color2 == null) ? String.Empty : color2;
                psi.param3 = (param3 == null) ? String.Empty : param3;
                psi.color3 = (color3 == null) ? String.Empty : color3;

                entities.Portfolio_Stock_Indicator.Add(psi);
                entities.SaveChanges();

                entities.Database.Connection.Close();
            }            
        }

        public static void AddShape(int portfolio_id, int symbol_id, int shape_id, DateTime start_date, double start_value, DateTime end_date, double end_value, string color, string name)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                Portfolio_Stock_Shape pss = new Portfolio_Stock_Shape();
                pss.portfolio_id = portfolio_id;
                pss.stock_id = symbol_id;
                pss.shape_id = shape_id;
                pss.name = name;
                pss.color = color;
                pss.date1 = start_date;
                pss.date2 = end_date;
                pss.value1 = start_value;
                pss.value2 = end_value;

                entities.Portfolio_Stock_Shape.Add(pss);
                entities.SaveChanges();

                entities.Database.Connection.Close();
            } 
        }

        public static void DeleteIndicator(int portfolio_symbol_indicator_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                Portfolio_Stock_Indicator psi = entities.Portfolio_Stock_Indicator.Where(ps => ps.Id == portfolio_symbol_indicator_id).FirstOrDefault();

                if (psi != null)
                {
                    entities.Portfolio_Stock_Indicator.Remove(psi);
                    entities.SaveChanges();
                }

                entities.Database.Connection.Close();
            }    
        }

        public static void DeleteShape(int portfolio_symbol_shape_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                Portfolio_Stock_Shape psi = entities.Portfolio_Stock_Shape.Where(ps => ps.Id == portfolio_symbol_shape_id).FirstOrDefault();

                if (psi != null)
                {
                    entities.Portfolio_Stock_Shape.Remove(psi);
                    entities.SaveChanges();
                }

                entities.Database.Connection.Close();
            }  
        }

        public static void AddPortfolioToDashboard(string username, string portfolio)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                int user_id = UserService.GetUserId(username);
                entities.Database.Connection.Open();

                Portfolio usc = new Portfolio() { name = portfolio, user_id = user_id };

                entities.Portfolios.Add(usc);
                entities.SaveChanges();

                entities.Database.Connection.Close();
            }
        }

        public static void DeletePortfolioFromDashboard(string username, string portfolio)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                int user_id = UserService.GetUserId(username);
                entities.Database.Connection.Open();

                var port = entities.Portfolios.Where(p => p.user_id == user_id && p.name == portfolio)
                                              .FirstOrDefault();

                var shapes = entities.Portfolio_Stock_Shape.Where(s => s.portfolio_id == port.Id);
                var indicators = entities.Portfolio_Stock_Indicator.Where(i => i.portfolio_id == port.Id);
                var symbols = entities.Portfolio_Stock.Where(s => s.portfolio_id == port.Id);

                entities.Portfolio_Stock_Shape.RemoveRange(shapes);
                entities.Portfolio_Stock_Indicator.RemoveRange(indicators);
                entities.Portfolio_Stock.RemoveRange(symbols);
                entities.Portfolios.Remove(port);

                entities.SaveChanges();

                entities.Database.Connection.Close();
            }
        }

        public static void AddSymbolToPortfolio(int portfolio_id, int symbol_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                Portfolio_Stock portfolio = new Portfolio_Stock() { portfolio_id = portfolio_id, stock_id = symbol_id };
                entities.Portfolio_Stock.Add(portfolio);
                entities.SaveChanges();
                entities.Database.Connection.Close();

                //DashboardService.AddIndicator(portfolio_id, symbol_id, 1, String.Empty, "#0000FF", String.Empty, String.Empty, String.Empty, String.Empty);
            }
        }

        public static void DeleteSymbolFromPortfolio(int portfolio_id, int stock_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var symbol = entities.Portfolio_Stock.Where(x => x.portfolio_id == portfolio_id && x.stock_id == stock_id)
                                                      .FirstOrDefault();
                var shapes = entities.Portfolio_Stock_Shape.Where(s => s.portfolio_id == symbol.portfolio_id && s.stock_id == symbol.stock_id);
                var indicators = entities.Portfolio_Stock_Indicator.Where(i => i.portfolio_id == symbol.portfolio_id && i.stock_id == symbol.stock_id);                

                entities.Portfolio_Stock_Shape.RemoveRange(shapes);
                entities.Portfolio_Stock_Indicator.RemoveRange(indicators);
                entities.Portfolio_Stock.Remove(symbol);
                entities.SaveChanges();
                entities.Database.Connection.Close();
            }
        }
        
        public static List<IndicatorParameter> GetIndicatorsDetails()
        {
            List<IndicatorParameter> result = new List<IndicatorParameter>();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var indicators = entities.Indicators.Where(x => x.active.HasValue && x.active.Value);

                result.AddRange(indicators.Select(x => new IndicatorParameter() { ID = x.Id, Color1 = x.color1, Color2 = x.color2, Color3 = x.color3, Name = x.name, Param1 = x.param1, Param2 = x.param2, Param3 = x.param3 }));

                entities.Database.Connection.Close();
            }
            return result;
        }
    }
}
