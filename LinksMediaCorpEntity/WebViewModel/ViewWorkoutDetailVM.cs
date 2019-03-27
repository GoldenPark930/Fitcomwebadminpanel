using System.Collections.Generic;
using System.Web.Mvc;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for View Workout Detail in admin
    /// </summary>
    public class ViewWorkoutDetailVM   
    {
        private IList<Exercise> availableExerciseVideoList { get; set; }
        public string DifficultyLevel { get; set; }

        public int ChallengeId { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; }  

        public string ChallengeName { get; set; }   
                
        public string Description { get; set; }  
             
        public int ChallengeType { get; set; }   
           
        public int ChallengeSubTypeId { get; set; }   
         
        public string ChallengeType_Name { get; set; }

        public string ChallengeSubType_Description { get; set; }

        public IList<Exercise> AvailableExerciseVideoList {
         get            
          {
            return availableExerciseVideoList;
          }
        }

        public int ExerciseId { get; set; }  
             
        public string FFChallengeDuration { get; set; }

        [AllowHtml]       
        public string ChallengeDetail { get; set; }  
              
        public int? ChallengeCategoryId { get; set; }  
                   
        public string ExeciseType { get; set; }

        public List<string> ChallengeCategoryNameList { get;   set; } 

        /// <summary>
        /// Set Available Exercise VideoList
        /// </summary>
        /// <param name="availableExerciseVidList"></param>
        public void SetAvailableExerciseVideoList(List<Exercise> availableExerciseVidList) 
        {
            availableExerciseVideoList = availableExerciseVidList;
        }

    }
}