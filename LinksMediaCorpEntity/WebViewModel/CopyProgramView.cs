using System.Collections.Generic;
using System.Web.Mvc;
namespace LinksMediaCorpEntity
{
   public  class CopyProgramView
    {
        private List<ProgramWeekWorkout> programWeekWorkoutList;

        public string DifficultyLevel { get; set; }

        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public string Description { get; set; }

        public int? ChallengeCategoryId { get; set; }

        public List<ProgramWeekWorkout> ProgramWeekWorkoutList
        {
            get
            {
                return programWeekWorkoutList;
            }
        }

        public int ChallengeType { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public string ChallengeType_Name { get; set; }

        public string ChallengeSubType_Description { get; set; }

        [AllowHtml]
        public string ChallengeDetail { get; set; }


        public List<string> ChallengeCategoryNameList { get; set; }

        /// <summary>
        /// Set Program Week WorkoutList
        /// </summary>
        /// <param name="prgWeekWorkoutList"></param>
        public void SetProgramWeekWorkoutList(List<ProgramWeekWorkout> prgWeekWorkoutList)
        {
            programWeekWorkoutList = new List<ProgramWeekWorkout>();
            programWeekWorkoutList = prgWeekWorkoutList;
        }
    }
}
