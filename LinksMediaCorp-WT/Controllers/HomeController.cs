namespace LinksMediaCorp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;

    public class HomeController : Controller
    {
        /// <summary>
        /// Action to Get DashBorad data
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 03/27/2015
        /// </devdoc>
        public ActionResult DashBoard()
        {
            StringBuilder traceLog = null;
            DashBoard model = null;

            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session["UserType"]) != Message.UserTypeAdmin)
            {
                return this.RedirectToAction("Login", "Login");

            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DashBoard controller");
                HttpContext.Session["SelectedTrainerId"] = 0;
                #region Second MiledStone

                /*Get Challenge count from the database*/
                //ViewBag.ChallengeCount = ChallengesCommonBL.GetChallengeCount();
                ///*Get Program count from the database*/
                //ViewBag.ProgramCount = UseresBL.GetProgramCount();

                ///*Get Challenge count from the database*/
                //ViewBag.ActiveChallengeCount = ChallengesCommonBL.GetActiveChallengeCount();

                ///*Get Trainer count from the database*/
                //ViewBag.TrainerCount = TrainersBL.GetTrainerCount();

                ///*Get User count from the database*/
                //ViewBag.UserCount = UseresBL.GetUserCount();

                ///*Get Team count from the database*/
                //ViewBag.TeamCount = TeamBL.GetTeamCount();

                /*Get Activitie count from the database*/
              //  ViewBag.ActivityCount = ActivityBL.GetActivityCount();

                DashBoardActivityCount objactivitycount = ActivityBL.GetDashBoardActivityCount();

                /*Get featured activity queue list from the database*/
                IEnumerable<FeaturedActivityQueue> objListFauturedActivity = ActivityBL.GetFeaturedActivity();

                /*Get COD queue list from the database*/
                IEnumerable<CODQueue> objListCOD = ChallengeOfTheDayBL.GetAllCOD();

                /*Get Sponser challenge queue list from the database*/
                IEnumerable<SponsorChallengeQueue> objListSponsorChallenge = SponsorChallengeBL.GetAllSponsorChallenge();

                /*Bind view model*/
                model = new DashBoard();
                model.FauturedActivityQueue = objListFauturedActivity;
                model.ChallengeOfTheDayQueue = objListCOD;
                model.SponsorChallengeQueue = objListSponsorChallenge;
                model.DashBoardActivity = objactivitycount;
                return this.View(model);
                #endregion
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return this.RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("DashBoard end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete sponsor challenge
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 03/27/2015
        /// </devdoc>
        public ActionResult DeleteSponsorChallenge(int id)
        {
            StringBuilder traceLog = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
               || Convert.ToString(HttpContext.Session["UserType"]) != Message.UserTypeAdmin)
            {
                return this.RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteSponsorChallenge  controller");
                SponsorChallengeBL.DeleteSponsorChallenge(id);
                this.TempData["AlertMessage"] = Message.DeleteMessage;
                return this.RedirectToAction("DashBoard");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteSponsorChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete challenge of the day challenge
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 03/27/2015
        /// </devdoc>
        public ActionResult DeleteCOD(int id)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteCOD  controller");
                ChallengeOfTheDayBL.DeleteCOD(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return this.RedirectToAction("DashBoard");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteCOD end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete featured activity
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 03/27/2015
        /// </devdoc>
        public ActionResult DeleteFeaturedActivity(int id)
        {
            StringBuilder traceLog = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
               || Convert.ToString(HttpContext.Session["UserType"]) != Message.UserTypeAdmin)
            {
                return this.RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteFeaturedActivity  controller");
                ActivityBL.DeleteFeaturedActivity(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return this.RedirectToAction("DashBoard");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteFeaturedActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
    }
}