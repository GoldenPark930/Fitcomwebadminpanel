var UpdateTrainerALLTeams = new Array();
jQuery(function () {
    jQuery(document).delegate("input:text[id*='EnteredTrainerID']", "keyup", function () {
        if (isNaN(this.value)) {
            this.value = "";
            alert('Please enter numeric value.');
        }
    });
    jQuery("#detailschkAssignTeam [id*=PostedTeams_TeamsID]:checked").each(function () {       
        var id = jQuery(this).attr('value');
        try {
            teamId = parseInt(id);
            if (this.checked) {
                if ($.inArray(teamId, UpdateTrainerALLTeams) == -1) {            
                    UpdateTrainerALLTeams.push(teamId);
                }
            }
            if (UpdateTrainerALLTeams.length > 0) {
                jQuery("#PrimaryTeamId").val(UpdateTrainerALLTeams[0]);
            } 
        }
        catch (err) {
            alert(err.message);
        }

    });
});

jQuery(function () {
    jQuery("#detailschkAssignTeam [id*=PostedTeams_TeamsID]").click(function () {       
        var id = jQuery(this).attr('value');
        try {
            teamId = parseInt(id);
            if (this.checked) {
                if ($.inArray(teamId, UpdateTrainerALLTeams) == -1) {               
                    UpdateTrainerALLTeams.push(teamId);
                }
            }
            else {               
                var i = $.inArray(teamId, UpdateTrainerALLTeams);
                if (i != -1) {
                    UpdateTrainerALLTeams.splice(i, 1);
                }               
            }
            if (UpdateTrainerALLTeams.length > 0) {
                jQuery("#PrimaryTeamId").val(UpdateTrainerALLTeams[0]);
            } else {
                jQuery("#PrimaryTeamId").val(null);
            }
        }
        catch (err) {
            alert(err.message);
        }

    });
});