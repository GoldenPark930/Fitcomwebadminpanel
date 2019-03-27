using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpEntity
{
    public class AppSubscriptionVM
    {
        public SubscriptionPurchaseStatus SubscriptionStatus { get; set; }
        public DeviceType DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string SubscriptionId { get; set; }
        public string Purchase_date { get; set; }
        public long Purchase_date_ms { get; set; }       
        public long Expires_date_ms { get; set; }
        public bool AutoRenewing { get; set; }
        public string ReceiptData { get; set; }
        public bool Is_Trial_Prerod { get; set; }
        public string AndroidOrderId { get; set; } 
        public string AndriodPurchaseToken { get; set; } 
        public string IOSPassword { get; set; }


    }

    public class AndriodSubscriptionData
    {
        public string orderId { get; set; }
        public string packageName { get; set; }
        public string productId { get; set; }
        public int purchaseTime { get; set; }
        public int purchaseState { get; set; }
        public string purchaseToken { get; set; }
        public bool autoRenewing { get; set; }
    }

   








}
