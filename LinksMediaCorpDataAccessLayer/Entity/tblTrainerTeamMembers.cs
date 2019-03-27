namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tblTrainerTeamMembers
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        public int TeamId { get; set; }  

        public int UserId { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}