namespace LinksMediaCorpEntity
{
    using LinksMediaCorpUtility.Resources;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    
    /// <summary>
    /// Classs For Add Activity on Admin
    /// </summary>
    public class ActivityVM
    {
        public int ActivityId { get; set; }

        [Required]
        [DisplayName("Activity Name")]
        public string NameOfActivity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "TrainerNotSelected", ErrorMessage = null)]
        public int TrainerId { get; set; }

        [Required]
        [DisplayName("Event Date")]
        public DateTime DateOfEvent { get; set; }

        [Required]
        [DisplayName("Address")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "CityNotSelected", ErrorMessage = null)]
        public string City { get; set; }

         [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "StateNotSelected", ErrorMessage = null)]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        [Range(10000, 99999, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidZip", ErrorMessage = null)]   
        public string Zip { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Learn More")]
        public string LearnMore { get; set; }

        public string Video { get; set; }

        public string Pic { get; set; }

        public HttpPostedFileBase VideoUrl { get; set; }

        public HttpPostedFileBase PicUrl { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [DisplayName("Promotion Text")]
        public string PromotionText { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}