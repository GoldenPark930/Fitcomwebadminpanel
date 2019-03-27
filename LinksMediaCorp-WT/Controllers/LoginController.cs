namespace LinksMediaCorp.Controllers.Web
{
    using LinksMediaCorpEntity;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.Security.AntiXss;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    public class LoginController : Controller
    {
        /// <summary>
        /// Action to view home page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/17/2015
        /// </devdoc>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        /// <summary>
        /// Action to view login oage
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 03/01/2015
        /// </devdoc>
        public ActionResult Login()
        {
            ViewBag.Title = "Login Page";
            // ViewBag.ReturnUrl=
            return View();
        }

        /// <summary>
        /// Action to loguot
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        public ActionResult LogOut()
        {
            if (LinksMediaCorpBusinessLayer.Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName])))
            {
                return RedirectToAction("Login", "Login");
            }
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: LogOut :");
                LinksMediaCorpBusinessLayer.Login.UpdateLastLogin();
                HttpContext.Session[LinksMediaCorpUtility.Resources.Message.LoginUserName] = string.Empty;
                HttpContext.Session["SelectedTrainerId"] = 0;
                if (HttpContext.Session[LinksMediaCorpUtility.Resources.Message.LoginUserName].Equals(string.Empty))
                    return RedirectToAction("Login", "Login");
                else
                    // return same page
                    return View();
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                // return error page
                return View();
            }
            finally
            {
                traceLog.AppendLine("LogOut end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to Login in to application
        /// </summary>
        /// <param name="model">model collection</param>
        /// <param name="returnUrl">url value</param>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        [HttpPost, ValidateInput(false)]
        [AllowAnonymous]
        public ActionResult Login(Credentials model, string returnUrl)
        {
            StringBuilder traceLog = null;
            Credentials objOut = null;
            EncryptDecrypt objEncrypt = null;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name Password");
                return View(model);
            }
            else
            {
                try
                {
                    traceLog = new StringBuilder();
                    objOut = new Credentials();
                    traceLog.AppendLine("Start: Login, LoginUser Method url:" + returnUrl);
                    string userName = AntiXssEncoder.HtmlEncode(model.UserName.Trim(), false);
                    objEncrypt = new EncryptDecrypt();
                    string password = objEncrypt.EncryptString(AntiXssEncoder.HtmlEncode(model.Password.Trim(), false));
                    bool success = LinksMediaCorpBusinessLayer.Login.ValidateUser(userName, password, ref objOut);
                    if (success)
                    {
                        HttpContext.Session["UserId"] = objOut.UserId;
                        HttpContext.Session["CredentialId"] = objOut.Id;
                        HttpContext.Session["UserType"] = objOut.UserType;
                        HttpContext.Session["LastLogin"] = LinksMediaCorpBusinessLayer.Login.LastLogIn(objOut.LastLogin);
                        HttpContext.Session[LinksMediaCorpUtility.Resources.Message.LoginUserName] = objOut.UserName;
                        FormsAuthentication.SetAuthCookie(userName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase) && !returnUrl.StartsWith("//", StringComparison.OrdinalIgnoreCase) && !returnUrl.StartsWith("/\\", StringComparison.OrdinalIgnoreCase))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            if (objOut.UserType == Message.UserTypeAdmin)
                            {
                                return RedirectToAction("DashBoard", "Home");
                            }
                            else if (objOut.UserType == Message.UserTypeTrainer)
                            {
                                return RedirectToAction("TrainerChallenges", "Reporting");
                            }
                            else if (objOut.UserType == Message.UserTypeTeam)
                            {
                                return RedirectToAction("TeamAdminView", "Reporting");                               
                            }
                            else
                            {
                                return Redirect(returnUrl);
                            }
                        }
                    }
                    else
                    {
                        // objEncrypt.Dispose();
                        ViewBag.Message = LinksMediaCorpUtility.Resources.Message.LoginError;
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return View(model);
                }
                finally
                {
                    objEncrypt.Dispose();
                    traceLog.AppendLine("Login end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function for Controller for password change
        /// </summary>
        /// <param name="model">model collection</param>
        /// <param name="returnUrl">url value</param>
        /// <returns>null</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        [HttpPost, ValidateInput(false)]
        public ActionResult TrainerChangePassword(ChangePassword model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                try
                {
                    traceLog.AppendLine("Start: TrainerChangePassword");
                    bool success = LinksMediaCorpBusinessLayer.Login.UpdatePassword(model);
                    if (!success)
                        ViewBag.StatusMessage = Message.IncorrectUserNameOrPassword;
                    else
                        ViewBag.StatusMessage = Message.PwdChangeSuccessfully;
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    ViewBag.StatusMessage = Message.ErrorPwdChange;
                }
                finally
                {
                    traceLog.AppendLine("TrainerChangePassword end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return View();
        }

        /// <summary>
        /// Action to Trainer change password
        /// </summary>        
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        public ActionResult TrainerChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Action to admin change password
        /// </summary>       
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/19/2015
        /// </devdoc>
        public ActionResult AdminChangePassword()
        {
            return View();
        }
    }
}