
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// classs for free form Workout Response
    /// </summary>
    public class WorkoutResponse
    {
        public int WorkoutId { get; set; }

        public string WorkoutName { get; set; }

        public string WorkoutUrl { get; set; }

        public int TrainerId { get; set; }

        public List<int> TargetZone { get; set; }

        public string DifficultyLevel { get; set; }

    }
}