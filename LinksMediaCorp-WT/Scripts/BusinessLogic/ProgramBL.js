var TotalFilterWorkoutsRecords = new Array(); 
var TotalWorkoutsRecords = new Array();
var programWeekcount = 1;
var programweekworkoutcount = 1;
var programweekTotalworkoutcount = 1;
var max_programfields = 999999999999; //maximum input boxes allowed
var removedWeekworkouts = "";
function BindTrainers(programweekNumber)
{

    if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var drpdownTrainers = "#WorkoutTrainerId" + programweekNumber;
        var urlgetAlltrainers = baseUrlString + "/Reporting/GetAllTrainers";
        jQuery.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: urlgetAlltrainers,
            async:false,
            dataType: "json",
            beforeSend: function () {
                //alert(id);
            },
            success: function (data) {                   
                $(drpdownTrainers).prop('disabled', false);
                jQuery(drpdownTrainers).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Trainer--</option>';
                jQuery.each(data, function (i, tr) {
                    items += "<option value='" + tr.TrainerId + "'>" + tr.TrainerName + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownTrainers).html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }        
}
function BindTrainingZone(programweekNumber) {

     if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var drpdownTrainingZone = "#WorkoutTraingZoneId" + programweekNumber;
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
function BindDifficultyLevel(programweekNumber) {

    if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var urlchallenge = baseUrlString + "/Reporting/GetAllDifficultyLevel";
        var drpdownDifficultyLevel = "#WorkoutDifficultyLevelId" + programweekNumber; 
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

                $(drpdownDifficultyLevel).prop('disabled', false);
                jQuery(drpdownDifficultyLevel).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Difficulty Level--</option>';
                jQuery.each(data, function (i, difficulyLevel) {
                    items += "<option value='" + difficulyLevel.DifficultyId + "'>" + difficulyLevel.Difficulty + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownDifficultyLevel).html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }

}

function SetBindTrainers(programweekNumber, value) {
    if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var drpdownTrainers = "#WorkoutTrainerId" + programweekNumber;
        var urlgetAlltrainers = baseUrlString + "/Reporting/GetAllTrainers";
        jQuery.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: urlgetAlltrainers,
            async: false,
            dataType: "json",
            beforeSend: function () {
                //alert(id);
            },
            success: function (data) {
                $(drpdownTrainers).prop('disabled', false);
                jQuery(drpdownTrainers).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Trainer--</option>';
                jQuery.each(data, function (i, tr) {
                    items += "<option value='" + tr.TrainerId + "'>" + tr.TrainerName + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownTrainers).html(items);
                jQuery(drpdownTrainers).val(value);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}
function SetBindTrainingZone(programweekNumber, value) {
     if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var drpdownTrainingZone = "#WorkoutTraingZoneId" + programweekNumber;
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
                $(drpdownTrainingZone).prop('disabled', false);
                jQuery(drpdownTrainingZone).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Training Zones--</option>';
                jQuery.each(data, function (i, traingzone) {
                    items += "<option value='" + traingzone.PartId + "'>" + traingzone.PartName + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownTrainingZone).html(items);
                jQuery(drpdownTrainingZone).val(value);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}
function SetBindDifficultyLevel(programweekNumber, value) {
    if (programweekNumber !== undefined && ((programweekNumber !== null && programweekNumber !== "" && programweekNumber !== "0") ||  programweekNumber > 0)) {
        var urlchallenge = baseUrlString + "/Reporting/GetAllDifficultyLevel";
        var drpdownDifficultyLevel = "#WorkoutDifficultyLevelId" + programweekNumber;
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

                $(drpdownDifficultyLevel).prop('disabled', false);
                jQuery(drpdownDifficultyLevel).css('background-color', '#FFFFFF');
                var items = '<option value="0">--Choose Difficulty Level--</option>';
                jQuery.each(data, function (i, difficulyLevel) {
                    items += "<option value='" + difficulyLevel.DifficultyId + "'>" + difficulyLevel.Difficulty + "</option>";
                });
                // Fill ChallengeSubType Dropdown list
                jQuery(drpdownDifficultyLevel).html(items);
                jQuery(drpdownDifficultyLevel).val(value);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }

}
//**********Autopcomplete with url clickable starts for exercise 1*******
$(document).delegate("input:text[id^='ProgramWeekWorkout']", "focusin", function () {
    var controlNumber = undefined;
    //var programworkoutControlId = $(this).attr('id').replace(/[^0-9]/g, '');
    var programworkoutControlId = $(this).parent('div').attr('id');  
    $(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {
            // Get data based on filter data
            var programFilterWorkoutsRecords = new Array();
           // GetSelectedFilterWorkouts(programworkoutControlId, request.term);
            var urlchallenge = baseUrlString + "/Reporting/GetAllFilterworkout";
            var serachSelectedTrainerId = 0;
            var serachSelectedTrainingZoneId = 0;
            var serachSelectedDifficultLevelId = "";
            var drpdownTrainerIdeValues = jQuery('#WorkoutTrainerId' + programworkoutControlId).find('option:selected').val();

            if (drpdownTrainerIdeValues !== undefined && drpdownTrainerIdeValues !== null && drpdownTrainerIdeValues !== "" && drpdownTrainerIdeValues !== "0") {
                serachSelectedTrainerId = parseInt(drpdownTrainerIdeValues);
            }
            var drpdownTrainingZonesValues = jQuery('#WorkoutTraingZoneId' + programworkoutControlId).find('option:selected').val();          
            if (drpdownTrainingZonesValues !== undefined && drpdownTrainingZonesValues !== null && drpdownTrainingZonesValues !== "" && drpdownTrainingZonesValues !== "--Choose Training Zones--") {
                serachSelectedTrainingZoneId = drpdownTrainingZonesValues;
            }
            var drpdownDifficultyLevelId = jQuery('#WorkoutDifficultyLevelId' + programworkoutControlId).find('option:selected').text();          
            if (drpdownDifficultyLevelId !== undefined && drpdownDifficultyLevelId !== null && drpdownDifficultyLevelId !== "" && drpdownDifficultyLevelId !== "--Choose Difficulty Level--") {
                serachSelectedDifficultLevelId = drpdownDifficultyLevelId;
            }           
            var selectedWorkoutInputdata = {
                selectedWorkoutTrainerId: serachSelectedTrainerId,
                selectedWorkoutTraingZone: serachSelectedTrainingZoneId,
                selectedWorkoutDifficultyLevel: serachSelectedDifficultLevelId,
                SearchTerm: request.term
            };           
            jQuery.ajax({
                type: "POST",
                url: urlchallenge,
                async: false,
                data: selectedWorkoutInputdata,
                success: function (data) {     
                   
                    TotalFilterWorkoutsRecords = [];
                    jQuery.each(data, function (i, workouts) {
                        var workoutrecord = {
                            WorkoutName: workouts.WorkoutName,
                            WorkoutUrl: workouts.WorkoutUrl,
                            WorkoutId: workouts.WorkoutId
                        };
                        TotalFilterWorkoutsRecords.push(workoutrecord);

                    });
                }
            });           
            response($.map(TotalFilterWorkoutsRecords, function (item) {                
                return {
                    label: item.WorkoutName,
                    value: item.WorkoutName,
                    url: item.WorkoutUrl,
                    Id: item.WorkoutId
                };
            }));           
            TotalFilterWorkoutsRecords = [];
        },
        minLength: 0,
        select: function (event, ui) {      
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var dvProgramWeekHideenWorkout = '#ProgramWeekHidenWorkout' + id;
            var programWorkoutLink = "#ProgramWorkoutLink" + id;
            if (!ui.item) {              
                $(this).siblings(dvProgramWeekHideenWorkout).val(0);
            } else {
                $(this).siblings(dvProgramWeekHideenWorkout).val(ui.item.Id);
               }

            var currentWeekCount = $(this).parent('div').parent('div').attr('id').replace(/[^0-9]/g, '');
            var weekMainWorkoutId = $('#weekMainWorkoutId' + currentWeekCount);
           // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
            weekMainWorkoutId.find("#ProgramWorkoutLink" + id).html('<a id="ProgramWorkoutLink' + id + '" href="'+ui.item.url+'" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');

        },
        change: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var dvProgramWeekHideenWorkout = '#ProgramWeekHidenWorkout' + id;
            if (!ui.item) {
                this.value = '';
                jQuery("#ProgramWorkoutLink" + id).html("");
                if (!ui.item) {
                    $(this).siblings(dvProgramWeekHideenWorkout).val(0);
                }
                this.focus();

            } else {              
                if (ui.item) {
                    $(this).siblings(dvProgramWeekHideenWorkout).val(ui.item.Id);
                }
            }
            $('.add_field_button').prop('disabled', false);

        },
        open: function () {
            $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
        },
        close: function () {           
            $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
          
        }      
    }).on('focus', function () {
        $(this).keydown();
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };

    $(this).autocomplete("search");
});
//Bind challenge sub-type on challenge type change
jQuery(function () {
    jQuery('#ProgramType').change(function () {       
        $('#loadingChallengeSubType').show();      
        $('.error-msg').html("");
        $('#ProgramCategoryId').prop('disabled', true);
        jQuery('#ProgramCategoryId').css('background-color', '#FAF9F9');
        var id = jQuery("#ProgramType :selected").val();     
        if (id != "") {          
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
                    $('#loadingChallengeSubType').hide();
                    $('#ProgramCategoryId').prop('disabled', false);
                    jQuery('#ProgramCategoryId').css('background-color', '#FFFFFF');
                    var items = '<option value="">--Choose Category Type--</option>';
                    jQuery.each(data, function (i, challenge) {
                        items += "<option value='" + challenge.Value + "'>" + challenge.Text + "</option>";
                    });
                    // Fill ChallengeSubType Dropdown list
                    jQuery('#ProgramCategoryId').html(items);
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



        } else {
            $('#loadingChallengeSubType').hide();
            $('#ProgramCategoryId').prop('disabled', false);
            jQuery('#ProgramCategoryId').css('background-color', '#FFFFFF');

            var items = '<option value="">--Choose Category Type--</option>';
            jQuery('#ProgramCategoryId').html(items);
        }
        return false;
    });
    var selectedTrainerId = $("#TrainerId option:selected").val();
    if (selectedTrainerId > 0) {
        $('#challengeNoTrainerTeam').hide();
    }
    else {
        $('#challengeNoTrainerTeam').show();
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

  
    jQuery("#chkAllChallengeCategory [id*=PostedChallengeCategory_ChallengeCategoryId]").click(function () {       
        jQuery("#SelectedChallengeCategoryCheck").val(null);
        if (jQuery("#chkAllChallengeCategory [id*=PostedChallengeCategory_ChallengeCategoryId]:checked").length > 0) {          
            jQuery("#SelectedChallengeCategoryCheck").val("Checked");
        }
    });

    jQuery("#chkAllNoTrainetWorkoutTeam [id*=PostedTeams_TeamsID]").click(function () {
        try {
            if (this.checked) {
                chkAllNoTrainetWorkoutTeam();
            }
            else {
                $("#SelectAllTeam").prop("checked", false);
            }
        }
        catch (err) {

        }

    });
});
function chkAllNoTrainetWorkoutTeam() {
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
    else {
        $("#SelectAllTeam").prop("checked", false);
    }
}
function IschkAllTrendingCategory() {
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
    }
    else {
        $("#SelectAllTrendingCategory").prop("checked", false);
    }
}
jQuery(function () {

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
   
    jQuery("#chkAllTrendingCategory [id*=PostedTrendingCategory_TrendingCategoryID]").click(function () {
        try {
            if (this.checked) {
                IschkAllTrendingCategory();
            }
            else {
                $("#SelectAllTrendingCategory").prop("checked", false);
            }
        }
        catch (err) {

        }

    });
});
function GetWeekWorkout(weekId) {
  
    var weekworkoutsIDs = "";
    var prgweekWoekoutsControlID = '#ProgramWorkoutsCount' + weekId;
    var icount = $(prgweekWoekoutsControlID).val();
    var weekMainWorkoutId = $('#weekMainWorkoutId' + weekId);
    for (var escount = 1; escount <= icount ; escount++) {
        var workoutId = '#ProgramWeekHidenWorkout' + escount; 
        var weekworkoutId = weekMainWorkoutId.find(workoutId).val();
        if (weekworkoutId !== undefined && weekworkoutId !== null && weekworkoutId !== "") {
            weekworkoutsIDs += weekworkoutId + "^";
        } else {
            weekworkoutsIDs += "SNA" + "^";
        }    

    }
    return weekworkoutsIDs;
}

function GetUpdatedWeekWorkout(weekId) {

    var weekworkoutsIDs = "";
    var prgweekWoekoutsControlID = '#ProgramWorkoutsCount' + weekId;
    var icount = $(prgweekWoekoutsControlID).val();
    var weekMainWorkoutId = $('#weekMainWorkoutId' + weekId);
    for (var escount = 1; escount <= icount ; escount++) {
        var workoutId = '#ProgramWeekHidenWorkout' + escount;
        var weekworkoutId = weekMainWorkoutId.find(workoutId).val();
        if (weekworkoutId !== undefined && weekworkoutId !== null && weekworkoutId !== "") {
            weekworkoutsIDs += weekworkoutId + "^";
        } else {
            weekworkoutsIDs += "SNA" + "^";
        }
        var weekworkoutID = '#IsProgramNewWeekWorkout' + escount;
        var weekworkoutIDValue = weekMainWorkoutId.find(weekworkoutID).val();
        if (weekworkoutIDValue !== undefined && weekworkoutIDValue !== null && weekworkoutIDValue !== "") {
            weekworkoutsIDs += weekworkoutIDValue + "^";
        } else {
            weekworkoutsIDs += "0" + "^";
        }
        weekworkoutsIDs += "<>";

    }
    return weekworkoutsIDs;
}
$(document).ready(function () {
    var wrapper = $("#programWeekWorkouttoggalediv"); //Fields wrapper
    var add_Week_button = $("#btnAddProgramWeek"); //Add button ID    
    var controlID = undefined;
    chkAllNoTrainetWorkoutTeam();
    IschkAllTrendingCategory();
    $(add_Week_button).click(function (e) { //on add input button click   
        e.preventDefault();
        if (programWeekcount < max_programfields) { //max input box allowed
           // programWeekcount++; //text box increment
            var insertedprogramWeekcount = parseInt(programWeekcount) + 1;
          programWeekcount=insertedprogramWeekcount ;
            var addprogramweekeurl = baseUrlString + "/Reporting/AddProgramWeek";
            jQuery.ajax({
                type: "GET",
                url: addprogramweekeurl,
                data: { id: insertedprogramWeekcount},
                async: false,
                success: function (data) {
                    $(wrapper).append(data);
                },
                error: function (result) {
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
            $('.add_week_button').prop('disabled', false);
            programweekTotalworkoutcount = parseInt(programweekTotalworkoutcount) + 1;

            $('#Duration').val(parseInt(programWeekcount) + " Week(s)");
            $('#Workouts').val(parseInt(programweekTotalworkoutcount) + " Workout(s)");
            BindTrainers(parseInt(insertedprogramWeekcount));
            BindTrainingZone(parseInt(insertedprogramWeekcount));
            BindDifficultyLevel(parseInt(insertedprogramWeekcount));
        }
        return false;
    });
    $(document).delegate("button[id^='btnAddProgramWprkout']", "click", function (e) {       
        e.preventDefault();
        var id = $(this).attr('id').replace(/[^0-9]/g, '');
        // Get the Execisise current set count
        var programWorkoutsCountControlID = '#ProgramWorkoutsCount' + id;
        var programWeekWorkoutsCount = parseInt($(programWorkoutsCountControlID).val());
        if (programWeekWorkoutsCount > 0) {
            programweekworkoutcount = programWeekWorkoutsCount;
        }
        else {
            programweekworkoutcount = 0;
        }
        var weekworkoutPlaceholdId = '#weekMainWorkoutId' + id;
        if (programweekworkoutcount < max_programfields) { //max input box allowed
            //text box increment
            var workoutscount = parseInt(programweekworkoutcount);
            ++ workoutscount;
            var url = baseUrlString + "/Reporting/AddProgramWeekWorkout";
            $.post(url, { WeekCountID: parseInt(id), WeekWorkoutCountID: parseInt(workoutscount) })
                 .done(function (response) {
                     $(weekworkoutPlaceholdId).append(response);
                 });
            $(programWorkoutsCountControlID).val(parseInt(workoutscount));
            programweekTotalworkoutcount = parseInt(programweekTotalworkoutcount) + 1;
            $('#Duration').val(parseInt(programWeekcount) + " Week(s)");
            $('#Workouts').val(parseInt(programweekTotalworkoutcount) + " Workout(s)");


        }
        return false;
    }); 
    

});



   
  






