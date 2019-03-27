namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblChallengeToFriend
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeToFriendId { get; set; }

        public int SubjectId { get; set; }

        public int TargetId { get; set; }


        public int ChallengeId { get; set; }

        public bool IsPending { get; set; }

        public DateTime ChallengeDate { get; set; }

        public string ChallengeByUserName { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public string ResultUnitSuffix { get; set; }

        public bool IsProgram { get; set; }

        public string PersonalMessage { get; set; }

        public int UserChallengeId { get; set; }

        public bool IsActive { get; set; } 
    }
}