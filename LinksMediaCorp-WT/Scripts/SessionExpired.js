//JQuery Function for Session timeout

var setIntervalSetTime;
var setIntervalShowExpired;

    function TableHidden() {
        $("#tbSessionExpired").css("display", "none");
        $("#tbSessionExpired']").css("visibility", "hidden");
    }

    function TableShow() {
        $("#tbSessionExpired").css("display", "block");
        $("#tbSessionExpired").css("visibility", "visible");
    }

    function sessLogOut() {       
        window.location = baseUrlString + "/Login/LogOut";
    }

    function SetTimeextend() {
       
        var url = baseUrlString + "/Reporting/GetUserRecord";
        var name = 'Test';
        jQuery.get(url, { input: name }, function (data) {
        });
        clearInterval(setIntervalSetTime);
        clearInterval(setIntervalShowExpired);
        clearInterval(sess_intervalID);
        sessSetInterval();       
        TableHidden();       
    }

    function SetTimeExpired() {   
        //CheckButtonDisable();
        sessLogOut();
        //TableHidden();
    } 

    function TimeextendOrExpired() {

        setIntervalSetTime = setInterval("SetTime()", 1000);
        setIntervalShowExpired = setInterval("ShowExpired()", (popUpTime * 60 + 1) * 1000);
            TableShow();       
    }

    function ShowExpired() {      
            $("span[id*='spSessionExpired'").css("display", "block");           
            TableHidden();       
    }
   
    //function CheckButtonDisable() {
    
    //    $("input[id*='btnSessionState']").attr("disabled","disabled");
    //}

    //function CheckButtonEnable() {

    //    $("input[id*='btnSessionState']").removeAttr("disabled");
    //}