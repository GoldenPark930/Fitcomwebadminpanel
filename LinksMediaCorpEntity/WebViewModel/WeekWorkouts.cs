namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Program Week Workout 
    /// </summary>
    public class WeekWorkouts
    {
        public long ProgramWeekId { get; set; }

        public int WorkoutChallengeId { get; set; }

        public long ProgramWeekWorkoutId { get; set; }

        public string WorkoutName { get; set; }

        public string WorkoutUrl { get; set; }

    }
}
