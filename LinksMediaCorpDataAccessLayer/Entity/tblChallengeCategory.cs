namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblChallengeCategory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeCategoryId { get; set; }

        [StringLength(255,MinimumLength=0)]
        public string ChallengeCategoryName { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public bool Isactive { get; set; }
    }
}