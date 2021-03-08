using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace vcssAPI.DBContext
{
    public partial class ctaDBContext : DbContext
    {
        public ctaDBContext()
        {
        }

        public ctaDBContext(DbContextOptions<ctaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<Indicator> Indicator { get; set; }
        public virtual DbSet<Market> Market { get; set; }
        public virtual DbSet<MarketIndex> MarketIndex { get; set; }
        public virtual DbSet<MarketIndexQuote> MarketIndexQuote { get; set; }
        public virtual DbSet<MarketIndexStock> MarketIndexStock { get; set; }
        public virtual DbSet<MarketQuote> MarketQuote { get; set; }
        public virtual DbSet<Portfolio> Portfolio { get; set; }
        public virtual DbSet<PortfolioStock> PortfolioStock { get; set; }
        public virtual DbSet<PortfolioStockIndicator> PortfolioStockIndicator { get; set; }
        public virtual DbSet<PortfolioStockShape> PortfolioStockShape { get; set; }
        public virtual DbSet<Shape> Shape { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<StockQuote> StockQuote { get; set; }
        public virtual DbSet<StockQuoteIntradiary> StockQuoteIntradiary { get; set; }
        public virtual DbSet<StockReport> StockReport { get; set; }
        public virtual DbSet<StockType> StockType { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<TenantType> TenantType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=AMRO-HP;Database=ctaDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasIndex(e => e.ConfigName)
                    .HasName("NonClusteredIndex-ConfigName");

                entity.Property(e => e.ConfigName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConfigValue)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Holidays>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasMaxLength(50);

                entity.Property(e => e.MarketId).HasColumnName("market_id");

                entity.HasOne(d => d.Market)
                    .WithMany(p => p.Holidays)
                    .HasForeignKey(d => d.MarketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Holidays_to_Market");
            });

            modelBuilder.Entity<Indicator>(entity =>
            {
                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Color1)
                    .HasColumnName("color1")
                    .HasMaxLength(50);

                entity.Property(e => e.Color2)
                    .HasColumnName("color2")
                    .HasMaxLength(50);

                entity.Property(e => e.Color3)
                    .HasColumnName("color3")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Param1)
                    .HasColumnName("param1")
                    .HasMaxLength(50);

                entity.Property(e => e.Param2)
                    .HasColumnName("param2")
                    .HasMaxLength(50);

                entity.Property(e => e.Param3)
                    .HasColumnName("param3")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Market>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.WorkHours)
                    .IsRequired()
                    .HasColumnName("work_hours")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MarketIndex>(entity =>
            {
                entity.Property(e => e.MarketId).HasColumnName("market_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Market)
                    .WithMany(p => p.MarketIndex)
                    .HasForeignKey(d => d.MarketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketIndex_To_Market");
            });

            modelBuilder.Entity<MarketIndexQuote>(entity =>
            {
                entity.ToTable("MarketIndex_Quote");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MarketindexId).HasColumnName("marketindex_id");

                entity.HasOne(d => d.Marketindex)
                    .WithMany(p => p.MarketIndexQuote)
                    .HasForeignKey(d => d.MarketindexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketIndex_Quote_To_MarketIndex");
            });

            modelBuilder.Entity<MarketIndexStock>(entity =>
            {
                entity.ToTable("MarketIndex_Stock");

                entity.Property(e => e.MarketindexId).HasColumnName("marketindex_id");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.HasOne(d => d.Marketindex)
                    .WithMany(p => p.MarketIndexStock)
                    .HasForeignKey(d => d.MarketindexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketIndex_Stock_To_MarketIndex");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.MarketIndexStock)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketIndex_Stock_To_Stock");
            });

            modelBuilder.Entity<MarketQuote>(entity =>
            {
                entity.ToTable("Market_Quote");

                entity.Property(e => e.MarketId).HasColumnName("market_id");

                entity.HasOne(d => d.Market)
                    .WithMany(p => p.MarketQuote)
                    .HasForeignKey(d => d.MarketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotesMarket_to_Market");
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Portfolio)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Portfolio_to_Tenant");
            });

            modelBuilder.Entity<PortfolioStock>(entity =>
            {
                entity.ToTable("Portfolio_Stock");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioStock)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStock_to_Portfolio");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.PortfolioStock)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStock_to_Stock");
            });

            modelBuilder.Entity<PortfolioStockIndicator>(entity =>
            {
                entity.ToTable("Portfolio_Stock_Indicator");

                entity.Property(e => e.Color1)
                    .IsRequired()
                    .HasColumnName("color1")
                    .HasMaxLength(50);

                entity.Property(e => e.Color2)
                    .IsRequired()
                    .HasColumnName("color2")
                    .HasMaxLength(50);

                entity.Property(e => e.Color3)
                    .IsRequired()
                    .HasColumnName("color3")
                    .HasMaxLength(50);

                entity.Property(e => e.IndicatorId).HasColumnName("indicator_id");

                entity.Property(e => e.Param1)
                    .IsRequired()
                    .HasColumnName("param1")
                    .HasMaxLength(50);

                entity.Property(e => e.Param2)
                    .IsRequired()
                    .HasColumnName("param2")
                    .HasMaxLength(50);

                entity.Property(e => e.Param3)
                    .IsRequired()
                    .HasColumnName("param3")
                    .HasMaxLength(50);

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.HasOne(d => d.Indicator)
                    .WithMany(p => p.PortfolioStockIndicator)
                    .HasForeignKey(d => d.IndicatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockIndicator_to_Indicator");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioStockIndicator)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockIndicator_to_Portfolio");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.PortfolioStockIndicator)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockIndicator_to_Stock");
            });

            modelBuilder.Entity<PortfolioStockShape>(entity =>
            {
                entity.ToTable("Portfolio_Stock_Shape");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasMaxLength(50);

                entity.Property(e => e.Date1)
                    .HasColumnName("date1")
                    .HasColumnType("datetime");

                entity.Property(e => e.Date2)
                    .HasColumnName("date2")
                    .HasColumnType("datetime");

                entity.Property(e => e.Date3)
                    .HasColumnName("date3")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ShapeId).HasColumnName("shape_id");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.Property(e => e.Value1).HasColumnName("value1");

                entity.Property(e => e.Value2).HasColumnName("value2");

                entity.Property(e => e.Value3).HasColumnName("value3");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioStockShape)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockShape_to_Portfolio");

                entity.HasOne(d => d.Shape)
                    .WithMany(p => p.PortfolioStockShape)
                    .HasForeignKey(d => d.ShapeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockShape_to_Shape");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.PortfolioStockShape)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortfolioStockShape_to_Stock");
            });

            modelBuilder.Entity<Shape>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MarketId).HasColumnName("market_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasColumnName("symbol")
                    .HasMaxLength(50);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Market)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.MarketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_to_Market");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_to_StockType");
            });

            modelBuilder.Entity<StockQuote>(entity =>
            {
                entity.ToTable("Stock_Quote");

                entity.Property(e => e.AdjClose).HasColumnName("adj_close");

                entity.Property(e => e.Closing).HasColumnName("closing");

                entity.Property(e => e.DateRound)
                    .HasColumnName("date_round")
                    .HasColumnType("date");

                entity.Property(e => e.Maximun).HasColumnName("maximun");

                entity.Property(e => e.Minimun).HasColumnName("minimun");

                entity.Property(e => e.Opening).HasColumnName("opening");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockQuote)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotesStock_to_Stock");
            });

            modelBuilder.Entity<StockQuoteIntradiary>(entity =>
            {
                entity.ToTable("Stock_Quote_Intradiary");

                entity.Property(e => e.Ask).HasColumnName("ask");

                entity.Property(e => e.AskSize).HasColumnName("ask_size");

                entity.Property(e => e.Bid).HasColumnName("bid");

                entity.Property(e => e.BidSize).HasColumnName("bid_size");

                entity.Property(e => e.Change).HasColumnName("change");

                entity.Property(e => e.ChangePercent).HasColumnName("change_percent");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastTradeDate)
                    .HasColumnName("last_trade_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastTradePrice).HasColumnName("last_trade_price");

                entity.Property(e => e.LastTradeSize).HasColumnName("last_trade_size");

                entity.Property(e => e.LastTradeTime)
                    .IsRequired()
                    .HasColumnName("last_trade_time")
                    .HasMaxLength(20);

                entity.Property(e => e.Opening).HasColumnName("opening");

                entity.Property(e => e.PrevClosing).HasColumnName("prev_closing");

                entity.Property(e => e.StockId).HasColumnName("stock_id");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockQuoteIntradiary)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockQuotesIntradiary_to_Stock");
            });

            modelBuilder.Entity<StockReport>(entity =>
            {
                entity.ToTable("Stock_Report");

                entity.Property(e => e.BoolLow).HasColumnName("BoolLOW");

                entity.Property(e => e.BoolUp).HasColumnName("BoolUP");

                entity.Property(e => e.DateRound)
                    .HasColumnName("Date_Round")
                    .HasColumnType("date");

                entity.Property(e => e.Ema12).HasColumnName("EMA12");

                entity.Property(e => e.Ema20).HasColumnName("EMA20");

                entity.Property(e => e.Ema26).HasColumnName("EMA26");

                entity.Property(e => e.Ma20).HasColumnName("MA20");

                entity.Property(e => e.Ma200).HasColumnName("MA200");

                entity.Property(e => e.Ma50).HasColumnName("MA50");

                entity.Property(e => e.Ma9).HasColumnName("MA9");

                entity.Property(e => e.Macd2612).HasColumnName("MACD2612");

                entity.Property(e => e.Rsi14).HasColumnName("RSI14");

                entity.Property(e => e.SofastD3).HasColumnName("SOFast%d3");

                entity.Property(e => e.SofastK14).HasColumnName("SOFast%k14");

                entity.Property(e => e.StockId).HasColumnName("Stock_ID");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockReport)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Report_Stock");
            });

            modelBuilder.Entity<StockType>(entity =>
            {
                entity.ToTable("Stock_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("NonClusteredIndex-email");

                entity.HasIndex(e => e.Username)
                    .HasName("NonClusteredIndex-Username");

                entity.Property(e => e.ActivationId)
                    .HasColumnName("activationId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Secretpass)
                    .IsRequired()
                    .HasColumnName("secretpass")
                    .HasMaxLength(20);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Tenant)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_Tenant_to_TenantType");
            });

            modelBuilder.Entity<TenantType>(entity =>
            {
                entity.ToTable("Tenant_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
