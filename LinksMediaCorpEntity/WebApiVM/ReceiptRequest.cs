using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpEntity
{
    public class ReceiptRequest
    {
        public string DeviceId { get; set; }
        public DeviceType DeviceType { get; set; }     
        public string SubscriptionId { get; set; } 
        public string ReceiptData { get; set; }
        public string Password { get; set; }  
        public string Purchase_date { get; set; }
        public string Original_purchase_date { get; set; }
    }
}
