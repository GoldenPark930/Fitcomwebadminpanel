namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblChallengeType
    {
         [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeSubTypeId { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string ChallengeSubType { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string ChallengeType { get; set; }

        public int MaxLimit { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string Unit { get; set; }

        [StringLength(5, MinimumLength = 0)]
        public string IsExerciseMoreThanOne { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string ResultUnit { get; set; }

        public bool IsActive { get; set; }
    }
}