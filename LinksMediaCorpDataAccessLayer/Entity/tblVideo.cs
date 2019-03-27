namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblVideo
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        public int UserId { get; set; }

        public int ChallengeId { get; set; }

        public string VideoUrl { get; set; }

        public string Description { get; set; }
    }
}