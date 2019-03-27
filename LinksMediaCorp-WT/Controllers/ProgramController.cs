using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using System;
using System.Text;
using System.Web.Mvc;
using LinksMediaCorpUtility.Resources;
namespace LinksMediaCorp.Controllers
{
    public class ProgramController : Controller
    {
        /// <summary>
        /// View challenges on based on assigned Program
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewChallenge()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: ViewChallenge  controller");
                int challengeId = 0;
                challengeId = Convert.ToInt32(Request.QueryString["id"]);
                ViewWorkoutDetailVM objChallenge = new ViewWorkoutDetailVM();
                if (challengeId > 0)
                {
                    objChallenge = ProgramBL.GetProgramWorkoutById(challengeId);
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("ViewChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// View ViewProgram based on Program Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewProgram()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: ViewProgram controller");
                int challengeId = 0;
                challengeId = Convert.ToInt32(Request.QueryString["id"]);
                ViewProgramDetail objChallenge = new ViewProgramDetail();
                if (challengeId > 0)
                {
                    objChallenge = ProgramBL.GetProgramById(challengeId);
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("ViewProgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
    }
}