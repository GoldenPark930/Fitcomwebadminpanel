namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;   
    public class tblActivity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int ActivityId { get; set; }

        public string NameOfActivity { get; set; }

        public int TrainerId { get; set; }

        public DateTime DateOfEvent { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public int City { get; set; }

        public string State { get; set; }

        public string Description { get; set; }

        public string PromotionText { get; set; }

        public string Zip { get; set; }

        public string LearnMore { get; set; }

        public string Video { get; set; }

        public string Pic { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}