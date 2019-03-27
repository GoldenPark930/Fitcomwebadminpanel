namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of User OnBorading Info
    /// </summary>
   public class UserOnBoradingInfo
    {
       public string DateOfBirth { get; set; }

       public Gender Gender { get; set; }

       public string Height { get; set; }

       public string Weight { get; set; }

       public FitnessLevel FitnessLevel { get; set; }

       public string OnboardingVideoUrl { get; set; }

       public string OnboardingVideoThumnailUrl { get; set; }  
    }
   
}
