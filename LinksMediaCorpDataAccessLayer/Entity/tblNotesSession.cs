using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblNotesExecise   
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotesExeciseId { get; set; }  

        public string Notes { get; set; }

        public string ExeciseName { get; set; }  
              
        public long UsedSessionNoteId { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}