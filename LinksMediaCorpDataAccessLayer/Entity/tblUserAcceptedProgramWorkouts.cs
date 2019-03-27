using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblUserAcceptedProgramWorkouts
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAcceptedWorkoutId { get; set; }

        public long UserAcceptedProgramWeekId { get; set; }

        public int ProgramChallengeId { get; set; }      
           
        public int UserCredID { get; set; }

        public long PWeekID { get; set; }

        public long PWWorkoutID { get; set; }

        public int WorkoutChallengeID { get; set; }  

        public bool IsCompleted { get; set; }

        public DateTime? ChallengeDate { get; set; }

        public int WeekWorkoutSequenceNumber { get; set; }
    }
}