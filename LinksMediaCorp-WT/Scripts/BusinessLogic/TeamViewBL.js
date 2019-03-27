//Hide date field if activity is active
jQuery(function () {
    jQuery(function () {
        var d = new Date();
        d.setHours(0, 0, 0, 0);
        var startDate = jQuery('#SearchStartDate').val();
        var endDate = jQuery('#SearchEndDate').val();      
        if (Date.parse(startDate) <= Date.parse(d) && Date.parse(d) <= Date.parse(endDate)) {
            jQuery('#SearchStartDate').prop('disabled', true);
            jQuery('#SearchEndDate').prop('disabled', true);
            jQuery('#SearchStartDate').css('background-color', '#EFF0BF');
            jQuery('#SearchEndDate').css('background-color', '#EFF0BF');
            
        }
    });
});

$(document).ready(function () {
    var isShownCurrentMonth = $('#IsShowCurrentMonth').val();
    if (isShownCurrentMonth !== undefined && isShownCurrentMonth === "False") {
        $('#CommissionShowHide').show();
    } else {
        $('#CommissionShowHide').hide();
    }
    jQuery('#btnGenerateTeamCommission').on("click", function () {
        $("#myLoadingElement").show();

        return false;
    });
    jQuery('#btnTeamViewExport').on("click", function () {
    var urlExportTeamViewUrl = baseUrlString + "/Reporting/ExportTeamViewCommissionExcelData";
    var teamCommissionMonth = {
        TeamId: jQuery("#PrimaryTeam").val(),
        Month: jQuery("#SearchMonth :selected").val(),
        Year: jQuery("#AdminTeamSearchYear :selected").val()
    };
    $.ajax({
        type: "POST",
        url: urlExportTeamViewUrl,
        data: teamCommissionMonth,
        dataType: "json",
        beforeSend: function () { },
        success: function (data) {         
         
        },
        error: function (err) {
           
        }
    });
    });
    jQuery('#SearchPrimaryTeam').on("change", function () {

        var urlTeamCoomissionYear = baseUrlString + "/Reporting/GetTeamCommissionYear";    
        var teamCommissionYear = {
            TeamId: jQuery("#SearchPrimaryTeam :selected").val(),
            Month: jQuery("#SearchMonth :selected").val(),
            Year: jQuery("#SearchYear :selected").val()
        };
       
        $.ajax({
            type: "POST",
            url: urlTeamCoomissionYear,
            data: teamCommissionYear,
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                var items = '<option value="0">Select Year</option>';
                jQuery.each(data, function (i, teamYear) {
                    items += "<option value='" + teamYear.Year + "'>" + teamYear.Year + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery('#SearchYear').html(items);
            },
            error: function (err) {

            }
        });
    });
    // Search months based on year
    jQuery('#SearchYear').on("change", function () {
        var urlTeamCoomissionMonth = baseUrlString + "/Reporting/GetTeamCommissionMonth";
        var teamCommissionMonth = {
            TeamId: jQuery("#SearchPrimaryTeam :selected").val(),
            Month: jQuery("#SearchMonth :selected").val(),
            Year: jQuery("#SearchYear :selected").val()
        };
        $.ajax({
            type: "POST",
            url: urlTeamCoomissionMonth,
            data: teamCommissionMonth,
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                var items = '<option value="0">Select Month</option>';
                jQuery.each(data, function (i, teammonth) {
                    items += "<option value='" + teammonth.Month + "'>" + teammonth.Name + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery('#SearchMonth').html(items);
            },
            error: function (err) {

            }
        });
    });

    jQuery('#AdminTeamSearchYear').on("change", function () {
        var urlTeamCoomissionMonth = baseUrlString + "/Reporting/GetTeamCommissionMonth";
        var teamCommissionMonth = {
            TeamId: jQuery("#PrimaryTeam").val(),
            Month: jQuery("#SearchMonth :selected").val(),
            Year: jQuery("#AdminTeamSearchYear :selected").val()
        };
       
        $.ajax({
            type: "POST",
            url: urlTeamCoomissionMonth,
            data: teamCommissionMonth,
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                var items = '<option value="0">Select Month</option>';
                jQuery.each(data, function (i, teammonth) {
                    items += "<option value='" + teammonth.Month + "'>" + teammonth.Name + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery('#SearchMonth').html(items);
            },
            error: function (err) {

            }
        });
    });
});