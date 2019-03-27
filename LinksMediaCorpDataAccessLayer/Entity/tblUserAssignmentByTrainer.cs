using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
   public class tblUserAssignmentByTrainer
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAssignmentId { get; set; }

        public int SubjectId { get; set; }

        public int TargetId { get; set; }

        public int ChallengeId { get; set; }

        public bool IsCompleted { get; set; } 

        public Nullable<DateTime> ChallengeDate { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public string ResultUnitSuffix { get; set; }

        public string ChallengeByUserName { get; set; }      
         
        public bool IsProgram { get; set; }

        public Nullable<DateTime> ChallengeCompletedDate { get; set; }

        public string PersonalMessage { get; set; }

        public int UserChallengeId { get; set; }

        public bool IsOnBoarding { get; set; }

        public bool IsRead { get; set; }

        public bool IsActive { get; set; } 
    }
}
