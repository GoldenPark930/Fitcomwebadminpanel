//global variable declaration
var dataUnit = "";
var Serach1Equipment = [];
var Serach1ExerciseType = [];
var Serach1TrainingZone = [];
var FFSelectIndex = []
var AllEquipment = new Array();
var ALLExerciseType = new Array();
var AllTrainingZone = new Array();
var max_fields = 999999;

var totalFilterexerciseRecords = new Array();
var totalexerciseRecords = new Array();
var isSerached = false;
var excount = 1; //initlal text box count
//hide and show exercises on page load on the basis of challenge type
jQuery(function () {
    ShowHideExecise();
    $('#FreeFormChallengeExeciseSection').hide();
    $('#FreeFormChallegeDescriptionHeaderSection').hide();
    $('#divChallengeCategorySection').hide();
    $('#FreeFormChallegeDetailsSection').hide();
    $('#challengeNoTrainerTeam').hide();
    $('#ChallenegeEquipmentRequirementHeader').text(" Select Equipment Requirements:");
});

jQuery(function () {
    var selectedChallengeTypeId = $("#ChallengeType option:selected").val();
    if (selectedChallengeTypeId == 13) {
        ShowFFChallengeSection();
        $('#TrendingCategoryHeader').text("Step 12 - Select Your Trending Category:");
    }
    else {
        HideFFChallengeSection();
    }
    // Hide/show based on Workout challenge 

    var selectedChallengeSubTypeId = $("#ChallengeSubTypeId option:selected").val();
    if (selectedChallengeSubTypeId == 14) {
        $('#divchkSubscription').show();
        $('#divchkFreeFitnessTest').hide();
        $('#TrendingCategoryHeader').text("Step 13 - Select Your Trending Category:");
        $('#divFittnessTrendingCategorySection').hide();
        $('#divTrendingCategorySection').show();
    }
    else if (selectedChallengeSubTypeId == 15) {
        $('#divchkSubscription').hide();
        $('#divchkFreeFitnessTest').hide();
        $('#divChallengeCategorySection').hide();

        $('#divFittnessTrendingCategorySection').hide();
        $('#divTrendingCategorySection').hide();
    }
    else {
        // $('#divchkSubscription').hide();
        $('#divchkFreeFitnessTest').show();
        $('#challengeNoTrainerTeam').show();
        $('#divFittnessTrendingCategorySection').show();
        $('#divTrendingCategorySection').hide();
        $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");

    }

});
//Validation variable value parameter
jQuery(function () {
    jQuery("#VariableValue").keyup(function () {
        var strMax = document.getElementById("lblValue").innerHTML;
        var input = this.value;
        if (jQuery("#VariableUnit").val() == "minutes") {
            var timeMinutes = input.split(":");
            var input = parseInt(timeMinutes[0]) * 60 * 60 + parseInt(timeMinutes[1]) * 60 + parseInt(timeMinutes[2]);
            //subString = parseFloat(subString) * 60;
        }
        if (parseInt(input) < 0) {
            this.value = '';
            maxLimitValidation.innerHTML = "Please enter valid value ";
            this.focus();
        } else {
            maxLimitValidation.innerHTML = "";
        }
        maxLimitValidation.innerHTML = "";

    });
});

//Validation variable value parameter
jQuery(function () {
    jQuery(document).delegate("input:text[id*='Reps']", "keyup", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    jQuery("[id*=ExecSetRep]").keyup(function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    jQuery("[id*=WeightForMan]").keyup(function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });

    jQuery("[id*=WeightForWoman]").keyup(function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    jQuery("#ResultWeightorDestance").keyup(function () {
        if (jQuery("#ResultUnitType").val() !== "Time") {
            if (isNaN(this.value)) {
                this.value = "";
                alert('Please enter numeric value.');
            } else {
                if (this.value != "") {
                    var value = this.value.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                    var intRegex = /\b[1-9][0-9]{0,2}\b/;
                    if (!intRegex.test(value)) {
                        this.value = "";
                        alert('Please enter numeric value between 1 and 999.');
                    }
                }
            }
        }
    });
    jQuery("#GlobalResultFilterValue").keyup(function (event) {
        var value = this.value.replace(/^\s\s*/, '').replace(/\s\s*$/, '');

        // var intRegex = /^(?:\d*\.\d{0,2}|\d+)$/
        var intRegex = /^(-?\d*)((\.(\d{0,2})?)?)$/
        if (!intRegex.test(value)) {
            this.value = "";
            alert('Please enter valid value.');
        }
    });
    $('#SelectedFitcomEquipment1,#SelectedFitcomTrainingZone1,#SelectedFitcomExeciseType1').on("change", function () {
        $('#ExeName1').trigger("focus");
    });

});
//Auto retrieve exrecise from database 
jQuery(function () {
    var urluser = baseUrlString + "/Reporting/GetUsers";
    var urlexe = baseUrlString + "/Reporting/GetExercise";
    //geting end user from the database
    jQuery("#EndUserName").autocomplete({
        source: urluser,
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblEndUserName").html("Please select User from list!");
                //this.focus();
            } else {
                jQuery("#lblEndUserName").html("");
                jQuery("#EndUserName").val(this.value.trim());
            }
        }
    });

    //**********Autopcomplete with url clickable starts for exercise 1*******

    $('#ExeName1').autocomplete({
        autoFocus: true,
        source: function (request, response) {

            var urlexe = baseUrlString + "/Reporting/GetSelectedExercises";
            var SelectedEquipement = "";
            var SelectedTrainingZone = "";
            var SelectedExeciseType = "";
            var drpdownEquipmentNameValues = jQuery('#SelectedFitcomEquipment1').find('option:selected').text();
            if (drpdownEquipmentNameValues !== undefined && drpdownEquipmentNameValues !== null && drpdownEquipmentNameValues !== "" && drpdownEquipmentNameValues !== "--Choose Equipments--") {
                SelectedEquipement = drpdownEquipmentNameValues;
            }
            var drpdownTrainingZonesValues = jQuery('#SelectedFitcomTrainingZone1').find('option:selected').text();
            if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "--Choose Training Zones--") {
                SelectedTrainingZone = drpdownTrainingZonesValues;
            }
            var drpdownExerciseTypes = jQuery('#SelectedFitcomExeciseType1').find('option:selected').text();
            if (drpdownExerciseTypes !== undefined && drpdownExerciseTypes !== null && drpdownExerciseTypes !== "" && drpdownExerciseTypes !== "--Choose Exercise Types--") {
                SelectedExeciseType = drpdownExerciseTypes;
            }
            var model = {
                SelectedEquipement: SelectedEquipement,
                SelectedTrainingZone: SelectedTrainingZone,
                SelectedExeciseType: SelectedExeciseType,
                SearchTerm: request.term
            };
            $.ajax({
                type: "POST",
                url: urlexe,
                data: model,
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ExerciseName,
                            value: item.ExerciseName,
                            url: item.VedioLink,
                            ExerciseId: item.ExerciseId
                        };
                    }));
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            //alert($(this).attr("id"));
            jQuery("#ExeVideoLink1").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');
            jQuery("#lblExe1").html("");
            jQuery('#ExeName2').prop('disabled', false);
            jQuery('#ExeDesc2').prop('disabled', false);
            if (!ui.item) {
                $("#ExerciseId" + controlNumber).val(0);
            } else {
                var selectedExerciseIdId = parseInt(ui.item.ExerciseId);
                $("#ExerciseId1" ).val(selectedExerciseIdId);

            }
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe1").html("No exercise found for the selected values.");
                jQuery("#spnExeName1").html("");
                jQuery("#lblExe2").html("");
                jQuery("#lblExe3").html("");
                jQuery("#lblExe4").html("");
                jQuery('#ExeName2').val("");
                jQuery('#ExeDesc2').val("");
                jQuery('#ExeName2').prop('disabled', true);
                jQuery('#ExeDesc2').prop('disabled', true);
                jQuery('#ExeName3').val("");
                jQuery('#ExeDesc3').val("");
                jQuery('#ExeName3').prop('disabled', true);
                jQuery('#ExeDesc3').prop('disabled', true);
                jQuery('#ExeName4').val("");
                jQuery('#ExeDesc4').val("");
                jQuery('#ExeName4').prop('disabled', true);
                jQuery('#ExeDesc4').prop('disabled', true);
                jQuery("#ExeVideoLink1").html("");
                jQuery("#ExeVideoLink2").html("");
                jQuery("#ExeVideoLink3").html("");
                jQuery("#ExeVideoLink4").html("");
                GetFractionList(1);
                this.focus();

            } else {
                jQuery("#lblExe1").html("");
                jQuery("#spnExeName1").html("");
                jQuery('#ExeName2').prop('disabled', false);
                jQuery('#ExeDesc2').prop('disabled', false);
                var selectedExerciseIdId = parseInt(ui.item.ExerciseId);
                $("#ExerciseId1").val(selectedExerciseIdId);
            }
        },
        open: function () {
            $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
        },
        close: function () {
            $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
        }
    }).on('focus', function () {
        $(this).keydown();
    })
        .data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };
});
//validation and text change on challeneg sub-type change
jQuery(function () {
    jQuery('#ChallengeSubTypeId').on("change", function () {
        $('#loadingChallengeCategory').show();
        var id = jQuery("#ChallengeSubTypeId :selected").val();
        if (id != "") {
            var urlchallenge = baseUrlString + "/Reporting/GetValueypeBySubType";
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlchallenge + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {
                    jQuery('#VariableUnit').val(data.variableUnit);
                    jQuery('#IsMoreThenOne').val(data.isMoreThanOne);
                    data.Result = data.Result.replace("# of minutes :", "Amount of time:");
                    jQuery('#lblValue').html(data.Result);
                    //jQuery('#lblGlobalResultFilterValue').html(data.Result);                    
                    var strMax = document.getElementById("lblValue").innerHTML;
                    var subString = data.MaxLimit;
                    jQuery("#VariableLimit").val(data.Result);
                    jQuery("#ResultUnitType").val(data.resultUnit);
                    jQuery('#VariableValue').val(null);
                    //jQuery('#GlobalResultFilterValue').val(null);
                    maxLimitValidation.innerHTML = "";
                    if (parseInt(subString) == 0) {
                        jQuery("#VariableValue").prop('disabled', true);
                        jQuery("#VariableValue").css('background-color', '#EFF0BF');
                        var lastIndex = data.Result.lastIndexOf('-');
                        jQuery('#lblValue').html(data.Result.substring(0, lastIndex) + ':');

                    } else {
                        jQuery("#VariableValue").prop('disabled', false);
                        jQuery("#VariableValue").css('background-color', '#ffffff');
                    }

                    //trainer result level
                    jQuery('#trainerResult').html("Step 4 - Enter Your #" + data.resultUnit + " (Optional):");
                    //show div which contain trainer result input textboxes
                    jQuery('#inputDiv').show();

                    jQuery('#ResultTime').val("NotValidate");
                    jQuery('#ResultWeightorDestance').val(0.0);
                    jQuery('#ResultRepsRound').val(0);
                    // jQuery('#GlobalResultFilterValue').val("");
                    ManageReps(data.resultUnit, data.variableUnit);
                    jQuery('#ResultTime').hide();
                    jQuery('#ResultWeightorDestance').hide();
                    jQuery('#ResultRepsRound').hide();
                    jQuery('#ResultFrection').hide();
                    if (data.variableUnit == "minutes") {
                        // jQuery("#VariableValue").mask("99:99:99", { placeholder: "HH:MM:SS" });
                        jQuery('#VariblesMinute-Section').show();
                        jQuery('#ResultFilterMinute-Section').show();
                        jQuery("#otheVariableValue").hide();
                        jQuery('#GlobalResultFilterValue').val(null);
                        //jQuery("#otheGlobalResultFilterValue").hide();
                    } else {
                        jQuery('#VariblesMinute-Section').hide();
                        jQuery('#ResultFilterMinute-Section').hide();
                        jQuery("#otheVariableValue").show();
                        //jQuery("#otheGlobalResultFilterValue").show();
                        jQuery('#lblotherValue').html(data.Result);
                    }
                    if (data.resultUnit == "Time") {
                        jQuery('#ResultTime').show();
                        jQuery('#otheGlobalResultFilterValue').hide();
                        jQuery('#ResultTime').val(null);
                        jQuery('#GlobalResultFilterValue').val(null);
                    } else if (data.resultUnit == "Weight" || data.resultUnit == "Distance") {
                        jQuery('#ResultWeightorDestance').show();
                        jQuery('#otheGlobalResultFilterValue').show();
                        jQuery('#ResultWeightorDestance').val(null);
                    } else if (data.resultUnit == "Reps" || data.resultUnit == "Rounds" || data.resultUnit == "Interval") {
                        jQuery('#ResultRepsRound').show();
                        jQuery('#otheGlobalResultFilterValue').show();
                        jQuery('#ResultRepsRound').val(null);
                    }
                    if (data.resultUnit == "Interval" || data.resultUnit == "Rounds") {
                        jQuery('#ResultFrection').show();
                        jQuery('#otheGlobalResultFilterValue').show();
                        dataUnit = data.resultUnit;
                        GetFractionList(1);
                    }
                    // Bind the selected challenge category list

                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
        }
        // Show and hide of FreeFormChallegeDetails based on Free form challenge sub type-Free Form Wellness
        if (id === "15") {
            ShowFFWellnessCChallengeSection();
            $('#TrendingCategoryHeader').text("Step 9 - Select Your Trending Category:");

        }
        else if (id === "14") {
            HideFFWellnessChallengeSection();
            $('#loadingTrendingCategory').show();
            $('#TrendingCategoryHeader').text("Step 13 - Select Your Trending Category:");

            $('#divFittnessTrendingCategorySection').hide();
            $('#divTrendingCategorySection').show();

        }
        else {
            $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");
            $('#divFittnessTrendingCategorySection').show();
            $('#divTrendingCategorySection').hide();

        }
        // Bind the Challenge Category Id
        if (id === "14") {
            var urlchallenge = baseUrlString + "/Reporting/GetChallengeCategory";
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlchallenge + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {
                    $('#loadingChallengeCategory').hide();
                    $('#ChallengeCategoryId').prop('disabled', false);
                    jQuery('#ChallengeCategoryId').css('background-color', '#FFFFFF');
                    var items = '<option value="0">--Choose Challenge Category--</option>';
                    jQuery.each(data, function (i, challenge) {
                        items += "<option value='" + challenge.Value + "'>" + challenge.Text + "</option>";
                    });
                    // Fill ChallengeSubType Dropdown list
                    jQuery('#ChallengeCategoryId').html(items);
                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                    $('#loadingChallengeCategory').hide();
                }
            });
        }
        else {
            var items = '<option value="0">--Choose Challenge Category--</option>';
            jQuery('#ChallengeCategoryId').html(items);
        }
        $('#loadingChallengeCategory').hide();
    });


});
//Bind challenge sub-type on challenge type change


jQuery(function () {
    jQuery('#ChallengeType').on("change", function () {
        $('#loadingChallengeSubType').show();
        $('#spnChallengeNameerror-msg,#spnChallengeNameerror-msg,#spnChallengeSubTypeId,#spnDescribechallenge,#spnExeName1,#spnFFExeName1').html("");
        $('.error-msg').html("");
        $('#ChallengeSubTypeId').prop('disabled', true);
        jQuery('#ChallengeSubTypeId').css('background-color', '#FAF9F9');
        var id = jQuery("#ChallengeType :selected").val();
        if (id != "") {
            var urlchallenge = baseUrlString + "/Reporting/GetSubTypeByType";
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlchallenge + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {
                    $('#loadingChallengeSubType').hide();
                    $('#ChallengeSubTypeId').prop('disabled', false);
                    jQuery('#ChallengeSubTypeId').css('background-color', '#FFFFFF');
                    var items = '<option value="">--Choose ChallengeSubType--</option>';
                    jQuery.each(data, function (i, challenge) {
                        items += "<option value='" + challenge.Value + "'>" + challenge.Text + "</option>";
                    });
                    // Fill ChallengeSubType Dropdown list
                    jQuery('#ChallengeSubTypeId').html(items);
                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });

            var urlTrending = baseUrlString + "/Reporting/GetTrendingCategoryByType";
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlTrending + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                beforeSend: function () {
                    //alert(id);

                },
                success: function (data) {
                    $('#loadingTrendingCategory').hide();
                    $('#TrendingCategoryId').prop('disabled', false);
                    jQuery('#TrendingCategoryId').css('background-color', '#FFFFFF');
                    var items = '<option value="">--Choose Trending Category--</option>';
                    jQuery.each(data, function (i, trendingCategory) {
                        items += "<option value='" + trendingCategory.Value + "'>" + trendingCategory.Text + "</option>";

                    });
                    // Fill ChallengeSubType Dropdown list
                    jQuery('#TrendingCategoryId').html(items);

                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);


                }
            });
            if (id === "13") {
                ShowFFChallengeSection();
                $('#TrendingCategoryHeader').text("Step 12 - Select Your Trending Category:");
            }
            else {
                HideFFChallengeSection();
                $('#challengeNoTrainerTeam').show();
                $('#TrendingCategoryHeader').text("Step 12 - Select Your Trending Category:");
            }



        } else {
            $('#loadingChallengeSubType').hide();
            $('#ChallengeSubTypeId').prop('disabled', false);
            jQuery('#ChallengeSubTypeId').css('background-color', '#FFFFFF');

            var items = '<option value="">--Choose ChallengeSubType--</option>';
            jQuery('#ChallengeSubTypeId').html(items);
        }
    });
});
jQuery(function () {
    // To set mindate in enddate
    function customRangeCOD(input) {
        return {
            minDate: (input.id == "CODEndDate" ? $("#CODStartDate").datepicker("getDate") : new Date())
        };
    }
    // To set maxdate in startdate
    function customRangeStartCOD(input) {
        return {
            maxDate: (input.id == "CODStartDate" ? $("#CODEndDate").datepicker("getDate") : null)
        };
    }
    // To set mindate in enddate
    function customRange(input) {
        return {

            minDate: (input.id == "TCEndDate" ? $("#TCStartDate").datepicker("getDate") : new Date())
        };
    }
    // To set maxdate in startdate
    function customRangeStart(input) {
        return {

            maxDate: (input.id == "TCStartDate" ? $("#TCEndDate").datepicker("getDate") : null)
        };
    }
    $(document).ready(function () {
        $('#CODStartDate').datepicker({
            beforeShow: customRangeStartCOD,
            minDate: 0,
            dateFormat: "yy-mm-dd",
            //changeYear: true
        });
        $('#CODEndDate').datepicker({
            beforeShow: customRangeCOD,
            dateFormat: "yy-mm-dd",
            //changeYear: true
        });
        $('#TCStartDate').datepicker({
            beforeShow: customRangeStart,
            minDate: 0,
            dateFormat: "yy-mm-dd",
            //changeYear: true
        });
        $('#TCEndDate').datepicker({
            beforeShow: customRange,
            dateFormat: "yy-mm-dd",
            //changeYear: true
        });
    });
    jQuery('#toggalediv').load(function () {
        jQuery.validator.defaults.ignore = ":hidden";
    });
    jQuery('#IsActive').load(function () {
        var id = jQuery("#ChallengeType :selected").val();
        if (id != "undefined" && id != "" && id != 13) {
            if (this.checked) {
                jQuery('#divqueue').show();
            } else {
                jQuery('#divqueue').hide();
            }
        }
    });
    jQuery('#IsActive').change(function () {
        if (id != undefined && id != "" && id != 13) {
            if (this.checked) {
                jQuery('#divqueue').show();
            } else {
                jQuery('#divqueue').hide();
            }
        }
    });
    //Hode or show COD or sponsor challenge block  
    jQuery('#IsSetToCOD').change(function () {
        if (this.checked) {
            jQuery('#divCOD').show();
        } else {
            jQuery('#divCOD').hide();
        }
    });
    jQuery('#IsSetToSponsorChallenge').change(function () {
        if (this.checked) {
            jQuery('#divSC').show();
        } else {
            jQuery('#divSC').hide();
        }
    });
});
//code for hide and show exercises on page load on the basis of challenge type

//function for show hide exercise block and variavle value parameteres
function ShowHideExecise() {
    jQuery('#trainerResult').html("Step 4 - Enter Your #" + jQuery("#ResultUnitType").val() + " (Optional):");
    var isMoreThanOne = jQuery('#IsMoreThenOne').val();
    var variableUnit = jQuery('#VariableUnit').val();
    if (variableUnit == "") {
        jQuery("#VariableValue").prop('disabled', true);
        jQuery("#VariableValue").css('background-color', '#EFF0BF');
        //jQuery("#GlobalResultFilterValue").prop('disabled', true);
        //jQuery("#GlobalResultFilterValue").css('background-color', '#EFF0BF');
    }
    if (variableUnit == "minutes") {
        jQuery('#VariblesMinute-Section').show();
        jQuery('#ResultFilterMinute-Section').show();

        jQuery("#otheVariableValue").hide();
        //jQuery("#otheGlobalResultFilterValue").hide();
        //jQuery("#VariableValue").mask("99:99:99", { placeholder: "HH:MM:SS" });
    } else {
        jQuery('#VariblesMinute-Section').hide();
        jQuery('#ResultFilterMinute-Section').hide();
        jQuery("#otheVariableValue").show();
        //jQuery("#otheGlobalResultFilterValue").show();
        // jQuery("#VariableValue").mask("", { placeholder: "" });
    }
    if (jQuery("#VariableLimit").val() != "") {
        jQuery("#lblValue").html(jQuery("#VariableLimit").val());
        jQuery("#lblotherValue").html(jQuery("#VariableLimit").val());
        //jQuery("#lblGlobalResultFilterValue").html(jQuery("#VariableLimit").val());        
        //jQuery("#lblGlobalResultFiletotherValue").html(jQuery("#VariableLimit").val());
    }
    jQuery('#ResultTime').hide();
    jQuery('#ResultWeightorDestance').hide();
    jQuery('#ResultRepsRound').hide();
    jQuery('#ResultFrection').hide();
    //jQuery('#otheGlobalResultFilterValue').hide();

    if (jQuery("#ResultUnitType").val() == "Time") {
        jQuery('#ResultTime').show();
        jQuery('#otheGlobalResultFilterValue').hide();
    }
    if ((jQuery("#ResultUnitType").val() == "Weight") || (jQuery("#ResultUnitType").val() == "Distance")) {
        jQuery('#ResultWeightorDestance').show();
        jQuery('#otheGlobalResultFilterValue').show();
    } else if ((jQuery("#ResultUnitType").val() == "Reps") || (jQuery("#ResultUnitType").val() == "Rounds") || (jQuery("#ResultUnitType").val() == "Interval")) {
        jQuery('#ResultRepsRound').show();
        jQuery('#otheGlobalResultFilterValue').show();
    }
    if ((jQuery("#ResultUnitType").val() == "Interval") || (jQuery("#ResultUnitType").val() == "Rounds")) {
        jQuery('#ResultFrection').show();
        jQuery('#otheGlobalResultFilterValue').show();
    }
    if ($('#IsActive').attr('checked')) {
        $('#divqueue').show();
    } else {
        $('#divqueue').hide();
    }
    if ($('#IsSetToCOD').attr('checked')) {
        $('#divCOD').show();
    }
    if ($('#IsSetToSponsorChallenge').attr('checked')) {
        $('#divSC').show();
    }
    ManageReps(jQuery("#ResultUnitType").val(), jQuery("#VariableUnit").val());
}

function HideFFChallengeSection() {
    $('#divchkPremium').hide();
    $('#ExeciseRepsSec1').show();

    $('#FitcomChallengeExeciseSection').show();
    $('#FreeFormChallengeExeciseSection').hide();
    $('#FreeFormChallegeDescriptionHeaderSection').hide();
    $('#AdminChallengeVaribaleSec').show();
    $('#ChallengeGlobalResultFilter').show();
    $('#CreateChallenegeEquipmentRequirement').show();
    $('#FreeFormChallengeCateorySection').hide();
    $('#challegeDifficultyHeader').text("Step 2 - Select Difficulty Level:");
    $('#AdminStepTrainingZone').text("Step 7 - Select Target Training Zone:");
    $('#AdminStepChallengeName').text("Step 9 - Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Step 10 - Choose Trainer:");
    $('#AdminStepExerciseType').text("Step 8 - Choose Exercise Type:");
    $('#ChallenegeEquipmentRequirementHeader').text("Step 6 Select Equipment Requirements:");
    $('#CraeteFreeformChallengeName').hide();
    $('#divqueue').show();
    $('#FreeFormChallegeDurationSection').hide();
    $('#divChallengeCategorySection').hide();
    $('#FreeFormChallegeDetailsSection').hide();
    $('#divchkFeatured').show();
    $('#divTrendingCategorySection').hide();
    $('#divFittnessTrendingCategorySection').show();
    $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");




}
function ShowFFWellnessCChallengeSection() {
    $('#FreeFormChallegeDetailsSection').show();
    $('#challegeDifficultyHeader').text("Step 2 - Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text("Step 3 - Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text("Step 4 - Challenge Details:");
    $('#ChallegeDurationHeader').text("Step 5 - How long is Workout:");
    $('#AdminFreefromExeciseSection').text("Step 6 - Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text("Step 7 - Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text("Step 8 - Select Target Training Zone:");
    $('#AdminStepExerciseType').text("Step 9 - Choose Exercise Type:");
    $('#AdminStepChallengeName').text("Step 7 - Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Step 8 - Choose Trainer:");
    $('#CreateChallenegeEquipmentRequirement').hide();
    $('#CreateChallenegeTrainingZone').hide();
    $('#CreateChallenegeExerciseType').hide();
    $('#challengeNoTrainerTeam').show();
    $('#divTrendingCategorySection').hide();
    $('#divchkFeatured').hide();
    $('#divChallengeCategorySection').hide();
}
function HideFFWellnessChallengeSection() {
    $('#FreeFormChallegeDetailsSection').hide();
    $('#challegeDifficultyHeader').text("Step 3 - Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text("Step 4 - Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text("Step 5 - Challenge Details:");
    $('#ChallegeDurationHeader').text("Step 5 - How long is Workout:");
    $('#AdminFreefromExeciseSection').text("Step 6 - Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text("Step 7 - Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text("Step 8 - Select Target Training Zone:");
    $('#AdminStepExerciseType').text("Step 9 - Choose Exercise Type:");
    $('#AdminStepChallengeName').text("Step 10 - Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Step 11 - Choose Trainer:");
    $('#CreateChallenegeEquipmentRequirement').show();
    $('#CreateChallenegeTrainingZone').show();
    $('#CreateChallenegeExerciseType').show();
    $('#challengeNoTrainerTeam').show();
    $('#divTrendingCategorySection').show();
    $('#divchkFeatured').show();
    $('#divChallengeCategorySection').show();

}
function ShowFFChallengeSection() {
    $('#toggalediv').show();
    $('#ExeciseRepsSec1').hide();
    $('#CraeteFreeformChallengeName').show();
    $('#FitcomChallengeExeciseSection').hide();
    $('#FreeFormChallengeExeciseSection').show();
    $('#FreeFormChallegeDescriptionHeaderSection').show();    
    $('#FreeFormChallengeCateorySection').show();
    $('#challegeDifficultyHeader').text("Step 3 - Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text("Step 4 - Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text("Step 5 - Challenge Details:");
    $('#ChallegeDurationHeader').text("Step 5 - How long is Workout:");
    $('#AdminFreefromExeciseSection').text("Step 6 - Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text("Step 7 - Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text("Step 8 - Select Target Training Zone:");
    $('#AdminStepExerciseType').text("Step 9 - Choose Exercise Type:");
    $('#AdminStepChallengeName').text("Step 10 - Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Step 11 - Choose Trainer:");
    $('#divqueue').hide();
    $('#divchkPremium').show();
    $('#FreeFormChallegeDurationSection').show();
    $('#divChallengeCategorySection').show();
    $('#divTrendingCategorySection').show();
    $('#divFittnessTrendingCategorySection').hide();
    $('#ChallengeGlobalResultFilter').hide();
    $('#AdminChallengeVaribaleSec').hide();

}
//function for manage weight for man and woman and reps
function ManageReps(resultUnit, variableUnit) {
    var chlngTypeName = jQuery("#lblChallengeTypeName").text().trim();
    if (jQuery("#ChallengeType option:selected").text() == "Power Challenges" || jQuery("#ChallengeType option:selected").text() == "Endurance Challenges") {
        if (resultUnit != "Reps" && variableUnit != "reps" && jQuery("#ChallengeType option:selected").text() == "Power Challenges") {
            $('#reps-level').show();
        } else {
            $('#reps-level').hide();
        }
        $('#weight-level').show();
        $('.reps-in-box').show();
    } else if (chlngTypeName == "Power" || chlngTypeName == "Endurance") {
        if (resultType != "Reps" && variableUnit != "reps" && chlngTypeName == "Power") {
            $('#reps-level').show();
        } else {
            $('#reps-level').hide();
        }
        $('#weight-level').show();
        $('.reps-in-box').show();
    } else {
        $('#reps-level').hide();
        $('#weight-level').hide();
        $('.reps-in-box').hide();
    }
}

//Method to select al least one traing zone
jQuery(function () {
    jQuery("#detailsTraiingZonechk1 [id*=PostedTargetZones_SelectedTargetZoneIDs]").click(function () {
        jQuery("#SelectedTargetZoneCheck").val(null);
        if (jQuery("#detailsTraiingZonechk1 [id*=PostedTargetZones_SelectedTargetZoneIDs]:checked").length > 0) {
            jQuery("#SelectedTargetZoneCheck").val("Checked");
        }
    });

    jQuery("#detailsEquipmentchk1 [id*=PostedEquipments_SelectedEquipmentIDs]").click(function () {
        jQuery("#SelectedEquipmentCheck").val(null);
        if (jQuery("#detailsEquipmentchk1 [id*=PostedEquipments_SelectedEquipmentIDs]:checked").length > 0) {
            jQuery("#SelectedEquipmentCheck").val("Checked");
        }
    });
    jQuery("#chkAllChallengeCategory [id*=PostedChallengeCategory_ChallengeCategoryId]").click(function () {
        jQuery("#SelectedChallengeCategoryCheck").val(null);
        if (jQuery("#chkAllChallengeCategory [id*=PostedChallengeCategory_ChallengeCategoryId]:checked").length > 0) {
            jQuery("#SelectedChallengeCategoryCheck").val("Checked");
        }
    });
});


function DisableSetResultControl(execisesetId, id) {
    var execiseSetSectionId = $('#' + execisesetId);
    execiseSetSectionId.find($('#SetTimeHours' + id)).prop("disabled", true);
    execiseSetSectionId.find($('#SetTimeMinute' + id)).prop("disabled", true);
    execiseSetSectionId.find($('#SetTimeSecond' + id)).prop("disabled", true);
    execiseSetSectionId.find($('#SetTimeHS' + id)).prop("disabled", true);
}
function EnabledSetResultControl(execisesetId, id) {
    var execiseSetSectionId = $('#' + execisesetId);
    execiseSetSectionId.find($('#SetTimeHours' + id)).prop("disabled", false);
    execiseSetSectionId.find($('#SetTimeMinute' + id)).prop("disabled", false);
    execiseSetSectionId.find($('#SetTimeSecond' + id)).prop("disabled", false);
    execiseSetSectionId.find($('#SetTimeHS' + id)).prop("disabled", false);
}
function ManageExeciseSerRep(execisesetId, id) {
    var execiseSetSectionId = $('#' + execisesetId);
    var setTimeHours = execiseSetSectionId.find($('#SetTimeHours' + id)).val();
    var setTimeMinute = execiseSetSectionId.find($('#SetTimeMinute' + id)).val();
    var setTimeSecond = execiseSetSectionId.find($('#SetTimeSecond' + id)).val();
    var setTimeHS = execiseSetSectionId.find($('#SetTimeHS' + id)).val();
    var hasExeSetvalue = false;
    if (setTimeHours != undefined && setTimeHours != null && setTimeHours !== "" && setTimeHours !== "00") {
        hasExeSetvalue = true;
    }
    else if (setTimeMinute != undefined && setTimeMinute != null && setTimeMinute !== "" && setTimeMinute !== "00") {
        hasExeSetvalue = true;
    }
    else if (setTimeSecond != undefined && setTimeSecond != null && setTimeSecond !== "" && setTimeSecond !== "00") {
        hasExeSetvalue = true;
    }
    else if (setTimeHS != undefined && setTimeHS != null && setTimeHS !== "" && setTimeHS !== "00") {
        hasExeSetvalue = true;
    }
    if (hasExeSetvalue) {
        execiseSetSectionId.find($('#ExecSetRep' + id)).prop("disabled", true);
    } else {
        execiseSetSectionId.find($('#ExecSetRep' + id)).prop("disabled", false);
    }

}

