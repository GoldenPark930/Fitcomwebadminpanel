namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblCEAssociation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RocordId { get; set; }  
          
        public int ChallengeId { get; set; }

       // [StringLength(500, MinimumLength = 0)]
        public string Description { get; set; }

        public int ExerciseId { get; set; }

        public int Reps { get; set; }

        public int WeightForMan { get; set; }

        public int WeightForWoman { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsAlternateExeciseName { get; set; }

        public bool IsShownFirstExecise { get; set; } 

        public string AlternateExeciseName { get; set; }

        public string SelectedEquipment { get; set; }

        public string SelectedTraingZone { get; set; }

        public string SelectedExeciseType { get; set; }
    }
}