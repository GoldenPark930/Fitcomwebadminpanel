namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of TrainerLibrary WorkoutList ByCategory
    /// </summary>
    public class TrainerLibraryWorkoutListByCategory
    {
        public int WorkoutCategoryID { get; set; }

        public int WorkoutSubCategoryID { get; set; }    
           
        public int UserId { get; set; }       

        public string UserType { get; set; }

        public int StartIndex { get; set; }  

        public int EndIndex { get; set; }          
    }
    
}