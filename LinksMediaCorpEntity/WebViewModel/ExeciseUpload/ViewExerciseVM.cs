using System;
using System.ComponentModel.DataAnnotations;

namespace LinksMediaCorpEntity
{
    public class ViewExerciseVM
    {
        public int ExerciseId { get; set; }
        [Required]
        public string ExerciseName { get; set; }
        [Required]
        public string Index { get; set; }
        public string Description { get; set; } 
        public string TrainerId { get; set; }
        public string TeamId { get; set; } 
        public string ThumnailUrl { get; set; }
        public string ExerciseVideoUrl { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int SelectedStatus { get; set; }


    }

    
}
