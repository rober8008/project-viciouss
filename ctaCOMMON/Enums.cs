using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON
{
    public enum DataSourceFieldUsed
    {
        Open, 
        Close, 
        Minimun, 
        Maximun,
        Volume,
        PriceVariation,
        PercentVariation,
        Stochastic,
        ToolTip,
        WilliansR,
        OBV,
        VolumeOscillator,
        CruceMedias
    }
    
    public enum SerieType
    {
        None,
        line,
        candelsticks,
        bars,
        dashed       
    }

    public enum LoginResult
    {
        NotAnUser,
        ValidUser,
        NotActivatedUser
    }

    public enum ChartRange
    {
        Month = 1,
        ThreeMonths = 2,
        SixMonths = 3,
        Year = 4,
        ThreeYears = 5,
        All = 6
    }

    public enum CandelRange
    {
        Daily = 1,
        Weekly =  2,
        Monthly = 3        
    }

    public enum TaskScheduleType
    {
        UpdateIntradiaryBOLSAR,
        UpdateHistoricalBOLSAR,
        DeleteIntradiaryBOLSAR,
        UpdateIntradiaryBOLSARINDEX,
        UpdateHistoricalBOLSARINDEX,
        DeleteIntradiaryBOLSARINDEX,
        UpdateIntradiaryEOD,
        UpdateHistoricalEOD,
        DeleteIntradiaryEOD,        
        DailyStockTechnicalReport,
        TenantTypeExpirationValidation        
    }

    public enum TaskScheduleStatus
    {
        Pending,
        InProcess
    }

    public enum TaskScheduleExecutionResult
    {
        Done,
        Skip,
        Error
    }

    public enum VcssTaskInfoEnum
    {
        IntradiaryBCBA = 1,
        IntradiaryBCBAINDEX = 2,
        IntradiaryNYSE = 3,
        IntradiaryNASDAQ = 4,
        ClearIntradiaryBCBA = 5,
        ClearIntradiaryNYSE = 6,
        ClearIntradiaryNASDAQ = 7,
        HistoryNYSE = 8,
        HistoryNASDAQ = 9,
        SubscriptionExpire = 14,
        AuthTokenBCBA = 15,
        DailyTechnicalReportARG = 19
    }

    public enum VcssTaskCanExecuteResult
    {
        Execute,
        InvalidData,
        InvalidTime,
        EMPTY
    }

    public enum VcssTaskCanScheduleResult
    {
        Schedule,
        InvalidTime,
        MarketClosed,
        EMPTY
    }
}
