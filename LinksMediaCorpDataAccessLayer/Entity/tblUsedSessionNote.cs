using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblUsedSessionNote
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UsedSessionNoteId { get; set; }

        public string Notes { get; set; }

        public long UseSessionId { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> createdDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}