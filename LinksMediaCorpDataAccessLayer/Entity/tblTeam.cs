using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LinksMediaCorpDataAccessLayer
{
    
    public partial class tblTeam
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }  
              
        public int UniqueTeamId { get; set; }

        public string TeamName { get; set; }

        public string Address { get; set; }

        public int City { get; set; }

        public string State { get; set; }  
            
        public string ZipCode { get; set; }

        public string EmailId { get; set; }

        public string PhoneNumber { get; set; } 

        public string ProfileImageUrl { get; set; }

        public string PremiumImageUrl { get; set; } 
            
        public Nullable<bool> LoggedIn { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public bool IsDefaultTeam { get; set; }

        public int OnboardingExeciseVideoId { get; set; }

        public int FitcomtestChallengeId1 { get; set; }

        public int FitcomtestChallengeId2 { get; set; }

        public int BeginnerProgramId { get; set; }

        public int AdvIntProgramId1 { get; set; }

        public int AdvIntProgramId2 { get; set; }

        public int AdvIntProgramId3 { get; set; }

        public string Nutrition1ImageUrl  { get; set; }

        public string Nutrition1HyerLink  { get; set; }

        public string Nutrition2ImageUrl { get; set; }

        public string Nutrition2HyerLink { get; set; }

        public bool IsShownNoTrainerWorkoutPrograms { get; set; }

        public bool IsShownNoTrainerFitnessTests { get; set; }        

        public string Website { get; set; }

         public decimal PrimaryCommissionRate { get; set; }

        public decimal Level1CommissionRate { get; set; }
        public decimal Level2CommissionRate { get; set; }

      
    }
}