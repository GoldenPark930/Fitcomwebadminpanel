using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblSessionNotesExeciseSet
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ExeciseSetId { get; set; }

        public int Reps { get; set; }

        public int Weight { get; set; }

        public long NotesExeciseId { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}