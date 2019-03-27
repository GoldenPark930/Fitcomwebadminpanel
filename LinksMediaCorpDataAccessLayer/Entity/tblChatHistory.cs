using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpDataAccessLayer
{
   public class tblChatHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ChatHistoryId { get; set; }

        [Required]
        public int SenderCredId { get; set; }

        [Required]
        public int ReceiverCredId { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime TrasactionDateTime { get; set; }

        public bool IsRead { get; set; }
    }
}
