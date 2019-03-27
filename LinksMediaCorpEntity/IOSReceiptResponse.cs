using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    public class InApp
    {
        public string quantity { get; set; }
        public string product_id { get; set; }
        public string transaction_id { get; set; }
        public string original_transaction_id { get; set; }
        public string purchase_date { get; set; }
        public string purchase_date_ms { get; set; }
        public string purchase_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string expires_date { get; set; }
        public string expires_date_ms { get; set; }
        public string expires_date_pst { get; set; }
        public string web_order_line_item_id { get; set; }
        public string is_trial_period { get; set; }
    }
    public class Receipt
    {
        public string receipt_type { get; set; }
        public int adam_id { get; set; }
        public int app_item_id { get; set; }
        public string bundle_id { get; set; }
        public string application_version { get; set; }
        public long download_id { get; set; }
        public int version_external_identifier { get; set; }
        public string receipt_creation_date { get; set; }
        public string receipt_creation_date_ms { get; set; }
        public string receipt_creation_date_pst { get; set; }
        public string request_date { get; set; }
        public string request_date_ms { get; set; }
        public string request_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string original_application_version { get; set; }
        public List<InApp> in_app { get; set; }
    }
    public class LatestReceiptInfo
    {
        public string quantity { get; set; }
        public string product_id { get; set; }
        public string transaction_id { get; set; }
        public string original_transaction_id { get; set; }
        public string purchase_date { get; set; }
        public string purchase_date_ms { get; set; }
        public string purchase_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string expires_date { get; set; }
        public string expires_date_ms { get; set; }
        public string expires_date_pst { get; set; }
        public string web_order_line_item_id { get; set; }
        public string is_trial_period { get; set; }
    }
    public class PendingRenewalInfo
    {
        public string auto_renew_product_id { get; set; }
        public string product_id { get; set; }
        public string auto_renew_status { get; set; }
    }
    public class IOSReceiptResponse 
    {
        public int status { get; set; }
        public string environment { get; set; }
        public Receipt receipt { get; set; }
        public List<LatestReceiptInfo> latest_receipt_info { get; set; }
        public string latest_receipt { get; set; }
        public List<PendingRenewalInfo> pending_renewal_info { get; set; }
    }
}
