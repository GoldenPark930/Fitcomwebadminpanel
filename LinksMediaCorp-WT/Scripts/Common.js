//var baseUrlString = "/LinksMediaWeb";
//var baseUrlString = "";
var baseUrlString = "/LinksMediaCorp-WTML15P4"; //LinksMediaCorp-WTML9
//var baseUrlString = "";
//var baseUrlString = "/test";

jQuery(function () {
    jQuery('.NoRecord').html('No record found.');
});



//Start Description of input maxlimit
//DesCription = 500
//Address = URL = 200
//TName = AName = CName = 100
//Email = PromotionText = 50
//UName = 30
//FName = LName = 20 
//Password = 16
//Zip = 5
//Height = Weight = 3
//Start Description of input maxlimit

//function for loading at the time of trainer update and create
jQuery(function () {
    $(document).ready(function () {
        $("#myform").submit(function (e) {
            $("#myLoadingElement").show();
        });
    });
});