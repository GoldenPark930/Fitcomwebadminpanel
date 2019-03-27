
namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblResultBoom
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultBoomId { get; set; }

        public int ResultId { get; set; } 

        public string Message { get; set; }

        public int BoomedBy { get; set; }

        public DateTime BoomedDate { get; set; }
    }
}