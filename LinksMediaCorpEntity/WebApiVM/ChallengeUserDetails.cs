namespace LinksMediaCorpEntity
{
   /// <summary>
   /// Classs for Challenged User Detail response
   /// </summary>
    public class ChallengeUserDetails
    {
       public string ChallengeByUserName { get; set; }

       public string Result { get; set; }

       public string Fraction { get; set; }

       public string ResultUnitSuffix { get; set; }

       public int UserChallengeId { get; set; }
    }
}
