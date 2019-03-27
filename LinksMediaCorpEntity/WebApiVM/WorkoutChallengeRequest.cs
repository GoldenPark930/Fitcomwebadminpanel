
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request for Workout Challenge 
    /// </summary>
    public class WorkoutChallengeRequest
    {
        public int WorkoutCategoryID { get; set; } 

        public int WorkoutSubCategoryID { get; set; }

        public bool IsPremiumWorkout { get; set; }
    }   
}