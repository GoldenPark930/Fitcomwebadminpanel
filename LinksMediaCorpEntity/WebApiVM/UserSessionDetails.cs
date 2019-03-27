using System;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for response for User Session Detail
    /// </summary>
    public class UserSessionDetailVM
    {
        public long UserSessionId { get; set; }

        public int RemaingNumberOfSession { get; set; }

        public int UsedNumberOfSession { get; set; }

        public int UserCredId { get; set; }

        public int TrainerId { get; set; }

        public int UserId { get; set; } 

        public string UserType { get; set; }

        public string UserName { get; set; }  

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDateTime { get; set; } 
    }
}