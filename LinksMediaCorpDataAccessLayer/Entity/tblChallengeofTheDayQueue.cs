namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tblChallengeofTheDayQueue
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QueueId { get; set; }

        public int UserId { get; set; }

        public int ChallengeId { get; set; }

        public string NameOfChallenge { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }

        public Nullable<bool> CurrentlyDisplayed { get; set; }

        public int ResultId { get; set; }   
         
        public int HypeVideoId { get; set; }

        public int? CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}