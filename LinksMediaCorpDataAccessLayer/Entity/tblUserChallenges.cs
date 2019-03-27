namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblUserChallenges
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ChallengeId { get; set; }

        public int UserId { get; set; }

        public DateTime AcceptedDate { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public bool IsGlobal { get; set; }

        public bool IsNewsFeed { get; set; }

        public bool IsProgramchallenge { get; set; }    
             
        public string ResultUnit { get; set; }

        public string CompletedTextMessage { get; set; }
      
        
    }
}