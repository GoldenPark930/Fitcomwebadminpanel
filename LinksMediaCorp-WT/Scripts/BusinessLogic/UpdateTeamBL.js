var UpdateTotalFilterChallengeRecords = new Array();
var TotalSeachLevel1TeamRecords = new Array();
var maxTrendingCategorySelection = 6;
var UpdateTeam = (function () {
    $('#uploadprofilephoto').hide();
    $('#uploadPremiumphoto').hide();
    $('#uploadNutrition1photo').hide();
    $('#uploadNutrition2photo').hide();
 
    
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
            $('#CropProfileImageRowData').val(result.src);
        }
        $('#myform').submit();
         $uploadCrop=null;      
         reader = null;
        return true;
    }
    function popupPremiumResult(result) {
        if (result.src) {
            $('#CropPremiumImageRowData').val(result.src);
        }
        $('#myform').submit();
         $uploadPrimumCrop=null;
         reader = null;
        return true;
    }
    function popupNutrition1Result(result) {
        if (result.src) {
            $('#CropNutrition1ImageRowData').val(result.src);
        }
        $('#myform').submit();
        $uploadNutrition1Crop = null;
        reader = null;
        return true;
    }
    function popupNutrition2Result(result) {
        if (result.src) {
            $('#CropNutrition2ImageRowData').val(result.src);
        }
        $('#myform').submit();
        $uploadNutrition2Crop = null;
        reader = null;
        return true;
    }
    function TeamPhotoUpload() {
        var isCroped = false;
        var $uploadCrop;
        var $uploadPrimumCrop;
        var $uploadNutrition1Crop;
        var $uploadNutrition2Crop;
        var reader = null;
        function readFile(input) {
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadCrop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.uploadprofilephoto').addClass('ready');
                    $('#existingProfileImg').hide();
                    $("#existingProfileImgConatiner").hide();
                }
                reader.readAsDataURL(input.files[0]);
                $('#uploadprofilephoto').show();
            }
           

        }
        function readPremiumFile(input) {

            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadPrimumCrop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.uploadPremiumphoto').addClass('ready');
                    $("#existingPremiumImg").hide();
                    $('#existingPremiumImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
                $('#uploadPremiumphoto').show();
            }
           
        }
        function readNutrition1File(input) {
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadNutrition1Crop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.uploadNutrition1photo').addClass('ready');
                    $("#existingNutrition1Img").hide();
                    $('#existingNutrition1ImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
                $('#uploadNutrition1photo').show();
            }
        }
        function readNutrition2File(input) {
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadNutrition2Crop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.uploadNutrition2photo').addClass('ready');
                    $("#existingNutrition2Img").hide();
                    $('#existingNutrition2ImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
                $('#uploadNutrition2photo').show();
            }
        }
        $uploadCrop = $('#uploadprofilephoto').croppie({
            viewport: {
                width: 300,
                height: 300,
                type: 'square'
            },
            boundary: {
                width: 300,
                height: 300
            }
        });

        $uploadPrimumCrop = $('#uploadPremiumphoto').croppie({
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
        $uploadNutrition1Crop = $('#uploadNutrition1photo').croppie({
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
        $uploadNutrition2Crop = $('#uploadNutrition2photo').croppie({
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

        $('#profileupload').on('change', function () { readFile(this); });
        $('#premiumphotoupload').on('change', function () { readPremiumFile(this); });
        $('#Nutrition1photoupload').on('change', function () { readNutrition1File(this); });
        $('#Nutrition2photoupload').on('change', function () { readNutrition2File(this); });

        $("#cropProfilePicbtn").on("click", function (e) {
                        isCroped = true;
                        var imagesrc = $("#existingProfileImg").attr("src");
                        $("#existingProfileImgConatiner").hide();
                        $('#uploadprofilephoto').show();
                        if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                            $uploadCrop.croppie('bind', imagesrc);
                            $('.uploadprofilephoto').addClass('ready');
                        }
                        $("#cropProfilePicbtn").hide();
                        return false;
                    });
        $("#cropTeamPremiumbtn").on("click", function (e) {
                        isCroped = true;
                        var imagesrc = $("#existingPremiumImg").attr("src");
                        $("#existingPremiumImgConatiner").hide();
                        $('#uploadPremiumphoto').show();
                        if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                            $uploadPrimumCrop.croppie('bind', imagesrc);
                            $('.uploadPremiumphoto').addClass('ready');
                        }           
                        $("#cropTeamPremiumbtn").hide();
                        return false;
        });
        $("#cropTeamNutrition1btn").on("click", function (e) {
            isCroped = true;
            var imagesrc = $("#existingNutrition1Img").attr("src");
            $("#existingNutrition1ImgConatiner").hide();
            $('#uploadNutrition1photo').show();
            if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                $uploadNutrition1Crop.croppie('bind', imagesrc);
                $('.uploadNutrition1photo').addClass('ready');
            }
            $("#cropTeamNutrition1btn").hide();
            return false;
        });
        $("#cropTeamNutrition2btn").on("click", function (e) {
            isCroped = true;
            var imagesrc = $("#existingNutrition2Img").attr("src"); 
            $("#existingNutrition2ImgConatiner").hide(); 
            $('#uploadNutrition2photo').show();
            if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                $uploadNutrition2Crop.croppie('bind', imagesrc);
                $('.uploadNutrition2photo').addClass('ready');
            }
            $("#cropTeamNutrition2btn").hide();
            return false;
        });
            $('#btnSubmit,#btntopSubmit').on('click', function (ev) {
            $uploadCrop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupResult({
                    src: resp
                });
            });

            $uploadPrimumCrop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupPremiumResult({
                    src: resp
                });
            });
            $uploadNutrition1Crop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupNutrition1Result({
                    src: resp
                });
            });
            $uploadNutrition2Crop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupNutrition2Result({
                    src: resp
                });
            });

            if (reader == null || reader == 'undefined') {
                $('#myform').submit();
                 $uploadCrop=null;
                 $uploadPrimumCrop = null;
                 $uploadNutrition1Crop = null;
                 $uploadNutrition2Crop = null;
                 reader = null
            }
        });
        reader = null;
    }
    function showHideControl()
            {
        $('#uploadprofilephoto').hide();
        $('#uploadPremiumphoto').hide();
        $('#uploadNutrition1photo').hide();
        $('#uploadNutrition2photo').hide();
                var imagesrc = $("#existingProfileImg").attr("src");
                if (imagesrc != undefined && imagesrc != null) {
                    $("#ProfileImage").show();

                } else {
                    $("#ProfileImage").hide();
                }
                var imagesrc1 = $("#existingPremiumImg").attr("src");
                if (imagesrc1 != undefined && imagesrc1 != null) {
                    $("#PrimuimtrainerImage").show();

                } else {
                    $("#PrimuimtrainerImage").hide();
                }
                var nutrition1imagesrc = $("#existingNutrition1Img").attr("src"); 
                if (nutrition1imagesrc != undefined && nutrition1imagesrc != null) {
                    $("#Nutrition1CropImage").show();

                } else {
                    $("#Nutrition1CropImage").hide();
                }
                var nutrition2imagesrc = $("#existingNutrition2Img").attr("src");
                if (nutrition2imagesrc != undefined && nutrition2imagesrc != null) {
                    $("#Nutrition2CropImage").show();

                } else {
                    $("#Nutrition2CropImage").hide();
                }
            }
    function init() {
        showHideControl();
        TeamPhotoUpload();
    }

    return {
        init: init
    };
})();
function chkAssignWorkoutTrendingCategory() {
    var isAllChecked = false;
    $('#detailschkAssignWorkoutTrendingCategory [id*=PostedWorkoutTrendingCategory_TrendingCategoryID]').each(function () {
        if (this.checked) {
            isAllChecked = true;
        } else {
            isAllChecked = false;
            return false;
        }
    });
    if (isAllChecked) {
        $("#SelectWorkoutAllTrendingCategory").prop("checked", true);
    } else {
        $("#SelectWorkoutAllTrendingCategory").prop("checked", false);
    }
}
function chkAssignFitnessTestTrendingCategory()
{
    var isAllChecked = false;
    $('#detailschkAssignFitnessTestTrendingCategory [id*=PostedFitnessTestTrendingCategory_TrendingCategoryID]').each(function () {
        if (this.checked) {
            isAllChecked = true;
        } else {
            isAllChecked = false;
            return false;
        }
    });
    if (isAllChecked) {
        $("#SelectAllFittnestTrendingCategory").prop("checked", true);
    } else {
        $("#SelectAllFittnestTrendingCategory").prop("checked", false);
    }
}
function chkAssignProgramTrendingCategory()
{
    var isAllChecked = false;
    $('#detailschkAssignProgramTrendingCategory [id*=PostedProgramTrendingCategory_TrendingCategoryID]').each(function () {
        if (this.checked) {
            isAllChecked = true;
        } else {
            isAllChecked = false;
            return false;
        }
    });
    if (isAllChecked) {
        $("#SelectAllProgramTrendingCategory").prop("checked", true);
    } else {
        $("#SelectAllProgramTrendingCategory").prop("checked", false);
    }
}
jQuery(function () {
    chkAssignWorkoutTrendingCategory();
    chkAssignFitnessTestTrendingCategory();
    chkAssignProgramTrendingCategory();
});
jQuery(function () {
    if ($("#SelectWorkoutAllTrendingCategory").is(':checked')) {
        $('#detailschkAssignWorkoutTrendingCategory [id*=PostedWorkoutTrendingCategory_TrendingCategoryID]').each(function () {
            this.checked = true;
        });
    }
    jQuery('#SelectWorkoutAllTrendingCategory').change(function () {
        if (this.checked) {
            $('#detailschkAssignWorkoutTrendingCategory [id*=PostedWorkoutTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#detailschkAssignWorkoutTrendingCategory [id*=PostedWorkoutTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = false;
            });
        }
    });
  
    jQuery("#detailschkAssignWorkoutTrendingCategory [id*=PostedWorkoutTrendingCategory_TrendingCategoryID]").click(function () {        
        try {           
            if (this.checked) {
                chkAssignWorkoutTrendingCategory();
            }
            else
            {
                $("#SelectWorkoutAllTrendingCategory").prop("checked", false);
            }            
        }
        catch (err) {
            
        }       

    });
    jQuery("#detailschkAssignFitnessTestTrendingCategory [id*=PostedFitnessTestTrendingCategory_TrendingCategoryID]").click(function () {
        try {
            if (this.checked) {
                chkAssignFitnessTestTrendingCategory();
            }
            else {
                $("#SelectAllFittnestTrendingCategory").prop("checked", false);
            }
        }
        catch (err) {

        }

    });
    jQuery("#detailschkAssignProgramTrendingCategory [id*=PostedProgramTrendingCategory_TrendingCategoryID]").click(function () {
        try {
            if (this.checked) {
                chkAssignProgramTrendingCategory();
            }
            else {
                $("#SelectAllProgramTrendingCategory").prop("checked", false);
            }
        }
        catch (err) {

        }

    });

});
jQuery(function () {
    if ($("#SelectAllFittnestTrendingCategory").is(':checked')) {
        $('#detailschkAssignFitnessTestTrendingCategory [id*=PostedFitnessTestTrendingCategory_TrendingCategoryID]').each(function () {
            this.checked = true;
        });
    }
    jQuery('#SelectAllFittnestTrendingCategory').change(function () {
        if (this.checked) {
            $('#detailschkAssignFitnessTestTrendingCategory [id*=PostedFitnessTestTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#detailschkAssignFitnessTestTrendingCategory [id*=PostedFitnessTestTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = false;
            });
        }
    });

});
jQuery(function () {

    if ($("#SelectAllProgramTrendingCategory").is(':checked')) {
        $('#detailschkAssignProgramTrendingCategory [id*=PostedProgramTrendingCategory_TrendingCategoryID]').each(function () {
            this.checked = true;
        });
    }
    jQuery('#SelectAllProgramTrendingCategory').change(function () {
        if (this.checked) {
            $('#detailschkAssignProgramTrendingCategory [id*=PostedProgramTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#detailschkAssignProgramTrendingCategory [id*=PostedProgramTrendingCategory_TrendingCategoryID]').each(function () {
                this.checked = false;
            });
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
                    //$("#progressbar").progressbar({
                    //    value: data.progressbar
                    //});
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

                    $('#loadingmessage').hide();
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

//Check user name from database
jQuery(function () {
    jQuery('#UserName').blur(function () {
        var name;
        var url = baseUrlString + "/Reporting/GetUserRecord";
        if ($('#TeamId').val() === undefined) {
            name = jQuery('#UserName').val();
        }
        else {
            name = jQuery('#UserName').val() + ',' + $('#TeamId').val();
        }
        jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
                jQuery("#resultUser").html("<span style='color:red'>User Name already exist!</span>");
                jQuery("#UserName").css('background-color', '#e97878');
                jQuery('#CheckUser').val('NotAvailable');
            }
            else {
                jQuery("#resultUser").html(null);
                jQuery("#UserName").css('background-color', '');
                jQuery('#CheckUser').val(null);

            }
        });
    })
});

//Check email from database
jQuery(function () {
    //jQuery('#EmailId').blur(function () {
    //    var name;

    //    if ($('#TeamId').val() === undefined) {
    //        name = $('#EmailId').val();
    //    }
    //    else {
    //        name = $('#EmailId').val() + ',' + $('#TeamId').val();
    //    }
    //    var url = baseUrlString + "/Reporting/GetTeamEmail";
    //    jQuery.get(url, { input: name }, function (data) {
    //        if (data == "Available") {
    //            jQuery("#resultEmail").html("<span style='color:red'>Email already exist!</span>");
    //            jQuery("#EmailId").css('background-color', '#e97878');
    //            jQuery('#CheckEmail').val('NotAvailable');
    //        }
    //        else {
    //            jQuery("#resultEmail").html(null);
    //            jQuery("#EmailId").css('background-color', '');
    //            jQuery('#CheckEmail').val(null);
    //        }
    //    });
    //})
});

//Check team name from database
jQuery(function () {
    jQuery('#TeamName').blur(function () {
        var name;
        var url = baseUrlString + "/Reporting/GetTeamRecord";
        if ($('#TeamId').val() === undefined) {
            name = jQuery('#TeamName').val();
        }
        else {
            name = $('#TeamName').val() + ',' + $('#TeamId').val();
        }
        jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
                jQuery("#resultTeam").html("<span style='color:red'>Team already exist!</span>");
                jQuery("#TeamName").css('background-color', '#e97878');
                jQuery('#CheckTeam').val('NotAvailable');
            }
            else {
                jQuery("#resultTeam").html(null);
                jQuery("#TeamName").css('background-color', '');
                jQuery('#CheckTeam').val(null);

            }
        });
    })
});
jQuery("[id*=ZipCode]").keyup(function () {
    if (isNaN(this.value)) {
        this.value = "";
        alert('Please enter numeric value.');
    }
});
jQuery("#PrimaryCommissionRate,#Level1CommissionRate,#Level2CommissionRate").keyup(function (event) {
    if (isNaN(this.value)) {
        this.value = "";
        alert('Please enter numeric value.');
    }
});
jQuery(function () {
    jQuery('#IsChangePassword').change(function () {
        if (this.checked) {
            jQuery('#divChangePassword').show();
          
        }
        else {
            jQuery('#divChangePassword').hide();          
        }
    });
    jQuery('#IsChangePassword').load(function () {
        if (this.checked) {
            jQuery('#divChangePassword').show();
        }
        else {
            jQuery('#divChangePassword').hide();
        }
    });
});
function CheckDuplicateFitennesTest(challengeId, selectedchallengeId, url) {   
    var fitcomtestExeciseId = "#FitcomtestChallengeId" + challengeId;
    if (challengeId !== undefined && challengeId !== null && parseInt(challengeId) === 1) {       
        var secondfitness = $("#FitcomtestChallengeId2").val();       
        if (secondfitness !== undefined && secondfitness !== null && secondfitness !== "" && secondfitness == selectedchallengeId) {          
            $("#lblUniqueFitnessTest" + challengeId).html("Fitness Test is already selected.");
            $(fitcomtestExeciseId).val(0);
            $("#SpnFitnessTestLink" + challengeId).html("");
            jQuery("#FitnessTest" + challengeId).value("");
        } else {
            $(fitcomtestExeciseId).val(selectedchallengeId);
            $("#lblUniqueFitnessTest" + challengeId).html("");
            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
            $("#SpnFitnessTestLink" + challengeId).html('<a id="FitnessTestLink' + challengeId + '" href="' + url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + url + '</a>');
        }
    }
    else if (challengeId !== undefined && challengeId !== null && parseInt(challengeId) === 2) {      
        var firstfitnessTest = $("#FitcomtestChallengeId1").val();       
        if (firstfitnessTest !== undefined && firstfitnessTest !== null && firstfitnessTest !== "" && firstfitnessTest == selectedchallengeId) {
            $("#lblUniqueFitnessTest" + challengeId).html("Fitness Test is already selected.");
            $(fitcomtestExeciseId).val(0);
            $("#SpnFitnessTestLink" + challengeId).html("");
            jQuery("#FitnessTest" + challengeId).value("");

        } else {
            $(fitcomtestExeciseId).val(selectedchallengeId);
            $("#lblUniqueFitnessTest" + challengeId).html("");
            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
            $("#SpnFitnessTestLink" + challengeId).html('<a id="FitnessTestLink' + challengeId + '" href="' + url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + url + '</a>');
        }
    }
}

function CheckDuplicateAdvIntermediaProgram(ProgramCtrlId, selectedProgramId, url) {
    var currentAdvIntProgramId = "#AdvIntProgramId" + ProgramCtrlId;
    if (ProgramCtrlId !== undefined && ProgramCtrlId !== null && parseInt(ProgramCtrlId) === 1) {
        var secondAdvIntPrgId = $("#AdvIntProgramId2").val();
        var thirdAdvIntPrgId = $("#AdvIntProgramId3").val();
        if ((secondAdvIntPrgId !== undefined && secondAdvIntPrgId !== null && secondAdvIntPrgId !== "" && secondAdvIntPrgId == selectedProgramId) || (thirdAdvIntPrgId !== undefined && thirdAdvIntPrgId !== null && secondAdvIntPrgId !== "" && thirdAdvIntPrgId == selectedProgramId)) {
            $("#lblAdvIntProgramError" + ProgramCtrlId).html("Advanced/Inetermediate program is already selected.");
            $(currentAdvIntProgramId).val(0);
            $("#AdvIntProgramUrl" + ProgramCtrlId).html("");
            jQuery("#AdvIntProgram" + ProgramCtrlId).value("");

        } else {
            $("#lblAdvIntProgramError" + ProgramCtrlId).html("");
            $(currentAdvIntProgramId).val(selectedProgramId);
            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
            $("#SpnAdvIntProgramLink" + ProgramCtrlId).html('<a id="AdvIntProgramLink' + ProgramCtrlId + '" href="' + url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + url + '</a>');
        }
    }
    else if (ProgramCtrlId !== undefined && ProgramCtrlId !== null && parseInt(ProgramCtrlId) === 2) {

        var firstAdvIntPrgId = $("#AdvIntProgramId1").val();
        var thirdAdvIntPrgId = $("#AdvIntProgramId3").val();
        if ((firstAdvIntPrgId !== undefined && firstAdvIntPrgId !== null && firstAdvIntPrgId !== "" && firstAdvIntPrgId == selectedProgramId) || (thirdAdvIntPrgId !== undefined && thirdAdvIntPrgId !== null && secondAdvIntPrgId !== "" && thirdAdvIntPrgId == selectedProgramId)) {
            $("#lblAdvIntProgramError" + ProgramCtrlId).html("Advanced/Inetermediate program is already selected.");
            $(currentAdvIntProgramId).val(0);
            $("#AdvIntProgramUrl" + ProgramCtrlId).html("");
            jQuery("#AdvIntProgram" + ProgramCtrlId).value("");
        } else {
            $("#lblAdvIntProgramError" + ProgramCtrlId).html("");
            $(currentAdvIntProgramId).val(selectedProgramId);
            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
            $("#SpnAdvIntProgramLink" + ProgramCtrlId).html('<a id="AdvIntProgramLink' + ProgramCtrlId + '" href="' + url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + url + '</a>');
        }
    }
    else if (ProgramCtrlId !== undefined && ProgramCtrlId !== null && parseInt(ProgramCtrlId) === 3) {
        {
            var firstAdvIntPrgId = $("#AdvIntProgramId1").val();
            var secondAdvIntPrgId = $("#AdvIntProgramId2").val();
            if ((secondAdvIntPrgId !== undefined && secondAdvIntPrgId !== null && secondAdvIntPrgId !== "" && secondAdvIntPrgId == selectedProgramId) || (firstAdvIntPrgId !== undefined && firstAdvIntPrgId !== null && firstAdvIntPrgId !== "" && firstAdvIntPrgId == selectedProgramId)) {
                $("#lblAdvIntProgramError" + ProgramCtrlId).html("Advanced/Inetermediate program is already selected.");
                $(currentAdvIntProgramId).val(0);
                $("#AdvIntProgramUrl" + ProgramCtrlId).html("");
                jQuery("#AdvIntProgram" + ProgramCtrlId).value("");
            } else {
                $("#lblAdvIntProgramError" + ProgramCtrlId).html("");
                $(currentAdvIntProgramId).val(selectedProgramId);
                // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
                $("#SpnAdvIntProgramLink" + ProgramCtrlId).html('<a id="AdvIntProgramLink' + ProgramCtrlId + '" href="' + url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + url + '</a>');
            }
        }
    }
}
jQuery(document).delegate("input:text[id^='OnboardingVideo']", "focusin", function () {
    jQuery(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {
            var urlexe = baseUrlString + "/Reporting/GetSelectedOnboradingVideExercises";
            jQuery.ajax({
                type: "POST",
                url: urlexe,
                data: { searchTerm: request.term },
                success: function (data) {                    
                    response($.map(data, function (item) {
                        return {
                            label: item.ExerciseName,
                            value: item.ExerciseName,
                            url: item.VedioLink,
                            Id: item.ExerciseId
                        };
                    }));
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {           
            jQuery("#lblOnboardingVideo").html("");
            if (!ui.item) {
                $("#OnboardingExeciseVideoId").val(0);
            } else {
                var selectedExeciseID = parseInt(ui.item.Id);
                $("#OnboardingExeciseVideoId").val(selectedExeciseID);
                jQuery("#SpnOnboardingVideoLink").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');

               
            }
           
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblOnboardingVideo").html("No exercise found for the selected values.");
                if (!ui.item) {
                    $("#OnboardingExeciseVideoId").val(0);
                }
                this.focus();
            } else {
                jQuery("#lblOnboardingVideo").html("");
                var selectedExeciseID = parseInt(ui.item.Id);
                $("#OnboardingExeciseVideoId").val(selectedExeciseID);
                jQuery("#SpnOnboardingVideoLink").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');


               
               
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
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };
    $(this).autocomplete("search");
});

jQuery(document).delegate("input:text[id^='FitnessTest']", "focusin", function () {  
    var controlNumber = undefined;
    //var programworkoutControlId = $(this).attr('id').replace(/[^0-9]/g, '');
    var fitnessTesttControlId = $(this).parent('div').attr('id');
    jQuery(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {
            // GetSelectedFilterWorkouts(programworkoutControlId, request.term);
            var urlchallenge = baseUrlString + "/Reporting/GetAllFilterFitnessTest";
            jQuery.ajax({
                type: "POST",
                url: urlchallenge,
                async: false,
                data: { serachItem: request.term },
                success: function (data) {
                    UpdateTotalFilterChallengeRecords = [];
                    jQuery.each(data, function (i, fitnessTest) {
                        var fitnessTestrecord = {
                            ChallengeName: fitnessTest.ChallengeName,
                            ChallengeUrl: fitnessTest.ChallengeUrl,
                            ChallengeId: fitnessTest.ChallengeId
                        };
                        UpdateTotalFilterChallengeRecords.push(fitnessTestrecord);

                    });
                }
            });
            response($.map(UpdateTotalFilterChallengeRecords, function (item) {
                return {
                    label: item.ChallengeName,
                    value: item.ChallengeName,
                    url: item.ChallengeUrl,
                    Id: item.ChallengeId
                };
            }));
            UpdateTotalFilterChallengeRecords = [];
        },
        minLength: 0,
        select: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var fitcomtestChallengeId = "#FitcomtestChallengeId" + id;
            if (!ui.item) {
                $(fitcomtestChallengeId).val(0);
            } else {
                CheckDuplicateFitennesTest(id, ui.item.Id, ui.item.url);
               // $(fitcomtestChallengeId).val(ui.item.Id);
               // $("#SpnFitnessTestLink" + id).html('<a id="FitnessTestLink' + id + '" href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');

            }


            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
           
        },
        change: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var dvProgramWeekHideenWorkout = '#FitcomtestChallengeId' + id;
            if (!ui.item) {
                this.value = '';
                jQuery("#SpnFitnessTestLink1" + id).html("");
                if (!ui.item) {
                    $(dvProgramWeekHideenWorkout).val(0);
                }
                this.focus();

            } else {
                if (ui.item) {
                    CheckDuplicateFitennesTest(id, ui.item.Id, ui.item.url);
                   // $(dvProgramWeekHideenWorkout).val(ui.item.Id);
                }
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
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };

    $(this).autocomplete("search");
});

jQuery(document).delegate("input:text[id^='BeginnerProgram']", "focusin", function () {   
    var controlNumber = undefined;  
    var programworkoutControlId = $(this).parent('div').attr('id');
    jQuery(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {            
            var urlchallenge = baseUrlString + "/Reporting/GetAllBeginnerProgram";
            jQuery.ajax({
                type: "POST",
                url: urlchallenge,
                async: false,
                data: { serachItem: request.term },
                success: function (data) {
                    UpdateTotalFilterChallengeRecords = [];
                    jQuery.each(data, function (i, fitnessTest) {
                        var beginnerProgramrecord = {
                            ChallengeName: fitnessTest.ChallengeName,
                            ChallengeUrl: fitnessTest.ChallengeUrl,
                            ChallengeId: fitnessTest.ChallengeId
                        };
                        UpdateTotalFilterChallengeRecords.push(beginnerProgramrecord);

                    });
                }
            });
            response($.map(UpdateTotalFilterChallengeRecords, function (item) {
                return {
                    label: item.ChallengeName,
                    value: item.ChallengeName,
                    url: item.ChallengeUrl,
                    Id: item.ChallengeId
                };
            }));
            UpdateTotalFilterChallengeRecords = [];
        },
        minLength: 0,
        select: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var beginnerProgramId = "#BeginnerProgramId" + id;
            if (!ui.item) {
                $(beginnerProgramId).val(0);
            } else {
                $(beginnerProgramId).val(ui.item.Id);
                $("#SpnBeginnerProgramUrlLink" + id).html('<a id="BeginnerProgramLink' + id + '" href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');

            }
            // var programWorkoutLink= weekMainWorkoutId.find("#ProgramWorkoutLink" + id);          
           
        },
        change: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var beginnerProgramId = '#BeginnerProgramId' + id;
            if (!ui.item) {
                this.value = '';
                jQuery("#SpnBeginnerProgramUrlLink" + id).html("");
                if (!ui.item) {
                    $(beginnerProgramId).val(0);
                }
                this.focus();

            } else {
                if (ui.item) {
                    $(beginnerProgramId).val(ui.item.Id);
                }
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
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };

    $(this).autocomplete("search");
});

jQuery(document).delegate("input:text[id^='AdvIntProgram']", "focusin", function () {   
    var controlNumber = undefined;
    //var programworkoutControlId = $(this).attr('id').replace(/[^0-9]/g, '');
    var programworkoutControlId = $(this).parent('div').attr('id');
    jQuery(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {
            // Get data based on filter data          
            var urlchallenge = baseUrlString + "/Reporting/GetAllAdvIntProgram";
            jQuery.ajax({
                type: "POST",
                url: urlchallenge,
                async: false,
                data: { serachItem: request.term },
                success: function (data) {
                    TotalFilterChalUpdateTotalFilterChallengeRecordslengeRecords = [];
                    jQuery.each(data, function (i, fitnessTest) {
                        var advIntProgramrecord = {
                            ChallengeName: fitnessTest.ChallengeName,
                            ChallengeUrl: fitnessTest.ChallengeUrl,
                            ChallengeId: fitnessTest.ChallengeId
                        };
                        UpdateTotalFilterChallengeRecords.push(advIntProgramrecord);
                    });
                }
            });
            response($.map(UpdateTotalFilterChallengeRecords, function (item) {
                return {
                    label: item.ChallengeName,
                    value: item.ChallengeName,
                    url: item.ChallengeUrl,
                    Id: item.ChallengeId
                };
            }));
            UpdateTotalFilterChallengeRecords = [];
        },
        minLength: 0,
        select: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var advIntProgramId = "#AdvIntProgramId" + id;
            if (!ui.item) {
                $(advIntProgramId).val(0);
            } else {
                CheckDuplicateAdvIntermediaProgram(id, ui.item.Id, ui.item.url);
               // $(advIntProgramId).val(ui.item.Id);
               // $("#SpnAdvIntProgramLink" + id).html('<a id="AdvIntProgramLink' + id + '" href="' + ui.item.url + '" target="_blank" style="font-size:12px; color:#4b2b77; position:relative; float:left; text-decoration:underline; margin:5px 0 0 0;">' + ui.item.url + '</a>');

            }
           
        },
        change: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            var advIntProgramId = '#AdvIntProgramId' + id;
            if (!ui.item) {
                this.value = '';
                jQuery("#SpnAdvIntProgramLink" + id).html("");
                if (!ui.item) {
                    $(advIntProgramId).val(0);
                }
                this.focus();

            } else {
                if (ui.item) {
                    CheckDuplicateAdvIntermediaProgram(id, ui.item.Id, ui.item.url);
                   // $(advIntProgramId).val(ui.item.Id);
                }
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
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };

    $(this).autocomplete("search");
});
$(document).delegate("input:text[id^='SeachLevelTeam']", "focusin", function () {
    $(this).autocomplete({
        autoFocus: true,
        source: function (request, response) {
            var seachLevel1TeamsRecords = new Array();
            var urlSeachLevel1Team = baseUrlString + "/Reporting/GetSeachLevel1Team";
            jQuery.ajax({
                type: "POST",
                url: urlSeachLevel1Team,
                async: false,
                data: {
                    serachItem: request.term,
                    guidRecord: "",
                    teamId: $("#TeamId").val()
                },
                success: function (data) {

                    TotalSeachLevel1TeamRecords = [];
                    jQuery.each(data, function (i, levelTeam) {
                        var levelTeamRecord = {
                            TeamId: levelTeam.TeamId,
                            TeamName: levelTeam.TeamName,
                            EmailId: levelTeam.EmailId,
                            PhoneNumber: levelTeam.PhoneNumber,
                            Users: levelTeam.Users,
                            Premium: levelTeam.Premium
                        };
                        TotalSeachLevel1TeamRecords.push(levelTeamRecord);

                    });
                },
                error: function (result) {
                    alert('SeachLevelTeam');
                    alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
                }
            });
            response($.map(TotalSeachLevel1TeamRecords, function (item) {
                return {
                    label: item.TeamName,
                    value: item.TeamName,
                    url: item.TeamName,
                    Id: item.TeamId,
                    Users: item.Users,
                    Premium: item.Premium
                };
            }));
            TotalSeachLevel1TeamRecords = [];
        },
        minLength: 0,
        select: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');

            jQuery("#spnLevelTeam" + id).html("");
            if (!ui.item) {
                $("#LevelTeamId" + id).val(0);
            } else {
                var selectedLevelTeamId = parseInt(ui.item.Id);
                $("#LevelTeamId" + id).val(selectedLevelTeamId);

            }
        },
        change: function (event, ui) {
            var id = $(this).attr('id').replace(/[^0-9]/g, '');
            if (!ui.item) {
                this.value = '';
                jQuery("#spnLevelTeam" + id).html("No Level Team found for the selected values.");
                if (!ui.item) {
                    $("#LevelTeamId" + id).val(0);
                }
                this.focus();

            } else {
                jQuery("#spnLevelTeam" + id).html("");
                if (ui.item) {
                    var selectedLevelTeamId = parseInt(ui.item.Id);
                    $("#LevelTeamId" + id).val(selectedLevelTeamId);
                }
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
    }).data('ui-autocomplete')._renderItem = function (ul, item) {
        return $('<li>')
            .data('item.autocomplete', item)
            .append('Name-' + item.label + ' &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;  ' + '&nbsp; &nbsp; &nbsp;&nbsp;  Users-' + item.Users + ' &nbsp; &nbsp;&nbsp;&nbsp;    Premiums-' + item.Premium + '&nbsp; &nbsp;&nbsp;&nbsp;    <a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
            .appendTo(ul);
    };
    $(this).autocomplete("search");
});

$("#AddLevel1Team").click(function () {      
    var urlAddLevel1Team = baseUrlString + "/Reporting/AddLevelTeam";      
    var SeachLevelTeam1 = $("#SeachLevelTeam1").val();
    if (SeachLevelTeam1 !== undefined && SeachLevelTeam1 !== null && SeachLevelTeam1 !== "") {
        jQuery.ajax({
            type: "POST",
            url: urlAddLevel1Team,
            data: {
                primaryTeamId: $("#TeamId").val(),
                GuidRecord: "NA",
                teamId: $("#LevelTeamId1").val(),
                teamName: $("#SeachLevelTeam1").val(),
                levelTypeId: 1
            },
            success: function (data) {
                $("table#tblLevel1Team tbody").append(data);
                $("#SeachLevelTeam1").val("");
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
    else {
        alert('Please first select level 1 team');
    }
    return false;
});
$("#AddLevel2Team").click(function () {    
    var urlAddLevel2Team = baseUrlString + "/Reporting/AddLevelTeam";
    var SeachLevelTeam2 = $("#SeachLevelTeam2").val();
    if (SeachLevelTeam2 !== undefined && SeachLevelTeam2 !== null && SeachLevelTeam2 !== "") {
        jQuery.ajax({
            type: "POST",
            url: urlAddLevel2Team,
            data: {
                primaryTeamId: $("#TeamId").val(),
                GuidRecord: "NA",
                teamId: $("#LevelTeamId2").val(),
                teamName: $("#SeachLevelTeam2").val(),
                levelTypeId: 2
            },
            success: function (data) {
                $("table#tblLevel2Team tbody").append(data);
                $("#SeachLevelTeam2").val("");
            },
            error: function (result) {
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
    else {
        alert('Please first select level 2 team');
    }
    return false;
});
$("table").on("click", ".DeleteLevelTeam", function () {
    var levelTeamId = $(this).prev('input').val();
    var urlDeleteTeam = baseUrlString + "/Reporting/DeleteLevelTeam";
    jQuery.ajax({
        type: "Get",
        url: urlDeleteTeam + "/" + levelTeamId,       
        success: function (data) {                     
        },
        error: function (result) {
            alert('SeachLevelTeam');
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });
    $(this).closest('tr').hide();
    $(this).prev('input').val('true');
    return false;
});
jQuery(function () {
    var urlGetLevel1Team = baseUrlString + "/Reporting/GetLevelTeams";
    jQuery.ajax({
        type: "POST",
        url: urlGetLevel1Team,
        data: {
            primaryTeamId: $("#TeamId").val(),
            GuidRecord: "NA",
            levelTypeId: 1
        },
        success: function (data) {
            $("table#tblLevel1Team tbody").append(data);
        }        ,
        error: function (result) {          
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });

});

jQuery(function () {  
    var urlGetLevel1Team = baseUrlString + "/Reporting/GetLevelTeams";
    jQuery.ajax({
        type: "POST",
        url: urlGetLevel1Team,
        data: {
            primaryTeamId: $("#TeamId").val(),
            GuidRecord: "NA",
            levelTypeId: 2
        },
        success: function (data) {
            $("table#tblLevel2Team tbody").append(data);
        },
        error: function (result) {           
                alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
    });

});

$('input[type=radio][name=CommissionRateUnit]').change(function () {
    if (this.value == 'Dollars') {
        alert("Dollar");
    }
    else if (this.value == 'Cents') {
        alert("Cent");
    }
});