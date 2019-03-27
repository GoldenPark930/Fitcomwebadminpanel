namespace LinksMediaCorp
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Programcss").Include(
                     "~/Content/bootstrap.min.css"
                     ));
            bundles.Add(new StyleBundle("~/Content/stylesheet").Include(
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/bootstrap-theme.css",
                        "~/Content/css/bootstrap-glyphicons.css" ,
                        "~/Content/css/signin.css",
                      "~/Content/bootstrap-multiselect.css"));
            bundles.Add(new ScriptBundle("~/bundles/homejquery").Include(                       
                        "~/Scripts/Common.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/ui").Include(  
                        "~/Scripts/jquery-ui.js",   
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/bootstrap-2.3.2.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/validation").Include(                       
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));
            bundles.Add(new ScriptBundle("~/bundles/sessionexpire").Include(                       
                        "~/Scripts/SessionExpired.js"));

            bundles.Add(new StyleBundle("~/Content/challenge").Include("~/Content/css/ui-lightness/jquery-ui-1.10.4.custom.min.css"));    
        
            bundles.Add(new ScriptBundle("~/bundles/user").Include("~/Scripts/BusinessLogic/UserBL.js"));
            bundles.Add(new ScriptBundle("~/bundles/activity").Include("~/Scripts/BusinessLogic/ActivityBL.js"));
            bundles.Add(new ScriptBundle("~/bundles/TeamView").Include("~/Scripts/BusinessLogic/TeamViewBL.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/trainer").Include(
                                         //"~/Scripts/file-upload.js",
                                       "~/Scripts/ckeditor/ckeditor.js",
                                      "~/Scripts/BusinessLogic/TrainerBL.js"
                                    ));

            bundles.Add(new ScriptBundle("~/bundles/challenge").Include(
                       "~/Scripts/jquery.maskedinput.js",
                       "~/Scripts/BusinessLogic/ChallengeBL.js",
                       "~/Scripts/bootstrap-multiselect.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/Workout").Include(
                     "~/Scripts/jquery.maskedinput.js",
                     "~/Scripts/BusinessLogic/WorkoutBL.js",
                     "~/Scripts/bootstrap-multiselect.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/Programchallenge").Include(                     
                      "~/Scripts/BusinessLogic/ProgramBL.js",
                      "~/Scripts/croppie.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/FeatureImageUpload").Include(                    
                      "~/Scripts/croppie.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/FreeFormchallenge").Include(
                     "~/Scripts/BusinessLogic/FreeFormChallengeBL.js",
                     "~/Scripts/bootstrap-multiselect.js"));

            bundles.Add(new ScriptBundle("~/bundles/CreateTrainercroppie").Include(
                      "~/Scripts/croppie.js",
                      "~/Scripts/CreateTrainercroppie.js"));
          
            bundles.Add(new ScriptBundle("~/bundles/CreateTeamcroppie").Include(
                   "~/Scripts/croppie.js",
                   "~/Scripts/BusinessLogic/CreateTeamBL.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/UpdateTrainercroppie").Include(
                    "~/Scripts/croppie.js",
                    "~/Scripts/BusinessLogic/TrainerImageUpdate.js"));
            bundles.Add(new ScriptBundle("~/bundles/UpdateTeamcroppie").Include(
                    "~/Scripts/croppie.js",
                    "~/Scripts/BusinessLogic/UpdateTeamBL.js"));
            

            bundles.Add(new StyleBundle("~/Content/croppie").Include(
                     "~/Content/croppie.css",
                     "~/Content/TrainerCropie.css"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/queue").Include("~/Scripts/BusinessLogic/Queues.js"));
           

            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                      "~/Scripts/jquery.form.min.js"));           

            
            BundleTable.EnableOptimizations = true;
        }
    }
}