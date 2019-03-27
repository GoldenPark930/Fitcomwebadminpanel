namespace LinksMediaCorp.Filters
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Threading;
    using System.Security.Principal;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// Class For preforming Token Validation for API calls
    /// </summary>
    /// <devdoc>
    /// Developer Name - 
    /// Date - 
    /// </devdoc>
    public sealed class TokenValidationFilterAttribute : AuthorizationFilterAttribute
    {
        public Login LoginRepository { get; set; }
        #region public Method
        /// <summary>
        /// overridden method for API Validation
        /// </summary>
        /// <param name="filterContext">Contains filter context</param>
        /// <devdoc>
        /// Developer Name - 
        /// Date - 
        /// </devdoc>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Case that user is authenticated using forms authentication 
            //so no need to check header for basic authentication.
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }
            var authHeader = actionContext.Request.Headers.Authorization;
            string token = string.Empty;
            //string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            string clientIPAddress = ConstantKey.DeviceID;
            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    token = ExtractToken(authHeader);
                    if (CommonWebApiBL.ValidateToken(token, clientIPAddress))
                    {
                        var currentPrincipal = new GenericPrincipal(new GenericIdentity(token), null);
                        Thread.CurrentPrincipal = currentPrincipal;
                        return;
                    }
                }
            }
            HandleUnauthorizedRequest(actionContext);
        }
        /// <summary>
        /// Get Token Id
        /// </summary>
        /// <param name="filterContext">AuthenticationHeaderValue</param>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02-25-2015
        /// </devdoc>
        private string ExtractToken(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {
            //Base 64 encoded string
            var token = authHeader.Parameter;
            return token;
        }
        /// <summary>
        /// Handle Unauthorized Request
        /// </summary>
        /// <param name="actionContext"></param>
        /// /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02-25-2015
        /// </devdoc>
        private static void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(new ServiceResponse<string>() { ErrorMessage = Message.InvalidToken, IsResultTrue = false });
            actionContext.Response.Headers.Add("Web-Authenticate",
                                               "Basic Scheme='LinksMedia' location='about:blank'");
        }
        #endregion
    }
}