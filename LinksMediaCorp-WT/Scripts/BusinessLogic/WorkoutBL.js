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

        $('#divFittnessTrendingCategorySection').show();
        $('#divTrendingCategorySection').hide();
        $('#TrendingCategoryHeader').text("Step 11 - Select Your Trending Category:");

    }

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
  
   
    

});

//validation and text change on challeneg sub-type change
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
            if (id === "13") {
                ShowFFChallengeSection();
                $('#TrendingCategoryHeader').text("Step 12 - Select Your Trending Category:");
            }
            else {
                HideFFChallengeSection();
            }



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
function HideFFChallengeSection() {
    $('#divchkPremium').hide();
    $('#ExeciseRepsSec1').show();
   
    $('#FreeFormChallengeExeciseSection').hide();
    $('#FreeFormChallegeDescriptionHeaderSection').hide();
  
    $('#CreateChallenegeEquipmentRequirement').show();
    $('#FreeFormChallengeCateorySection').hide();
    $('#challegeDifficultyHeader').text("Step 2 - Select Difficulty Level:");
    $('#AdminStepTrainingZone').text("Step 7 - Select Target Training Zone:");
    $('#AdminStepChallengeName').text("Step 9 - Enter Challenge Name:");
    $('#AdminStepTrainerName').text("Step 10 - Choose Trainer:");
    $('#AdminStepExerciseType').text("Step 8 - Choose Exercise Type:");
    $('#ChallenegeEquipmentRequirementHeader').text("Step 6 Select Equipment Requirements:");
    $('#CraeteFreeformChallengeName').hide();
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
    $('#challengeNoTrainerTeam').hide();
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
//function for manage weight for man and woman and reps


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

