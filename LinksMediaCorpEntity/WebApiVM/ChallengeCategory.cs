namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Challenge Category like Workout ,Wellness and Fitness Test
    /// </summary>
    public class ChallengeCategory   
    {
        public int ChallengeCategoryId { get; set; }    
         
        public string ChallengeCategoryName { get; set; }

        public int ProgramTypeId { get; set; }       
    }   
}