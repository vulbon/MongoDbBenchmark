using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataModel
{
    public class SalesDataViewModel
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


    }
}
