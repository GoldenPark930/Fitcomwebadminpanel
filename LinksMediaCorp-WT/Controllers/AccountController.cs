namespace LinksMediaCorp.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Mvc;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility.Resources;
    public class AccountController : Controller
    {
        /// <summary>
        /// Action to view change password pop up
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        [HttpPost]
        public ActionResult LoadChangePassword()
        {
            ChangePassword tempModel = new ChangePassword();
            return PartialView("_ChangePasswordPartial", tempModel);
        }

        /// <summary>
        /// Action to change password
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/20/2015
        /// </devdoc>
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    bool isSuccess = Login.UpdatePassword(model);
                    var resp = HttpStatusCode.BadRequest;
                    string respMessage = string.Empty;
                    if (isSuccess == true)
                    {
                        resp = HttpStatusCode.OK;
                        respMessage = Message.PasswordChangeSuccess;
                    }
                    else
                    {
                        resp = HttpStatusCode.BadRequest;
                        respMessage = Message.IncorrectPassword;
                    }
                    return Json(new { status = resp, statusMsg = respMessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = HttpStatusCode.ExpectationFailed, statusMsg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Action to display session expire pop up
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 05/14/2015
        /// </devdoc>
        public ActionResult _SessionExpired()
        {
            return PartialView();
        }
    }
}