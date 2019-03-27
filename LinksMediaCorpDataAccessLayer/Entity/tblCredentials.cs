namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblCredentials
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }   
            
        [StringLength(256, MinimumLength = 0)]
        public string Password { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 0)]
        public string EmailId { get; set; } 

        [Required]
        public int UserId { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string UserType { get; set; }

        public Nullable<System.DateTime> LastLogin { get; set; }

        public bool AndriodSubscriptionStatus { get; set; }

        public bool IOSSubscriptionStatus { get; set; } 

        
    }
}