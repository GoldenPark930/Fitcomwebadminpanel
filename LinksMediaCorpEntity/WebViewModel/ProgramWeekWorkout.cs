using System.Collections.Generic;

namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Program Week Workouts in admin
    /// </summary>
    public class ProgramWeekWorkout
    {
        public long WeekId { get; set; }

        public List<ProgramWorkout> WeekWorkoutList { get; set; }
    }
}
