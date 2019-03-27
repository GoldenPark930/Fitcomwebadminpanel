namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tblTrainer
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerId { get; set; }

        public int EnteredTrainerID { get; set; }  
              
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int City { get; set; }

        public string State { get; set; }

        public string Gender { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string ZipCode { get; set; }

        public string EmailId { get; set; }

        public int TeamId { get; set; }  
         
        public string AboutMe { get; set; }

        public string TrainerImageUrl { get; set; }

        public string ActivityId { get; set; }

        public Nullable<bool> LoggedIn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string TrainerType { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; } 

        public string Website { get; set; } 

    }
}