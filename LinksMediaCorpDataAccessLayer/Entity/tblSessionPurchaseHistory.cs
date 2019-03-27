using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblSessionPurchaseHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PurchaseHistoryId { get; set; }

        public int UserCredId { get; set; }

        public int TrainerId { get; set; }

        public int NumberOfSession { get; set; }

        public decimal Amount { get; set; }
    
        public System.DateTime PurchaseDateTime { get; set; } 
      
        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}