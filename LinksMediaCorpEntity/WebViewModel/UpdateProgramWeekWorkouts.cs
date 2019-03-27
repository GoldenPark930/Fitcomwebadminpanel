using System.Collections.Generic;

namespace LinksMediaCorpEntity
{
    public class UpdateProgramWeekWorkouts
    {   
        public long ProgramWeekId { get; set; }
        public int ProgramId { get; set; }
        public int UserAcceptedProgramId { get; set; }
        public List<WeekWorkout> WeekWorkoutsRecords { get; set; }
    }
  
}