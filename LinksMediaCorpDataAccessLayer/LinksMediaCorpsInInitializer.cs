namespace LinksMediaCorpDataAccessLayer
{
    using LinksMediaCorpDataAccessLayer;
    using System.Collections.Generic;
    /// <summary>
    /// Links Media Corps InInitializer for master data setup .It was comment for future use
    /// </summary>
    public class LinksMediaCorpsInInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LinksMediaContext>
    {
        protected override void Seed(LinksMediaContext context)
        {
            var bodyParts = new List<tblBodyPart>
            {
                //new tblBodyPart{PartName="Abs"},
                //new tblBodyPart{PartName="Back"},
                //new tblBodyPart{PartName="Biceps"},
                //new tblBodyPart{PartName="Chest"},
                //new tblBodyPart{PartName="Combo"},
                //new tblBodyPart{PartName="Forearms"},
                //new tblBodyPart{PartName="Full Body"},
                //new tblBodyPart{PartName="Legs"},
                //new tblBodyPart{PartName="Lower Back"},
                //new tblBodyPart{PartName="Shoulders"},
                //new tblBodyPart{PartName="Triceps"},
            };
            bodyParts.ForEach(s => context.BodyPart.Add(s));
            context.SaveChanges();
            var trainingType = new List<tblTrainingType>
            {
                //new tblTrainingType{TypeName="Cardio"},
                //new tblTrainingType{TypeName="Balance"},               
                //new tblTrainingType{TypeName="Flexibility"},
                //new tblTrainingType{TypeName="Home"},
                //new tblTrainingType{TypeName="Pilates"},
                //new tblTrainingType{TypeName="Power"},
                //new tblTrainingType{TypeName="Strength"},
                //new tblTrainingType{TypeName="Yoga"},
                //new tblTrainingType{TypeName="Activities"},
            };
            trainingType.ForEach(s => context.TrainingType.Add(s));
            context.SaveChanges();
            var pieceofEquipment = new List<tblPieceOfEquipment>
            {
                 //new tblPieceOfEquipment{PieceName="Barbell"},
                 //new tblPieceOfEquipment{PieceName="Dumbbells"},
                 //new tblPieceOfEquipment{PieceName="Bodyweight"},
                 //new tblPieceOfEquipment{PieceName="Cables"},
                 //new tblPieceOfEquipment{PieceName="Machine"},
                 //new tblPieceOfEquipment{PieceName="Suspension"},
                 //new tblPieceOfEquipment{PieceName="Plate"},
                 //new tblPieceOfEquipment{PieceName="Leg Weights"},
                 //new tblPieceOfEquipment{PieceName="Swiss Ball"},
                 //new tblPieceOfEquipment{PieceName="Bosu"},
                 //new tblPieceOfEquipment{PieceName="Foam Roller"},
                 //new tblPieceOfEquipment{PieceName="Kettle bells"},
                 //new tblPieceOfEquipment{PieceName="Bands"},
                 //new tblPieceOfEquipment{PieceName="Medicine Ball"},
            };
            pieceofEquipment.ForEach(s => context.PieceofEquipment.Add(s));
            context.SaveChanges();
            var exercise = new List<tblExercise>
            {
                 //new tblExercise{ExerciseName="Barbell Bench Press", Index="Chest, Barbell, Strength", VedioLink="xxxxxxxxxxxxxx", Description=Resources.MessagesAndDescription.ExeDesc1},
                 //new tblExercise{ExerciseName="Barbell Bench Press", Index="Chest, Barbell, Strength", VedioLink="xxxxxxxxxxxxxx", 
                 //Description="Lie on a flat bench holding the barbell over chest with your arms straight and hands shoulder width apart. Lower barbell to upper chest level bending at the elbows. Press barbell back up to starting position."},
            };
            exercise.ForEach(s => context.Exercise.Add(s));
            context.SaveChanges();
         var challengeType = new List<tblChallengeType>
            {
                 //new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type1, ChallengeType="Power Challenges:", 
                 //    MaxLimit=1000, Unit="reps", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type2, ChallengeType="Power Challenges:", 
                 //    MaxLimit=20, Unit="rounds", IsExersizeMoreThanOne="Yes"},
                 //     new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type3, ChallengeType="Power Challenges:", 
                 //    MaxLimit=30, Unit="minutes", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type4, ChallengeType="Power Challenges:", 
                 //    MaxLimit=30, Unit="minutes", IsExersizeMoreThanOne="Yes"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type5, ChallengeType="Endurance Challenges:", 
                 //    MaxLimit=0, Unit="", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type6, ChallengeType="Endurance Challenges:", 
                 //    MaxLimit=0, Unit="", IsExersizeMoreThanOne="No"},
                 //     new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type7, ChallengeType="Strength Challenges:", 
                 //    MaxLimit=5, Unit="reps", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type8, ChallengeType="Strength Challenges:", 
                 //    MaxLimit=20, Unit="seconds", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type9, ChallengeType="Cardio Challenges:", 
                 //    MaxLimit=50, Unit="miles", IsExersizeMoreThanOne="No"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type10, ChallengeType="Cardio Challenges:", 
                 //    MaxLimit=120, Unit="minutes", IsExersizeMoreThanOne="no"},
                 //     new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type11, ChallengeType="Cardio Challenges:", 
                 //    MaxLimit=20, Unit="intervals", IsExersizeMoreThanOne="yes"},
                 //    new tblChallengeType{ChallengeSubType=Resources.MessagesAndDescription.Type12, ChallengeType="Cardio Challenges:", 
                 //    MaxLimit=120, Unit="minutes", IsExersizeMoreThanOne="Yes"},               
            };
            challengeType.ForEach(s => context.ChallengeType.Add(s));
            context.SaveChanges();
            var specialization = new List<tblSpecialization>
            {
//                 new tblSpecialization{SpecializationName="Aerobic and Cardiovascular"},
//new tblSpecialization{SpecializationName="Body Building"},
//new tblSpecialization{SpecializationName="Boot Camps"},
//new tblSpecialization{SpecializationName="Bosus"},
//new tblSpecialization{SpecializationName="Boxing Training"},
//new tblSpecialization{SpecializationName="Core and Balance"}, 
//new tblSpecialization{SpecializationName="Cross-Fit"},
//new tblSpecialization{SpecializationName="Endurance"},
//new tblSpecialization{SpecializationName="Flexibility"},
//new tblSpecialization{SpecializationName="Functional Training"},
//new tblSpecialization{SpecializationName="Kettle Bells"},
//new tblSpecialization{SpecializationName="Men's Fitness"},
//new tblSpecialization{SpecializationName="MMA training"},
//new tblSpecialization{SpecializationName="Pilates"},
//new tblSpecialization{SpecializationName="Power & Strength"},
//new tblSpecialization{SpecializationName="Pre and Post-natal"},
//new tblSpecialization{SpecializationName="Pre and Post-rehab"},
//new tblSpecialization{SpecializationName="Rehabilitation"},
//new tblSpecialization{SpecializationName="Self-Defense"},
//new tblSpecialization{SpecializationName="Senior Fitness"},
//new tblSpecialization{SpecializationName="Speed"},
//new tblSpecialization{SpecializationName="Sports/Athlete Training"},
//new tblSpecialization{SpecializationName="Triathlon training"},
//new tblSpecialization{SpecializationName="TRX"},
//new tblSpecialization{SpecializationName="Weight loss"},
//new tblSpecialization{SpecializationName="Women's Fitness"},
//new tblSpecialization{SpecializationName="Yoga"},
            };
            specialization.ForEach(s => context.Specialization.Add(s));
            context.SaveChanges();
        }
    }
}