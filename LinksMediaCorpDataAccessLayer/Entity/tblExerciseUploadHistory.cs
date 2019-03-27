namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblExerciseUploadHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UploadExerciseId { get; set; }

        public string UploadHistoryGuidId { get; set; }
        public string ExerciseName { get; set; }
        public string Index { get; set; }
        public string TeamId { get; set; }
        public string TrainerId { get; set; }
        public string FailedVideoName { get; set; }        
        public DateTime CreatedDate { get; set; }


    }
}
