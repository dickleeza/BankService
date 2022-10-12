using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Models
{
    public class Bankadvice
    {
        public string ServiceName { get; set; }
        public string MessageID { get; set; }
        public string TransactionRefCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Currency { get; set; }
        public string DocumentRefNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentCode { get; set; }
        public string PaymentMode { get; set; }
        public decimal PaymentAmount { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
        public string AdditionalInfo { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
    }

}