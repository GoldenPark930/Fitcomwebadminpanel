var TotalUploadedFileNames = new Array();
$(document).ready(function () {
    $(".UploadProgress").hide();
    //$('#SearchExercise').keyup(function (e) {
    //    if (this.value.length >= 2 || e.keyCode == 13 || e.which == 13 || e.keyCode == 8) {
    //        var searchItem = $(this).val();
    //        if (searchItem !== undefined && searchItem !== null) {
    //            var item = searchItem;
    //            var urlchallenge = baseUrlString + "/Exercise/SearchExercise";
    //            $.get(urlchallenge + "/" + item, function (data) {
    //                $('#exercises_1').replaceWith(data);
    //                if (data) {
    //                }
    //            });
    //        }
    //    }
    //});

    $(document).delegate("select[id^='Status_']", "change", function (e) {
        var str = $(this).attr("id").split("_");
        id = str[1];
        var flag = confirm('Are you sure to change status ?');
        if (id != "" && flag) {
            var urlchallenge = baseUrlString + "/Exercise/ChangeExerciseStatus";
            var request = this;
            var exercisedata = {
                execiseId: id,
                operationId: parseInt($(request).val())
            };
            var ischanged = false;
            $.ajax({
                type: "POST",
                url: urlchallenge,
                data: exercisedata,
                async: false,
                dataType: "json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.OperationId == 3) {
                        if (data.OperationResultId == -1) {
                            ischanged = true;
                            alert("Exercise has associated and changed into Inactive status!");
                        }
                        else if (data.OperationResultId == 1) {
                            $("#Status_" + id).parents("tr").remove();
                            alert("Exercise has not associated and deleted successfully!");
                        }
                    }
                }
            });
            if (ischanged) {
                $("#Status_" + id).val(2);
            }
        }
    });
   
    $('#btnUpload').click(function () {
        $(".UploadProgress").show();
        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {
            var fileUpload = $("#uploadExerciseFile").get(0);
            var files = fileUpload.files;
            // Create FormData object
            var fileData = new FormData();
            // Looping over all files and add it to FormData object
            TotalUploadedFileNames = [];
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
                TotalUploadedFileNames.push(files[i].name);
            }
            var IsExcelExist = false;
            jQuery.each(TotalUploadedFileNames, function (i, val) {
                if (val.indexOf('.csv') > -1  || val.indexOf('.xlsx') > -1  || val.indexOf('.xls') > -1 ) {
                    IsExcelExist = true;
                    return false;
                }               
            });
            
            if (IsExcelExist == true) {
                var urlchallenge = baseUrlString + "/Exercise/UploadExerciseVideosAndExcel";
                $.ajax({
                    url: urlchallenge,
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        $(".UploadProgress").hide();
                        $("#uploadExerciseFile").val('');
                        alert(result.Message);
                    },
                    error: function (err) {
                        $(".UploadProgress").hide();
                        alert(err.statusText);
                        return false;
                    }
                });
            } else {
                $(".UploadProgress").hide();
                alert("Please Upload one Excel or CSV file");
                return false;
            }
        } else {
            alert("FormData is not supported.");
            return false;
        }
    });

    $('#btnExcelUpload').click(function () {
        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {
            var fileUpload = $("#fileExcelUpload").get(0);
            var files = fileUpload.files;
            // Create FormData object
            var fileData = new FormData();
            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            var urlchallenge = baseUrlString + "/Exercise/UploadExcel";
            $.ajax({
                url: urlchallenge,
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {
                    alert(result);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("Update your broswer");
        }
    });
    $('#ShowFailUploadHistory').click(function () {

        $.get('FailUploadHistory', function (data) {
            $('#exercisesFailUpload_1').replaceWith(data);
            if (data) {
                $("#exercisesFailUpload_1").val("");
            }
        });
    });
});




