namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblTrainerMobileCoachTeam
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MobileCoachTeamId { get; set; }
        public int TrainerCredId { get; set; }
        public int TeamId { get; set; }

    }
}
