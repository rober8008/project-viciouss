using ctaCOMMON.AdminModel;
using ctaSERVICES;
using ctaWEB.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class ctaWEBAdminContext
    {
        public ctaWEBAdminContext()
        {
        }

        #region Stocks

        private List<AdminStocksModel> stocks;

        internal List<AdminStocksModel> GetStocks(bool recalculate = false)
        {
            if (recalculate || this.stocks == null)
            {
                this.stocks = StockService.GetStocks().Select(s => new AdminStocksModel() { active = s.active, description = s.description, Id = s.Id, market_id = s.market_id, name = s.name, symbol = s.symbol, type_id = s.type_id, symbol_market = s.symbol + " (" + s.market_name + ")"}).ToList();
            }
            return this.stocks; 
        }        

        internal void SaveNewStock(AdminStocksModel adminStockModel)
        {
            StockService.CreateStock(new StockModel() { active = adminStockModel.active, description = "<p class=\"align-left\" style=\"text-align:justify\">" + adminStockModel.description + "</p>", market_id = adminStockModel.market_id, name = adminStockModel.name, symbol = adminStockModel.symbol, type_id = adminStockModel.type_id });
        }

        internal void UpdateStock(AdminStocksModel adminStockModel)
        {
            StockService.UpdateStock(new StockModel() { Id = adminStockModel.Id, active = adminStockModel.active, description = adminStockModel.description, market_id = adminStockModel.market_id, name = adminStockModel.name, symbol = adminStockModel.symbol, type_id = adminStockModel.type_id });
        }

        internal void DeleteStock(AdminStocksModel adminStockModel)
        {
            StockService.DeleteStock(adminStockModel.Id);
        }

        #endregion

        #region Markets

        private List<AdminMarketsModel> markets;

        internal List<AdminMarketsModel> GetMarkets(bool recalculate = false)
        {
            if (recalculate || this.markets == null)
            {
                this.markets = MarketService.GetMarkets().Select(s => new AdminMarketsModel() { Id = s.Id, name = s.name, work_hours = s.work_hours }).ToList();
            }
            return this.markets; 
        }

        internal void SaveNewMarket(AdminMarketsModel adminStockModel)
        {
            MarketService.CreateMarket(new MarketModel() { name = adminStockModel.name, work_hours = adminStockModel.work_hours });
        }

        internal void UpdateMarket(AdminMarketsModel adminStockModel)
        {
            MarketService.UpdateMarket(new MarketModel() { Id = adminStockModel.Id, name = adminStockModel.name, work_hours = adminStockModel.work_hours });
        }

        internal void DeleteMarket(AdminMarketsModel adminStockModel)
        {
            MarketService.DeleteMarket(adminStockModel.Id);
        }
        
        #endregion

        #region StockTypes

        private List<AdminStockTypesModel> stocktypes;

        internal List<AdminStockTypesModel> GetStockTypes(bool recalculate = false)
        {
            if (recalculate || this.stocktypes == null)
            {
                this.stocktypes = StockTypeService.GetStockTypes().Select(s => new AdminStockTypesModel() { Id = s.Id, name = s.name }).ToList();
            }
            return this.stocktypes;
        }

        internal void SaveNewStockType(AdminStockTypesModel adminStockModel)
        {
            StockTypeService.CreateStockType(new StockTypeModel() { name = adminStockModel.name });
        }

        internal void UpdateStockType(AdminStockTypesModel adminStockModel)
        {
            StockTypeService.UpdateStockType(new StockTypeModel() { Id = adminStockModel.Id, name = adminStockModel.name });
        }

        internal void DeleteStockType(AdminStockTypesModel adminStockModel)
        {
            StockTypeService.DeleteStockType(adminStockModel.Id);
        }

        #endregion

        #region StockQuotes

        private List<AdminStockQuotesModel> stockquotes;

        internal List<AdminStockQuotesModel> GetStockQuotes(int stockid = 0, bool recalculate = false)
        {
            if (recalculate || this.stockquotes == null || stockid != 0)
            {
                this.stockquotes = StockQuoteService.GetStockQuotes().Where(s => s.stock_id == stockid).Select(s => new AdminStockQuotesModel() { Id = s.Id, adj_close = s.adj_close, closing = s.closing, date_round = s.date_round, maximun = s.maximun, minimun = s.minimun, opening = s.opening, stock_id = s.stock_id, volume = s.volume, AdminStocksModel = new AdminStocksModel() { Id = s.stock.Id, symbol = s.stock.symbol } }).ToList();
            }
            return this.stockquotes;
        }

        internal void SaveNewStockQuote(AdminMarketsModel adminStockModel)
        {
            //MarketService.CreateMarket(new MarketModel() { name = adminStockModel.name, work_hours = adminStockModel.work_hours });
        }

        internal void UpdateStockQuote(AdminMarketsModel adminStockModel)
        {
            //MarketService.UpdateMarket(new MarketModel() { Id = adminStockModel.Id, name = adminStockModel.name, work_hours = adminStockModel.work_hours });
        }

        internal void DeleteStockQuote(AdminMarketsModel adminStockModel)
        {
            //MarketService.DeleteMarket(adminStockModel.Id);
        }
        
        #endregion        

        #region MarketIndex

        private List<AdminMarketIndexModel> marketIndexes;

        internal List<AdminMarketIndexModel> GetMarketIndexes()
        {
            if (this.marketIndexes == null)
            {
                this.marketIndexes = MarketIndexService.GetMarketIndexes().Select(s => new AdminMarketIndexModel() { Id = s.Id, name = s.name, market_id = s.market_id, market_name = s.market_name }).ToList();
            }
            return this.marketIndexes;
        }

        internal void UpdateMarketIndex(AdminMarketIndexModel adminMarketIndexModel)
        {
            MarketIndexService.UpdateMarketIndex(new MarketIndexModel() { Id = adminMarketIndexModel.Id, market_id = adminMarketIndexModel.market_id, name = adminMarketIndexModel.name });
        }

        internal void DeleteMarketIndex(AdminMarketIndexModel adminMarketIndexModel)
        {
            MarketIndexService.DeleteMarketIndex(adminMarketIndexModel.Id);
        }

        internal void SaveNewMarketIndex(AdminMarketIndexModel adminMarketIndexModel)
        {
            MarketIndexService.CreateMarketIndex(new MarketIndexModel() { name = adminMarketIndexModel.name, market_id = adminMarketIndexModel.market_id });
        }

        #endregion

        #region MarketIndexStocks

        private List<AdminMarketIndexStockModel> marketIndexStocks;

        internal List<AdminMarketIndexStockModel> GetMarketIndexStocks()
        {
            if (this.marketIndexStocks == null)
            {
                this.marketIndexStocks = MarketIndexStockService.GetMarketIndexStocks().Select(s => new AdminMarketIndexStockModel() { Id = s.Id, marketindex_id = s.marketindex_id, marketindex_name = s.marketindex_name, stock_id = s.stock_id, stock_symbol = s.stock_symbol }).ToList();
            }
            return this.marketIndexStocks;
        }

        internal void UpdateMarketIndexStock(AdminMarketIndexStockModel AdminMarketIndexStockModel)
        {
            MarketIndexStockService.UpdateMarketIndexStock(new MarketIndexStockModel() { Id = AdminMarketIndexStockModel.Id, marketindex_id = AdminMarketIndexStockModel.marketindex_id, stock_id = AdminMarketIndexStockModel.stock_id });
        }

        internal void DeleteMarketIndexStock(AdminMarketIndexStockModel AdminMarketIndexStockModel)
        {
            MarketIndexStockService.DeleteMarketIndexStock(AdminMarketIndexStockModel.Id);
        }

        internal void SaveNewMarketIndexStock(AdminMarketIndexStockModel AdminMarketIndexStockModel)
        {
            MarketIndexStockService.CreateMarketIndexStock(new MarketIndexStockModel() { marketindex_id = AdminMarketIndexStockModel.marketindex_id, stock_id = AdminMarketIndexStockModel.stock_id });
        }

        #endregion        
    }
}
