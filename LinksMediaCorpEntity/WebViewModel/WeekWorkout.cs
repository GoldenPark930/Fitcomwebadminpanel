namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get WeekWorkout in admin
    /// </summary>
    public class WeekWorkout
    {
        public long UserAcceptedProgramId { get; set; }

        public long ProgramWeekId { get; set; }

        public int ProgramChallengeId { get; set; }

        public int WorkoutChallengeId { get; set; }

        public long ProgramWeekWorkoutId { get; set; }

    }
}
