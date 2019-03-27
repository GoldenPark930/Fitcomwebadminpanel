using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of Exercise
    /// </summary>
    public class ExerciseVM
    {
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public string Index { get; set; }

        public string VedioLink { get; set; }

        public string ExcersiceDescription { get; set; }

        public string ChallengeExcerciseDescription { get; set; }

        public string AlternateExcerciseDescription { get; set; }

        public bool IsAlternateExeciseName { get; set; }

        public bool IsFirstExecise { get; set; } 

        public int Reps { get; set; }

        public int WeightForManDB { get; set; }

        public string  WeightForMan { get; set; }

        public int WeightForWomanDB { get; set; }

        public string WeightForWoman { get; set; }

        public string ExerciseThumnail { get; set; }

        public List<ExeciseSetVM> ExeciseSetList { get; set; }
        
    }
}