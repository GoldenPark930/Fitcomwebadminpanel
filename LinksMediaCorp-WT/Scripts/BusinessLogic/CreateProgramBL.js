﻿var $uploadCrop;
var reader;

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
    function TrainerPhotoUpload() { 
        function readFile(input) {
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadCrop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.upload-demo').addClass('ready');
                    $("#existingTrainertImg").hide();
                    $('#existingTrainertImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                //   swal("Sorry - you're browser doesn't support the FileReader API");
            }
        }      

        $uploadCrop = $('#upload-demo').croppie({
            viewport: {
                width: 300,
                height: 230,
                type: 'rectangle'
            },
            boundary: {
                width: 300,
                height: 230
            },
            enforceBoundary: false
        });


        $('#upload').on('change', function () { readFile(this); });
       
        //  reader = null;
    }
    


$(document).ready(function () {
    TrainerPhotoUpload();
    var wrapper = $("#programWeekWorkouttoggalediv"); //Fields wrapper   
    $('#Duration').val("1 Week(s)");
    $('#Workouts').val(" 1 Workout(s)");
    $(document).delegate("button[id^='btnWorkoutRemove']", "click", function (e) {
        e.preventDefault();
        var currentweekCount = $(this).parent('span').parent('div').attr('id');
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        if (removedWeekworkouts !== undefined && removedWeekworkouts !== null) {
            var deletedweekworkout = "";
            var programweekId = $('#IsProgramNewWeek' + currentweekCount).val();
            if (programweekId !== undefined && programweekId !== null && programweekId !== "") {
                deletedweekworkout += programweekId + "~";
            }
            else {
                deletedweekworkout += "0" + "~";
            }

            var removeWeekWorkoutSectionId = $('#weekMainWorkoutId' + currentweekCount);
            if (id > 0) {
                var removeWeekworkoutvalue = removeWeekWorkoutSectionId.find('#IsProgramNewWeekWorkout' + id).val();
            }
            if (removeWeekworkoutvalue !== undefined && removeWeekworkoutvalue !== null && removeWeekworkoutvalue !== "") {
                deletedweekworkout += removeWeekworkoutvalue;
            }
            else {
                deletedweekworkout += "0";
            }
            deletedweekworkout = deletedweekworkout + "|"
            removedWeekworkouts += deletedweekworkout;
        }
        $(this).parent('span').parent('div').remove();

        var programWorkoutsCountControlID = '#ProgramWorkoutsCount' + currentweekCount;
        var programWeekWorkoutsCount = parseInt($(programWorkoutsCountControlID).val());
        var exsetcount = 0;
        if (programWeekWorkoutsCount > 0) {
            exsetcount = programWeekWorkoutsCount;
        }

        var totalremaingExe = exsetcount + 1;
        var removelexeNumber = $(this).attr('id').replace(/[^0-9]/g, '');
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber != "") {
            // var removelexeID = parseInt(removelexeNumber);
            if (parseInt(removelexeNumber) >= 1) {
                var execisesetnumber = parseInt(removelexeNumber) + 1;
                for (var icounter = execisesetnumber; icounter <= totalremaingExe; icounter++) {
                    removeWeekWorkoutSectionId.find('#weekworkoutTittle' + icounter).text(" Workout " + removelexeNumber);
                    removeWeekWorkoutSectionId.find('#weekworkoutTittle' + icounter).attr("id", "weekworkoutTittle" + removelexeNumber);

                    removeWeekWorkoutSectionId.find('#ProgramWeekWorkout' + icounter).attr("name", "ProgramWeekWorkout" + removelexeNumber);
                    removeWeekWorkoutSectionId.find('#btnWorkoutRemove' + icounter).attr("id", "btnWorkoutRemove" + removelexeNumber);

                    removeWeekWorkoutSectionId.find('#ProgramWeekWorkout' + icounter).attr("id", "ProgramWeekWorkout" + removelexeNumber);
                    removeWeekWorkoutSectionId.find('#ProgramWorkoutLink' + icounter).attr("id", "ProgramWorkoutLink" + removelexeNumber);
                    removeWeekWorkoutSectionId.find('#ProgramWeekHidenWorkout' + icounter).attr("id", "ProgramWeekHidenWorkout" + removelexeNumber);
                    removeWeekWorkoutSectionId.find('#IsProgramNewWeekWorkout' + icounter).attr("id", "IsProgramNewWeekWorkout" + removelexeNumber);
                    removelexeNumber = parseInt(removelexeNumber) + 1;
                }
            }
            else {
                //  $('#execeMainsetId' + currentExeciseCount).css('min-height', '50px');

            }
        }
        if (programWeekWorkoutsCount > 0) {
            programWeekWorkoutsCount = parseInt(programWeekWorkoutsCount) - 1;
            programweekTotalworkoutcount = parseInt(programweekTotalworkoutcount) - 1;
            $('#Workouts').val(programweekTotalworkoutcount + " Workout(s)");
            $(programWorkoutsCountControlID).val(programWeekWorkoutsCount);
        }

        return false;
    });
    $(wrapper).on("click", ".remove_field", function (e) { //u
        e.preventDefault();
        var removelexeNumber = $(this).attr('id').replace(/[^0-9]/g, '');
        programWeekcount = parseInt(programWeekcount) - 1;
        //  var removelexeNumber = $(this).parent('div').parent('div').attr('id');
        var programWorkoutsCountControlID = '#ProgramWorkoutsCount' + removelexeNumber;
        var programWeekWorkoutsCount = parseInt($(programWorkoutsCountControlID).val());
        if (programWeekWorkoutsCount > 0) {
            programweekTotalworkoutcount = parseInt(programweekTotalworkoutcount) - programWeekWorkoutsCount;
            $('#Duration').val(parseInt(programWeekcount) + " Week(s)");
            $('#Workouts').val(parseInt(programweekTotalworkoutcount) + " Workout(s)");
        }

        if (removedWeekworkouts !== undefined && removedWeekworkouts !== null) {
            var deletedallweek = "";
            var programweekId = $('#IsProgramNewWeek' + removelexeNumber).val();
            if (programweekId !== undefined && programweekId !== null && programweekId !== "") {
                deletedallweek += programweekId + "~" + "ALL";
            }
            else {
                deletedallweek += "0" + "~" + "ALL"
            }
            deletedallweek = deletedallweek + "|"
            removedWeekworkouts += deletedallweek;
        }
        $(this).parent('div').parent('div').remove();
        var totalremaingExe = programWeekcount + 1;
        if (removelexeNumber !== undefined && removelexeNumber !== null && removelexeNumber != "") {
            var removelexeID = parseInt(removelexeNumber);
            var fromremaneexecise = removelexeID + 1;
            for (var icounter = fromremaneexecise; icounter <= totalremaingExe; icounter++) {


                $('#WorkoutTrainerId' + icounter).attr("name", "WorkoutTrainerId" + removelexeID);
                $('#WorkoutTrainerId' + icounter).attr("id", "WorkoutTrainerId" + removelexeID);

                $('#WorkoutTraingZoneId' + icounter).attr("name", "WorkoutTraingZoneId" + removelexeID);
                $('#WorkoutTraingZoneId' + icounter).attr("id", "WorkoutTraingZoneId" + removelexeID);

                $('#WorkoutDifficultyLevelId' + icounter).attr("name", "WorkoutDifficultyLevelId" + removelexeID);
                $('#WorkoutDifficultyLevelId' + icounter).attr("id", "WorkoutDifficultyLevelId" + removelexeID);

                $('#FFExeName' + icounter).attr("id", "FFExeName" + removelexeID);
                $('#lblweek' + icounter).attr("id", "lblweek" + removelexeID);
                $('#lblweek' + removelexeID).text(" Week " + removelexeID);


                $('#weekMainWorkoutId' + icounter).attr("id", "weekMainWorkoutId" + removelexeID);
                $('#ProgramWorkoutsCount' + icounter).attr("id", "ProgramWorkoutsCount" + removelexeID);
                $('#btnAddProgramWprkout' + icounter).attr("id", "btnAddProgramWprkout" + removelexeID);
                $('#IsProgramNewWeek' + icounter).attr("id", "IsProgramNewWeek" + removelexeID);
                $('#' + icounter).attr("id", removelexeID);
                removelexeID = removelexeID + 1;
            }
        }
        return false;
    });
    $('#btnTopAdminProgramSubmit,#btnButtomAdminProgramSubmit').click(function () {
        var isValid = true;
        var programSubtypeId = jQuery('#ProgramType').val();
        if (programSubtypeId === undefined || programSubtypeId === null || programSubtypeId === "") {
            jQuery('#spnProgramType').text("The Program Type field is required");
            isValid = false;
        } else {
            jQuery('#spnProgramType').text("");
        }
        var programName = jQuery('#ProgramName').val();
        if (programName === undefined || programName === null || programName === "") {
            jQuery('#spnProgramNameerror-msg').text("The Program Name field is required.");
            isValid = false;
        } else {
            jQuery('#spnProgramNameerror-msg').text("");
        }
        var description = $("#cke_Description iframe").contents().find("body").text();
        if (description === undefined || description === null || description == "") {
            jQuery('#spnDescribechallenge').text("The Program Description field is required");
            isValid = false;
        } else {
            jQuery('#spnDescribechallenge').text("");
        }
        if (!isValid) {
            return false;
        }
        $uploadCrop.croppie('result', {
            type: 'canvas',
            size: 'original'
        }).then(function (resp) {
            popupResult({
                src: resp
            });
        });
        var ProgramWeekWorkout1 = "";
        ProgramWeekWorkout1 = GetWeekWorkout(1);
        //  var programSubtypeId = jQuery('#ProgramType').val();
        var weektotalworkoutstotalCount = $('#ProgramWorkoutsCount1').val();
        var weekfirstworkoutIdvalue = $('#weekMainWorkoutId1').find('#ProgramWeekHidenWorkout1').val();
        if (programSubtypeId != "") {
            if (programSubtypeId !== undefined && programSubtypeId !== null && programSubtypeId === "16" && weekfirstworkoutIdvalue === "0") {
                alert("Please enter at least one set for each workouts in Week.");
                return false;
            }
        }
        $('#ProgramWorkouts').val(ProgramWeekWorkout1);
        var AllweekworkoutId = "";
        for (var icount = 2; icount <= programWeekcount ; icount++) {
            var weekWorkoutsDetails = "";
            weekWorkoutsDetails = GetWeekWorkout(icount);
            var weektotalworkoutstotalCount = $('#ProgramWorkoutsCount' + icount).val();
            var weekfirstworkoutIdvalue = $('#weekMainWorkoutId' + icount).find('#ProgramWeekHidenWorkout1').val();
            if (programSubtypeId != "") {
                if (programSubtypeId !== undefined && programSubtypeId !== null && programSubtypeId === "16" && weekfirstworkoutIdvalue === "0") {
                    alert("Please enter at least one set for each workouts in Week.");
                    return false;
                }
            }
            if (weekWorkoutsDetails !== undefined && weekWorkoutsDetails !== null && weekWorkoutsDetails !== "") {
                AllweekworkoutId += weekWorkoutsDetails + "~";
            }
            else {
                AllweekworkoutId += "NA" + "~";
            }
            // Get selected filter option
            var drpdownWorkoutTrainerIdValues = jQuery('#WorkoutTrainerId' + icount).find('option:selected').val();
            if (drpdownWorkoutTrainerIdValues !== undefined && drpdownWorkoutTrainerIdValues !== null && drpdownWorkoutTrainerIdValues !== "" && drpdownWorkoutTrainerIdValues !== "0") {
                AllweekworkoutId += drpdownWorkoutTrainerIdValues + "~";
            }
            else {
                AllweekworkoutId += "NA" + "~";
            }
            var drpdownTrainingZonesValues = jQuery('#WorkoutTraingZoneId' + icount).find('option:selected').val();
            if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "0") {
                AllweekworkoutId += drpdownTrainingZonesValues + "~";
            }
            else {
                AllweekworkoutId += "NA" + "~";
            }
            var drpdownDifficultyLevelIdValue = jQuery('#WorkoutDifficultyLevelId' + icount).find('option:selected').val();
            if (drpdownDifficultyLevelIdValue !== undefined && drpdownDifficultyLevelIdValue !== null && drpdownDifficultyLevelIdValue !== "" && drpdownDifficultyLevelIdValue !== "0") {
                AllweekworkoutId += drpdownDifficultyLevelIdValue;
            }
            else {
                AllweekworkoutId += "NA";
            }
            AllweekworkoutId += "|";
        }
        $('#ProgramWeekWorkoutList').val(AllweekworkoutId);       
        if (reader == null || reader == 'undefined') {
            $('#myform').submit();
            $uploadCrop = null;
            reader = null;
        }
        $uploadCrop = null;
    });
    jQuery(document).delegate("#TrainerId", "change", function () {
        var selectedTrainerId = $("#TrainerId option:selected").val();
        if (selectedTrainerId > 0) {
            $('#challengeNoTrainerTeam').hide();
            $('#TrendingCategoryHeader').text("Step 10 - Select Your Trending Category:");
        }
        else {
            $('#challengeNoTrainerTeam').show();
            $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");
        }
    });


});





