namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblUser
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public int TeamId { get; set; }

        public int PersonalTrainerCredId { get; set; }    
             
        [StringLength(50, MinimumLength = 0)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string LastName { get; set; } 
              
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; } 
             
       
        public string Gender { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string ZipCode { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; } 
            
        [StringLength(256, MinimumLength = 0)]
        public string Password { get; set; }

        public string UserImageUrl { get; set; }

        public bool IsMailReceive { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string UserBrief { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int ModifiedBy { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public bool MTActive { get; set; }

        public string FitnessLevel { get; set; }  

        
    }
}