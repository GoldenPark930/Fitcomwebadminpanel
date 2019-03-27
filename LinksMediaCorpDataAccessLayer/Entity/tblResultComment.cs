
namespace LinksMediaCorpDataAccessLayer
{
   
      using System;
    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    public class tblResultComment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultCommentId { get; set; }

        public int Id { get; set; } 
         
        public string Message { get; set; }

        public int CommentedBy { get; set; }

        public DateTime CommentedDate { get; set; }
       
    }
}