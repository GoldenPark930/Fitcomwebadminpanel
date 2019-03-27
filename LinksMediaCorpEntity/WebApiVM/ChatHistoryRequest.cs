using System;
namespace LinksMediaCorpEntity.WebApiVM
{
   /// <summary>
   /// Class for Request of Chat History
   /// </summary>
   public class ChatHistoryRequest
   {       
       public int SenderCredId { get; set; }     

       public bool IsRead { get; set; }         
   }
}
