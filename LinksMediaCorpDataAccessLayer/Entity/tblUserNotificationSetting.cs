

namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblUserNotificationSetting
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

        [Required]
        public int UserCredId { get; set; } 
               
        public string DeviceID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 0)]
        public string NotificationType { get; set; } 

        [Required]
        public bool IsNotify { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }   
            
        public string DeviceType { get; set; }  
        
    }
   
}