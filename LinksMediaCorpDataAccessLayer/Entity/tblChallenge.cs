namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblChallenge
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeId { get; set; }

        public int TrainerId { get; set; }

        public string Description { get; set; } 

        [StringLength(200, MinimumLength = 0)]
        public string ChallengeName { get; set; }

        public int ChallengeSubTypeId { get; set; }  

        [StringLength(20, MinimumLength = 0)]
        public string DifficultyLevel { get; set; }

        public bool IsActive { get; set; }

       
        [StringLength(50, MinimumLength = 0)]
       
        public string VariableValue { get; set; }

        public string GlobalResultFilterValue { get; set; }  
               
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDraft { get; set; }

        public bool IsPremium { get; set; }

        public string FFChallengeDuration { get; set; }
      
        public string ChallengeDetail { get; set; }

        public string ProgramImageUrl { get; set; }

        public bool IsSubscription { get; set; }

        public bool IsFeatured { get; set; }

        public string FeaturedImageUrl { get; set; }

        public bool IsFreeFitnessTest { get; set; }

    }
}