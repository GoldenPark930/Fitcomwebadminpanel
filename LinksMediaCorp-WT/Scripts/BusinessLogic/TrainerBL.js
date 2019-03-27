
//hide secondery specialization on prymary specialization check
jQuery(function () {
    jQuery("#detailschk1 [id*=PostedSpecializations_PrimarySpecializationIDs]").click(function () {
        var maxSelection = 3;

        jQuery("#PrimarySpecializationCheck").val(null);
        if (jQuery("#detailschk1 [id*=PostedSpecializations_PrimarySpecializationIDs]:checked").length > 0) {
            jQuery("#PrimarySpecializationCheck").val("Checked");
        }

        if (jQuery("#detailschk1 [id*=PostedSpecializations_PrimarySpecializationIDs]:checked").length > maxSelection) {
            jQuery(this).prop("checked", false);
            alert("Please select a maximum of " + maxSelection + " items.");
        }
        else {
            var id = jQuery(this).attr('id');
            try {
                var primaryNo = id.replace('PostedSpecializations_PrimarySpecializationIDs', '');
                var seconderyNo = parseInt(primaryNo) + 27;
                var secondryId = '#PostedSpecializations_SecondarySpecializationIDs' + seconderyNo;
                if (this.checked) {
                    jQuery(secondryId).prop("disabled", true);
                    jQuery(secondryId).prop("checked", false);
                }
                else {
                    jQuery(secondryId).prop("disabled", false);
                }
            }
            catch (err) {
                alert(err.message);
            }

        }
    });

    //jQuery("#detailschkAssignTeam [id*=PostedTeams_TeamsID]").click(function () {           
    //        var id = jQuery(this).attr('id');
    //        try {
    //            var primaryNo = id.replace('PostedTeams_TeamsID', '');
    //            var seconderyNo = parseInt(primaryNo) + 22; 
    //            var secondryId = '#PostedMobileCoachTeams_TeamsID' + seconderyNo;
    //            if (this.checked) {
    //                jQuery(secondryId).prop("disabled", false);
    //                jQuery(secondryId).prop("checked", true);
    //            }
    //            else {
    //                jQuery(secondryId).prop("disabled", false);
    //                jQuery(secondryId).prop("checked", false);
    //            }
                
    //        }
    //        catch (err) {
    //            alert(err.message);
    //        }        
    //});
});


//Check user name from database
jQuery(function () {
    jQuery('#UserName').blur(function () {
        var name;
        var url = baseUrlString + "/Reporting/GetUserRecord";
        if ($('#TrainerId').val() === undefined) {
            name = jQuery('#UserName').val();
        }
        else {
            name = jQuery('#UserName').val() + ',' + $('#TrainerId').val();
        }     
        
        jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
                jQuery("#resultUser").html("<span style='color:red'>User Name already exist!</span>");
                jQuery("#UserName").css('background-color', '#e97878');
                jQuery('#CheckUser').val('NotAvailable');
            }
            else {
                jQuery("#resultUser").html(null);
                jQuery("#UserName").css('background-color', '');
                jQuery('#CheckUser').val(null);
                
            }
        });
    })
});

//Check email from database
jQuery(function () {
    jQuery('#EmailId').blur(function () {
        var name;
       
        if ($('#TrainerId').val() === undefined) {
            name = $('#EmailId').val();
        }
        else {

            name = $('#EmailId').val() + ',' + $('#TrainerId').val();
        }
        var url = baseUrlString + "/Reporting/GetTrainerEmail";       
        jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
                jQuery("#resultEmail").html("<span style='color:red'>Email already exist!</span>");
                jQuery("#EmailId").css('background-color', '#e97878');
                jQuery('#CheckEmail').val('NotAvailable');
            }
            else {
                jQuery("#resultEmail").html(null);
                jQuery("#EmailId").css('background-color', '');
                jQuery('#CheckEmail').val(null);                
            }
        });
    })
});

//Check team name from database
jQuery(function () {
    jQuery('#TeamName').blur(function () {
        var name;
        var url = baseUrlString + "/Reporting/GetTeamRecord";

        if ($('#TrainerId').val() === undefined) {
            name = jQuery('#TeamName').val();
        }
        else {

            name = $('#TeamName').val() + ',' + $('#TrainerId').val();
        }

        jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
                jQuery("#resultTeam").html("<span style='color:red'>Team already exist!</span>");
                jQuery("#TeamName").css('background-color', '#e97878');
                jQuery('#CheckTeam').val('NotAvailable');
            }
            else {
                jQuery("#resultTeam").html(null);
                jQuery("#TeamName").css('background-color', '');
                jQuery('#CheckTeam').val(null);
               
            }
        });
    })
});

//Retrieve cities on state change
jQuery(function () {
    jQuery('#State').change(function () {
        $('#loadingmessage').show();
        $('#City').prop('disabled', true);
        jQuery('#City').css('background-color', '#FAF9F9');

        var id = jQuery("#State :selected").val();
        var url = baseUrlString + "/Reporting/GetCities";
        if (id != "") {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: url + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {
                    //$("#progressbar").progressbar({
                    //    value: data.progressbar
                    //});
                    $('#loadingmessage').hide();
                    $('#City').prop('disabled', false);
                    jQuery('#City').css('background-color', '#FFFFFF');

                    var items = '<option value="">--Choose City--</option>';
                    jQuery.each(data, function (i, city) {
                        items += "<option value='" + city.Value + "'>" + city.Text + "</option>";
                    });
                    // Fill ChallengeSubType Dropdown list
                    jQuery('#City').html(items);
                },
                error: function (result) {

                    $('#loadingmessage').hide();
                    $('#City').prop('disabled', false);
                    jQuery('#City').css('background-color', '#FFFFFF');

                    $('#loadingmessage').hide();
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
        }
        else {
            $('#loadingmessage').hide();
            $('#City').prop('disabled', false);
            jQuery('#City').css('background-color', '#FFFFFF');

            var items = '<option value="">--Choose City--</option>';
            jQuery('#City').html(items);
        }
    });
});

//Hide secondary specialization which is checked on load
jQuery(function () {
    jQuery("#detailschk1 [id*=PostedSpecializations_PrimarySpecializationIDs]:checked").each(function () {

        var id = jQuery(this).attr('id');
        try {
            var primaryNo = id.replace('PostedSpecializations_PrimarySpecializationIDs', '');
            var seconderyNo = parseInt(primaryNo) + 27;
            var secondryId = '#PostedSpecializations_SecondarySpecializationIDs' + seconderyNo;
            if (this.checked) {
                jQuery(secondryId).prop("disabled", true);
                jQuery(secondryId).prop("checked", false);
            }
            else {
                jQuery(secondryId).prop("disabled", false);
            }
        }
        catch (err) {
            alert(err.message);
        }

    });
});

//function for enable password block
jQuery(function () {
    jQuery('#IsChangePassword').change(function () {
        if (this.checked) {
            jQuery('#divChangePassword').show();          
        }
        else {
            jQuery('#divChangePassword').hide();           
        }
    });
    jQuery('#IsChangePassword').load(function () {
        if (this.checked) {
            jQuery('#divChangePassword').show();         
        }
        else {
            jQuery('#divChangePassword').hide();          
        }
    });
});
