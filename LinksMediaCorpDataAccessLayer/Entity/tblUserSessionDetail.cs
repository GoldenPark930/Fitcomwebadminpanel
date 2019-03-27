using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblUserSessionDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserSessionId { get; set; }

        public Nullable<int> RemaingNumberOfSession { get; set; }

        public Nullable<int> UsedNumberOfSession { get; set; }

        public Nullable<int> UserCredId { get; set; }

        public Nullable<int> TrainerId { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDateTime { get; set; } 
    }
}