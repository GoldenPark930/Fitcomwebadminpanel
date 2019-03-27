using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
   public class tblAppSubscription
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AppSubscriptionId { get; set; }
        public int UserCredId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string SubscriptionStatus { get; set; }
        public Nullable<System.DateTime> SubscriptionBuyDate { get; set; }
        public string SubscriptionId { get; set; }
        public string ReceiptData { get; set; }
        public bool AutoRenewing { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long Purchase_date_ms { get; set; }
        public long Expires_date_ms { get; set; }
        public bool Is_Trial_Period { get; set; }
        public string Purchase_date { get; set; }

        public Nullable<DateTime> Expires_date { get; set; }
        public string AndroidOrderId { get; set; }
        public string AndriodPurchaseToken { get; set; }
        public string IOSPassword { get; set; }
    }
}
