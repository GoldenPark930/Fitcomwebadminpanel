
namespace LinksMediaCorpDataAccessLayer
{    
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class tblNoTrainerChallengeTeam
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoTrainerWorkoutId { get; set; }

        public int TeamId { get; set; }

        public int ChallengeId { get; set; }  
             
        public Nullable<System.Boolean> IsProgram { get; set; }

        public Nullable<System.Boolean> IsFittnessTest { get; set; } 


    }
}
