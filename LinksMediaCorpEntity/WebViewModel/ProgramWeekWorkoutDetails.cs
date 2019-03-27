using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Program Week Workout Detail
    /// </summary>
    public class ProgramWeekWorkoutDetails
    {
        public List<ProgramWeekWorkouts> ProgramWeekWorkouts { get; set; }

        public string Durations { get; set; }

        public string Workouts { get; set; }
    }
}
