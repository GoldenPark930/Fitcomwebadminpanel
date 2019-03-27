using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Program Week Workout
    /// </summary>
    public class ProgramWeekWorkouts
    {
        public long ProgramWeekId { get; set; }

        public int AssignedTrainerId { get; set; }

        public int AssignedTrainingzone { get; set; }

        public int AssignedDifficulyLevelId { get; set; }

        public List<WeekWorkouts> WeekWorkoutsRecords { get; set; } 
    }
  
}