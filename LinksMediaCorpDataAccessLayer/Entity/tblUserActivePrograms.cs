using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LinksMediaCorpDataAccessLayer
{
    public class tblUserActivePrograms
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAcceptedProgramId { get; set; } 

        public int UserCredId { get; set; }

        public DateTime AcceptedDate { get; set; } 

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int ProgramId { get; set; } 

        public int ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}