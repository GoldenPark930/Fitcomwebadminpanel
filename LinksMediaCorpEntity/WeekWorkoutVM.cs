
namespace LinksMediaCorpEntity
{   
    /// <summary>
    /// Program Week Workout 
    /// </summary>
    public class WeekWorkoutVM
    {
        public long UserAcceptedProgramId { get; set; }

        public long ProgramWeekId { get; set; }

        public int ProgramChallengeId { get; set; }

        public int WorkoutChallengeId { get; set; }

        public long ProgramWeekWorkoutId { get; set; }

        public string WorkoutHashNumber { get; set; }

        public string WorkoutName { get; set; }

        public string workoutDuration { get; set; }

        public bool IsCompleted { get; set; }
    }
}
