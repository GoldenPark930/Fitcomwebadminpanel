// Add the set reps and result time and rest time in pi seperated string
function GetExeciseSets(execiseindexId) {
    var Execisetrecord = "";
    var exesetControlID = '#ExerciseSetCount' + execiseindexId;
    var icount = $(exesetControlID).val();
    var isEntededSetRep = true;
    var isEntededSetTime = true;
    var execiseSetSectionId = $('#execeMainsetId' + execiseindexId);
    for (var escount = 1; escount <= icount ; escount++) {
        var setrepId = '#ExecSetRep' + escount;
        var setrepvalue = execiseSetSectionId.find(setrepId).val();
        setrepvalue = parseInt(setrepvalue);
        if (!isNaN(setrepvalue) && setrepvalue !== undefined && setrepvalue !== null && setrepvalue !== "" && setrepvalue !== 0) {
            Execisetrecord += setrepvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
            isEntededSetRep = false;
        }
        // var setresultId = '#ExecSetTime' + escount;
        var setresultHHValue = execiseSetSectionId.find('#SetTimeHours' + escount).val();
        if (setresultHHValue === undefined || setresultHHValue === "undefined" || setresultHHValue === null || setresultHHValue === "") {
            setresultHHValue = "00";
        }
        var setresultMMValue = execiseSetSectionId.find('#SetTimeMinute' + escount).val();
        if (setresultMMValue === undefined || setresultMMValue === "undefined" || setresultMMValue === null || setresultMMValue === "") {
            setresultMMValue = "00";
        }
        var setresultSSValue = execiseSetSectionId.find('#SetTimeSecond' + escount).val();
        if (setresultSSValue === undefined || setresultSSValue === "undefined" || setresultSSValue === null || setresultSSValue === "") {
            setresultSSValue = "00";
        }
        var setresultHSValue = execiseSetSectionId.find('#SetTimeHS' + escount).val();
        if (setresultHSValue === undefined || setresultHSValue === "undefined" || setresultHSValue === null || setresultHSValue === "") {
            setresultHSValue = "00";
        }
        var setresultvalue = setresultHHValue + ":" + setresultMMValue + ":" + setresultSSValue + "." + setresultHSValue;
        if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "") {
            Execisetrecord += setresultvalue + "^";
            isEntededSetTime = (setresultvalue !== "00:00:00.00")
        } else {
            Execisetrecord += "SNA" + "^";
        }

        // var setRestresultId = '#ExecRestTime' + escount;
        var setRestresultHHValue = execiseSetSectionId.find('#RestTimeHours' + escount).val();
        if (setRestresultHHValue === undefined || setRestresultHHValue === "undefined" || setRestresultHHValue === null || setRestresultHHValue === "") {
            setRestresultHHValue = "00";
        }
        var setRestresultMMValue = execiseSetSectionId.find('#RestTimeMinute' + escount).val();
        if (setRestresultMMValue === undefined || setRestresultMMValue === "undefined" || setRestresultMMValue === null || setRestresultMMValue === "") {
            setRestresultMMValue = "00";
        }
        var setRestresultSSValue = execiseSetSectionId.find('#RestTimeSecond' + escount).val();
        if (setRestresultSSValue === undefined || setRestresultSSValue === "undefined" || setRestresultSSValue === null || setRestresultSSValue === "") {
            setRestresultSSValue = "00";
        }
        var setRestresultHSValue = execiseSetSectionId.find('#RestTimeHS' + escount).val();
        if (setRestresultHSValue === undefined || setRestresultHSValue === "undefined" || setRestresultHSValue === null || setRestresultHSValue === "") {
            setRestresultHSValue = "00";
        }
        var setRestresultvalue = setRestresultHHValue + ":" + setRestresultMMValue + ":" + setRestresultSSValue + "." + setRestresultHSValue;
        //  var setRestresultvalue = execiseSetSectionId.find(setRestresultId).val();
        if (setRestresultvalue !== undefined && setRestresultvalue !== null && setRestresultvalue !== "") {
            Execisetrecord += setRestresultvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
        }
        var setDescription = '#ExecSetDescription' + escount;
        var setDescriptionvalue = execiseSetSectionId.find(setDescription).val();
        if (setDescriptionvalue !== undefined && setDescriptionvalue !== null && setDescriptionvalue !== "") {
            Execisetrecord += setDescriptionvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
        }
        var setAutocountdownYesNo = "SNA";
        var isSetAutoCountDownCtr = '#IsSetAutoCountDown' + escount;
        var setSetAutoCountYesDownvalue = execiseSetSectionId.find('#IsSetAutoCountDown' + escount).is(':checked')
        if (setSetAutoCountYesDownvalue) {
            setAutocountdownYesNo = "Yes";
        } else {
            setAutocountdownYesNo = "No";
        }
        Execisetrecord += setAutocountdownYesNo;
        Execisetrecord += "<>";
    }
    if (isEntededSetRep || isEntededSetTime) {
        return Execisetrecord;
    } else {
        return "";
    }
}
// Bind the Equipemet,Trainig and Execise Type dropdown dynamically
function BindEquipment(execountNumber) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownEquipments = "#FFdrpdownEquipments" + execountNumber;
        var urlchallenge = baseUrlString + "/Reporting/GetAllEquipment";
        jQuery.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: urlchallenge,
            dataType: "json",
            beforeSend: function () {
                //alert(id);
            },
            success: function (data) {
                $(drpdownEquipments).prop('disabled', false);
                jQuery(drpdownEquipments).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Equipments--</option>';
                jQuery.each(data, function (i, equip) {
                    items += "<option value='" + equip.EquipmentId + "'>" + equip.Equipment + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownEquipments).html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}
function BindTrainingZone(execountNumber) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownTrainingZone = "#FFdrpdownTrainingZones" + execountNumber;
        var urlchallenge = baseUrlString + "/Reporting/GetAllTrainingZone";
        jQuery.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: urlchallenge,
            dataType: "json",
            beforeSend: function () {
                //alert(id);
            },
            success: function (data) {
                $(drpdownTrainingZone).prop('disabled', false);
                jQuery(drpdownTrainingZone).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Training Zones--</option>';
                jQuery.each(data, function (i, traingzone) {
                    items += "<option value='" + traingzone.PartId + "'>" + traingzone.PartName + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownTrainingZone).html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}
function BindExerciseType(execountNumber) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var urlchallenge = baseUrlString + "/Reporting/GetAllExerciseType";
        var drpdownExerciseType = "#FFdrpdownExerciseTypes" + execountNumber;
        jQuery.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: urlchallenge,
            dataType: "json",
            beforeSend: function () {
                //alert(id);
            },
            success: function (data) {

                $(drpdownExerciseType).prop('disabled', false);
                jQuery(drpdownExerciseType).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Exercise Types--</option>';
                jQuery.each(data, function (i, execiseType) {
                    items += "<option value='" + execiseType.ExerciseTypeId + "'>" + execiseType.ExerciseName + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownExerciseType).html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }

}
function RenameExeciseId(starIdex, endIndex) {
    if (starIdex > 0 && endIndex > 0) {
        var removeedIndex = 0;
        for (var icounter = endIndex; icounter >= starIdex; icounter--) {
            removeedIndex = icounter + 1;
            var ffExeciseLabelID = "#FFlblExeName" + icounter;
            var ffremoveExeciseLabelID = "#FFRemoveExeName" + icounter;
            $(ffExeciseLabelID).text("Name of Exercise " + removeedIndex + ":");
            $(ffExeciseLabelID).attr("id", "FFlblExeName" + removeedIndex);
            $(ffremoveExeciseLabelID).attr("id", "FFRemoveExeName" + removeedIndex);

            $('#FFdrpdownEquipments' + icounter).attr("name", "FFdrpdownEquipments" + removeedIndex);
            $('#FFdrpdownEquipments' + icounter).attr("id", "FFdrpdownEquipments" + removeedIndex);

            $('#FFdrpdownTrainingZones' + icounter).attr("name", "FFdrpdownTrainingZones" + removeedIndex);
            $('#FFdrpdownTrainingZones' + icounter).attr("id", "FFdrpdownTrainingZones" + removeedIndex);

            $('#FFdrpdownExerciseTypes' + icounter).attr("name", "FFdrpdownExerciseTypes" + removeedIndex);
            $('#FFdrpdownExerciseTypes' + icounter).attr("id", "FFdrpdownExerciseTypes" + removeedIndex);

            $('#FFExeVideoLink' + icounter).attr("id", "FFExeVideoLink" + removeedIndex);

            $('#FFExeName' + icounter).attr("name", "FFExeName" + removeedIndex);
            $('#FFExeName' + icounter).attr("id", "FFExeName" + removeedIndex);

            $('#IsFFAExeName' + icounter).attr("name", "IsFFAExeName" + removeedIndex);
            $('#IsFFAExeName' + icounter).attr("id", "IsFFAExeName" + removeedIndex);

            $('#FFAExeName' + icounter).attr("name", "FFAExeName" + removeedIndex);
            $('#FFAExeName' + icounter).attr("id", "FFAExeName" + removeedIndex);

            $('#spnFFAExeName' + icounter).attr("data-valmsg-for", "spnFFAExeName" + removeedIndex);
            $('#spnFFAExeName' + icounter).attr("id", "spnFFAExeName" + removeedIndex);

            $('#FFExeName' + icounter).attr("data-valmsg-for", "FFExeName" + removeedIndex);

            $('#FFlblExe' + icounter).attr("id", "FFlblExe" + removeedIndex);
            $('#FFExeDesc' + icounter).attr("name", "FFExeDesc" + removeedIndex);
            $('#FFExeDesc' + icounter).attr("id", "FFExeDesc" + removeedIndex);
            $('#FFExeVideoLinkUrl' + icounter).attr("id", "FFExeVideoLinkUrl" + removeedIndex);
            $('#FFExeName' + icounter).attr("name", "FFExeName" + removeedIndex);

            $('#execiseDivId' + icounter).attr("id", "execiseDivId" + removeedIndex);
            $('#ExerciseId' + icounter).attr("id", "ExerciseId" + removeedIndex);
            $('#ExerciseSetCount' + icounter).attr("id", "ExerciseSetCount" + removeedIndex);
            $('#execeMainsetId' + icounter + " :first-child").each(function () {
                $('#' + icounter).attr("id", removeedIndex);
            });
            $('#execeMainsetId' + icounter).attr("id", "execeMainsetId" + removeedIndex);
            $('#btnAddExeciseSet' + icounter).attr("id", "btnAddExeciseSet" + removeedIndex);
            $('#InsertExecise' + icounter).attr("id", "InsertExecise" + removeedIndex);


            $('#IsNewAddedExercise' + icounter).attr("id", "IsNewAddedExercise" + removeedIndex);
            $('#CEARocordId' + icounter).attr("id", "CEARocordId" + removeedIndex);
            $('#IsSetFirstExecise' + icounter).attr("id", "IsSetFirstExecise" + removeedIndex);

        }
    }
}
function GetSetTimeFormat(enterdtime) {
    if (enterdtime !== undefined && enterdtime !== "undefined" && enterdtime !== null && enterdtime !== "") {
        return enterdtime
    } else {
        return "00";
    }
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
}
$(document).ready(function () {   

    ShowFFChallengeSection();
    var max_fields = 999999999999; //maximum input boxes allowed
    var wrapper = $("#freeformtoggalediv"); //Fields wrapper
    var add_button = $(".add_field_button"); //Add button ID  
    $('.add_field_button').prop('disabled', false);
    $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");
    var controlID = undefined;
    var exsetcount = 1;
    $(document).delegate("button[id^='InsertExecise']", "click", function (e) {
        $("#myLoadingElement").show();
        e.preventDefault();
        //$(add_button).click(function (e) { //on add input button click   
        var currentExeciseId = $(this).attr('id').replace(/[^0-9]/g, '');
        if (excount < max_fields) { //max input box allowed
            var lastExeciseCount = excount;
            excount++; //text box increment
            var insertedExeciseCount = parseInt(currentExeciseId) + 1;
            currentExeciseId = parseInt(currentExeciseId);
            var addexeciseurl = baseUrlString + "/Reporting/AddFFExecise";
            jQuery.ajax({
                type: "GET",
                url: addexeciseurl,
                data: { id: insertedExeciseCount, IsNewAddedExercise: 1 },
                async: false,
                success: function (data) {
                    var endIndex = 0;
                    if (excount > 0) {
                        endIndex = excount - 1;
                    }
                    var startIndex = 0;
                    if (currentExeciseId > 0) {
                        startIndex = currentExeciseId + 1;
                    }
                    //Rename Execise Ids
                    RenameExeciseId(startIndex, endIndex);
                    if (currentExeciseId > 1) {
                        var execiseSectionId = $('#execiseDivId' + currentExeciseId);
                        $(execiseSectionId).after(data);

                    } else {
                        if (excount > 2) {
                            $(wrapper).prepend(data);
                        }
                        else {
                            $(wrapper).append(data);
                        }
                    }

                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
            // $(wrapper).append('<div class="add-remove-container chose-ex-parts"><div class="row-colum padding-topbtm5 add-step"><a id="FFRemoveExeName' + excount + '" href="#" class="remove_field remove_execiseset second">X</a> <div class="clearfix"></div> <label id="FFlblExeName' + excount + '">Name of Exercise ' + excount + ':</label><input class="freeforminput FreeFormExerciseVideos col-md-10 col-sm-10 col-xs-10  ui-autocomplete-input ui-corner-all" id="FFExeName' + excount + '" name="FFExeName' + excount + '" type="text" value="" autocomplete="off"> <div  class="row-colum col-md-12 col-sm-12 col-xs-12"><input type="hidden" id="ExerciseSetCount' + excount + '" value="0"><span id="FFExeVideoLink' + excount + '"> <a  id="FFExeVideoLinkUrl' + excount + '" href="#" target="_blank"></a></span><span id="FFlblExe' + excount + '" class="error-msg secondary-err-msg"></span><span class="field-validation-valid error-msg" data-valmsg-for="FFExeName' + excount + '" data-valmsg-replace="true"></span></div><div class="row-colum col-md-12 col-lg-12 col-xs-12"><label><span class="active-new-chckbox pull-left">Alternate Name:</span> <input  id="IsFFAExeName' + excount + '" name="IsFFAExeName' + excount + '"  type="checkbox"  class="IsChangedExeciseName checkbox-btn"/></label><input class="freeforminput FreeFormExerciseVideos1 col-md-10 col-sm-10 col-xs-10" id="FFAExeName' + excount + '"  name="FFAExeName' + excount + '" type="text" value="" style="display: none;"/><span class="field-validation-valid error-msg" data-valmsg-for="FFAExeName' + excount + '" data-valmsg-replace="true" id="spnFFAExeName' + excount + '"></span></div></div><div class="row-colum"><div id="execeMainsetId' + excount + '" class="Main-set main-bg-set"></div><div class="add-set"><button id="btnAddExeciseSet' + excount + '" class="add_set_button marginBot10">Add Set</button></div></div>');
            var nextaddexecise = currentExeciseId + 1;
            $('#InsertExecise' + nextaddexecise).prop('disabled', true);
            BindEquipment(insertedExeciseCount);
            BindTrainingZone(insertedExeciseCount);
            BindExerciseType(insertedExeciseCount);
        }
        $("#myLoadingElement").hide();
        return false;
    });
    $(document).delegate("input:text[id^='ExecSetRep']", "focusout", function () {
        var execeMainsetId = $(this).parent('div').parent('div').attr('id');
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        var setrepId = $('#ExecSetRep' + id);
        // var setrepvalue = execiseSetSectionId.find(setrepId).val();
        var setrepId = $('#' + execeMainsetId).find(setrepId).val();
        if (setrepId != undefined && setrepId != null && setrepId !== "") {
            DisableSetResultControl(execeMainsetId, id);
        } else {
            EnabledSetResultControl(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeHours']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        } else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeMinute']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        } else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeSecond']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        } else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }
    });
    $(document).delegate("input:text[id^='SetTimeHS']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        } else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }
    });
    $(document).delegate("input:text[id^='RestTimeHours']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    $(document).delegate("input:text[id^='RestTimeMinute']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    $(document).delegate("input:text[id^='RestTimeSecond']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    $(document).delegate("input:text[id^='RestTimeHS']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });
    $(document).delegate("input:text[id^='ExecSetRep']", "keyup", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
    });

    $(document).delegate("button[id^='btnAddExeciseSet']", "click", function (e) {
        $("#myLoadingElement").show();
        e.preventDefault();
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        // Get the Execisise current set count
        var exesetControlID = '#ExerciseSetCount' + id;
        var execiseSetCurrentCount = parseInt($(exesetControlID).val());
        if (execiseSetCurrentCount >= 1) {
            exsetcount = execiseSetCurrentCount;
        }
        else {
            exsetcount = 1;
        }
        var exericeSetPlaceholdId = '#execeMainsetId' + id;
        if (exsetcount < max_fields) { //max input box allowed
            //text box increment
            var path = baseUrlString + "/images/RemoveSet.png";
            var exericeControlID = '#FFExeName' + id;
            var execiseName = $(exericeControlID).val();
            var isAlternativeExeciseName = $("#IsFFAExeName" + id).val();
            if (isAlternativeExeciseName !== undefined && $("#IsFFAExeName" + id).is(':checked')) {
                execiseName = $('#FFAExeName' + id).val();
            }
            if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
                var execount = excount;
                if (execiseSetCurrentCount >= 1) {
                    ++exsetcount;
                }
                var model = {
                    ExeciseCountID: parseInt(id),
                    ExeciseSetCountID: exsetcount,
                    ExeciseName: execiseName,
                    IsNewAddedSet: 1
                };
                //var url = "Reporting/AddFFExeciseSet/";
                var url = baseUrlString + "/Reporting/AddFFExeciseSet";
                $.post(url, { ExeciseCountID: parseInt(id), ExeciseSetCountID: exsetcount, ExeciseName: execiseName })
                 .done(function (response) {
                     $(exericeSetPlaceholdId).append(response);
                 });
                // $(exericeSetPlaceholdId).append('<div id=' + id + '><div class="set-heading"><span id="exesetTittle' + exsetcount + '">' + execiseName + ', Set ' + exsetcount + '</span>  <span class="remove-set"><button id="btnsetRemove' + exsetcount + '" class="remove_execiseset" >X</button></span></div><label>How Many Reps:</label><input class="freeforminput FreeFormExerciseVideos1 col-md-10 col-sm-10 col-xs-10 ui-autocomplete-input ui-corner-all" id="ExecSetRep' + exsetcount + '" name="ExecSetRep' + exsetcount + '" type="text" value="" autocomplete="off"><div class="or-section"><span>OR</span></div><label>How Much Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="SetTimeHours' + exsetcount + '" maxlength="2" name="SetTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input" data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeMinute' + exsetcount + '" maxlength="2" name="SetTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeSecond' + exsetcount + '" maxlength="2" name="SetTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="SetTimeHS' + exsetcount + '" maxlength="2" name="SetTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;height:32px;" type="text" value="00"><div class="clr-both"></div><label>How Much Rest Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="RestTimeHours' + exsetcount + '" maxlength="2" name="RestTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeMinute' + exsetcount + '" maxlength="2" name="RestTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeSecond' + exsetcount + '" maxlength="2" name="RestTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="RestTimeHS' + exsetcount + '" maxlength="2" name="RestTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value="00"><div class="clr-both"></div><label>Description:</label><textarea class="freeformtextarea col-md-10 col-sm-10 col-xs-9" cols="20" id="ExecSetDescription' + exsetcount + '" maxlength="500" name="ExecSetDescription' + exsetcount + '" placeholder="Max Char limit 500" rows="2"></textarea></div>');
                $(exesetControlID).val(exsetcount);
                $('#InsertExecise' + id).prop('disabled', false);
            } else {
                alert('Please select an execise name.');
            }
        }
        $("#myLoadingElement").hide();
        return false;
    });
    $(document).delegate("select[id^='FFdrpdownEquipments']", "change", function () {
        var exeId = $(this).attr('id').replace(/[^0-9]/g, '');
        var ExeNamecontrolId = '#FFExeName' + exeId;
        $(ExeNamecontrolId).trigger("focus");
    });
    $(document).delegate("select[id^='FFdrpdownTrainingZones']", "change", function () {
        var exeId = $(this).attr('id').replace(/[^0-9]/g, '');
        var ExeNamecontrolId = '#FFExeName' + exeId;
        $(ExeNamecontrolId).trigger("focus");
    });
    $(document).delegate("select[id^='FFdrpdownExerciseTypes']", "change", function () {
        var exeId = $(this).attr('id').replace(/[^0-9]/g, '');
        var ExeNamecontrolId = '#FFExeName' + exeId;
        $(ExeNamecontrolId).trigger("focus");
    });
    $(document).delegate("input:text[id^='FFExeName']", "focusin", function () {
        var controlNumber = undefined;
        var execisenameControlId = $(this).attr('id').replace(/[^0-9]/g, '');
        $(this).autocomplete({
            autoFocus: true,
            source: function (request, response) {
                var urlexe = baseUrlString + "/Reporting/GetSelectedExercises";
                var SelectedEquipement = "";
                var SelectedTrainingZone = "";
                var SelectedExeciseType = "";
                var drpdownEquipmentNameValues = jQuery('#FFdrpdownEquipments' + execisenameControlId).find('option:selected').text();
                if (drpdownEquipmentNameValues !== undefined && drpdownEquipmentNameValues !== null && drpdownEquipmentNameValues !== "" && drpdownEquipmentNameValues !== "--Choose Equipments--") {
                    SelectedEquipement = drpdownEquipmentNameValues;
                }
                var drpdownTrainingZonesValues = jQuery('#FFdrpdownTrainingZones' + execisenameControlId).find('option:selected').text();
                if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "--Choose Training Zones--") {
                    SelectedTrainingZone = drpdownTrainingZonesValues;
                }
                var drpdownExerciseTypes = jQuery('#FFdrpdownExerciseTypes' + execisenameControlId).find('option:selected').text();
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
                    async: false,
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
                var controlID = ($(this).attr("id"));
                var index = controlID.indexOf("FFExeName");
                controlNumber = controlID.substr(index + 9);
                var exeVideoLinkId = "#FFExeVideoLink" + controlNumber;
                var FFlblExeId = "#FFlblExe" + controlNumber;
                jQuery(exeVideoLinkId).html('<a id="FFExeVideoLinkUrl' + controlNumber + '" href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');
                jQuery(FFlblExeId).html("");
                var removeExeciseSetSectionId = $('#execeMainsetId' + controlNumber);
                // removeExeciseSetSectionId.find('#exesetTittle1').text(ui.item.value + " , Set 1");
                removeExeciseSetSectionId.find('[id^=exesetTittle]').each(function (key, value) {
                    var controlID = key + 1;
                    removeExeciseSetSectionId.find("#exesetTittle" + controlID).text(ui.item.value + " , Set " + controlID);
                    removeExeciseSetSectionId.find('#exesetTittle1').text(ui.item.value + " , Set 1");
                });
                // $('.add_field_button').prop('disabled', false);
                $('#InsertExecise' + controlNumber).prop('disabled', false);
                if (!ui.item) {
                     $("#ExerciseId" + controlNumber).val(0);                    
                } else {
                    var selectedExerciseIdId = parseInt(ui.item.ExerciseId);                  
                    $("#ExerciseId" + controlNumber).val(selectedExerciseIdId);

                }
            },
            change: function (event, ui) {
                var controlID = ($(this).attr("id"));
                var index = controlID.indexOf("FFExeName");
                controlNumber = controlID.substr(index + 9);
                if (!ui.item) {
                    this.value = '';
                    var lblExeId = "#FFlblExe" + controlNumber;
                    var exeVideoLinkId = "#FFExeVideoLink" + controlNumber;
                    jQuery(exeVideoLinkId).html("");
                    $("#ExerciseId" + controlNumber).val(0);                   
                    this.focus();

                } else {
                    var lblExeId = "#FFlblExe" + controlNumber;
                    jQuery(lblExeId).html("");
                    if (dataUnit == "Interval" || dataUnit == "Rounds") {
                    }
                    $('.add_field_button').prop('disabled', false);
                    var selectedExerciseIdId = parseInt(ui.item.ExerciseId);                 
                    $("#ExerciseId" + controlNumber).val(selectedExerciseIdId);
                }
                $('#InsertExecise' + controlNumber).prop('disabled', false);
            },
            open: function () {
                $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
            },
            close: function () {
                var id = $(this).attr('id').replace(/[^0-9]/g, '');
                controlNumber = id;
                $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
                var selectedExeciseName = "#FFExeName" + controlNumber;
                var isAlternativeExeciseName = "#IsFFAExeName" + controlNumber;
                var exelinkurl = "#FFExeVideoLinkUrl" + controlNumber;
                var selecteExeciseName = jQuery(selectedExeciseName).val();
                if (selecteExeciseName === undefined || selecteExeciseName === null || selecteExeciseName === "") {
                    jQuery(isAlternativeExeciseName).prop("disabled", false);
                    // $('.add_field_button').prop('disabled', false);
                    $('#InsertExecise' + controlNumber).prop('disabled', true);
                    // jQuery(exeVideoLinkId).html("");
                    jQuery(exelinkurl).html("");

                } else {
                    jQuery(isAlternativeExeciseName).prop("disabled", true);
                    // $('.add_field_button').prop('disabled', false);
                    $('#InsertExecise' + controlNumber).prop('disabled', false);
                }
                //var selectedExerciseIdId = parseInt(ui.item.ExerciseId);
                //$("#ExerciseId" + controlNumber).val(selectedExerciseIdId);

            }
        }).on('focus', function () {
            $(this).keydown();
        }).on('focusout', function () {
            var selectedExeciseName = "#FFExeName" + controlNumber;
            var isAlternativeExeciseName = "#IsFFAExeName" + controlNumber;
            var exelinkurl = "#FFExeVideoLinkUrl" + controlNumber;
            var selecteExeciseName = jQuery(selectedExeciseName).val();
            if (selecteExeciseName === undefined || selecteExeciseName === null || selecteExeciseName === "") {
                jQuery(isAlternativeExeciseName).prop("disabled", false);
                jQuery(exelinkurl).html("");
            } else {
                jQuery(isAlternativeExeciseName).prop("disabled", true);
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };

        $(this).autocomplete("search");
    });
    $(document).delegate("button[id^='btnsetRemove']", "click", function (e) {
        $("#myLoadingElement").show();
        var removelexeNumber = $(this).attr('id').replace(/[^0-9]/g, '');
        var currentExeciseCount = $(this).parent('span').parent('div').parent('div').attr('id');
        exsetcount = 0;
        $(this).parent('span').parent('div').parent('div').remove();
        //  $('#execeMainsetId1')
        var exesetControlID = '#ExerciseSetCount' + currentExeciseCount;
        var removeExeciseSetSectionId = $('#execeMainsetId' + currentExeciseCount);
        var exesetControlCount = $(exesetControlID).val();
        var execiseSetCurrentCount = parseInt(exesetControlCount);
        if (execiseSetCurrentCount > 0) {
            exsetcount = execiseSetCurrentCount;
        }
        else {
            exsetcount = 0;
        }
        var totalremaingExe = exsetcount + 1;
        var exericeControlID = '#FFExeName' + currentExeciseCount;
        var execiseName = $(exericeControlID).val();
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber != "") {
            // var removelexeID = parseInt(removelexeNumber);
            if (parseInt(removelexeNumber) >= 1) {
                var execisesetnumber = parseInt(removelexeNumber) + 1;
                for (var icounter = execisesetnumber; icounter <= totalremaingExe; icounter++) {


                    removeExeciseSetSectionId.find('#exesetTittle' + icounter).attr("name", "exesetTittle" + removelexeNumber);
                    removeExeciseSetSectionId.find('#exesetTittle' + icounter).attr("id", "exesetTittle" + removelexeNumber);

                    removeExeciseSetSectionId.find('#exesetTittle' + removelexeNumber).text(execiseName + " , Set " + removelexeNumber);

                    removeExeciseSetSectionId.find('#ExecSetRep' + icounter).attr("name", "ExecSetRep" + removelexeNumber);
                    removeExeciseSetSectionId.find('#ExecSetRep' + icounter).attr("id", "ExecSetRep" + removelexeNumber);


                    removeExeciseSetSectionId.find('#SetTimeHours' + icounter).attr("name", "SetTimeHours" + removelexeNumber);
                    removeExeciseSetSectionId.find('#SetTimeHours' + icounter).attr("id", "SetTimeHours" + removelexeNumber);


                    removeExeciseSetSectionId.find('#SetTimeMinute' + icounter).attr("name", "SetTimeMinute" + removelexeNumber);
                    removeExeciseSetSectionId.find('#SetTimeMinute' + icounter).attr("id", "SetTimeMinute" + removelexeNumber);


                    removeExeciseSetSectionId.find('#SetTimeSecond' + icounter).attr("name", "SetTimeSecond" + removelexeNumber);
                    removeExeciseSetSectionId.find('#SetTimeSecond' + icounter).attr("id", "SetTimeSecond" + removelexeNumber);


                    removeExeciseSetSectionId.find('#SetTimeHS' + icounter).attr("name", "SetTimeHS" + removelexeNumber);
                    removeExeciseSetSectionId.find('#SetTimeHS' + icounter).attr("id", "SetTimeHS" + removelexeNumber);


                    removeExeciseSetSectionId.find('#RestTimeHours' + removelexeNumber).attr("name", "RestTimeHours" + removelexeNumber);
                    removeExeciseSetSectionId.find('#RestTimeHours' + icounter).attr("id", "RestTimeHours" + removelexeNumber);


                    removeExeciseSetSectionId.find('#RestTimeMinute' + icounter).attr("name", "RestTimeMinute" + removelexeNumber);
                    removeExeciseSetSectionId.find('#RestTimeMinute' + icounter).attr("id", "RestTimeMinute" + removelexeNumber);


                    removeExeciseSetSectionId.find('#RestTimeSecond' + icounter).attr("name", "RestTimeSecond" + removelexeNumber);
                    removeExeciseSetSectionId.find('#RestTimeSecond' + icounter).attr("id", "RestTimeSecond" + removelexeNumber);


                    removeExeciseSetSectionId.find('#RestTimeHS' + icounter).attr("name", "RestTimeHS" + removelexeNumber);
                    removeExeciseSetSectionId.find('#RestTimeHS' + icounter).attr("id", "RestTimeHS" + removelexeNumber);


                    removeExeciseSetSectionId.find('#ExecSetDescription' + icounter).attr("name", "ExecSetDescription" + removelexeNumber);
                    removeExeciseSetSectionId.find('#ExecSetDescription' + icounter).attr("id", "ExecSetDescription" + removelexeNumber);
                    removeExeciseSetSectionId.find('#IsNewAddedSet' + icounter).attr("id", "IsNewAddedSet" + removelexeNumber);

                    removelexeNumber = parseInt(removelexeNumber) + 1;
                }
            }
            else {
                $('#execeMainsetId' + currentExeciseCount).css('min-height', '50px');

            }
        }
        exsetcount--;
        // Get the Execisise current set count
        var exesetControlID = '#ExerciseSetCount' + currentExeciseCount;
        $(exesetControlID).val(exsetcount);
        $("#myLoadingElement").hide();
        return false;
    });
    $(wrapper).on("click", ".remove_field", function (e) { //user click on remove text           
        $("#myLoadingElement").show();
        e.preventDefault(); $(this).parent('div').parent('div').remove(); excount--;
        //$('.add_field_button').prop('disabled', false);
        var controlID = ($(this).attr("id"));
        var index = controlID.indexOf("FFRemoveExeName");
        var removelexeNumber = controlID.substr(index + 15);
        var totalremaingExe = excount + 1;
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber !== "") {
            var removelexeID = parseInt(removelexeNumber);
            var fromremaneexecise = removelexeID + 1;
            for (var icounter = fromremaneexecise; icounter <= totalremaingExe; icounter++) {
                var ffExeciseLabelID = "#FFlblExeName" + icounter;
                var ffremoveExeciseLabelID = "#FFRemoveExeName" + icounter;
                $(ffExeciseLabelID).text("Name of Exercise " + removelexeID + ":");
                $(ffExeciseLabelID).attr("id", "FFlblExeName" + removelexeID);
                $(ffremoveExeciseLabelID).attr("id", "FFRemoveExeName" + removelexeID);
                $('#FFExeVideoLink' + icounter).attr("id", "FFExeVideoLink" + removelexeID);
                $('#FFExeName' + icounter).attr("id", "FFExeName" + removelexeID);
                $('#FFlblExe' + icounter).attr("id", "FFlblExe" + removelexeID);

                $('#FFExeDesc' + icounter).attr("name", "FFExeDesc" + removelexeID);
                $('#FFExeDesc' + icounter).attr("id", "FFExeDesc" + removelexeID);

                $('#FFExeVideoLinkUrl' + icounter).attr("id", "FFExeVideoLinkUrl" + removelexeID);
                $('#FFExeName' + icounter).attr("name", "FFExeName" + removelexeID);
                ////
                $('#FFdrpdownEquipments' + icounter).attr("name", "FFdrpdownEquipments" + removelexeID);
                $('#FFdrpdownEquipments' + icounter).attr("id", "FFdrpdownEquipments" + removelexeID);

                $('#FFdrpdownTrainingZones' + icounter).attr("name", "FFdrpdownTrainingZones" + removelexeID);
                $('#FFdrpdownTrainingZones' + icounter).attr("id", "FFdrpdownTrainingZones" + removelexeID);

                $('#FFdrpdownExerciseTypes' + icounter).attr("name", "FFdrpdownExerciseTypes" + removelexeID);
                $('#FFdrpdownExerciseTypes' + icounter).attr("id", "FFdrpdownExerciseTypes" + removelexeID);

                $('#FFExeVideoLink' + icounter).attr("id", "FFExeVideoLink" + removelexeID);

                $('#FFExeName' + icounter).attr("name", "FFExeName" + removelexeID);
                $('#FFExeName' + icounter).attr("id", "FFExeName" + removelexeID);
                $('#FFExeName' + icounter).attr("data-valmsg-for", "FFExeName" + removelexeID);
                $('#IsFFAExeName' + icounter).attr("name", "IsFFAExeName" + removelexeID);
                $('#IsFFAExeName' + icounter).attr("id", "IsFFAExeName" + removelexeID);

                $('#FFAExeName' + icounter).attr("name", "FFAExeName" + removelexeID);
                $('#FFAExeName' + icounter).attr("id", "FFAExeName" + removelexeID);

                $('#spnFFAExeName' + icounter).attr("data-valmsg-for", "spnFFAExeName" + removelexeID);
                $('#spnFFAExeName' + icounter).attr("id", "spnFFAExeName" + removelexeID);
                $('#execiseDivId' + icounter).attr("id", "execiseDivId" + removelexeID);
                $('#ExerciseId' + icounter).attr("id", "ExerciseId" + removelexeID);
                $('#ExerciseSetCount' + icounter).attr("id", "ExerciseSetCount" + removelexeID);
                $('#execeMainsetId' + icounter + " :first-child").each(function () {
                    $('#' + icounter).attr("id", removelexeID);
                });
                $('#execeMainsetId' + icounter).attr("id", "execeMainsetId" + removelexeID);
                $('#btnAddExeciseSet' + icounter).attr("id", "btnAddExeciseSet" + removelexeID);
                $('#InsertExecise' + icounter).attr("id", "InsertExecise" + removelexeID);
                $('#IsNewAddedExercise' + icounter).attr("id", "IsNewAddedExercise" + removelexeID);
                $('#CEARocordId' + icounter).attr("id", "CEARocordId" + removelexeID);
                $('#execiseDivId' + icounter).attr("id", "execiseDivId" + removelexeID);
                $('#IsSetFirstExecise' + icounter).attr("id", "IsSetFirstExecise" + removelexeID);

                removelexeID = removelexeID + 1;
            }
        }
        $("#myLoadingElement").hide();
    });
    //  submit the challenges with free form with dynamic execise
    $('#btnTopAdminChallengeSubmit,#btnButtomAdminChallengeSubmit').click(function (e) {
        $("#myLoadingElement").show();
        var ExeciseDescription = "";
        var Execiset1record = "";
        var wellnessSubtypeId = jQuery('#ChallengeSubTypeId').val();
        Execiset1record = GetExeciseSets(1);
        if (Execiset1record === "" && (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId !== "" && wellnessSubtypeId === "14")) {
            alert("Please enter execise set with reps or time.");
            $("#myLoadingElement").hide();
            return false;
        }
        var execisesettotalCount = $('#ExerciseSetCount1').val();
        var firstExeciseName = $('#FFExeName1').val();
        var firstAExeciseName = $('#FFAExeName1').val();
        var IsEnterfFirstExecise1 = false;
        if (firstExeciseName !== undefined && firstExeciseName !== null && firstExeciseName !== "") {
            IsEnterfFirstExecise1 = true;
        }
        else if (firstAExeciseName !== undefined && firstAExeciseName !== null && firstAExeciseName !== "") {
            IsEnterfFirstExecise1 = true;
        }

        if (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId !== "" && wellnessSubtypeId === "14" && IsEnterfFirstExecise1 === false) {
            alert("Please enter at least one workout exercise.");
            $("#myLoadingElement").hide();
            return false;
        }

        $('#FFExeDesc1').val(Execiset1record);
        // Get All Execise with set execpt first execise with sets details
        for (var icount = 2; icount <= excount ; icount++) {
            var exericeControlID = '#FFExeName' + icount;
            var execiseName = $(exericeControlID).val();
            if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
                ExeciseDescription += execiseName + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var execiseSetDetails = "";
            var execisesetcount = '#ExerciseSetCount' + icount;
            var execisesetremaingtotalCount = $(execisesetcount).val();
            if (execisesetremaingtotalCount !== undefined) {
                execiseSetDetails = GetExeciseSets(icount);
                if (execiseSetDetails === "" && (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId !== "" && wellnessSubtypeId === "14")) {
                    alert("Please enter execise set with reps or time.");
                    $("#myLoadingElement").hide();
                    return false;
                }
            }

            if (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId !== "" && wellnessSubtypeId === "14" && (execisesetremaingtotalCount === "0" || execisesetremaingtotalCount == 0)) {
                alert("Please enter at least one set for each workouts exercise.");
                $("#myLoadingElement").hide();
                return false;
            }

            if (execiseSetDetails !== undefined && execiseSetDetails !== null && execiseSetDetails !== "") {
                ExeciseDescription += execiseSetDetails + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var exeVideoLinkurlID = '#FFExeVideoLinkUrl' + icount;
            var exeVideoLinkurl = $(exeVideoLinkurlID).text();
            if (exeVideoLinkurl !== undefined && exeVideoLinkurl !== null && exeVideoLinkurl !== "") {
                ExeciseDescription += exeVideoLinkurl + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var isFFAExeNameCtr = '#IsFFAExeName' + icount;
            var exeIsFFAExeName;
            if ($(isFFAExeNameCtr).is(':checked')) {
                exeIsFFAExeName = "true";
            } else {
                exeIsFFAExeName = "false";
            }
            if (exeIsFFAExeName !== undefined && exeIsFFAExeName !== null && exeIsFFAExeName !== "") {
                ExeciseDescription += exeIsFFAExeName + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var exeFFAExeNameCtrl = '#FFAExeName' + icount;
            var exeFFAExeName = $(exeFFAExeNameCtrl).val();
            if (exeFFAExeName !== undefined && exeFFAExeName !== null && exeFFAExeName !== "") {
                ExeciseDescription += exeFFAExeName + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var drpdownEquipmentNameValues = jQuery('#FFdrpdownEquipments' + icount).find('option:selected').val();
            if (drpdownEquipmentNameValues !== undefined && drpdownEquipmentNameValues !== null && drpdownEquipmentNameValues !== "" && drpdownEquipmentNameValues !== "--Choose Equipments--") {
                ExeciseDescription += drpdownEquipmentNameValues + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var drpdownTrainingZonesValues = jQuery('#FFdrpdownTrainingZones' + icount).find('option:selected').val();
            if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "--Choose Training Zones--") {
                ExeciseDescription += drpdownTrainingZonesValues + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var drpdownExerciseTypes = jQuery('#FFdrpdownExerciseTypes' + icount).find('option:selected').val();
            if (drpdownExerciseTypes !== undefined && drpdownExerciseTypes !== null && drpdownExerciseTypes !== "" && drpdownExerciseTypes !== "--Choose Exercise Types--") {
                ExeciseDescription += drpdownExerciseTypes + "~";
            }
            else {
                ExeciseDescription += "NA" + "~";
            }

            var isFirstExecise = '#IsSetFirstExecise' + icount;
            var isCheckedFirstExecise = "false";
            if ($(isFirstExecise).is(':checked')) {
                isCheckedFirstExecise = "true";
            } else {
                isCheckedFirstExecise = "false";
            }
            ExeciseDescription += isCheckedFirstExecise + "~";
            var exeFFExerciseIdCtrl = '#ExerciseId' + icount;
            var exeFFExerciseId = $(exeFFExerciseIdCtrl).val(); 
            if (exeFFExerciseId !== undefined && exeFFExerciseId !== null && exeFFExerciseId !== "") {
                ExeciseDescription += exeFFExerciseId ;
            }
            else {
                ExeciseDescription += "NA" ;
            }

            ExeciseDescription += "|";
        }
        $('#FreeFormExerciseNameDescriptionList').val(ExeciseDescription);
        var FFExeVideoLinkUrl1 = $('#FFExeVideoLinkUrl1').text();
        if (FFExeVideoLinkUrl1 !== undefined && FFExeVideoLinkUrl1 !== null && FFExeVideoLinkUrl1 !== "") {
            $('#FFExeVideoUrl1').val(FFExeVideoLinkUrl1);
        }
        $('#myform').submit();
        $("#myLoadingElement").hide();
    });
    // Build the FirstExecise set on validation while creating challenges
    var firstexeciseset = $('#FFExeDesc1').val();
    var execiseName = $('#FFExeName1').val();
    var alnateexeciseName = $('#FFAExeName1').val();
    var isAltenateexeciseName = $('#IsFFAExeName');
    if (isAltenateexeciseName !== undefined && (isAltenateexeciseName).is(':checked')) {
        execiseName = alnateexeciseName;
    }
    if (firstexeciseset !== undefined && firstexeciseset !== null && firstexeciseset !== "") {
        var execiseSetResultdata = firstexeciseset.split('<>');
        var exsetcount = 0;
        var execisesetitem = '';
        var addedsetcount = execiseSetResultdata.length - 1;
        for (var icounter = 0; icounter < execiseSetResultdata.length - 1; icounter++) {
            var execiseset = execiseSetResultdata[icounter].split('^');
            if (execiseset !== undefined && execiseset !== null && execiseset !== "" && execiseset.length > 4) {
                var exexiseresp = parseInt(execiseset[0]);
                var setresultvalue = execiseset[1].split('.');
                var SetTimeHHValue = "00";
                var SetTimeMMValue = "00";
                var SetTimeSSValue = "00";
                var SetTimeHSValue = "00";
                if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "") {
                    var setresultHHMMSS = setresultvalue[0].split(':');
                    if (setresultHHMMSS != undefined && setresultHHMMSS.length > 2) {
                        SetTimeHHValue = GetSetTimeFormat(setresultHHMMSS[0]);
                        SetTimeMMValue = GetSetTimeFormat(setresultHHMMSS[1]);
                        SetTimeSSValue = GetSetTimeFormat(setresultHHMMSS[2]);
                        SetTimeHSValue = GetSetTimeFormat(setresultvalue[1]);

                    }
                }
                var RestTimeHHValue = "00";
                var RestTimeMMValue = "00";
                var RestTimeSSValue = "00";
                var RestTimeHSValue = "00";
                //setRestTimeHHMMSS
                var setRestTimevalue = execiseset[2].split('.');
                if (setRestTimevalue !== undefined && setRestTimevalue !== null && setRestTimevalue !== "" && setRestTimevalue.length > 1) {
                    var setRestTimeHHMMSS = setRestTimevalue[0].split(':');
                    if (setRestTimeHHMMSS != undefined && setRestTimeHHMMSS.length > 2) {

                        RestTimeHHValue = GetSetTimeFormat(setRestTimeHHMMSS[0]);
                        RestTimeMMValue = GetSetTimeFormat(setRestTimeHHMMSS[1]);
                        RestTimeSSValue = GetSetTimeFormat(setRestTimeHHMMSS[2]);
                        RestTimeHSValue = GetSetTimeFormat(setRestTimevalue[1]);

                    }
                }
                var execisesetdescription = "";
                if (execiseset[3] !== undefined && execiseset[3] !== null && execiseset[3] !== "SNA") {
                    execisesetdescription = execiseset[3];
                }

                var setautocountchecked = "";

                if (execiseset[4] !== undefined && execiseset[4] !== null && execiseset[4] === "Yes") {
                    setautocountchecked = "checked";
                } else {
                    setautocountchecked = "";
                }
                ++exsetcount;
                if (exsetcount === 1) {
                    $('#execeMainsetId1').find('#exesetTittle1').text(execiseName + " , Set 1");
                    $('#execeMainsetId1').find('#ExecSetRep1').val(exexiseresp);
                    $('#execeMainsetId1').find('#SetTimeHours1').val(SetTimeHHValue);
                    $('#execeMainsetId1').find('#SetTimeMinute1').val(SetTimeMMValue);
                    $('#execeMainsetId1').find('#SetTimeSecond1').val(SetTimeSSValue);
                    $('#execeMainsetId1').find('#SetTimeHS1').val(SetTimeHSValue);
                    if (setautocountchecked !== "" && setautocountchecked === "checked") {
                        $('#execeMainsetId1').find('#IsSetAutoCountDown1').prop("checked", "cheched");
                    }
                    $('#execeMainsetId1').find('#ExecSetDescription1').text(execisesetdescription);
                    $('#execeMainsetId1').find('#RestTimeHours1').val(RestTimeHHValue);
                    $('#execeMainsetId1').find('#RestTimeMinute1').val(RestTimeMMValue);
                    $('#execeMainsetId1').find('#RestTimeSecond1').val(RestTimeSSValue);
                    $('#execeMainsetId1').find('#RestTimeHS1').val(RestTimeHSValue);
                }
                else {
                    var model = {
                        ExeciseCountID: parseInt(excount),
                        ExeciseSetCountID: exsetcount,
                        ExeciseName: execiseName,
                        SetReps: exexiseresp,
                        ResultHH: SetTimeHHValue,
                        ResultMM: SetTimeMMValue,
                        ResultSS: SetTimeSSValue,
                        ResultHS: SetTimeHSValue,
                        RestHH: RestTimeHHValue,
                        RestMM: RestTimeMMValue,
                        RestSS: RestTimeSSValue,
                        RestHS: RestTimeHSValue,
                        Execisesetdescription: execisesetdescription,
                        AutoCountDownYesStatus: setautocountchecked,
                        IsNewAddedSet: 0,
                        IsHideBtnDelete: false
                    };
                    var url = baseUrlString + "/Reporting/GetFFExeciseSetDetail";
                    jQuery.ajax({
                        type: "POST",
                        url: url,
                        data: model,
                        async: false,
                        success: function (data) {
                            execisesetitem += data;
                        },
                        error: function (result) {
                            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                        }
                    });
                }
                //  execisesetitem += '<div id=' + excount + '><div class="set-heading"><span id="exesetTittle' + exsetcount + '">' + execiseName + ', Set ' + exsetcount + '</span>  <span class="remove-set"><button id="btnsetRemove' + exsetcount + '" class="remove_execiseset" >X</button></span></div><label>How Many Reps:</label><input class="freeforminput FreeFormExerciseVideos1 col-md-9 col-sm-9 col-xs-9 ui-autocomplete-input ui-corner-all" id="ExecSetRep' + exsetcount + '" name="ExecSetRep' + exsetcount + '" type="text" value=' + exexiseresp + ' autocomplete="off"><div class="or-section"><span>OR</span></div><label>How Much Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="SetTimeHours' + exsetcount + '" maxlength="2" name="SetTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeHHValue + '><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input" data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeMinute' + exsetcount + '" maxlength="2" name="SetTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeMMValue + '><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeSecond' + exsetcount + '" maxlength="2" name="SetTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeSSValue + '><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="SetTimeHS' + exsetcount + '" maxlength="2" name="SetTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeHSValue + '><div class="clr-both"></div><label>How Much Rest Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="RestTimeHours' + exsetcount + '" maxlength="2" name="RestTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeHHValue + '><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeMinute' + exsetcount + '" maxlength="2" name="RestTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeMMValue + '><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeSecond' + exsetcount + '" maxlength="2" name="RestTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeSSValue + '><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="RestTimeHS' + exsetcount + '" maxlength="2" name="RestTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeHSValue + '><div class="clr-both"></div><label>Description:</label><textarea class="freeformtextarea col-md-10 col-sm-10 col-xs-9" cols="20" id="ExecSetDescription' + exsetcount + '" maxlength="500" name="ExecSetDescription' + exsetcount + '" placeholder="Max Char limit 500" rows="2">' + execisesetdescription + '</textarea></div>';

            }
        }

        $('#execeMainsetId1').append(execisesetitem);
        // Add added execise setcount;
        if (addedsetcount !== undefined && addedsetcount !== null && addedsetcount !== "0") {
            $('#ExerciseSetCount1').val(addedsetcount);
        }
    }
    // Build the Execise and set on validation while creating challenges
    var createdExeciseList = $('#FreeFormExerciseNameDescriptionList').val();
    if (createdExeciseList !== undefined && createdExeciseList !== null && createdExeciseList !== "") {
        var execiselist = createdExeciseList.split('|');
        var items = '';
        // var path = baseUrlString + "/images/remove.png";
        if (execiselist !== undefined && execiselist !== null && execiselist !== "") {
            for (var icounter = 0; icounter < execiselist.length; icounter++) {
                var execisedata = execiselist[icounter];
                if (execisedata !== undefined && execisedata !== null && execisedata !== "") {
                    var execisedetails = execisedata.split('~');
                    var insertedexeciseName = execisedetails[0];
                    var description = execisedetails[1];
                    var LinksDescription = execisedetails[2];
                    var isAlternateEName = execisedetails[3];
                    var alternateEName = execisedetails[4];
                    if (alternateEName === "NA" && insertedexeciseName === "NA") {
                        continue;
                    }
                    if (insertedexeciseName === "NA") {
                        insertedexeciseName = "";
                    }
                    if (LinksDescription === "NA") {
                        LinksDescription = "";
                    }
                    var dispalyAlternateEName = "none";
                    var isAlternateText = "";
                    var isExecisedisabled = "";
                    var isExecisedisabled = "";
                    var ischeckboxdisabled = "";
                    if (isAlternateEName === "NA") {
                        isAlternateEName = "false";
                    }
                    if (alternateEName === "NA") {
                        alternateEName = "";
                    }
                    if (isAlternateEName == "true") {
                        isAlternateText = "checked";
                        isExecisedisabled = "disabled";
                        dispalyAlternateEName = "block";
                        insertedexeciseName = alternateEName;
                    } else {
                        ischeckboxdisabled = "disabled";
                    }
                    var exsetcount = 0;
                    if (description === "NA") {
                        description = "";
                    }
                    else {
                        var execiseSetResultdata = execisedetails[1].split('<>');
                        exsetcount = 0;
                        var execisesetitemdesc = '';

                        for (var exesetcounter = 0; exesetcounter < execiseSetResultdata.length - 1; exesetcounter++) {
                            var execiseset = execiseSetResultdata[exesetcounter].split('^');
                            if (execiseset !== undefined && execiseset !== null && execiseset !== "" && execiseset.length > 4) {
                                var exexiseresp = parseInt(execiseset[0]);
                                var setresultvalue = execiseset[1].split('.');
                                var SetTimeHHValue = "00";
                                var SetTimeMMValue = "00";
                                var SetTimeSSValue = "00";
                                var SetTimeHSValue = "00";
                                if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "" && setresultvalue.length > 1) {
                                    var setresultHHMMSS = setresultvalue[0].split(':');
                                    if (setresultHHMMSS !== undefined && setresultHHMMSS.length > 2) {
                                        SetTimeHHValue = GetSetTimeFormat(setresultHHMMSS[0]);
                                        SetTimeMMValue = GetSetTimeFormat(setresultHHMMSS[1]);
                                        SetTimeSSValue = GetSetTimeFormat(setresultHHMMSS[2]);
                                        SetTimeHSValue = GetSetTimeFormat(setresultvalue[1]);

                                    }
                                }
                                var RestTimeHHValue = "00";
                                var RestTimeMMValue = "00";
                                var RestTimeSSValue = "00";
                                var RestTimeHSValue = "00";
                                //setRestTimeHHMMSS
                                var setRestTimevalue = execiseset[2].split('.');
                                if (setRestTimevalue !== undefined && setRestTimevalue !== null && setRestTimevalue !== "" && setRestTimevalue.length > 1) {
                                    var setRestTimeHHMMSS = setRestTimevalue[0].split(':');
                                    if (setRestTimeHHMMSS != undefined && setRestTimeHHMMSS.length > 2) {

                                        RestTimeHHValue = GetSetTimeFormat(setRestTimeHHMMSS[0]);
                                        RestTimeMMValue = GetSetTimeFormat(setRestTimeHHMMSS[1]);
                                        RestTimeSSValue = GetSetTimeFormat(setRestTimeHHMMSS[2]);
                                        RestTimeHSValue = GetSetTimeFormat(setRestTimevalue[1]);

                                    }
                                }
                                var execisesetdescription = "";
                                if (execiseset[3] !== undefined && execiseset[3] !== null && execiseset[3] !== "SNA") {
                                    execisesetdescription = execiseset[3];
                                }
                                var setautocountchecked = "";
                                if (execiseset[4] !== undefined && execiseset[4] !== null && execiseset[4] === "Yes") {
                                    setautocountchecked = "checked";
                                } else {
                                    setautocountchecked = "checked";
                                }
                                ++exsetcount;
                                var isHideDeleteOption = false;
                                isHideDeleteOption = (exesetcounter == 0) ? true : false;
                                var setmodel = {
                                    ExeciseCountID: parseInt(excount),
                                    ExeciseSetCountID: exsetcount,
                                    ExeciseName: insertedexeciseName,
                                    SetReps: exexiseresp,
                                    ResultHH: SetTimeHHValue,
                                    ResultMM: SetTimeMMValue,
                                    ResultSS: SetTimeSSValue,
                                    ResultHS: SetTimeHSValue,
                                    RestHH: RestTimeHHValue,
                                    RestMM: RestTimeMMValue,
                                    RestSS: RestTimeSSValue,
                                    RestHS: RestTimeHSValue,
                                    Execisesetdescription: execisesetdescription,
                                    AutoCountDownYesStatus: setautocountchecked,
                                    IsNewAddedSet: 0,
                                    IsHideBtnDelete: isHideDeleteOption
                                };
                                var seturl = baseUrlString + "/Reporting/GetFFExeciseSetDetail";
                                jQuery.ajax({
                                    type: "POST",
                                    url: seturl,
                                    data: setmodel,
                                    async: false,
                                    success: function (data) {
                                        execisesetitemdesc += data;
                                    },
                                    error: function (result) {
                                        alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                                    }
                                });
                                // execisesetitem += '<div id=' + excount + '><div class="set-heading"><span id="exesetTittle' + exsetcount + '">' + execiseName + ', Set ' + exsetcount + '</span>  <span class="remove-set"><button  id="btnsetRemove' + exsetcount + '" class="remove_execiseset">X</button></span></div><label>How Many Reps:</label><input class="freeforminput FreeFormExerciseVideos1 col-md-9 col-sm-9 col-xs-9 ui-autocomplete-input ui-corner-all" id="ExecSetRep' + exsetcount + '" name="ExecSetRep' + exsetcount + '" type="text" value=' + exexiseresp + ' autocomplete="off"><div class="or-section"><span>OR</span></div><label>How Much Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="SetTimeHours' + exsetcount + '" maxlength="2" name="SetTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeHHValue + '><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input" data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeMinute' + exsetcount + '" maxlength="2" name="SetTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeMMValue + '><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="SetTimeSecond' + exsetcount + '" maxlength="2" name="SetTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeSSValue + '><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="SetTimeHS' + exsetcount + '" maxlength="2" name="SetTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + SetTimeHSValue + '><div class="clr-both"></div><label>How Much Rest Time:</label><label style="width:47px;margin-right:2px;" class="lbl-txt">Hour(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Hour(s)." data-val-range-max="24" data-val-range-min="0" id="RestTimeHours' + exsetcount + '" maxlength="2" name="RestTimeHours' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeHHValue + '><label style="width:55px;margin-right:5px;" class="lbl-txt">Minute(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Minute(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeMinute' + exsetcount + '" maxlength="2" name="RestTimeMinute' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeMMValue + '><label style="width:58px;margin-right:5px;" class="lbl-txt">Second(s)</label><input class="input " data-val="true" data-val-range="Please enter valid Second(s)." data-val-range-max="60" data-val-range-min="0" id="RestTimeSecond' + exsetcount + '" maxlength="2" name="RestTimeSecond' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeSSValue + '><label style="width:30px;margin-right:5px;" class="lbl-txt">HS(s)</label><input class="input" data-val="true" data-val-range="Please enter valid HS(s)." data-val-range-max="99" data-val-range-min="0" id="RestTimeHS' + exsetcount + '" maxlength="2" name="RestTimeHS' + exsetcount + '" style="width:60px;padding:5px;margin-right:5px;text-align:center;height:32px;" type="text" value=' + RestTimeHSValue + '><div class="clr-both"></div><label>Description:</label><textarea class="freeformtextarea col-md-9 col-sm-9 col-xs-9" cols="20" id="ExecSetDescription' + exsetcount + '" maxlength="500" name="ExecSetDescription' + exsetcount + '" placeholder="Max Char limit 500" rows="2">' + execisesetdescription + '</textarea></div>';

                            }
                        }
                        // Add added execise setcount;

                    }
                    if (excount < max_fields) {
                        //max input box allowed
                        excount++; //text box increment                      
                        if (isAlternateEName == "true") {
                            insertedexeciseName = "";
                        }
                        var execisemodel = {
                            ExeciseCountID: excount,
                            Exsetcount: exsetcount,
                            IsExecisedisabled: isExecisedisabled,
                            LinksDescription: LinksDescription,
                            ExerciseThumnail: LinksDescription,
                            FFExeName: insertedexeciseName,
                            IsAlternateText: isAlternateText,
                            Ischeckboxdisabled: ischeckboxdisabled,
                            IsAlternateEName: isAlternateEName,
                            FFAExeName: alternateEName,
                            DispalyAlternateEName: dispalyAlternateEName,
                            ExeciseItemElement: execisesetitemdesc,
                            IsNewAddedExercise: 0
                        };
                        var execiseurl = baseUrlString + "/Reporting/GetFFExeciseDetail";
                        jQuery.ajax({
                            type: "POST",
                            url: execiseurl,
                            data: execisemodel,
                            async: false,
                            success: function (data) {
                                items += data;
                            },
                            error: function (result) {
                                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                            }
                        });
                        if (items !== undefined && items != null && items !== '') {
                            $(wrapper).append(items);
                            items = "";
                            BindEquipment(excount);
                            BindTrainingZone(excount);
                            BindExerciseType(excount);
                        }
                    }
                }
            }
        }
    }
    jQuery(document).delegate("input:checkbox[id^='IsFFAExeName']", "change", function () {
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        var exericeNameID = '#FFExeName' + id;
        var dvFFAExeName = '#FFAExeName' + id;
        var exeLinkedUrl = '#FFExeVideoLinkUrl' + id;
        if (this.checked) {
            jQuery(dvFFAExeName).show();
            jQuery(exeLinkedUrl).hide();
            jQuery(exericeNameID).prop("disabled", true);
            // $('.add_field_button').prop('disabled', false);
        }
        else {
            jQuery(dvFFAExeName).hide();
            jQuery(exeLinkedUrl).show();
            jQuery(exericeNameID).prop("disabled", false);
        }
    });
    jQuery(document).delegate("input:checkbox[id^='IsSetFirstExecise']", "change", function () {
        var th = $(this), name = th.prop('name');
        if (th.is(':checked')) {
            $(':checkbox[name="' + name + '"]').not($(this)).prop('checked', false);
        }
    });
    var isAlternativeExeciseName = jQuery('#IsFFAExeName1');
    var dvFFAExeName1 = jQuery('#FFExeName1');
    var ffexecise = $('#FFExeName1').val();
    jQuery('#FFAExeName1').hide();
    if (isAlternativeExeciseName != undefined && isAlternativeExeciseName != null && isAlternativeExeciseName.is(':checked')) {
        jQuery('#FFAExeName1').show();
        JQuery('#FFExeVideoLinkUrl1').hide();
        jQuery(dvFFAExeName1).prop("disabled", true);
        //  $('.add_field_button').prop('disabled', false);
        jQuery('#IsFFAExeName1').prop("disabled", false);
    } else if (ffexecise !== undefined && ffexecise !== null && ffexecise !== "") {
        jQuery('#FFAExeName1').hide();
        //  JQuery('#FFExeVideoLinkUrl1').show();
        jQuery('#IsFFAExeName1').prop("disabled", true);
        jQuery(dvFFAExeName1).prop("disabled", false);
    }
    jQuery(function () {      
        jQuery('#ChallengeSubTypeId').on("change", function () {           
            var id = jQuery("#ChallengeSubTypeId :selected").val();          
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
                     
        });


    });
    jQuery(function () {
        var challengeSubtypeId = jQuery('#ChallengeSubTypeId').val();
        if (challengeSubtypeId !== undefined && challengeSubtypeId !== null && challengeSubtypeId != "") {
            if (challengeSubtypeId === "15") {
                $('#FreeFormChallegeDetailsSection').show();
                $('#CreateChallenegeEquipmentRequirement').hide();
                $('#CreateChallenegeTrainingZone').hide();
                $('#CreateChallenegeExerciseType').hide();
                $('#challengeNoTrainerTeam').hide();
                $('#TrendingCategoryHeader').text("Step 9 - Select Your Trending Category:");
                $('#divTrendingCategorySection').hide();
                $('#divChallengeCategorySection').hide();
            }
            else if (challengeSubtypeId === "14") {
                $('#FreeFormChallegeDetailsSection').hide();
                $('#CreateChallenegeEquipmentRequirement').show();
                $('#CreateChallenegeTrainingZone').show();
                $('#CreateChallenegeExerciseType').show();
                $('#challengeNoTrainerTeam').show();
                $('#TrendingCategoryHeader').text("Step 13 - Select Your Trending Category:");
                $('#divTrendingCategorySection').show();
                $('#divChallengeCategorySection').show();
                $('#divFittnessTrendingCategorySection').hide();
                $('#divTrendingCategorySection').show();


            }
            else {
                $('#divFittnessTrendingCategorySection').show();
                $('#divTrendingCategorySection').hide();
            }
        }

    });
    jQuery(document).delegate("#TrainerId", "change", function () {
        var challengeSubtypeId = jQuery('#ChallengeSubTypeId').val();
        if (challengeSubtypeId != "") {
            if (challengeSubtypeId === "14") {
                var selectedTrainerId = $("#TrainerId option:selected").val();
                if (selectedTrainerId > 0) {
                    $('#challengeNoTrainerTeam').hide();
                    $('#TrendingCategoryHeader').text("Step 12 - Select Your Trending Category:");
                }
                else {
                    $('#challengeNoTrainerTeam').show();
                    $('#TrendingCategoryHeader').text("Step 13 - Select Your Trending Category:");
                }
                $('#divTrendingCategorySection').show();
            }
        }
    });
    jQuery(function () {
        jQuery('#SelectAllTeam').change(function () {
            if (this.checked) {
                $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                    this.checked = false;
                });
            }
        });
        jQuery('#SelectAllTeam').load(function () {
            if (this.checked) {
                $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                    this.checked = false;
                });
            }
        });

        jQuery("#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]").click(function () {
            try {
                if (this.checked) {
                    var isAllChecked = false;
                    $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                        if (this.checked) {
                            isAllChecked = true;
                        } else {
                            isAllChecked = false;
                            return false;
                        }
                    });
                    if (isAllChecked) {
                        $("#SelectAllTeam").prop("checked", true);
                    }
                }
                else {
                    $("#SelectAllTeam").prop("checked", false);
                }
            }
            catch (err) {

            }

        });
    });

    jQuery(function () {
        if ($("#SelectWorkoutAllTrendingCategory").is(':checked')) {
            $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = true;
            });
        }
        jQuery('#SelectWorkoutAllTrendingCategory').change(function () {
            if (this.checked) {
                $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = false;
                });
            }
        });
        jQuery('#SelectWorkoutAllTrendingCategory').load(function () {
            if (this.checked) {
                $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = false;
                });
            }
        });

        jQuery("#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]").click(function () {
            try {
                if (this.checked) {
                    var isAllChecked = false;
                    $('#chkAllWorkoutTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                        if (this.checked) {
                            isAllChecked = true;
                        } else {
                            isAllChecked = false;
                            return false;
                        }
                    });
                    if (isAllChecked) {
                        $("#SelectWorkoutAllTrendingCategory").prop("checked", true);
                    }
                }
                else {
                    $("#SelectWorkoutAllTrendingCategory").prop("checked", false);
                }
            }
            catch (err) {

            }

        });
    });
    //jQuery(function () {
    //    if ($("#SelectAllFittnestTrendingCategory").is(':checked')) {
    //        $('#chkAllFittnestTrendingCategory [id*=PostedFittnessTrendingCategory_TrendingCategoryID]').each(function () {
    //            this.checked = true;
    //        });
    //    }
    //    jQuery('#SelectAllFittnestTrendingCategory').change(function () {
    //        if (this.checked) {
    //            $('#chkAllFittnestTrendingCategory [id*=PostedFittnessTrendingCategory_TrendingCategoryID]').each(function () {
    //                this.checked = true;
    //            });
    //        }
    //        else {
    //            $('#chkAllFittnestTrendingCategory [id*=PostedFittnessTrendingCategory_TrendingCategoryID]').each(function () {
    //                this.checked = false;
    //            });
    //        }
    //    });

    //    jQuery("#chkAllFittnestTrendingCategory [id*=PostedFittnessTrendingCategory_TrendingCategoryID]").click(function () {
    //        try {
    //            if (this.checked) {
    //                var isAllChecked = false;
    //                $('#chkAllFittnestTrendingCategory [id*=PostedFittnessTrendingCategory_TrendingCategoryID]').each(function () {
    //                    if (this.checked) {
    //                        isAllChecked = true;
    //                    } else {
    //                        isAllChecked = false;
    //                        return false;
    //                    }
    //                });
    //                if (isAllChecked) {
    //                    $("#SelectAllFittnestTrendingCategory").prop("checked", true);
    //                }
    //            }
    //            else {
    //                $("#SelectAllFittnestTrendingCategory").prop("checked", false);
    //            }
    //        }
    //        catch (err) {

    //        }

    //    });

    //});


    jQuery(function () {
        var selectedTrainerId = $("#TrainerId option:selected").val();
        if (selectedTrainerId > 0) {
            $('#challengeNoTrainerTeam').hide();
        }
        else {
            $('#challengeNoTrainerTeam').show();
        }
    });

    jQuery('#TrainerId').on("change", function () {
        var selectedTrainerId = $("#TrainerId option:selected").val();
        if (selectedTrainerId > 0) {
            $('#challengeNoTrainerTeam').hide();
        }
        else {
            $('#challengeNoTrainerTeam').show();
        }
    });
});


