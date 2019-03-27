namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblTrainerSpecialization
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SpecializationId { get; set; }

        public int TrainerId { get; set; }

        public int IsInTopThree { get; set; }
    }
}