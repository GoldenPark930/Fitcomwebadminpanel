using LinksMediaCorpUtility.Resources;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for request of Purchase Traing Session
    /// </summary>
    public class PurchaseTraingSessionVM
    {
        public long PurchaseHistoryId { get; set; }
        [Required]
        public int UserCredId { get; set; }
        [Required]
        public int TrainerId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumberOfTrainingSession", ErrorMessage = null)] 
        [DisplayName("Sessions")]
        public int NumberOfSession { get; set; }
        [DisplayName("Amount")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidTrainingSessionAmount", ErrorMessage = null)]
        public decimal Amount { get; set; }
        public System.DateTime PurchaseDateTime { get; set; }     
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}