//Get trainer result list for any challenge from the database
// Jquery Function For Update Challenge UI
jQuery(function () {
    jQuery('#TrainerId').change(function () {

        $('#loadingTrainerResult').show();
        $('#ResultId').prop('disabled', true);
        jQuery('#ResultId').css('background-color', '#FAF9F9');
        var trainerId = jQuery("#TrainerId :selected").val();
        var id = trainerId + "," + jQuery('#ChallengeId').val();
        var urlresult = baseUrlString + "/Reporting/GetResults";
        if (trainerId != "") {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlresult + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {

                    $('#loadingTrainerResult').hide();
                    $('#ResultId').prop('disabled', false);
                    jQuery('#ResultId').css('background-color', '#FFFFFF');

                    var items = '<option value="">--Choose Result--</option>';
                    jQuery.each(data, function (i, challenge) {
                        items += "<option value='" + challenge.Value + "'>" + challenge.Text + "</option>";
                    });
                    // Fill ResultId Dropdown list
                    jQuery('#ResultId').html(items);

                },
                error: function (result) {

                    $('#loadingTrainerResult').hide();
                    $('#ResultId').prop('disabled', false);
                    jQuery('#ResultId').css('background-color', '#FFFFFF');

                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
        }
        else {
            $('#loadingTrainerResult').hide();
            $('#ResultId').prop('disabled', false);
            jQuery('#ResultId').css('background-color', '#FFFFFF');

            var items = '<option value="">--Choose Result--</option>';
            jQuery('#ResultId').html(items);
        }
    });
});

//Get trainer result list for any challenge from the database
jQuery(function () {
    jQuery('#EndUserNameId').change(function () {

        $('#loadingUserResult').show();
        $('#UserResultId').prop('disabled', true);
        jQuery('#UserResultId').css('background-color', '#FAF9F9');
        var userId = jQuery("#EndUserNameId :selected").val();
        var id = userId + "," + jQuery('#ChallengeId').val();
        var urlresult = baseUrlString + "/Reporting/GetUserResults";
        if (userId != "") {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlresult + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {

                    $('#loadingUserResult').hide();
                    $('#UserResultId').prop('disabled', false);
                    jQuery('#UserResultId').css('background-color', '#FFFFFF');

                    var items = '<option value="">--Choose Result--</option>';
                    jQuery.each(data, function (i, challenge) {
                        items += "<option value='" + challenge.Value + "'>" + challenge.Text + "</option>";
                    });
                    // Fill ResultId Dropdown list
                    jQuery('#UserResultId').html(items);

                },
                error: function (result) {

                    $('#loadingUserResult').hide();
                    $('#UserResultId').prop('disabled', false);
                    jQuery('#UserResultId').css('background-color', '#FFFFFF');

                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
        }
        else {
            $('#loadingUserResult').hide();
            $('#UserResultId').prop('disabled', false);
            jQuery('#UserResultId').css('background-color', '#FFFFFF');

            var items = '<option value="">--Choose Result--</option>';
            jQuery('#UserResultId').html(items);
        }
    });
});