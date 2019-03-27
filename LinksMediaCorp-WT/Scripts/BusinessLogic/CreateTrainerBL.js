var CreateTrainerALLTeams = new Array(); 
jQuery(function () {

    jQuery(document).delegate("input:text[id*='EnteredTrainerID']", "keyup", function () {
        if (isNaN(this.value)) {
            this.value = "";
            alert('Please enter numeric value.');
        }
    });
    jQuery("#detailschkAssignTeam [id*=PostedTeams_TeamsID]").click(function () {      
        var id = jQuery(this).attr('value');
        try {
            teamId = parseInt(id);
            if (this.checked) {
                if( $.inArray(teamId, CreateTrainerALLTeams) == -1){              
                    CreateTrainerALLTeams.push(teamId);
                }
            }
            else {                
                var i = $.inArray(teamId, CreateTrainerALLTeams);
                if (i != -1) {
                    CreateTrainerALLTeams.splice(i, 1);
                }
            }
            if (CreateTrainerALLTeams.length > 0) {
                jQuery("#TeamId").val(CreateTrainerALLTeams[0]);
            } else {
                jQuery("#TeamId").val(null);
            }
        }
        catch (err) {
            alert(err.message);
        }       
        //try {
        //    var primaryNo = id.replace('PostedTeams_TeamsID', '');
        //    var seconderyNo = parseInt(primaryNo) + 22;
        //    var secondryId = '#PostedMobileCoachTeams_TeamsID' + seconderyNo;
        //    if (this.checked) {

        //        jQuery(secondryId).prop("disabled", true);
        //        jQuery(secondryId).prop("checked", false);
        //    }
        //    else {
        //        jQuery(secondryId).prop("disabled", false);
        //    }
        //}
        //catch (err) {
        //    alert(err.message);
        //}

    });
});

window.onerror = function (message, url, lineNo) {
   // console.log('Error: ' + message + '\n' + 'Line Number: ' + lineNo);

    return true;
}