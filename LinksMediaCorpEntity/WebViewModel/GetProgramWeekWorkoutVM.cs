namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of  Program Week Workout
    /// </summary>
    public class GetProgramWeekWorkoutVM
    {
        public int WeekCountID { get; set; }

        public int WeekWorkoutCountID { get; set; }

        public string WorkoutName { get; set; }

        public string WorkoutUrl { get; set; }

        public int ChallengeWorkoutsId { get; set; }

        public int IsNewProgramWeekWorkout { get; set; }

        public string LinksDescription { get; set; }

    }
}
