using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinksMediaCorpDataAccessLayer
{
    public class tblUTCPostMessage
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UTCMessagePostId { get; set; } 

        public string PostMessage { get; set; }

        public DateTime PostDateTime { get; set; }  
    }
}