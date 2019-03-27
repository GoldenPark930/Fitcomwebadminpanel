//calender on textbox focus
jQuery(function () {   
    jQuery("#DateOfEvent").datepicker({ minDate: 0 });

    // To set mindate in enddate
        function customRange(input) {
            return {
                minDate: (input.id == "EndDate" ? $("#StartDate").datepicker("getDate") : new Date())
            };
        }

    // To set maxdate in startdate
        function customRangeStart(input) {
            return {
                maxDate: (input.id == "StartDate" ? $("#EndDate").datepicker("getDate") : null)
            };
        }

        $(document).ready(function () {

            $('#StartDate').datepicker(
            {
                beforeShow: customRangeStart,
                minDate: 0,
                dateFormat: "yy-mm-dd",
                //changeYear: true
            });

            $('#EndDate').datepicker(
            {
                beforeShow: customRange,
                dateFormat: "yy-mm-dd",
                //changeYear: true
            });
        });
});

//Hide date field if activity is active
jQuery(function () {
    jQuery(function () {
        var d = new Date();
        d.setHours(0, 0, 0, 0);
        var startDate = jQuery('#StartDate').val();
        var endDate = jQuery('#EndDate').val();
        if (Date.parse(startDate) <= Date.parse(d) && Date.parse(d) <= Date.parse(endDate)) {
            jQuery('#StartDate').prop('disabled', true);
            jQuery('#EndDate').prop('disabled', true);
            jQuery('#StartDate').css('background-color', '#EFF0BF');
            jQuery('#EndDate').css('background-color', '#EFF0BF');

        }
    });
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