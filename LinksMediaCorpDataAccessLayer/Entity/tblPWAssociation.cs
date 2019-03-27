using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinksMediaCorpDataAccessLayer
{
    public class tblPWAssociation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PWRocordId { get; set; }

        public int ProgramChallengeId { get; set; }  
           
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int AssignedTrainerId { get; set; }

        public int AssignedTrainingzone { get; set; }

        public int AssignedDifficulyLevelId { get; set; }
       
    }
}