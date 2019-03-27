namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    public class tblBodyPart
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartId { get; set; }

        public string PartName { get; set; }

        public bool IsShownHoneyComb { get; set; }
    }
}