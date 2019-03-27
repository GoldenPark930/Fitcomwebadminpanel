var UpdateChallengeExeciseCount = 1;
var selectedEquipmenitems = "";
var selectedTrainingZoneitems = "";
var selectedExerciseTypitems = "";
var $uploadCrop;
var reader;
// Add the set reps and result time and rest time in pi seperated string
function GetExeciseSets(execiseindexId) {
    var Execisetrecord = "";
    var exesetControlID = '#ExerciseSetCount' + execiseindexId;
    var icount = $(exesetControlID).val();
    var isEntededSetRep = true;
    var isEntededSetTime = true;
    var execiseSetSectionId = $('#execeMainsetId' + execiseindexId);
    var escount = 1;
    var setrepId = '#ExecSetRep' + escount;
    var setrepvalue = execiseSetSectionId.find(setrepId).val();
    //
    var setresultHHValue;
    var setresultMMValue;
    var setresultSSValue;
    var setresultHSValue;
    //
    var setRestresultHHValue;
    var setRestresultMMValue;
    var setRestresultSSValue;
    var setRestresultHSValue;
    //
    var setresultvalue;
    var setRestresultvalue;
    var setDescription;
    var setDescriptionvalue;
    //
    var setAutocountdownYesNo;
    var isSetAutoCountDownCtr;
    var setSetAutoCountYesDownvalue;
    while (escount <= icount) {
        var setrepId = '#ExecSetRep' + escount;
        var setrepvalue = execiseSetSectionId.find(setrepId).val();
        setrepvalue = parseInt(setrepvalue);
        if (!isNaN(setrepvalue) && setrepvalue !== undefined && setrepvalue !== null && setrepvalue !== "" && setrepvalue !== 0) {
            Execisetrecord += setrepvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
            isEntededSetRep = false;
        }
        setresultHHValue = execiseSetSectionId.find('#SetTimeHours' + escount).val();
        if (setresultHHValue === undefined || setresultHHValue === "undefined" || setresultHHValue === null || setresultHHValue === "") {
            setresultHHValue = "00";
        }
        setresultMMValue = execiseSetSectionId.find('#SetTimeMinute' + escount).val();
        if (setresultMMValue === undefined || setresultMMValue === "undefined" || setresultMMValue === null || setresultMMValue === "") {
            setresultMMValue = "00";
        }
        setresultSSValue = execiseSetSectionId.find('#SetTimeSecond' + escount).val();
        if (setresultSSValue === undefined || setresultSSValue === "undefined" || setresultSSValue === null || setresultSSValue === "") {
            setresultSSValue = "00";
        }
        setresultHSValue = execiseSetSectionId.find('#SetTimeHS' + escount).val();
        if (setresultHSValue === undefined || setresultHSValue === "undefined" || setresultHSValue === null || setresultHSValue === "") {
            setresultHSValue = "00";
        }
        setresultvalue = setresultHHValue + ":" + setresultMMValue + ":" + setresultSSValue + "." + setresultHSValue;
        if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "") {
            Execisetrecord += setresultvalue + "^";
            isEntededSetTime = (setresultvalue !== "00:00:00.00")
        } else {
            Execisetrecord += "SNA" + "^";
        }
        setRestresultHHValue = execiseSetSectionId.find('#RestTimeHours' + escount).val();
        if (setRestresultHHValue === undefined || setRestresultHHValue === "undefined" || setRestresultHHValue === null || setRestresultHHValue === "") {
            setRestresultHHValue = "00";
        }
        setRestresultMMValue = execiseSetSectionId.find('#RestTimeMinute' + escount).val();
        if (setRestresultMMValue === undefined || setRestresultMMValue === "undefined" || setRestresultMMValue === null || setRestresultMMValue === "") {
            setRestresultMMValue = "00";
        }
        setRestresultSSValue = execiseSetSectionId.find('#RestTimeSecond' + escount).val();
        if (setRestresultSSValue === undefined || setRestresultSSValue === null || setRestresultSSValue === "") {
            setRestresultSSValue = "00";
        }
        setRestresultHSValue = execiseSetSectionId.find('#RestTimeHS' + escount).val();
        if (setRestresultHSValue === undefined || setRestresultHSValue === null || setRestresultHSValue === "") {
            setRestresultHSValue = "00";
        }
        setRestresultvalue = setRestresultHHValue + ":" + setRestresultMMValue + ":" + setRestresultSSValue + "." + setRestresultHSValue;
        if (setRestresultvalue !== undefined && setRestresultvalue !== null && setRestresultvalue !== "") {
            Execisetrecord += setRestresultvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
        }
        setDescription = '#ExecSetDescription' + escount;
        setDescriptionvalue = execiseSetSectionId.find(setDescription).val();
        if (setDescriptionvalue !== undefined && setDescriptionvalue !== null && setDescriptionvalue !== "") {
            Execisetrecord += setDescriptionvalue + "^";
        } else {
            Execisetrecord += "SNA" + "^";
        }

        setAutocountdownYesNo = "SNA";
        isSetAutoCountDownCtr = '#IsSetAutoCountDown' + escount;
        setSetAutoCountYesDownvalue = execiseSetSectionId.find('#IsSetAutoCountDown' + escount).is(':checked')
        if (setSetAutoCountYesDownvalue) {
            setAutocountdownYesNo = "Yes";
        } else {
            setAutocountdownYesNo = "No";
        }
        Execisetrecord += setAutocountdownYesNo;
        Execisetrecord += "<>";
        escount += 1;
    }
    escount = 0;
    if (isEntededSetRep || isEntededSetTime) {
        return Execisetrecord;
    } else {
        return "";
    }
    //return Execisetrecord;
}
//Update Execise Set flag
function UpdateNewAddedExeciseSetFlage(execiseindexId) {
    var exesetControlID = '#ExerciseSetCount' + execiseindexId;
    var icount = $(exesetControlID).val();
    var execiseSetSectionId = $('#execeMainsetId' + execiseindexId);
    var escount = 1;
    while (escount <= icount) {
        var setrepId = '#IsNewAddedSet' + escount;
        execiseSetSectionId.find(setrepId).val("0");
        escount += 1;
    }
    return false;
}
// Auto save Execise details based on
function SaveExeciseLastAutoSavedSet(execiseindexId, execisesetId) {
    var lastExeciseSet = "";
    var lastexeciseSetSectionId = $('#execeMainsetId' + execiseindexId);
    var escount = parseInt(execisesetId);
    var lastIsAddedSetControlId = ('#IsNewAddedSet' + escount);
    var lastIsAddedSetId = lastexeciseSetSectionId.find(lastIsAddedSetControlId).val();
    var lastIsAddedExercise = $('#IsNewAddedExercise' + execiseindexId).val();
    var challengeAssociateRecordId = $('#page-wrapper').find('#CEARocordId' + execiseindexId).val();
    if (lastIsAddedExercise !== undefined && lastIsAddedExercise !== null && lastIsAddedExercise !== "" && lastIsAddedExercise === "0") {
        if (lastIsAddedSetId !== undefined && lastIsAddedSetId !== null && lastIsAddedSetId !== "" && lastIsAddedSetId === "1") {
            if (escount !== undefined && escount !== null && escount > 0) {
                var setrepId = '#ExecSetRep' + escount;
                var setrepvalue = lastexeciseSetSectionId.find(setrepId).val();
                if (setrepvalue !== undefined && setrepvalue !== null && setrepvalue !== "") {
                    lastExeciseSet += setrepvalue + "^";
                } else {
                    lastExeciseSet += "SNA" + "^";
                }
                var setresultHHValue = lastexeciseSetSectionId.find('#SetTimeHours' + escount).val();
                if (setresultHHValue === undefined || setresultHHValue === "undefined" || setresultHHValue === null || setresultHHValue === "") {
                    setresultHHValue = "00";
                }
                var setresultMMValue = lastexeciseSetSectionId.find('#SetTimeMinute' + escount).val();
                if (setresultMMValue === undefined || setresultMMValue === "undefined" || setresultMMValue === null || setresultMMValue === "") {
                    setresultMMValue = "00";
                }
                var setresultSSValue = lastexeciseSetSectionId.find('#SetTimeSecond' + escount).val();
                if (setresultSSValue === undefined || setresultSSValue === "undefined" || setresultSSValue === null || setresultSSValue === "") {
                    setresultSSValue = "00";
                }
                var setresultHSValue = lastexeciseSetSectionId.find('#SetTimeHS' + escount).val();
                if (setresultHSValue === undefined || setresultHSValue === "undefined" || setresultHSValue === null || setresultHSValue === "") {
                    setresultHSValue = "00";
                }
                var setresultvalue = setresultHHValue + ":" + setresultMMValue + ":" + setresultSSValue + "." + setresultHSValue;
                if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "") {
                    lastExeciseSet += setresultvalue + "^";
                } else {
                    lastExeciseSet += "SNA" + "^";
                }
                var setRestresultHHValue = lastexeciseSetSectionId.find('#RestTimeHours' + escount).val();
                if (setRestresultHHValue === undefined || setRestresultHHValue === "undefined" || setRestresultHHValue === null || setRestresultHHValue === "") {
                    setRestresultHHValue = "00";
                }
                var setRestresultMMValue = lastexeciseSetSectionId.find('#RestTimeMinute' + escount).val();
                if (setRestresultMMValue === undefined || setRestresultMMValue === "undefined" || setRestresultMMValue === null || setRestresultMMValue === "") {
                    setRestresultMMValue = "00";
                }
                var setRestresultSSValue = lastexeciseSetSectionId.find('#RestTimeSecond' + escount).val();
                if (setRestresultSSValue === undefined || setRestresultSSValue === null || setRestresultSSValue === "") {
                    setRestresultSSValue = "00";
                }
                var setRestresultHSValue = lastexeciseSetSectionId.find('#RestTimeHS' + escount).val();
                if (setRestresultHSValue === undefined || setRestresultHSValue === null || setRestresultHSValue === "") {
                    setRestresultHSValue = "00";
                }
                var setRestresultvalue = setRestresultHHValue + ":" + setRestresultMMValue + ":" + setRestresultSSValue + "." + setRestresultHSValue;
                if (setRestresultvalue !== undefined && setRestresultvalue !== null && setRestresultvalue !== "") {
                    lastExeciseSet += setRestresultvalue + "^";
                } else {
                    lastExeciseSet += "SNA" + "^";
                }
                var setDescription = '#ExecSetDescription' + escount;
                var setDescriptionvalue = lastexeciseSetSectionId.find(setDescription).val();
                if (setDescriptionvalue !== undefined && setDescriptionvalue !== null && setDescriptionvalue !== "") {
                    lastExeciseSet += setDescriptionvalue + "^";
                } else {
                    lastExeciseSet += "SNA" + "^";
                }

                var setAutocountdownYesNo = "SNA";
                var isSetAutoCountDownCtr = '#IsSetAutoCountDown' + escount;
                var setSetAutoCountYesDownvalue = lastexeciseSetSectionId.find('#IsSetAutoCountDown' + escount).is(':checked')
                if (setSetAutoCountYesDownvalue) {
                    setAutocountdownYesNo = "Yes";
                } else {
                    setAutocountdownYesNo = "No";
                }
                lastExeciseSet += setAutocountdownYesNo;
                lastExeciseSet += "<>";
                // save set data into database
                var lastExeciseSaveurl = baseUrlString + "/Reporting/AutoSaveExeciseSet";
                $.post(lastExeciseSaveurl, { CEARecordId: parseInt(challengeAssociateRecordId), ExeciseSetDataString: lastExeciseSet })
               .done(function (response) {
                   lastexeciseSetSectionId.find(lastIsAddedSetControlId).val("0");
               });
                lastExeciseSet = "";

            }
        }
    }


    return false;
}
// Auto save Execise details with all set based on challenge
function SaveLastExeciseWithSets(execiseindexId) {
    var lastAutoSaveExeciseDescription = "";
    var lastexeciseSectionId = $('#execeMainsetId' + execiseindexId).val();
    var challengeId = $('#ChallengeId').val();
    var lastIsAddedExercise = $('#IsNewAddedExercise' + execiseindexId).val();
    if (lastIsAddedExercise !== undefined && lastIsAddedExercise !== null && lastIsAddedExercise !== "" && lastIsAddedExercise === "1") {
        var exericeControlID = '#FFExeName' + execiseindexId;
        var execiseName = $(exericeControlID).val();
        if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
            lastAutoSaveExeciseDescription += execiseName + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var execiseSetDetails = "";
        execiseSetDetails = GetExeciseSets(execiseindexId);
        var execisesetcount = '#ExerciseSetCount' + execiseindexId;
        var execisesetremaingtotalCount = $(execisesetcount).val();
        var wellnessSubtypeId = jQuery('#ChallengeSubTypeId').val();
        if (wellnessSubtypeId != "") {
            if (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId === "14" && (execisesetremaingtotalCount === "0" || execisesetremaingtotalCount == 0)) {
                alert("Please enter at least one set for each workouts exercise.");
                return false;
            }
        }
        if (execiseSetDetails !== undefined && execiseSetDetails !== null && execiseSetDetails !== "") {
            lastAutoSaveExeciseDescription += execiseSetDetails + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var exeVideoLinkurlID = '#FFExeVideoLinkUrl' + execiseindexId;
        var exeVideoLinkurl = $(exeVideoLinkurlID).text();
        if (exeVideoLinkurl !== undefined && exeVideoLinkurl !== null && exeVideoLinkurl !== "") {
            lastAutoSaveExeciseDescription += exeVideoLinkurl + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var isFFAExeNameCtr = '#IsFFAExeName' + execiseindexId;
        var exeIsFFAExeName;
        if ($(isFFAExeNameCtr).is(':checked')) {
            exeIsFFAExeName = "true";
        } else {
            exeIsFFAExeName = "false";
        }
        if (exeIsFFAExeName !== undefined && exeIsFFAExeName !== null && exeIsFFAExeName !== "") {
            lastAutoSaveExeciseDescription += exeIsFFAExeName + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var exeFFAExeNameCtrl = '#FFAExeName' + execiseindexId;
        var exeFFAExeName = $(exeFFAExeNameCtrl).val();
        if (exeFFAExeName !== undefined && exeFFAExeName !== null && exeFFAExeName !== "") {
            lastAutoSaveExeciseDescription += exeFFAExeName + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var exeFFAExerciseIdCtrl = '#ExerciseId' + execiseindexId;
        var exeFFAExerciseId = $(exeFFAExerciseIdCtrl).val();
        if (exeFFAExerciseId !== undefined && exeFFAExerciseId !== null && exeFFAExerciseId !== "") {
            lastAutoSaveExeciseDescription += exeFFAExerciseId + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var drpdownEquipmentNameValues = jQuery('#FFdrpdownEquipments' + execiseindexId).find('option:selected').val();
        if (drpdownEquipmentNameValues !== undefined && drpdownEquipmentNameValues !== null && drpdownEquipmentNameValues !== "" && drpdownEquipmentNameValues !== "--Choose Equipments--") {
            lastAutoSaveExeciseDescription += drpdownEquipmentNameValues + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var drpdownTrainingZonesValues = jQuery('#FFdrpdownTrainingZones' + execiseindexId).find('option:selected').val();
        if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "--Choose Training Zones--") {
            lastAutoSaveExeciseDescription += drpdownTrainingZonesValues + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var drpdownExerciseTypes = jQuery('#FFdrpdownExerciseTypes' + execiseindexId).find('option:selected').val();
        if (drpdownExerciseTypes !== undefined && drpdownExerciseTypes !== null && drpdownExerciseTypes !== "" && drpdownExerciseTypes !== "--Choose Exercise Types--") {
            lastAutoSaveExeciseDescription += drpdownExerciseTypes + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }
        var cEARocordIdvalue = jQuery('#CEARocordId' + execiseindexId).val();
        if (cEARocordIdvalue !== undefined && cEARocordIdvalue !== null && cEARocordIdvalue !== "") {
            lastAutoSaveExeciseDescription += cEARocordIdvalue + "~";
        }
        else {
            lastAutoSaveExeciseDescription += "NA" + "~";
        }

        var isFirstExecise = '#IsSetFirstExecise' + execiseindexId;
        var isCheckedFirstExecise = "false";
        if ($(isFirstExecise).is(':checked')) {
            isCheckedFirstExecise = "true";
        } else {
            isCheckedFirstExecise = "false";
        }
        lastAutoSaveExeciseDescription += isCheckedFirstExecise;
        lastAutoSaveExeciseDescription += "|";

        var lastExeciseSaveurl = baseUrlString + "/Reporting/AutoSaveExecise";
        $.post(lastExeciseSaveurl, { challengeId: parseInt(challengeId), execiseDescription: lastAutoSaveExeciseDescription })
         .done(function (response) {
             // update the execise new added execise and asscocites sets
             $('#IsNewAddedExercise' + execiseindexId).val("0");
             UpdateNewAddedExeciseSetFlage(execiseindexId);
         });

    }
    lastAutoSaveExeciseDescription = "";
    return false;
}

function ShowFFUpdateWellnessCChallengeSection() {
    $('#FreeFormChallegeDetailsSection').show();
    $('#challegeDifficultyHeader').text(" Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text(" Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text(" Challenge Details:");
    $('#ChallegeDurationHeader').text(" How long is Workout:");
    $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text(" Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text(" Select Target Training Zone:");
    $('#AdminStepExerciseType').text(" Choose Exercise Type:");
    $('#AdminStepChallengeName').text(" Enter Challenge Name:");
    $('#AdminStepTrainerName').text(" Choose Trainer:");
    $('#CreateChallenegeEquipmentRequirement').hide();
    $('#CreateChallenegeTrainingZone').hide();
    $('#CreateChallenegeExerciseType').hide();
    $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
    $('#challengeNoTrainerTeam').hide();
    $('#divTrendingCategorySection').hide();
    $('#divchkFeatured').hide();
    $('#divChallengeCategorySection').hide();
}
function HideFFUpdateWellnessChallengeSection() {
    $('#FreeFormChallegeDetailsSection').hide();
    $('#challegeDifficultyHeader').text(" Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text(" Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text(" Challenge Details:");
    $('#ChallegeDurationHeader').text(" How long is Workout:");
    $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text("Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text(" Select Target Training Zone:");
    $('#AdminStepExerciseType').text(" Choose Exercise Type:");
    $('#AdminStepChallengeName').text(" Enter Challenge Name:");
    $('#AdminStepTrainerName').text(" Choose Trainer:");
    $('#CreateChallenegeEquipmentRequirement').show();
    $('#CreateChallenegeTrainingZone').show();
    $('#CreateChallenegeExerciseType').show();
    $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
    $('#challengeNoTrainerTeam').show();
    $('#divTrendingCategorySection').show();
    $('#divchkFeatured').show();
    $('#divChallengeCategorySection').show();
}
function GetEquipmentItem() {
    var urlGetAllEquipment = baseUrlString + "/Reporting/GetAllEquipment";
    jQuery.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: urlGetAllEquipment,
        async: false,
        dataType: "json",
        beforeSend: function () {
            //alert(id);
        },
        success: function (data) {
            selectedEquipmenitems = "";
            selectedEquipmenitems = '<option value="0">--Choose Equipments--</option>';
            jQuery.each(data, function (i, equip) {
                selectedEquipmenitems += "<option value='" + equip.EquipmentId + "'>" + equip.Equipment + "</option>";
            });
        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });
}
function GetTrainingZoneItems() {
    var urlchallenge = baseUrlString + "/Reporting/GetAllTrainingZone";
    jQuery.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: urlchallenge,
        async: false,
        dataType: "json",
        beforeSend: function () {
            //alert(id);
        },
        success: function (data) {
            selectedTrainingZoneitems = "";
            selectedTrainingZoneitems = '<option value="0">--Choose Training Zones--</option>';
            jQuery.each(data, function (i, traingzone) {
                selectedTrainingZoneitems += "<option value='" + traingzone.PartId + "'>" + traingzone.PartName + "</option>";
            });
        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });
}
function GetExerciseTypeItems() {
    var urlchallenge = baseUrlString + "/Reporting/GetAllExerciseType";
    jQuery.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        url: urlchallenge,
        async: false,
        dataType: "json",
        beforeSend: function () {
            //alert(id);
        },
        success: function (data) {
            selectedExerciseTypitems = "";
            selectedExerciseTypitems = '<option value="0">--Choose Exercise Types--</option>';
            jQuery.each(data, function (i, execiseType) {
                selectedExerciseTypitems += "<option value='" + execiseType.ExerciseTypeId + "'>" + execiseType.ExerciseName + "</option>";
            });
        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });
}
// Bind the Equipemet,Trainig and Execise Type dropdown dynamically
function BindEquipment(execountNumber) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownEquipments = "#FFdrpdownEquipments" + execountNumber;
        // var urlchallenge = baseUrlString + "/Reporting/GetAllEquipment";
        if (selectedEquipmenitems === undefined && selectedEquipmenitems === null || selectedEquipmenitems === "") {
            GetEquipmentItem();
        }
        jQuery(drpdownEquipments).html(selectedEquipmenitems);

    }
}
function BindTrainingZone(execountNumber) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownTrainingZone = "#FFdrpdownTrainingZones" + execountNumber;
        if (selectedTrainingZoneitems === undefined || selectedTrainingZoneitems === null || selectedTrainingZoneitems === "") {
            GetTrainingZoneItems()
        }
        jQuery(drpdownTrainingZone).html(selectedTrainingZoneitems);


    }
}
function BindExerciseType(execountNumber) {
    if (execountNumber !== undefined || execountNumber !== null || execountNumber !== "" || execountNumber !== "0") {
        var drpdownExerciseType = "#FFdrpdownExerciseTypes" + execountNumber;
        if (selectedExerciseTypitems === undefined || selectedExerciseTypitems === null || selectedExerciseTypitems === "") {
            GetExerciseTypeItems();
        }
        jQuery(drpdownExerciseType).html(selectedExerciseTypitems);


    }

}
function BindSelectedEquipment(execountNumber, selectedvalue) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownEquipmentsId = "#FFdrpdownEquipments" + execountNumber;
        if (selectedEquipmenitems !== undefined && selectedEquipmenitems !== "") {
            jQuery(drpdownEquipmentsId).html(selectedEquipmenitems);
            jQuery(drpdownEquipmentsId).val(selectedvalue);
        }
    }
}
function SelectedBindTrainingZone(execountNumber, selectedvalue) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownTrainingZone = "#FFdrpdownTrainingZones" + execountNumber;
        if (selectedTrainingZoneitems !== undefined && selectedTrainingZoneitems !== "") {
            jQuery(drpdownTrainingZone).html(selectedTrainingZoneitems);
            jQuery(drpdownTrainingZone).val(selectedvalue);
        }
    }
}
function SelectedBindExerciseType(execountNumber, selectedvalue) {
    if (execountNumber !== undefined && execountNumber !== null && execountNumber !== "" && execountNumber !== "0") {
        var drpdownExerciseType = "#FFdrpdownExerciseTypes" + execountNumber;
        if (selectedExerciseTypitems !== undefined && selectedExerciseTypitems !== "") {
            jQuery(drpdownExerciseType).html(selectedExerciseTypitems);
            jQuery(drpdownExerciseType).val(selectedvalue);
        }
    }

}
function RenameExeciseId(starIdex, endIndex) {
    if (starIdex > 0 && endIndex > 0 && starIdex !== endIndex) {
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

function SaveAllexecisewithsets() {
    var ExeciseDescription = "";
    for (var icount = 1; icount <= UpdateChallengeExeciseCount ; icount++) {
        var iSEneterFirstset = false;
        var exericeControlID = '#FFExeName' + icount;
        var execiseName = $(exericeControlID).val();
        if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
            ExeciseDescription += execiseName + "~";
            iSEneterFirstset = true;
        }
        else {
            ExeciseDescription += "NA" + "~";
        }
        var execiseSetDetails = "";
        execiseSetDetails = GetExeciseSets(icount);
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
            iSEneterFirstset = true;
        }
        else {
            ExeciseDescription += "NA" + "~";
        }
        var exeFFAExerciseIdCtrl = '#ExerciseId' + icount;
        var exeFFAExerciseId = $(exeFFAExerciseIdCtrl).val();
        if (exeFFAExerciseId !== undefined && exeFFAExerciseId !== null && exeFFAExerciseId !== "") {
            ExeciseDescription += exeFFAExerciseId + "~";
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
        var cEARocordIdvalue = jQuery('#CEARocordId' + icount).val();
        if (cEARocordIdvalue !== undefined && cEARocordIdvalue !== null && cEARocordIdvalue !== "") {
            ExeciseDescription += cEARocordIdvalue;
        }
        else {
            ExeciseDescription += "NA";
        }

        ExeciseDescription += "|";
    }
    var lastExeciseSaveurl = baseUrlString + "/Reporting/AutoSaveAllExeciseWithSets";
    $.post(lastExeciseSaveurl, { challengeId: parseInt(challengeId), execiseDescription: ExeciseDescription })
     .done(function (response) {
     });
    return false;

}



function output(node) {
    var existing = $('#result .croppie-result');
    if (existing.length > 0) {
        existing[0].parentNode.replaceChild(node, existing[0]);
    }
    else {
        $('#result')[0].appendChild(node);
    }
}
function popupResult(result) {
    if (result.src) {
        $('#CropImageRowData').val(result.src);
        $('#myform').submit();
    }
    $uploadCrop = null;
    reader = null;
    return true;
}
function FeaturedPhotoUpload() {
    reader = null;
    function readFile(input) {
        if (input.files && input.files[0]) {
            reader = new FileReader();
            reader.onload = function (e) {
                $uploadCrop.croppie('bind', {
                    url: e.target.result
                });
                $('.upload-FeaturedImage').addClass('ready');
                $("#existingFeaturedImg").hide();
                $('#existingFeaturedImgConatiner').hide();
                $("#upload-FeaturedImage").show();
            }
            reader.readAsDataURL(input.files[0]);
        }
        else {
            //   swal("Sorry - you're browser doesn't support the FileReader API");
        }
    }

    $uploadCrop = $('#upload-FeaturedImage').croppie({
        viewport: {
            width: 300,
            height: 180,
            type: 'rectangle'
        },
        boundary: {
            width: 300,
            height: 180
        },
        enforceBoundary: false
    });


    $('#FeaturedUpload').on('change', function () { readFile(this); });

    //  reader = null;
}
function ShowFFChallengeSection() {
 
    $('#ExeciseRepsSec1').hide();
    $('#CraeteFreeformChallengeName').show();
    
    $('#FreeFormChallengeExeciseSection').show();
    $('#FreeFormChallegeDescriptionHeaderSection').show();   
    $('#FreeFormChallengeCateorySection').show();
    $('#challegeDifficultyHeader').text("Select Difficulty Level:");
    $('#ChallegeDescriptionHeader').text("Describe Your Challenge:");
    $('#FFChallegeDetailsHeader').text("Challenge Details:");
    $('#ChallegeDurationHeader').text("How long is Workout:");
    $('#AdminFreefromExeciseSection').text("Choose Exercise(s):");
    $('#ChallenegeEquipmentRequirementHeader').text("Select Equipment Requirements:");
    $('#AdminStepTrainingZone').text("Select Target Training Zone:");
    $('#AdminStepExerciseType').text("Choose Exercise Type:");
    $('#AdminStepChallengeName').text("Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Choose Trainer:"); 
    $('#divchkPremium').show();
    $('#FreeFormChallegeDurationSection').show();
    $('#divChallengeCategorySection').show();
    $('#divTrendingCategorySection').show();
  
}
function IsAllchkAllNoTrainetWorkoutTeam() {
    try {
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
        } else {
            $("#SelectAllTeam").prop("checked", false);
        }
    }
    catch (err) {

    }
}
function chkAllTrendingCategory() {
    var isAllChecked = false;
    $('#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
        if (this.checked) {
            isAllChecked = true;
        } else {
            isAllChecked = false;
            return false;
        }
    });
    if (isAllChecked) {
        $("#SelectAllTrendingCategory").prop("checked", true);
    } else {
        $("#SelectAllTrendingCategory").prop("checked", false);
    }
}
$(document).ready(function (e) {
    ShowFFChallengeSection();
    FeaturedPhotoUpload();
    IsAllchkAllNoTrainetWorkoutTeam();
    $("#myLoadingElement").show();
    $("#upload-FeaturedImage").hide();
    chkAllTrendingCategory();
    var max_fields = 99999999; //maximum input boxes allowed
    //  var UpdateChallengeExeciseSetcount = 1;
    var wrapper = $("#freeformtoggalediv"); //Fields wrapper
    var add_button = $(".add_field_button"); //Add button ID
    var selectedChallengeTypeId = $('#SelectedChallengeTypeId').val();
    var freeformchgId = 13;
    var createdExeciseList = $('#FreeFormExerciseNameDescriptionList').val();
    if (selectedChallengeTypeId !== undefined && selectedChallengeTypeId !== null && selectedChallengeTypeId == freeformchgId) {
        $('#ChallenegeEquipmentRequirementHeader').text(" Select Equipment Requirements:");
        $('#AdminStepTrainingZone').text(" Select Target Training Zone:");
        $('#AdminStepExerciseType').text(" Choose Exercise Type:");
        $('#AdminStepChallengeName').text(" Enter Challenge Name:");
        $('#AdminStepTrainerName').text(" Choose Trainer:");
        $('#AdminStepTrainerName').text(" Choose Trainer:");
        $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
        var execiseName = $('#FFExeName1').val()
        if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
            //$('.add_field_button').prop('disabled', false);
            $('#InsertExecise1').prop('disabled', false);
        }
        else {
            // $('.add_field_button').prop('disabled', true);
            $('#InsertExecise1').prop('disabled', true);
        }
    } else {
        $('#AdminStepTrainingZone').text(" Select Target Training Zone:");
        $('#AdminStepChallengeName').text(" Enter Challenge Name:");
        $('#AdminStepTrainerName').text(" Choose Trainer:");
        $('#AdminStepExerciseType').text(" Choose Exercise Type:");
        $('#AdminFreefromExeciseSection').text(" Choose Exercise(s):");
    }
    var firstexeciseset = $('#FFExeDesc1').val();
    var execiseName = $('#FFExeName1').val();
    var alnateexeciseName = $('#FFAExeName1').val();
    var isAltenateexeciseName = $('#IsFFAExeName1');
    if (isAltenateexeciseName !== undefined && isAltenateexeciseName !== "" && (isAltenateexeciseName).is(':checked')) {
        execiseName = alnateexeciseName;
    }
    // Update the First Execise Set Details
    if (firstexeciseset !== undefined && firstexeciseset !== null && firstexeciseset !== "") {
        var execiseSetResultdata = firstexeciseset.split('<>');
        var UpdateChallengeExeciseSetcount = 0;
        var execisesetitem = '';
        var addedsetcount = execiseSetResultdata.length - 1;
        for (var icounter = 0; icounter < execiseSetResultdata.length; icounter++) {
            var execiseset = execiseSetResultdata[icounter].split('^');
            if (execiseset !== undefined && execiseset !== null && execiseset !== "" && execiseset.length > 4) {
                var exexiseresp = parseInt(execiseset[0]);
                var setresultvalue = execiseset[1].split('.');
                var SetTimeHHValue = "00";
                var SetTimeMMValue = "00";
                var SetTimeSSValue = "00";
                var SetTimeHSValue = "00";
                if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "" && setresultvalue.length > 1) {
                    var setresultHHMMSS = setresultvalue[0].split(':');
                    if (setresultHHMMSS !== undefined && setresultHHMMSS !== null && setresultHHMMSS.length > 2) {

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
                    if (setRestTimeHHMMSS !== undefined && setRestTimeHHMMSS !== null && setRestTimeHHMMSS.length > 2) {
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
                ++UpdateChallengeExeciseSetcount;
                if (UpdateChallengeExeciseSetcount === 1) {
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
                    var isHideDeleteOption = false;
                    isHideDeleteOption = (icounter == 0) ? true : false;
                    var model = {
                        ExeciseCountID: parseInt(UpdateChallengeExeciseCount),
                        ExeciseSetCountID: UpdateChallengeExeciseSetcount,
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
                        IsHideBtnDelete: isHideDeleteOption
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
                            --UpdateChallengeExeciseSetcount;
                        }
                    });
                }
            }
        }
        $('#execeMainsetId1').append(execisesetitem);
        // Add added execise setcount;
        if (addedsetcount !== undefined && addedsetcount !== null && addedsetcount !== "0") {
            $('#ExerciseSetCount1').val(addedsetcount);
        }
    }

    // Create the execise list in case of update        
    if (selectedChallengeTypeId !== undefined && selectedChallengeTypeId !== null && createdExeciseList === "" && selectedChallengeTypeId == freeformchgId) {
        $("#myLoadingElement").show();
        GetEquipmentItem();
        GetTrainingZoneItems();
        GetExerciseTypeItems();
        var id = jQuery("#ChallengeId").val();
        if (id != "") {
            var urlchallenge = baseUrlString + "/Reporting/GetAllExeciseVideoByChallengeID";
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlchallenge + "/" + id,
                data: JSON.stringify(id),
                dataType: "json",
                async: false,
                beforeSend: function () {
                    //alert(id);
                },
                success: function (data) {
                    var items = '';
                    var path = baseUrlString + "/images/remove.png";
                    var addsetpath = baseUrlString + "/images/AddSet.png";
                    UpdateChallengeExeciseCount = 1;
                    jQuery.each(data, function (i, item) {
                        // Bind the First Execise Set
                        if (i == 0) {
                            var UpdateChallengeExeciseSetcount = 0;
                            var execisesetitem = '';
                            var exenumber = i;
                            exenumber = exenumber + 1;
                            var exesetControlID = '#ExerciseSetCount' + exenumber;
                            var exericeSetPlaceholdId = '#execeMainsetId' + exenumber;
                            $.each(item.ExeciseSetRecords, function (firstkey, val) {
                                if (UpdateChallengeExeciseSetcount < max_fields) { //max input box allowed
                                    //text box increment  
                                    var path = baseUrlString + "/images/RemoveSet.png";
                                    var setresultvalue = this.SetResult.split('.');
                                    var SetTimeHHValue = "00";
                                    var SetTimeMMValue = "00";
                                    var SetTimeSSValue = "00";
                                    var SetTimeHSValue = "00";
                                    if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "" && setresultvalue.length > 1) {
                                        var setresultHHMMSS = setresultvalue[0].split(':');
                                        if (setresultHHMMSS !== undefined && setresultHHMMSS !== null && setresultHHMMSS.length > 2) {
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
                                    var setRestTimevalue = this.RestTime.split('.');
                                    if (setRestTimevalue !== undefined && setRestTimevalue !== null && setRestTimevalue !== "" && setRestTimevalue.length > 1) {
                                        var setRestTimeHHMMSS = setRestTimevalue[0].split(':');
                                        if (setRestTimeHHMMSS !== undefined && setRestTimeHHMMSS !== null && setRestTimeHHMMSS.length > 2) {
                                            RestTimeHHValue = GetSetTimeFormat(setRestTimeHHMMSS[0]);
                                            RestTimeMMValue = GetSetTimeFormat(setRestTimeHHMMSS[1]);
                                            RestTimeSSValue = GetSetTimeFormat(setRestTimeHHMMSS[2]);
                                            RestTimeHSValue = GetSetTimeFormat(setRestTimevalue[1]);
                                        }
                                    }
                                    ++UpdateChallengeExeciseSetcount;
                                    var setautocountchecked = "";
                                    if (this.AutoCountDown !== null && this.AutoCountDown === "Yes") {
                                        setautocountchecked = "checked";
                                    } else {
                                        setautocountchecked = "";
                                    }
                                    if (firstkey == 0) {

                                        $('#execeMainsetId1').find('#exesetTittle1').text(execiseName + " , Set 1");
                                        $('#execeMainsetId1').find('#ExecSetRep1').val(this.SetReps);
                                        $('#execeMainsetId1').find('#SetTimeHours1').val(SetTimeHHValue);
                                        $('#execeMainsetId1').find('#SetTimeMinute1').val(SetTimeMMValue);
                                        $('#execeMainsetId1').find('#SetTimeSecond1').val(SetTimeSSValue);
                                        $('#execeMainsetId1').find('#SetTimeHS1').val(SetTimeHSValue);
                                        if (this.AutoCountDown !== null && this.AutoCountDown === "Yes") {
                                            $('#execeMainsetId1').find('#IsSetAutoCountDown1').prop("checked", "cheched");
                                        }
                                        $('#execeMainsetId1').find('#ExecSetDescription1').text(this.Description);
                                        $('#execeMainsetId1').find('#RestTimeHours1').val(RestTimeHHValue);
                                        $('#execeMainsetId1').find('#RestTimeMinute1').val(RestTimeMMValue);
                                        $('#execeMainsetId1').find('#RestTimeSecond1').val(RestTimeSSValue);
                                        $('#execeMainsetId1').find('#RestTimeHS1').val(RestTimeHSValue);
                                    }
                                    else {

                                        var setmodel1 = {
                                            ExeciseCountID: parseInt(UpdateChallengeExeciseCount),
                                            ExeciseSetCountID: UpdateChallengeExeciseSetcount,
                                            ExeciseName: execiseName,
                                            SetReps: this.SetReps,
                                            ResultHH: SetTimeHHValue,
                                            ResultMM: SetTimeMMValue,
                                            ResultSS: SetTimeSSValue,
                                            ResultHS: SetTimeHSValue,
                                            RestHH: RestTimeHHValue,
                                            RestMM: RestTimeMMValue,
                                            RestSS: RestTimeSSValue,
                                            RestHS: RestTimeHSValue,
                                            Execisesetdescription: this.Description,
                                            AutoCountDownYesStatus: setautocountchecked,
                                            IsNewAddedSet: 0,
                                            IsHideBtnDelete: false
                                        };
                                        var seturl1 = baseUrlString + "/Reporting/GetFFExeciseSetDetail";
                                        jQuery.ajax({
                                            type: "POST",
                                            url: seturl1,
                                            data: setmodel1,
                                            async: false,
                                            success: function (data) {
                                                execisesetitem += data;
                                            },
                                            error: function (result) {
                                                --UpdateChallengeExeciseSetcount;
                                                // alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                                            }
                                        });
                                    }
                                }
                            });
                            $(exericeSetPlaceholdId).append(execisesetitem);
                            $(exesetControlID).val(UpdateChallengeExeciseSetcount);
                            UpdateChallengeExeciseSetcount = 0;
                            exenumber = 0;
                        }
                        if (i > 0) {
                            // Bind the other than First Execise Set
                            if (UpdateChallengeExeciseCount < max_fields) { //max input box allowed                                  
                                var isAlternate = item.IsAlternateExeciseName;
                                var isAlternateText = "";
                                var dispalyAlternateEName = "none";
                                var ischeckboxdisabled = "";
                                var isExecisedisabled = "";
                                var isAlternateEName = "false";
                                var alternateEName = "";
                                var ischechedFirstExecise = "";
                                if (item.IsSetFirstExecise) {
                                    ischechedFirstExecise = "checked";
                                }

                                if (isAlternate) {
                                    dispalyAlternateEName = "block";
                                    isAlternateText = "checked";
                                    isExecisedisabled = "disabled";
                                    isAlternateEName = "true";
                                    execiseName = item.AlternateExeciseName;
                                } else {
                                    ischeckboxdisabled = "disabled";
                                    execiseName = item.ExerciseName;
                                    isAlternateEName = "false";
                                }
                                var execisesetitemdesc = '';
                                var UpdateChallengeExeciseSetcount = 0;
                                var exenumber = i;
                                exenumber = exenumber + 1;
                                $.each(item.ExeciseSetRecords, function (key, val) {
                                    if (UpdateChallengeExeciseSetcount < max_fields) { //max input box allowed
                                        //text box increment  
                                        //  var path = baseUrlString + "/images/RemoveSet.png";                                   
                                        //  var execount = exenumber;
                                        var setresultvalue = this.SetResult.split('.');
                                        var SetTimeHHValue = "00";
                                        var SetTimeMMValue = "00";
                                        var SetTimeSSValue = "00";
                                        var SetTimeHSValue = "00";
                                        if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "" && setresultvalue.length > 1) {
                                            var setresultHHMMSS = setresultvalue[0].split(':');
                                            if (setresultHHMMSS !== undefined && setresultHHMMSS !== null && setresultHHMMSS.length > 2) {
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
                                        var setRestTimevalue = this.RestTime.split('.');
                                        if (setRestTimevalue !== undefined && setRestTimevalue !== null && setRestTimevalue !== "" && setRestTimevalue.length > 1) {
                                            var setRestTimeHHMMSS = setRestTimevalue[0].split(':');
                                            if (setRestTimeHHMMSS !== undefined && setRestTimeHHMMSS !== null && setRestTimeHHMMSS.length > 2) {
                                                RestTimeHHValue = GetSetTimeFormat(setRestTimeHHMMSS[0]);
                                                RestTimeMMValue = GetSetTimeFormat(setRestTimeHHMMSS[1]);
                                                RestTimeSSValue = GetSetTimeFormat(setRestTimeHHMMSS[2]);
                                                RestTimeHSValue = GetSetTimeFormat(setRestTimevalue[1]);
                                            }
                                        }
                                        ++UpdateChallengeExeciseSetcount;
                                        var setautocountchecked = "";
                                        if (this.AutoCountDown !== null && this.AutoCountDown === "Yes") {
                                            setautocountchecked = "checked";
                                        } else {
                                            setautocountchecked = "";
                                        }
                                        var isHideDeleteOption = false;
                                        isHideDeleteOption = (key == 0) ? true : false;

                                        var setmodel = {
                                            ExeciseCountID: parseInt(UpdateChallengeExeciseCount),
                                            ExeciseSetCountID: parseInt(UpdateChallengeExeciseSetcount),
                                            ExeciseName: execiseName,
                                            SetReps: this.SetReps,
                                            ResultHH: SetTimeHHValue,
                                            ResultMM: SetTimeMMValue,
                                            ResultSS: SetTimeSSValue,
                                            ResultHS: SetTimeHSValue,
                                            RestHH: RestTimeHHValue,
                                            RestMM: RestTimeMMValue,
                                            RestSS: RestTimeSSValue,
                                            RestHS: RestTimeHSValue,
                                            Execisesetdescription: this.Description,
                                            AutoCountDownYesStatus: setautocountchecked,
                                            IsNewAddedSet: 0,
                                            IsHideBtnDelete: isHideDeleteOption
                                        };

                                        var newseturl = baseUrlString + "/Reporting/GetFFExeciseSetDetail";
                                        jQuery.ajax({
                                            type: "POST",
                                            url: newseturl,
                                            data: setmodel,
                                            async: false,
                                            success: function (data) {
                                                execisesetitemdesc += data;

                                            },
                                            error: function (result) {
                                                --UpdateChallengeExeciseSetcount;
                                                alert('Service call failed: GetFFExeciseSetDetail ' + result.status + ' Type :' + result.statusText);
                                            }
                                        });

                                    }
                                });
                                if (isAlternate) {
                                    execiseName = "";
                                    alternateEName = item.AlternateExeciseName;
                                    isAlternateEName = "true";
                                } else {
                                    execiseName = item.ExerciseName;
                                    isAlternateEName = "false";
                                }
                                var execisemodel = {
                                    ExeciseCountID: UpdateChallengeExeciseCount,
                                    ExerciseId: item.ExerciseId,
                                    Exsetcount: UpdateChallengeExeciseSetcount,
                                    IsExecisedisabled: isExecisedisabled,
                                    LinksDescription: item.VedioLink,
                                    ExerciseThumnail: item.ExerciseThumnail,
                                    FFExeName: execiseName,
                                    IsAlternateText: isAlternateText,
                                    Ischeckboxdisabled: ischeckboxdisabled,
                                    IsAlternateEName: isAlternateEName,
                                    FFAExeName: alternateEName,
                                    DispalyAlternateEName: dispalyAlternateEName,
                                    ExeciseItemElement: execisesetitemdesc,
                                    IsNewAddedExercise: 0,
                                    CEARocordId: item.CEARocordId,
                                    IsExeciseCountIDText: ischechedFirstExecise,

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
                                        alert('Service call failed: GetFFExeciseDetail ' + result.status + ' Type :' + result.statusText);
                                    }
                                });
                                UpdateChallengeExeciseSetcount = 0;
                                exenumber = 0;
                            }
                            // Fill ChallengeSubType Dropdown list
                            if (items !== undefined && items != null && items !== '') {
                                $(wrapper).append(items);
                                items = "";
                                BindSelectedEquipment(UpdateChallengeExeciseCount, this.SelectedEquipment);
                                SelectedBindTrainingZone(UpdateChallengeExeciseCount, this.SelectedTraingZone);
                                SelectedBindExerciseType(UpdateChallengeExeciseCount, this.SelectedExeciseType);
                            }
                        }
                        UpdateChallengeExeciseCount++; //text box increment                                               

                    });

                    //jQuery('#ChallengeSubTypeId').html(items);
                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                    $("#myLoadingElement").hide();
                }
            });
        }
        $("#myLoadingElement").hide();
    }
    // Appende the execise List in case of validation

    if (createdExeciseList !== undefined && createdExeciseList !== null && createdExeciseList !== "") {
        GetEquipmentItem();
        GetTrainingZoneItems();
        GetExerciseTypeItems();
        var execiselist = createdExeciseList.split('|');
        var items = '';
        var path = baseUrlString + "/images/remove.png";
        var addsetpath = baseUrlString + "/images/AddSet.png";
        if (execiselist !== undefined && execiselist !== null && execiselist !== "") {
            UpdateChallengeExeciseCount = 1;
            for (var icounter = 0; icounter < execiselist.length - 1; icounter++) {
                var execisedata = execiselist[icounter];
                if (execisedata !== undefined && execisedata !== null && execisedata !== "") {
                    var execisedetails = execisedata.split('~');
                    var execiseName = execisedetails[0];
                    var description = execisedetails[1];
                    var LinksDescription = execisedetails[2];
                    var isAlternateEName = execisedetails[3];
                    var alternateEName = execisedetails[4];
                    var alternateExeciseId = execisedetails[5];
                    var selectedEquipementValue = execisedetails[6];
                    var selectedTrainingzoneValue = execisedetails[7];
                    var selectedExeciseTypeValue = execisedetails[8];
                    var selectedCEARecordId = parseInt(execisedetails[9]);
                    var selectedFirstExecise = execisedetails[10];
                    if (execiseName === "NA" && alternateEName === "NA") {
                        continue;
                    }
                    if (execiseName === "NA") {
                        execiseName = "";
                    }
                    if (LinksDescription === "NA") {
                        LinksDescription = "";
                    }
                    var dispalyAlternateEName = "none";
                    if (isAlternateEName === "NA") {
                        isAlternateEName = "false";
                    }
                    var checkedFirstExeise = "";
                    if (selectedFirstExecise == "true") {
                        checkedFirstExeise = "checked";
                    }
                    var ischeckboxdisabled = ""
                    var isExecisedisabled = "";
                    var isExecisedisabled = "";
                    var isAlternateText = "";
                    if (isAlternateEName == "true") {
                        dispalyAlternateEName = "block";
                        isAlternateText = "checked";
                        isExecisedisabled = "disabled";
                        execiseName = alternateEName;
                    } else {
                        ischeckboxdisabled = "disabled";
                    }
                    if (alternateEName === "NA") {
                        alternateEName = "";
                    }
                    if (alternateExeciseId === "NA") {
                        alternateExeciseId = "";
                    }
                    var UpdateChallengeExeciseSetcount = 0;
                    if (description === "NA") {
                        description = "";
                    }
                    else {
                        var execiseSetResultdata = description.split('<>');
                        UpdateChallengeExeciseSetcount = 0;
                        var execisesetitemdesc = '';
                        for (var exesetcounter = 0; exesetcounter < execiseSetResultdata.length - 1; exesetcounter++) {
                            var execiseset = execiseSetResultdata[exesetcounter].split('^');
                            if (execiseset !== undefined && execiseset !== null && execiseset !== "" && execiseset.length > 4) {
                                var exexiseresp = execiseset[0];
                                var setresultvalue = execiseset[1].split('.');
                                var SetTimeHHValue = "00";
                                var SetTimeMMValue = "00";
                                var SetTimeSSValue = "00";
                                var SetTimeHSValue = "00";
                                if (setresultvalue !== undefined && setresultvalue !== null && setresultvalue !== "" && setresultvalue.length > 1) {
                                    var setresultHHMMSS = setresultvalue[0].split(':');
                                    if (setresultHHMMSS !== undefined && setresultHHMMSS !== null && setresultHHMMSS.length > 3) {
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
                                    if (setRestTimeHHMMSS !== undefined && setRestTimeHHMMSS !== null && setRestTimeHHMMSS.length > 2) {
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
                                ++UpdateChallengeExeciseSetcount;

                                var isHideDeleteOption = false;
                                isHideDeleteOption = (exesetcounter == 0) ? true : false;
                                var setmodel = {
                                    ExeciseCountID: parseInt(UpdateChallengeExeciseCount),
                                    ExeciseSetCountID: parseInt(UpdateChallengeExeciseSetcount),
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
                                // }
                            }
                        }
                    }
                    if (UpdateChallengeExeciseCount < max_fields) { //max input box allowed
                        UpdateChallengeExeciseCount++; //text box increment   
                        if (isAlternateEName == "true") {
                            execiseName = "";
                        }
                        var execisemodel = {
                            ExeciseCountID: UpdateChallengeExeciseCount,
                            IsExecisedisabled: isExecisedisabled,
                            LinksDescription: LinksDescription,
                            ExerciseThumnail: LinksDescription,
                            FFExeName: execiseName,
                            Exsetcount: UpdateChallengeExeciseSetcount,
                            IsAlternateText: isAlternateText,
                            Ischeckboxdisabled: ischeckboxdisabled,
                            IsAlternateEName: isAlternateEName,
                            FFAExeName: alternateEName,
                            DispalyAlternateEName: dispalyAlternateEName,
                            ExeciseItemElement: execisesetitemdesc,
                            IsNewAddedExercise: 0,
                            CEARocordId: selectedCEARecordId,
                            IsExeciseCountIDText: checkedFirstExeise,
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

                            BindSelectedEquipment(UpdateChallengeExeciseCount, selectedEquipementValue);
                            SelectedBindTrainingZone(UpdateChallengeExeciseCount, selectedTrainingzoneValue);
                            SelectedBindExerciseType(UpdateChallengeExeciseCount, selectedExeciseTypeValue);
                        }
                    }
                }
            }
        }
        $('.add_field_button').prop('disabled', false);
    }
    //  $(add_button).click(function (e) { //on add input button click
    $(document).delegate("button[id^='InsertExecise']", "click", function (e) {
        $("#myLoadingElement").show();
        var currentExeciseId = $(this).attr('id').replace(/[^0-9]/g, '');
        e.preventDefault();
        if (UpdateChallengeExeciseCount < max_fields) { //max input box allowed
            var lastExeciseCount = UpdateChallengeExeciseCount;
            UpdateChallengeExeciseCount++; //text box increment  
            var insertedExeciseCount = parseInt(currentExeciseId) + 1;
            currentExeciseId = parseInt(currentExeciseId);
            var addexeciseurl = baseUrlString + "/Reporting/AddFFExecise";
            jQuery.ajax({
                type: "GET",
                url: addexeciseurl,
                data: { id: insertedExeciseCount },
                async: false,
                success: function (data) {
                    // $(wrapper).append(data);
                    var endIndex = 0;
                    if (UpdateChallengeExeciseCount > 0) {
                        endIndex = UpdateChallengeExeciseCount - 1;
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
                        if (UpdateChallengeExeciseCount > 2) {
                            $(wrapper).prepend(data);
                        }
                        else {
                            $(wrapper).append(data);
                        }
                    }
                    $('#InsertExecise' + insertedExeciseCount).prop('disabled', true);
                    // Save new Added execise with all sets
                    SaveLastExeciseWithSets(currentExeciseId);

                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                    $("#myLoadingElement").hide();
                }
            });
            //  $('.add_field_button').prop('disabled', true);

            BindEquipment(insertedExeciseCount);
            BindTrainingZone(insertedExeciseCount);
            BindExerciseType(insertedExeciseCount);
        }
        $("#myLoadingElement").hide();
        return false;
    });

    $(document).delegate("button[id^='btnAddExeciseSet']", "click", function (e) {
        $("#myLoadingElement").show();
        var UpdateChallengeExeciseSetcount = 0;
        e.preventDefault();
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        // Get the Execisise current set count
        var exesetControlID = '#ExerciseSetCount' + id;
        var execiseSetCurrentCount = parseInt($(exesetControlID).val());
        if (execiseSetCurrentCount >= 1) {
            UpdateChallengeExeciseSetcount = execiseSetCurrentCount;
        }
        else {
            UpdateChallengeExeciseSetcount = 1;
        }
        var exericeSetPlaceholdId = '#execeMainsetId' + id;
        if (UpdateChallengeExeciseSetcount < max_fields) { //max input box allowed
            //text box increment  
            var path = baseUrlString + "/images/RemoveSet.png";
            var exericeControlID = '#FFExeName' + id;
            var execiseName = $(exericeControlID).val();
            var isAlternativeExeciseName = $("#IsFFAExeName" + id).val();
            if (isAlternativeExeciseName !== undefined && $("#IsFFAExeName" + id).is(':checked')) {
                execiseName = $('#FFAExeName' + id).val();
            }
            if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
                var lastExeciseSet = UpdateChallengeExeciseSetcount;
                if (execiseSetCurrentCount >= 1) {
                    ++UpdateChallengeExeciseSetcount;
                }
                var url = baseUrlString + "/Reporting/AddFFExeciseSet";
                $.post(url, { ExeciseCountID: parseInt(id), ExeciseSetCountID: UpdateChallengeExeciseSetcount, ExeciseName: execiseName, IsNewAddedSet: "0" })
                 .done(function (response) {
                     $(exericeSetPlaceholdId).append(response);
                 });
                $(exesetControlID).val(UpdateChallengeExeciseSetcount);
                $('#InsertExecise' + id).prop('disabled', false);
                //Auto save the privoius set details
                SaveExeciseLastAutoSavedSet(parseInt(id), lastExeciseSet);

            } else {
                alert('Please select an execise name.');
            }

        }
        $("#myLoadingElement").hide();
        return false;
    });
    $(document).delegate("input:text[id^='ExecSetRep']", "keyup", function () {
        if (isNaN(this.value)) {
            this.value = "";
            alert('Please enter numeric value.');
        }
    });
    $(document).delegate("input:text[id^='ExecSetRep']", "focusout", function () {
        var execeMainsetId = $(this).parent('div').parent('div').attr('id');
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        var setrepId = $('#ExecSetRep' + id);
        // var setrepvalue = execiseSetSectionId.find(setrepId).val();
        var setrepId = $('#' + execeMainsetId).find(setrepId).val();
        if (setrepId !== undefined && setrepId !== null && setrepId !== "") {
            DisableSetResultControl(execeMainsetId, id);
        } else {
            EnabledSetResultControl(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeHours']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
        else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeMinute']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
        else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }

    });
    $(document).delegate("input:text[id^='SetTimeSecond']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
        else {
            var execeMainsetId = $(this).parent('div').parent('div').attr('id');
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            ManageExeciseSerRep(execeMainsetId, id);
        }
    });
    $(document).delegate("input:text[id^='SetTimeHS']", "focusout", function () {
        if (isNaN(this.value)) {
            this.value = "00";
            alert('Please enter numeric value.');
        }
        else {
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
    $(document).delegate("button[id^='btnsetRemove']", "click", function () {
        var currentExeciseCount = $(this).parent('span').parent('div').parent('div').attr('id');
        var UpdateChallengeExeciseSetcount = 0;
        $(this).parent('span').parent('div').parent('div').remove();
        var exesetControlID = '#ExerciseSetCount' + currentExeciseCount;
        var exesetControlCount = $(exesetControlID).val();
        var execiseSetCurrentCount = parseInt(exesetControlCount);
        if (execiseSetCurrentCount > 0) {
            UpdateChallengeExeciseSetcount = execiseSetCurrentCount;
        }
        else {
            UpdateChallengeExeciseSetcount = 0;
        }
        var removelexeNumber = $(this).attr('id').replace(/[^0-9]/g, '');
        var totalremaingExe = UpdateChallengeExeciseSetcount + 1;
        var exericeControlID = '#FFExeName' + currentExeciseCount;
        var removeExeciseSetSectionId = $('#execeMainsetId' + currentExeciseCount);
        var execiseName = $(exericeControlID).val();
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber !== "") {
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


                    removeExeciseSetSectionId.find('#RestTimeHours' + icounter).attr("name", "RestTimeHours" + removelexeNumber);
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
        UpdateChallengeExeciseSetcount--;
        // Get the Execisise current set count           
        $(exesetControlID).val(UpdateChallengeExeciseSetcount);
        return false;
    });
    // $("#chkFreeFormSearchEquipment").on("change", function () {
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
    jQuery(document).delegate("input:text[id^='FFExeName']", "focusin", function () {
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
                jQuery(exeVideoLinkId).html('<a href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');
                jQuery(FFlblExeId).html("");
                var removeExeciseSetSectionId = $('#execeMainsetId' + controlNumber);

                removeExeciseSetSectionId.find('[id^=exesetTittle]').each(function (key, value) {
                    var controlID = key + 1;
                    removeExeciseSetSectionId.find("#exesetTittle" + controlID).text(ui.item.value + " , Set " + controlID);
                    removeExeciseSetSectionId.find('#exesetTittle1').text(ui.item.value + " , Set 1");

                });
                // jQuery('.add_field_button').prop('disabled', false);
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
                    // #lblExe4
                    var lblExeId = "#FFlblExe" + controlNumber;
                    //jQuery(lblExeId).html("No exercise found for the selected values.");
                    //"#ExeVideoLink4
                    var exeVideoLinkId = "#FFExeVideoLink" + controlNumber;
                    jQuery(exeVideoLinkId).html("");
                    //  GetFractionList(2);
                    this.focus();
                    $("#ExerciseId" + controlNumber).val(0);
                } else {                    
                    var lblExeId = "FFlblExe" + controlNumber;
                    jQuery("#lblExeId").html("");
                    if (dataUnit == "Interval" || dataUnit == "Rounds") {
                        //  GetFractionList(3);
                    }
                    var selectedExerciseIdId = parseInt(ui.item.ExerciseId);
                    $("#ExerciseId" + controlNumber).val(selectedExerciseIdId);
                }
                //jQuery('.add_field_button').prop('disabled', false);
                $('#InsertExecise' + controlNumber).prop('disabled', false);
            },
            open: function () {
                $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
            },
            close: function () {
                var controlID = ($(this).attr("id"));                
                var index = controlID.indexOf("FFExeName");
                controlNumber = controlID.substr(index + 9);
                $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
                var selectedExeciseName = "#FFExeName" + controlNumber;
                var isAlternativeExeciseName = "#IsFFAExeName" + controlNumber;
                var exeVideoLinkId = "#FFExeVideoLink" + controlNumber;
                var selecteExeciseName = jQuery(selectedExeciseName).val();
                if (selecteExeciseName === undefined || selecteExeciseName === null || selecteExeciseName === "") {
                    jQuery(isAlternativeExeciseName).prop("disabled", false);
                    // $('.add_field_button').prop('disabled', false);
                    $('#InsertExecise' + controlNumber).prop('disabled', true);
                    jQuery(exeVideoLinkId).html("");
                } else {
                    jQuery(isAlternativeExeciseName).prop("disabled", true);
                    $('#InsertExecise' + controlNumber).prop('disabled', false);
                }
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
    $(wrapper).on("click", ".remove_field", function (e) { //user click on remove text
        $('#loadingChallengeCategory').show();
        e.preventDefault(); $(this).parent('div').parent('div').remove(); UpdateChallengeExeciseCount--;
        $('.add_field_button').prop('disabled', false);
        var controlID = ($(this).attr("id"));
        var index = controlID.indexOf("FFRemoveExeName");
        var removelexeNumber = controlID.substr(index + 15);
        var totalremaingExe = UpdateChallengeExeciseCount + 1;
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber !== "") {
            var removelexeID = parseInt(removelexeNumber);
            var fromremaneexecise = removelexeID + 1;
            for (var icounter = fromremaneexecise; icounter <= totalremaingExe; icounter++) {
                var ffExeciseLabelID = "#FFlblExeName" + icounter;
                var ffremoveExeciseLabelID = "#FFRemoveExeName" + icounter;
                $(ffExeciseLabelID).text("Name of Exercise " + removelexeID + ":");
                $(ffExeciseLabelID).attr("id", "FFlblExeName" + removelexeID);
                $(ffremoveExeciseLabelID).attr("id", "FFRemoveExeName" + removelexeID);
                $('#FFdrpdownEquipments' + icounter).attr("name", "FFdrpdownEquipments" + removelexeID);
                $('#FFdrpdownEquipments' + icounter).attr("id", "FFdrpdownEquipments" + removelexeID);

                $('#FFdrpdownTrainingZones' + icounter).attr("name", "FFdrpdownTrainingZones" + removelexeID);
                $('#FFdrpdownTrainingZones' + icounter).attr("id", "FFdrpdownTrainingZones" + removelexeID);

                $('#FFdrpdownExerciseTypes' + icounter).attr("name", "FFdrpdownExerciseTypes" + removelexeID);
                $('#FFdrpdownExerciseTypes' + icounter).attr("id", "FFdrpdownExerciseTypes" + removelexeID);

                $('#FFExeVideoLink' + icounter).attr("id", "FFExeVideoLink" + removelexeID);

                $('#FFExeName' + icounter).attr("name", "FFExeName" + removelexeID);
                $('#FFExeName' + icounter).attr("id", "FFExeName" + removelexeID);

                $('#IsFFAExeName' + icounter).attr("name", "IsFFAExeName" + removelexeID);
                $('#IsFFAExeName' + icounter).attr("id", "IsFFAExeName" + removelexeID);

                $('#FFAExeName' + icounter).attr("name", "FFAExeName" + removelexeID);
                $('#FFAExeName' + icounter).attr("id", "FFAExeName" + removelexeID);

                $('#spnFFAExeName' + icounter).attr("data-valmsg-for", "spnFFAExeName" + removelexeID);
                $('#spnFFAExeName' + icounter).attr("id", "spnFFAExeName" + removelexeID);

                $('#FFExeName' + icounter).attr("data-valmsg-for", "FFExeName" + removelexeID);

                $('#FFExeVideoLink' + icounter).attr("id", "FFExeVideoLink" + removelexeID);
                $('#FFExeName' + icounter).attr("id", "FFExeName" + removelexeID);
                $('#FFlblExe' + icounter).attr("id", "FFlblExe" + removelexeID);
                $('#FFExeDesc' + icounter).attr("id", "FFExeDesc" + removelexeID);
                $('#FFExeDesc' + icounter).attr("name", "FFExeDesc" + removelexeID);
                $('#FFExeVideoLinkUrl' + icounter).attr("id", "FFExeVideoLinkUrl" + removelexeID);
                $('#FFExeName' + icounter).attr("name", "FFExeName" + removelexeID);

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
        $('#loadingChallengeCategory').hide();
    });
    $('#btnbuttomadminupdatechallange,#btnTopAdminUpdateChallange').click(function () {
        $("#myLoadingElement").show();
        var ExeciseDescription = "";
        var Execiset1record = "";
        Execiset1record = GetExeciseSets(1);
        var wellnessSubtypeId = jQuery('#ChallengeSubTypeId').val();
        $uploadCrop.croppie('result', {
            type: 'canvas',
            size: 'original'
        }).then(function (resp) {
            popupResult({
                src: resp
            });
        });

        if (Execiset1record === "" && (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId !== "" && wellnessSubtypeId === "14")) {
            alert("Please enter execise set with reps or time.");
            $("#myLoadingElement").hide();
            return false;
        }
        var execisesettotalCount = $('#ExerciseSetCount1').val();

        var firstExeciseName = $('#FFExeName1').val();
        var firstAExeciseName = $('#FFAExeName1').val();
        // Find out Execise name or Alternate Exercise name is entered or not
        var IsEnterfFirstExecise1 = false;
        if (firstExeciseName !== undefined && firstExeciseName !== null && firstExeciseName !== "") {
            IsEnterfFirstExecise1 = true;
        }
        else if (firstAExeciseName !== undefined && firstAExeciseName !== null && firstAExeciseName !== "") {
            IsEnterfFirstExecise1 = true;
        }
        if (wellnessSubtypeId !== "") {
            if (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId === "14" && IsEnterfFirstExecise1 === false) {
                alert("Please enter at least one workout exercise.");
                $("#myLoadingElement").hide();
                return false;
            }
        }
        $('#FFExeDesc1').val(Execiset1record);
        for (var icount = 2; icount <= UpdateChallengeExeciseCount ; icount++) {
            var iSEneterFirstset = false;
            var exericeControlID = '#FFExeName' + icount;
            var execiseName = $(exericeControlID).val();
            if (execiseName !== undefined && execiseName !== null && execiseName !== "") {
                ExeciseDescription += execiseName + "~";
                iSEneterFirstset = true;
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
            if (wellnessSubtypeId !== "") {
                if (wellnessSubtypeId !== undefined && wellnessSubtypeId !== null && wellnessSubtypeId === "14" && (execisesetremaingtotalCount === "0" || execisesetremaingtotalCount == 0)) {
                    alert("Please enter at least one set for each workouts exercise.");
                    $("#myLoadingElement").hide();
                    return false;
                }
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
                iSEneterFirstset = true;
            }
            else {
                ExeciseDescription += "NA" + "~";
            }
            var exeFFAExerciseIdCtrl = '#ExerciseId' + icount;
            var exeFFAExerciseId = $(exeFFAExerciseIdCtrl).val();
            if (exeFFAExerciseId !== undefined && exeFFAExerciseId !== null && exeFFAExerciseId !== "") {
                ExeciseDescription += exeFFAExerciseId + "~";
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
            var cEARocordIdvalue = jQuery('#CEARocordId' + icount).val();
            if (cEARocordIdvalue !== undefined && cEARocordIdvalue !== null && cEARocordIdvalue !== "") {
                ExeciseDescription += cEARocordIdvalue + "~";
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
                ExeciseDescription += exeFFExerciseId;
            }
            else {
                ExeciseDescription += "NA";
            }

            ExeciseDescription += "|";
        }
        $('#FreeFormExerciseNameDescriptionList').val(ExeciseDescription);

        if (reader == null || reader == 'undefined') {
            $('#myform').submit();
            $uploadCrop = null;
            reader = null;
        }
        $("#myLoadingElement").hide();
    });
    var isCheckedFeatured = jQuery('#IsFeatured');
    if (isCheckedFeatured !== undefined && isCheckedFeatured !== null && isCheckedFeatured.is(':checked')) {
        $('#FeaturedUploadSection').show();
    } else {
        $('#FeaturedUploadSection').hide();
    }
    jQuery(document).delegate("input:checkbox[id='IsFeatured']", "change", function () {
        if (this.checked) {
            $('#FeaturedUploadSection').show();
        }
        else {
            $('#FeaturedUploadSection').hide();
        }
    });
    jQuery(document).delegate("input:checkbox[id^='IsFFAExeName']", "change", function () {
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        var exericeNameID = '#FFExeName' + id; //FFExeName3
        var dvFFAExeName = '#FFAExeName' + id;
        var exeLinkedUrl = '#FFExeVideoLinkUrl' + id;
        var exeExeciseId = '#ExerciseId' + id;
        var selectedIndex = jQuery('#SelectedAllIndex').val();
        var challengeId = jQuery("#ChallengeId").val();

        if (this.checked) {
            jQuery(dvFFAExeName).show();
            jQuery(exeLinkedUrl).hide();
            jQuery(exericeNameID).prop("disabled", true);
            jQuery('.add_field_button').prop('disabled', false);
        }
        else {

            jQuery(exericeNameID).prop("disabled", false);
            jQuery(dvFFAExeName).hide();
            jQuery(exeLinkedUrl).hide();
        }
    });
    var isAlternativeExeciseName = jQuery('#IsFFAExeName1');
    var dvFFAExeName1 = jQuery('#FFExeName1');
    if (isAlternativeExeciseName !== undefined && isAlternativeExeciseName !== null && isAlternativeExeciseName.is(':checked')) {
        jQuery('#FFAExeName1').show();
        dvFFAExeName1.prop("disabled", true);
        isAlternativeExeciseName.prop("disabled", false);
        $('.add_field_button').prop('disabled', false);
    } else {
        jQuery('#FFAExeName1').hide();
        dvFFAExeName1.prop("disabled", false);
        isAlternativeExeciseName.prop("disabled", true);
    }
    jQuery(function () {
        $('#challengeNoTrainerTeam').hide();
        var challengeSubtypeId = jQuery('#ChallengeSubTypeId').val();
        if (challengeSubtypeId != "") {
            if (challengeSubtypeId === "15") {
                ShowFFUpdateWellnessCChallengeSection();
                $('#divTrendingCategorySection').hide();
            }
            else if (challengeSubtypeId === "14") {
                HideFFUpdateWellnessChallengeSection();
                $('#divTrendingCategorySection').show();
                var selectedTrainerId = $("#TrainerCredntialId option:selected").val();
                if (selectedTrainerId > 0) {
                    $('#challengeNoTrainerTeam').hide();
                }
                else {
                    $('#challengeNoTrainerTeam').show();
                }
            }
            else {
                $('#divTrendingCategorySection').show();
            }
        }
        jQuery('#ChallengeSubTypeId').on("change", function () {
            $('#loadingChallengeCategory').show();
            $('#challengeNoTrainerTeam').hide();
            challengeSubtypeId = "";
            challengeSubtypeId = jQuery('#ChallengeSubTypeId').val();
            if (challengeSubtypeId != "") {
                if (challengeSubtypeId === "15") {
                    ShowFFUpdateWellnessCChallengeSection();
                    $('#divTrendingCategorySection').hide();
                }
                else if (challengeSubtypeId === "14") {
                    HideFFUpdateWellnessChallengeSection();
                    var selectedTrainerId = $("#TrainerCredntialId option:selected").val();
                    if (selectedTrainerId > 0) {
                        $('#challengeNoTrainerTeam').hide();
                    }
                    else {
                        $('#challengeNoTrainerTeam').show();
                    }
                    $('#divTrendingCategorySection').show();
                } else {
                    $('#divTrendingCategorySection').show();
                }
            }
            // Bind the Challenge Category Id
            if (challengeSubtypeId === "14") {
                var urlchallenge = baseUrlString + "/Reporting/GetChallengeCategory";
                jQuery.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: urlchallenge + "/" + challengeSubtypeId,
                    data: JSON.stringify(challengeSubtypeId),
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
    $("#myLoadingElement").hide();
    jQuery(document).delegate("#TrainerCredntialId", "change", function () {
        var challengeSubtypeId = jQuery('#ChallengeSubTypeId').val();
        if (challengeSubtypeId != "") {
            if (challengeSubtypeId === "14") {
                var selectedTrainerId = $("#TrainerCredntialId option:selected").val();
                if (selectedTrainerId > 0) {
                    $('#challengeNoTrainerTeam').hide();
                }
                else {
                    $('#challengeNoTrainerTeam').show();
                }
            }
        }
    });
    jQuery(document).delegate("input:checkbox[id^='IsSetFirstExecise']", "change", function () {
        var th = $(this), name = th.prop('name');
        if (th.is(':checked')) {
            $(':checkbox[name="' + name + '"]').not($(this)).prop('checked', false);
        }
    });
    jQuery(function () {
     if ($("#SelectAllTeam").is(':checked')) {
            $('#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]').each(function () {
                this.checked = true;
            });
        }
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
       
        if ($("#SelectAllTrendingCategory").is(':checked')) {
            $('#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = true;
            });
        }
        jQuery('#SelectAllTrendingCategory').change(function () {
            if (this.checked) {
                $('#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]').each(function () {
                    this.checked = false;
                });
            }
        });
        jQuery("#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]").click(function () {
            try {
                if (this.checked) {
                    IsAllchkAllNoTrainetWorkoutTeam();
                }
                else {
                    $("#SelectAllTeam").prop("checked", false);
                }
            }
            catch (err) {

            }

        });
        jQuery("#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]").click(function () {
            try {
                if (this.checked) {
                    chkAllTrendingCategory();
                }
                else {
                    $("#SelectAllTrendingCategory").prop("checked", false);
                }
            }
            catch (err) {

            }

        });
    });
    $("#myLoadingElement").hide();
});

jQuery(function () {
    var selectedTrainerId = $("#TrainerId option:selected").val();
    if (selectedTrainerId > 0) {
        $('#challengeNoTrainerTeam').hide();
    }
    else {
        $('#challengeNoTrainerTeam').show();
    }
});



