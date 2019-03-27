namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblTrainingType
    {
         [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string TypeName { get; set; }
    }
}