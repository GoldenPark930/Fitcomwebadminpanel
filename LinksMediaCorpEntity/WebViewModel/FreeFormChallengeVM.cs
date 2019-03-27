namespace LinksMediaCorpEntity
{   
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// Classs for Create Free form challenge in admin
    /// </summary>
    public class FreeFormChallengeVM
    {       
        public int ChallengeId { get; set; }

        public int? TrainerId { get; set; }

        public int? TrainerCredntialId { get; set; }  
              
        [Required]
        [AllowHtml]       
        public string Description { get; set; }

        [Required]
        [DisplayName("Challenge Name")]
        [StringLength(200, MinimumLength = 0)]
        public string ChallengeName { get; set; }

        public int Strength { get; set; }  
            
        public string ExeName1 { get; set; }

        [StringLength(80000, MinimumLength = 0)]       
        public string ExeDesc1 { get; set; }

        public string ExeVideoLink1 { get; set; }

        public string ExeIndexLink1 { get; set; }  
             
        public string ExeName2 { get; set; }

        [StringLength(800000, MinimumLength = 0)]
        public string ExeDesc2 { get; set; }

        public string ExeVideoLink2 { get; set; }

        public string ExeIndexLink2 { get; set; }

        public string ExeName3 { get; set; }

        [StringLength(800000, MinimumLength = 0)]
        public string ExeDesc3 { get; set; }


        public string ExeVideoLink3 { get; set; }


        public string ExeIndexLink3 { get; set; }


        public string ExeName4 { get; set; }

        [StringLength(80000, MinimumLength = 0)]
        public string ExeDesc4 { get; set; }

        public string ExeVideoLink4 { get; set; }

        public string ExeIndexLink4 { get; set; }

        public string ExeName5 { get; set; }
         
        [StringLength(80000, MinimumLength = 0)]
        public string ExeDesc5 { get; set; }

        public string ExeVideoLink5 { get; set; }

        public string ExeIndexLink5 { get; set; }

        public string ExeName6 { get; set; } 

        [StringLength(80000, MinimumLength = 0)]
        public string ExeDesc6 { get; set; }

        public string ExeVideoLink6 { get; set; }

        public string ExeIndexLink6 { get; set; } 

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps2 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan2 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman2 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps3 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan3 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman3 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps4 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan4 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman4 { get; set; }

        public int? Reps5 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan5 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman5 { get; set; }

        public int? Reps6 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan6 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForWoman6 { get; set; }

        public bool IsActive { get; set; }

        public string ChallengeStaus { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; } 

        public DateTime ModifiedDate { get; set; }

        public string ExeVideoUrl1 { get; set; }

        public string ExeVideoUrl2 { get; set; }

        public string ExeVideoUrl3 { get; set; }

        public string ExeVideoUrl4 { get; set; }

        public string ExeVideoUrl5 { get; set; }

        public string ExeVideoUrl6 { get; set; }

        public bool isAdminUser = false;

        public bool IsAdminUser
        {
            get
            {
                return isAdminUser;
            }
            set
            {
                isAdminUser = value;
            }
        }

        public bool IsDrafted { get; set; }
       
    }
}