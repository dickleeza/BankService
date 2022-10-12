using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Models
{
    public class Validation
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int AccountStatus { get; set; }
        public string BillCurrency { get; set; }
    }
}