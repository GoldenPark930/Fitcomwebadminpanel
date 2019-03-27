using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using System;
using System.Collections.Generic;
using System.Text;
namespace LinksMediaCorpBusinessLayer
{
    public class CommonTrainerBL
    {
        public static List<tblTrainerSpecialization> GetPostedSpecializationsBasedTrainer(PostedSpecializations postedSpecializations, int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedSpecializationsBasedTrainer trainerId:" + trainerId);
                List<tblTrainerSpecialization> trainerSpecializations = new List<tblTrainerSpecialization>();
                /*primary specialization*/
                if (postedSpecializations != null)
                {
                    if (postedSpecializations.PrimarySpecializationIDs != null)
                    {
                        for (int i = 0; i < postedSpecializations.PrimarySpecializationIDs.Count; i++)
                        {
                            tblTrainerSpecialization trainerSpecialization = new tblTrainerSpecialization();
                            trainerSpecialization.SpecializationId = Convert.ToInt32(postedSpecializations.PrimarySpecializationIDs[i]);
                            trainerSpecialization.TrainerId = trainerId;
                            trainerSpecialization.IsInTopThree = 1;
                            trainerSpecializations.Add(trainerSpecialization);
                        }
                    }

                    if (postedSpecializations.SecondarySpecializationIDs != null)
                    {
                        for (int i = 0; i < postedSpecializations.SecondarySpecializationIDs.Count; i++)
                        {
                            tblTrainerSpecialization trainerSpecialization = new tblTrainerSpecialization();
                            trainerSpecialization.SpecializationId = Convert.ToInt32(postedSpecializations.SecondarySpecializationIDs[i]);
                            trainerSpecialization.TrainerId = trainerId;
                            trainerSpecialization.IsInTopThree = 0;
                            trainerSpecializations.Add(trainerSpecialization);
                        }
                    }
                }
                return trainerSpecializations;
            }
            finally
            {
                traceLog.AppendLine("GetPostedSpecializationsBasedTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
    }
}
