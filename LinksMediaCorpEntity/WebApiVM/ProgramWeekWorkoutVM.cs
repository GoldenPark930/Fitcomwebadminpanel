using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Program Week Workout detail
    /// </summary>
    public class ProgramWeekWorkoutVM
    {
        public long ProgramWeekId { get; set; }

        public int ProgramId { get; set; }

        public int UserAcceptedProgramId { get; set; }

        public int WeekSequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public List<WeekWorkoutVM> WeekWorkoutsRecords { get; set; }
    }
}
