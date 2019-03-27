namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblSpecialization
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecializationId { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string SpecializationName { get; set; }
    }
}