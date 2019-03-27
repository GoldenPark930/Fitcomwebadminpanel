namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for add or get Execise in admin
    /// </summary>
    public class Exercise
    {
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public int CEARocordId { get; set; }     
           
        public string Index { get; set; }

        public bool IsActive { get; set; }

        public string VedioLink { get; set; }

        public string Description { get; set; }

        public string ExerciseThumnail { get; set; }

        public bool IsAlternateExeciseName { get; set; }

        public bool IsSetFirstExecise { get; set; }     
           
        public string AlternateExeciseName { get; set; }

        public string SelectedEquipment { get; set; }

        public string SelectedTraingZone { get; set; }

        public string SelectedExeciseType { get; set; }

        public List<ExeciseSetVM> ExeciseSetRecords { get; set; }
    }
    
}