using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataModel
{
    public class SalesData
    {
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public string Currency { get; set; }
        public string LocalCurrency { get; set; }
        public DateTime IssueDate { get; set; }
        public int IssueYear { get; set; }
        public string IssueMonth { get; set; }
        public string IssueQuater { get; set; }

        public int? ForecastQty { get; set; }
        public decimal? ForecastUnitCost { get; set; }
        public decimal? ForecastAmount { get; set; }
        public decimal? ForecastAmountLocalCurrency { get; set; }

        public int? OrderQty { get; set; }
        public decimal? OrderUnitCost { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal? OrderAmountLocalCurrency { get; set; }

        public int? ShipQty { get; set; }
        public decimal? ShipUnitCost { get; set; }
        public decimal? ShipAmount { get; set; }
        public decimal? ShipAmountLocalCurrency { get; set; }

        public decimal? SalesCost { get; set; }
        public decimal? SalesCost_Spec { get; set; }
        public decimal? GrossProfit_Spec { get; set; }
        public decimal? SalesCost_TMC_RMB { get; set; }
        public decimal? SalesCost_TMC_Spec { get; set; }
        public decimal? GrossProfit_TMC_Spec { get; set; }
        public decimal? SalesCost_TMF_NTD { get; set; }
        public decimal? SalesCost_TMF_Spec { get; set; }

        public decimal? SalesCost_MA_TMC_RMB { get; set; }
        public decimal? SalesCost_LA_TMC_RMB { get; set; }
        public decimal? SalesCost_OV_TMC_RMB { get; set; }
        public decimal? SalesCost_CO_TMC_RMB { get; set; }
        public decimal? SalesCost_MA_TMF_NTD { get; set; }
        public decimal? SalesCost_LA_TMF_NTD { get; set; }
        public decimal? SalesCost_OV_TMF_NTD { get; set; }
        public decimal? SalesCost_CO_TMF_NTD { get; set; }

        public int? SalesQty { get; set; }
        public double? SalesAmount { get; set; }
        public double? SalesAmount_Local { get; set; }
        public double? SalesAmount_Spec { get; set; }
        public double? SalesCostWoReturn { get; set; }
        public double? SalesCostWoReturn_Spec { get; set; }
        public double? GrossProfitWoReturn_Spec { get; set; }
        public double? SalesCostWoReturn_TMC_RMB { get; set; }
        public double? SalesCostWoReturn_TMC_Spec { get; set; }
        public double? GrossProfitWoReturn_TMC_Spec { get; set; }

    }
}
