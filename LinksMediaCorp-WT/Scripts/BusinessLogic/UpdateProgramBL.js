// Create the execise list in case of update 
var updatedprogramWeekcount = 0; 
 programweekworkoutcount = 1;
 programweekTotalworkoutcount = 0;
 var $uploadFeaturedCrop;
 var uploadFeaturedreader;
 var $uploadCrop;
 var reader;
 var isCroped = false;
 var programwrapper = $("#programWeekWorkouttoggalediv");
 function Featuredoutput(node) {
     var existing = $('#result .croppie-result');
     if (existing.length > 0) {
         existing[0].parentNode.replaceChild(node, existing[0]);
     }
     else {
         $('#result')[0].appendChild(node);
     }
 }
 function FeaturedPopupResult(result) {
     if (result.src) {
         $('#FeaturedImageRowData').val(result.src);
         $('#myform').submit();
         $uploadFeaturedCrop = null;
         uploadFeaturedreader = null;
     }     
     return true;
 }
 function FeaturedPhotoUpload() {
     uploadFeaturedreader = null;
       function FeaturedreadFile(input) {
         if (input.files && input.files[0]) {
             uploadFeaturedreader = new FileReader();
             uploadFeaturedreader.onload = function (e) {
                 $uploadFeaturedCrop.croppie('bind', {
                     url: e.target.result
                 });
                 $('.upload-FeaturedImage').addClass('ready');
                 $("#existingFeaturedImg").hide();
                 $('#existingFeaturedImgConatiner').hide();
                 $("#upload-FeaturedImage").show();
             }
             uploadFeaturedreader.readAsDataURL(input.files[0]);
         }        
     }
     $uploadFeaturedCrop = $('#upload-FeaturedImage').croppie({
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
     $('#FeaturedUpload').on('change', function () { FeaturedreadFile(this); });
     uploadFeaturedreader = null;
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
             $uploadCrop = null;
             reader = null;
             removedWeekworkouts = "";
         }        
         return true;
     }
 function ProgramImageUpload() {         
         isCroped = false;
         reader = null;
         function readFile(input) {
             if (input.files && input.files[0]) {
                 reader = new FileReader();
                 reader.onload = function (e) {
                     $uploadCrop.croppie('bind', {
                         url: e.target.result
                     });
                     $('.upload-demo').addClass('ready');
                     $("#existingProfileImg").hide();
                     $('.fileupload fileupload-new col-md-10').hide();
                     $("#cropTrainerbtn").hide();
                     $('#existingProfileImgConatiner').hide();
                 }
                 reader.readAsDataURL(input.files[0]);
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
             }
         });
         $('#upload').on('change', function () { readFile(this); });             
         $("#cropTrainerbtn").on("click", function (e) {
             isCroped = true;
             var imagesrc = $("#existingProfileImg").attr("src");
             $("#existingProfileImgConatiner").hide();
             if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                 $uploadCrop.croppie('bind', imagesrc);
                 $('.upload-demo').addClass('ready');
             }
             return false;
         });
         reader = null;
     }
 jQuery(function (e) {
     $("#myLoadingElement").show();
     $("#upload-FeaturedImage").hide();     
    var id = jQuery("#ChallengeId").val();

    if (id != "") {

        var urlchallenge = baseUrlString + "/Reporting/GetAllWeeWorkoutsByProgramID";
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
                var items = '';          
                $('#Workouts').val(data.Workouts);
                programWeekcount = data.Durations.split(' ')[0];
                programweekTotalworkoutcount = data.Workouts.split(' ')[0];

                $('#Duration').val(data.Durations);               
                jQuery.each(data.ProgramWeekWorkouts, function (i, item) {
                    updatedprogramWeekcount =parseInt(updatedprogramWeekcount) + 1; //text box increment
                    // Bind the First Execise Set
                    if (i == 0) {
                        var programweekworkoutcount = 0;
                        var weekworkoutsitem = '';
                        var weeknumber = i;
                        weeknumber = weeknumber + 1;
                        var ProgramWorkoutsControlID = '#ProgramWorkoutsCount' + weeknumber;
                        var weekMainWorkoutPlaceholdId = '#weekMainWorkoutId' + weeknumber;                   
                        $.each(item.WeekWorkoutsRecords, function (v,e) {
                            if (programweekworkoutcount < max_programfields) { //max input box allowed                            
                                programweekworkoutcount=parseInt(programweekworkoutcount) + 1;                               
                                if (v == 0) {
                                    $('#ProgramWeekWorkout1').val(this.WorkoutName);
                                   $('#ProgramWeekHidenWorkout1').val(parseInt(this.WorkoutChallengeId));                                 
                                   $('#IsProgramNewWeekWorkout1').val(this.ProgramWeekWorkoutId);
                                }
                                else {
                                    var weekworkoutmodel = {
                                        WeekCountID: parseInt(weeknumber),
                                        WeekWorkoutCountID: parseInt(programweekworkoutcount),
                                        WorkoutName: this.WorkoutName,
                                        WorkoutUrl: this.WorkoutUrl,
                                        ChallengeWorkoutsId: parseInt(this.WorkoutChallengeId),
                                        IsNewProgramWeekWorkout: this.ProgramWeekWorkoutId,                                      
                                    };
                                    var weekworkouturl = baseUrlString + "/Reporting/GetProgramWeekWorkoutDetail";
                                    jQuery.ajax({
                                        type: "POST",
                                        url: weekworkouturl,
                                        data: weekworkoutmodel,
                                        async: false,
                                        success: function (data) {
                                            weekworkoutsitem += data;
                                        },
                                        error: function (result) {
                                            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                                        }
                                    });
                                }
                            }
                        });
                        $(weekMainWorkoutPlaceholdId).append(weekworkoutsitem);
                        $(ProgramWorkoutsControlID).val(parseInt(programweekworkoutcount));
                        programweekworkoutcount = 0;
                        workoutnumber = 0;
                    }
                    if (i > 0) {
                        // Bind the other than First Execise Set
                        if (updatedprogramWeekcount < max_programfields) { //max input box allowed              
                            var wekworkoutsitemdesc = '';
                            var programweekworkoutcount = 0;                           
                            $.each(item.WeekWorkoutsRecords, function () {
                                if (programweekworkoutcount < max_programfields) { //max input box allowed
                                    //text box increment                                
                                    ++ programweekworkoutcount;                                   
                                    var weekworkoutmodel = {
                                        WeekCountID: parseInt(updatedprogramWeekcount),
                                        WeekWorkoutCountID: parseInt(programweekworkoutcount),
                                        WorkoutName: this.WorkoutName,
                                        WorkoutUrl: this.WorkoutUrl,
                                        ChallengeWorkoutsId: this.WorkoutChallengeId,
                                        IsNewProgramWeekWorkout: this.ProgramWeekWorkoutId,                                      
                                    };
                                    var weekworkouturl = baseUrlString + "/Reporting/GetProgramWeekWorkoutDetail";
                                    jQuery.ajax({
                                        type: "POST",
                                        url: weekworkouturl,
                                        data: weekworkoutmodel,
                                        async: false,
                                        success: function (data) {
                                            wekworkoutsitemdesc += data;
                                        },
                                        error: function (result) {
                                            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                                        }
                                    });
                                  }
                            });
                          
                            var weekmodel = {
                                WeekCountID: parseInt(updatedprogramWeekcount),
                                WeekWorkoutCountID: parseInt(programweekworkoutcount),
                                WeekItemElement: wekworkoutsitemdesc,
                                IsNewProgramWeek: this.ProgramWeekId
                            };
                            var weekeurl = baseUrlString + "/Reporting/GetProgramWeekDetail";
                            jQuery.ajax({
                                type: "POST",
                                url: weekeurl,
                                data: weekmodel,
                                async: false,
                                success: function (data) {
                                    items += data;
                                },
                                error: function (result) {
                                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                                }
                            });                         
                            programweekworkoutcount = 0;
                            // Fill ChallengeSubType Dropdown list
                            if (items !== undefined && items != null && items !== '') {
                                $(programwrapper).append(items);
                            }
                            SetBindTrainers(parseInt(updatedprogramWeekcount), item.AssignedTrainerId)
                            SetBindTrainingZone(parseInt(updatedprogramWeekcount), item.AssignedTrainingzone)
                            SetBindDifficultyLevel(parseInt(updatedprogramWeekcount), item.AssignedDifficulyLevelId)
                            items = "";                         
                        }
                    }                  

                });
                programWeekcount=parseInt(updatedprogramWeekcount);
                updatedprogramWeekcount = 0;               
                //jQuery('#ChallengeSubTypeId').html(items);
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
      
    }
    $("#myLoadingElement").hide();
});
// Appende the workout List in case of validation


 jQuery(function () {
     var selectedTrainerId = $("#TrainerCredId option:selected").val();
     if (selectedTrainerId > 0) {
         $('#challengeNoTrainerTeam').hide();
     }
     else {
         $('#challengeNoTrainerTeam').show();
     }
 });
 $(document).ready(function () {
     ProgramImageUpload();
     FeaturedPhotoUpload();
     var wrapper = $("#programWeekWorkouttoggalediv"); //Fields wrapper   

     $('#btnBottomAdminUpdateProgram,#btnTopAdminUpdateProgram').click(function () {

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


         var ProgramWeekWorkout1 = "";
         $uploadCrop.croppie('result', {
             type: 'canvas',
             size: 'original'
         }).then(function (resp) {
             popupResult({
                 src: resp
             });
         });

         $uploadFeaturedCrop.croppie('result', {
             type: 'canvas',
             size: 'original'
         }).then(function (resp) {
             FeaturedPopupResult({
                 src: resp
             });
         });
         ProgramWeekWorkout1 = GetUpdatedWeekWorkout(1);
         var programSubtypeId = jQuery('#ProgramType').val();
         var weektotalworkoutstotalCount = $('#ProgramWeekHidenWorkout1').val();
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
             weekWorkoutsDetails = GetUpdatedWeekWorkout(icount);
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
                 AllweekworkoutId += drpdownDifficultyLevelIdValue + "~";
             }
             else {
                 AllweekworkoutId += "NA" + "~";
             }
             var weekIdValueId = '#IsProgramNewWeek' + icount
             var weekIdValue = jQuery(weekIdValueId).val();
             if (weekIdValue !== undefined && weekIdValue !== null && weekIdValue !== "") {
                 AllweekworkoutId += weekIdValue;
             }
             else {
                 AllweekworkoutId += "0";
             }
             AllweekworkoutId += "|";
         }
         $('#ProgramWeekWorkoutList').val(AllweekworkoutId);
         $('#RemovedWeekWorkouts').val(removedWeekworkouts);
         if (!isCroped && (reader == null || reader == 'undefined')) {
             $('#myform').submit();
             $uploadCrop = null;
             reader = null;
             removedWeekworkouts = "";
         }
     });
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
                    removeWeekWorkoutSectionId.find('#weekworkoutTittle' + icounter).attr("id","weekworkoutTittle" + removelexeNumber);
                                     
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
            -- programWeekWorkoutsCount;
           -- programweekTotalworkoutcount;
            $('#Workouts').val(programweekTotalworkoutcount + " Workout(s)");
            $(programWorkoutsCountControlID).val(parseInt(programWeekWorkoutsCount));
        }        
      
        return false;
    });
    $(wrapper).on("click", ".remove_field", function (e) { //u
        e.preventDefault();
        var removelexeNumber = $(this).attr('id').replace(/[^0-9]/g, '');
        --programWeekcount;
      //  var removelexeNumber = $(this).parent('div').parent('div').attr('id');
        var programWorkoutsCountControlID = '#ProgramWorkoutsCount' + removelexeNumber;
        var programWeekWorkoutsCount = parseInt($(programWorkoutsCountControlID).val());
        if (programWeekWorkoutsCount > 0) {
            programweekTotalworkoutcount = programweekTotalworkoutcount - programWeekWorkoutsCount;
            $('#Duration').val(programWeekcount + " Week(s)");
            $('#Workouts').val(programweekTotalworkoutcount + " Workout(s)");
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
                $('#' + icounter).attr("id",  removelexeID);
                removelexeID = removelexeID + 1;
            }
        }
        return false;
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
    jQuery(document).delegate("#TrainerCredId", "change", function () {
        var selectedTrainerId = $("#TrainerCredId option:selected").val();
        if (selectedTrainerId !== undefined && selectedTrainerId > 0) {
            $('#challengeNoTrainerTeam').hide();
        }
        else {
            $('#challengeNoTrainerTeam').show(); 
        }
    });
});




