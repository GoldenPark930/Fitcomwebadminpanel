using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblUsedSession
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UseSessionId { get; set; }

        public string UseSessionDateTime { get; set; }

        [StringLength(50)]
        public string TrainingType { get; set; }

        public bool IsAttended { get; set; }

        public bool IsTracNotes { get; set; }

        public int UserCredId { get; set; }

        public int TrainerId { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; } 
    }
}