using System;

namespace LinksMediaCorpEntity
{
    public class UpdateExerciseSproutLink
    {
        public int ExerciseId { get; set; }
        public string VideoId { get; set; }
        public string V1080pUrl { get; set; }
        public string V720pUrl { get; set; }
        public string V480pUrl { get; set; }
        public string V360pUrl { get; set; }
        public string V240pUrl { get; set; }
        public string SourceUrl { get; set; }
        public string SecuryId { get; set; }
        public string ThumnailUrl { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public string VideoLink { get; set; }
        public string ExerciseName { get; set; }
        public string Index { get; set; }      
        public string Description { get; set; }
        public bool IsActive { get; set; }    
        public string TrainerID { get; set; }
        public string TeamID { get; set; }
        public string FailedVideoName { get; set; } 


    }
}
