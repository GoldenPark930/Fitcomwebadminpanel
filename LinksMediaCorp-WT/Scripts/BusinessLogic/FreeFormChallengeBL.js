//global variable declaration
var dataUnit = "";
var Serach1Equipment = [];
var Serach1ExerciseType = [];
var Serach1TrainingZone = [];
var AllSerachedData = [];
var totalFilterexerciseRecords = new Array();
var totalexerciseRecords = new Array();
var isSerached = false;
//var baseUrlString = "/LinksMediaCorp-WTNew";
//hide and show exercises on page load on the basis of challenge type
jQuery(function () {
    ShowHideExecise();
});
jQuery(function () {

    $('#chkSearchEquipment').multiselect({
        buttonText: function (options, select) {
            return 'Select Equipment';
        },
        enableHTML: true,
        numberDisplayed: 2,
        maxHeight: 100,
        buttonWidth: '160px',
        dropRight: true

    });

    $('#chkTrainingZone').multiselect({

        buttonText: function (options, select) {
            return 'Select Training Zone';
        },
        enableHTML: true,
        maxHeight: 100,
        numberDisplayed: 2,
        buttonWidth: '160px',
        dropRight: true

    });

    $('#chkSearchExerciseType').multiselect({
        buttonText: function (options, select) {
            return 'Select Exercise Type';
        },
        enableHTML: true,
        maxHeight: 100,
        numberDisplayed: 2,
        buttonWidth: '160px',
        dropRight: true

    });

    totalexerciseRecords = [];
    var urlexe = baseUrlString + "/Reporting/GetAllExercise";
    $.ajax({
        url: urlexe,
        success: function (data) {
            totalexerciseRecords = data;
        }
    });
    var urlSeachQuipment = baseUrlString + "/Reporting/GetAllEquipment";

    var selectIndex = []
    if (jQuery('#ExeIndexLink1')) {
        var selectedExeIndexLink = $('#ExeIndexLink1').val();
        if (selectedExeIndexLink != null && selectedExeIndexLink != "") {
            var list = selectedExeIndexLink.split(",");
            list.forEach(function (i) {
                selectIndex.push(i);
            });
        }
    }

    if (jQuery('#ExeIndexLink2')) {
        var selectedExeIndexLink = $('#ExeIndexLink2').val();
        if (selectedExeIndexLink != null && selectedExeIndexLink != "") {
            var list = selectedExeIndexLink.split(",");
            list.forEach(function (i) {
                selectIndex.push(i);
            });
        }

    }
    if (jQuery('#ExeIndexLink3')) {
        var selectedExeIndexLink = $('#ExeIndexLink3').val();
        if (selectedExeIndexLink != null && selectedExeIndexLink != "") {
            var list = selectedExeIndexLink.split(",");
            list.forEach(function (i) {
                selectIndex.push(i);
            });
        }

    }
    if (jQuery('#ExeIndexLink4')) {
        var selectedExeIndexLink1 = $('#ExeIndexLink4').val();
        if (selectedExeIndexLink != null && selectedExeIndexLink != "") {
            var list = selectedExeIndexLink.split(",");
            list.forEach(function (i) {
                selectIndex.push(i);
            });
        }

    }

    jQuery.ajax({
        url: urlSeachQuipment,
        dataType: "json",
        beforeSend: function () {
            // alert(id);
        },
        success: function (data) {
            $('#chkSearchEquipment').prop('disabled', false);
            jQuery('#chkSearchEquipment').css('background-color', '#FFFFFF');
            //var items = '<option  value="">Please select Equipment</option>';
            var items = "";
            jQuery.each(data, function (i, item) {
                var indexexist = false;
                for (var i = 0; i < selectIndex.length; i++) {
                    if (selectIndex[i] == item.Equipment) {
                        indexexist = true;
                        break;
                    }
                }
                if (indexexist) {
                    items += "<option selected=\"selected\" value=\"" + item.Equipment + "\">" + item.Equipment + "</option>";
                    totalFilterexerciseRecords = [];
                    AllSerachedData = [];
                    Search1EquipmentChange();
                    Search1TrainingZoneChange();
                    Search1ExerciseTypeChange();
                    Search1EquipmentChangeSerch();
                    isSerached = true;
                } else {
                    items += "<option value=\"" + item.Equipment + "\">" + item.Equipment + "</option>";
                }
            });
            jQuery('#chkSearchEquipment').html(items);
            $('#chkSearchEquipment').multiselect('rebuild');
            $('#chkSearchEquipment').multiselect("refresh");


        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });
    var urlSeachtrainingZone = baseUrlString + "/Reporting/GetAllTrainingZone";
    jQuery.ajax({
        url: urlSeachtrainingZone,
        dataType: "json",
        beforeSend: function () {
            //  alert(urlSeachtrainingZone);
        },
        success: function (data) {
            $('#chkTrainingZone').prop('disabled', false);
            jQuery('#chkTrainingZone').css('background-color', '#FFFFFF');
            var items = "";
            //   '<option  value="">Please Training Zone</option>';
            jQuery.each(data, function (i, item) {
                var indexexist = false;
                for (var i = 0; i < selectIndex.length; i++) {
                    if (selectIndex[i] == item.PartName) {
                        indexexist = true;
                        break;
                    }
                }
                if (indexexist) {

                    items += "<option selected=\"selected\" value=\"" + item.PartName + "\">" + item.PartName + "</option>";
                    totalFilterexerciseRecords = [];
                    AllSerachedData = [];
                    Search1EquipmentChange();
                    Search1TrainingZoneChange();
                    Search1ExerciseTypeChange();
                    Search1trainingZoneSerch();
                    isSerached = true;
                } else {
                    items += "<option value=\"" + item.PartName + "\">" + item.PartName + "</option>";
                }

            });
            jQuery('#chkTrainingZone').html(items);
            $('#chkTrainingZone').multiselect('rebuild');
            $('#chkTrainingZone').multiselect("refresh");



        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });

    var urlSeachExerise = baseUrlString + "/Reporting/GetAllExerciseType";
    jQuery.ajax({
        url: urlSeachExerise,
        dataType: "json",
        beforeSend: function () {
            //alert(urlSeachExerise);
        },
        success: function (data) {
            $('#chkSearchExerciseType').prop('disabled', false);
            jQuery('#chkSearchExerciseType').css('background-color', '#FFFFFF');
            var items = "";
            //   '<option  value="">Select Exercise Type</option>';
            jQuery.each(data, function (i, item) {

                var indexexist = false;
                for (var i = 0; i < selectIndex.length; i++) {
                    if (selectIndex[i] == item.ExerciseName) {
                        indexexist = true;
                        break;
                    }
                }
                if (indexexist) {
                    items += "<option selected=\"selected\" value=\"" + item.ExerciseName + "\">" + item.ExerciseName + "</option>";
                    totalFilterexerciseRecords = [];
                    AllSerachedData = [];
                    Search1EquipmentChange();
                    Search1TrainingZoneChange();
                    Search1ExerciseTypeChange();
                    Search1ExerciseTypeSerch();
                    isSerached = true;
                } else {
                    items += "<option value=\"" + item.ExerciseName + "\">" + item.ExerciseName + "</option>";
                }

            });
            jQuery('#chkSearchExerciseType').html(items);
            $('#chkSearchExerciseType').multiselect('rebuild');
            $('#chkSearchExerciseType').multiselect("refresh");


        },
        error: function (result) {
            alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
        }
    });


});


//**********Autopcomplete with url clickable ends for exercise 6*******
//validation and text change on challeneg sub-type change
jQuery(function () {
    jQuery('#toggalediv').show();
    jQuery('#ExeName2').prop('disabled', true);
    jQuery('#ExeDesc2').prop('disabled', true);
    jQuery('#ExeName3').prop('disabled', true);
    jQuery('#ExeDesc3').prop('disabled', true);
    jQuery('#ExeName4').prop('disabled', true);
    jQuery('#ExeDesc4').prop('disabled', true);
    jQuery('#ExeName5').prop('disabled', true);
    jQuery('#ExeDesc5').prop('disabled', true);
    jQuery('#ExeName6').prop('disabled', true);
    jQuery('#ExeDesc6').prop('disabled', true);
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

            var exercise1filterData = [];
            if (totalFilterexerciseRecords.length > 0 || isSerached == true) {
                exercise1filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });

            } else {
                exercise1filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise1filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink1").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
            jQuery("#lblExe1").html("");
            jQuery('#ExeName2').prop('disabled', false);
            jQuery('#ExeDesc2').prop('disabled', false);
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe1").html("Please select exercise name from list!");
                jQuery("#lblExe2").html("");
                jQuery("#lblExe3").html("");
                jQuery("#lblExe4").html("");
                jQuery("#lblExe5").html("");
                jQuery("#lblExe6").html("");
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
                jQuery('#ExeName5').val("");
                jQuery('#ExeDesc5').val("");
                jQuery('#ExeName5').prop('disabled', true);
                jQuery('#ExeDesc5').prop('disabled', true);
                jQuery('#ExeName6').val("");
                jQuery('#ExeDesc6').val("");
                jQuery('#ExeName6').prop('disabled', true);
                jQuery('#ExeDesc6').prop('disabled', true);
                jQuery("#ExeVideoLink1").html("");
                jQuery("#ExeVideoLink2").html("");
                jQuery("#ExeVideoLink3").html("");
                jQuery("#ExeVideoLink4").html("");
                jQuery("#ExeVideoLink5").html("");
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(1);
                this.focus();

            } else {
                jQuery("#lblExe1").html("");
                jQuery('#ExeName2').prop('disabled', false);
                jQuery('#ExeDesc2').prop('disabled', false);
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

    //**********Autopcomplete with url clickable ends for exercise 1*******


    //**********Autopcomplete with url clickable starts for exercise 2*******

    $('#ExeName2').autocomplete({
        autoFocus: true,
        source: function (request, response) {
            //$.ajax({
            //    url: urlexe,
            //    data: { term: request.term },
            //    success: function (data) {
            //        response($.map(data, function (item) {
            //            //console.log(item)
            //            return {
            //                label: item.ExerciseName,
            //                value: item.ExerciseName,
            //                url: item.VedioLink
            //            };
            //        }));
            //    }
            //});

            //var exercise2filterData = jQuery.grep(totalexerciseRecords, function (a) {
            //    return a.Index.indexOf(request.term) > -1;
            //});

            var exercise2filterData = [];
            if (totalFilterexerciseRecords.length > 0 || isSerached) {
                exercise2filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            } else {
                exercise2filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise2filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink2").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
            jQuery("#lblExe2").html("");
            jQuery('#ExeName3').prop('disabled', false);
            jQuery('#ExeDesc3').prop('disabled', false);
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe2").html("Please select exercise name from list!");
                jQuery("#lblExe3").html("");
                jQuery("#lblExe4").html("");
                jQuery("#lblExe5").html("");
                jQuery("#lblExe6").html("");
                jQuery('#ExeName3').val("");
                jQuery('#ExeDesc3').val("");
                jQuery('#ExeName3').prop('disabled', true);
                jQuery('#ExeDesc3').prop('disabled', true);
                jQuery('#ExeName4').val("");
                jQuery('#ExeDesc4').val("");
                jQuery('#ExeName4').prop('disabled', true);
                jQuery('#ExeDesc4').prop('disabled', true);
                jQuery('#ExeName5').val("");
                jQuery('#ExeDesc5').val("");
                jQuery('#ExeName5').prop('disabled', true);
                jQuery('#ExeDesc5').prop('disabled', true);
                jQuery('#ExeName6').val("");
                jQuery('#ExeDesc6').val("");
                jQuery('#ExeName6').prop('disabled', true);
                jQuery('#ExeDesc6').prop('disabled', true);
                jQuery("#ExeVideoLink2").html("");
                jQuery("#ExeVideoLink3").html("");
                jQuery("#ExeVideoLink4").html("");
                jQuery("#ExeVideoLink5").html("");
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(1);
                this.focus();

            } else {

                jQuery("#lblExe2").html("");
                jQuery('#ExeName3').prop('disabled', false);
                jQuery('#ExeDesc3').prop('disabled', false);
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

    //**********Autopcomplete with url clickable ends for exercise 2*******


    //**********Autopcomplete with url clickable starts for exercise 3*******

    $('#ExeName3').autocomplete({
        autoFocus: true,
        source: function (request, response) {
            //$.ajax({
            //    url: urlexe,
            //    data: { term: request.term },
            //    success: function (data) {
            //        response($.map(data, function (item) {
            //            //console.log(item)
            //            return {
            //                label: item.ExerciseName,
            //                value: item.ExerciseName,
            //                url: item.VedioLink
            //            };
            //        }));
            //    }
            //});
            //var exercise3filterData = jQuery.grep(totalexerciseRecords, function (a) {
            //    return a.Index.indexOf(request.term) > -1;
            //});
            var exercise3filterData = [];

            if (totalFilterexerciseRecords.length > 0 || isSerached) {
                exercise3filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            } else {
                exercise3filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise3filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink3").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
            jQuery("#lblExe3").html("");
            jQuery('#ExeName4').prop('disabled', false);
            jQuery('#ExeDesc4').prop('disabled', false);
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe3").html("Please select exercise name from list!");
                jQuery("#lblExe4").html("");
                jQuery('#ExeName4').val("");
                jQuery('#ExeDesc4').val("");
                jQuery('#ExeName4').prop('disabled', true);
                jQuery('#ExeDesc4').prop('disabled', true);
                jQuery('#ExeName5').val("");
                jQuery('#ExeDesc5').val("");
                jQuery('#ExeName5').prop('disabled', true);
                jQuery('#ExeDesc5').prop('disabled', true);
                jQuery('#ExeName6').val("");
                jQuery('#ExeDesc6').val("");
                jQuery('#ExeName6').prop('disabled', true);
                jQuery('#ExeDesc6').prop('disabled', true);
                jQuery("#ExeVideoLink3").html("");
                jQuery("#ExeVideoLink4").html("");
                jQuery("#ExeVideoLink5").html("");
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(1);
                this.focus();

            } else {
                jQuery("#lblExe3").html("");
                jQuery('#ExeName4').prop('disabled', false);
                jQuery('#ExeDesc4').prop('disabled', false);
                if (dataUnit == "Interval" || dataUnit == "Rounds") {
                    GetFractionList(2);
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
    })
        .data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };

    //**********Autopcomplete with url clickable ends for exercise 3*******


    //**********Autopcomplete with url clickable starts for exercise 4*******

    $('#ExeName4').autocomplete({
        autoFocus: true,
        source: function (request, response) {
            //$.ajax({
            //    url: urlexe,
            //    data: { term: request.term },
            //    success: function (data) {
            //        response($.map(data, function (item) {
            //            //console.log(item)
            //            return {
            //                label: item.ExerciseName,
            //                value: item.ExerciseName,
            //                url: item.VedioLink
            //            };
            //        }));
            //    }
            //});

            var exercise4filterData = [];
            if (totalFilterexerciseRecords.length > 0 || isSerached) {
                exercise4filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            } else {
                exercise4filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise4filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink4").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe4").html("Please select exercise name from list!");
                jQuery('#ExeName5').val("");
                jQuery('#ExeDesc5').val("");
                jQuery('#ExeName5').prop('disabled', true);
                jQuery('#ExeDesc5').prop('disabled', true);
                jQuery('#ExeName6').val("");
                jQuery('#ExeDesc6').val("");
                jQuery('#ExeName6').prop('disabled', true);
                jQuery('#ExeDesc6').prop('disabled', true);

                jQuery("#ExeVideoLink4").html("");
                jQuery("#ExeVideoLink5").html("");
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(2);
                this.focus();

            } else {
                jQuery("#lblExe4").html("");
                jQuery('#ExeName5').prop('disabled', false);
                jQuery('#ExeDesc5').prop('disabled', false);
                if (dataUnit == "Interval" || dataUnit == "Rounds") {
                    GetFractionList(3);
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
    })
        .data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };

    //**********Autopcomplete with url clickable ends for exercise 4*******

    //**********Autopcomplete with url clickable starts for exercise 5*******

    $('#ExeName5').autocomplete({
        autoFocus: true,
        source: function (request, response) {


            var exercise5filterData = [];
            if (totalFilterexerciseRecords.length > 0 || isSerached) {
                exercise5filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            } else {
                exercise5filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise5filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink5").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe5").html("Please select exercise name from list!");
                jQuery('#ExeName6').val("");
                jQuery('#ExeDesc6').val("");
                jQuery('#ExeName6').prop('disabled', true);
                jQuery('#ExeDesc6').prop('disabled', true);
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(2);
                this.focus();

            } else {
                jQuery("#lblExe5").html("");
                jQuery('#ExeName6').prop('disabled', false);
                jQuery('#ExeDesc6').prop('disabled', false);
                if (dataUnit == "Interval" || dataUnit == "Rounds") {
                    GetFractionList(3);
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
    })
        .data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };

    //**********Autopcomplete with url clickable ends for exercise 5*******


    //**********Autopcomplete with url clickable starts for exercise 6*******

    $('#ExeName6').autocomplete({
        autoFocus: true,
        source: function (request, response) {

            var exercise6filterData = [];
            if (totalFilterexerciseRecords.length > 0 || isSerached) {
                exercise6filterData = jQuery.grep(totalFilterexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            } else {
                exercise6filterData = jQuery.grep(totalexerciseRecords, function (a) {
                    var ExerciseName = a.ExerciseName.toUpperCase();
                    return ExerciseName.indexOf(request.term.toUpperCase()) > -1;
                });
            }

            response($.map(exercise6filterData, function (item) {
                //console.log(item)                       
                return {
                    label: item.ExerciseName,
                    value: item.ExerciseName,
                    url: item.VedioLink
                };
            }));
        },
        minLength: 0,
        select: function (event, ui) {
            jQuery("#ExeVideoLink6").html('<a href="' + ui.item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:left;text-decoration:underline;">' + ui.item.url + '</a>');
        },
        change: function (event, ui) {
            if (!ui.item) {
                this.value = '';
                jQuery("#lblExe6").html("Please select exercise name from list!");
                jQuery("#ExeVideoLink6").html("");
                GetFractionList(2);
                this.focus();

            } else {
                jQuery("#lblExe6").html("");
                if (dataUnit == "Interval" || dataUnit == "Rounds") {
                    GetFractionList(3);
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
    })
        .data('ui-autocomplete')._renderItem = function (ul, item) {
            return $('<li>')
                .data('item.autocomplete', item)
                .append(item.label + '<a href="' + item.url + '" target="_blank" style="font-size:10px;color:blue;position:relative; float:right;text-decoration:underline;">' + item.url + '</a>')
                .appendTo(ul);
        };
});


//Bind challenge sub-type on challenge type change


jQuery(function () {

    jQuery('#toggalediv').load(function () {
        jQuery.validator.defaults.ignore = ":hidden";
    });

    jQuery('#IsActive').load(function () {
        if (this.checked) {
            jQuery('#divqueue').show();
        } else {
            jQuery('#divqueue').hide();
        }
    });

    jQuery('#IsActive').change(function () {
        if (this.checked) {
            jQuery('#divqueue').show();
        } else {
            jQuery('#divqueue').hide();
        }
    });

});
//function for show hide exercise block and variavle value parameteres
function ShowHideExecise() {
    jQuery('#toggalediv').show();
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

//function for manage weight for man and woman and reps
function ManageReps(resultUnit, variableUnit) {
    $('#reps-level').show();
    $('#weight-level').show();
    $('.reps-in-box').show();
}



function Search1TrainingZoneChange() {
    // var selectedItem = [];
    $('#chkTrainingZone :selected').each(function (i, selected) {
        // selectedItem[i] = $(selected).text();
        var serchItem = $(selected).text();
        AllSerachedData.push(serchItem.toString());
    });

}

function Search1ExerciseTypeChange() {
    // var selectedItem = [];
    $('#chkSearchExerciseType :selected').each(function (i, selected) {
        //selectedItem[i] = $(selected).text();
        var serchItem = $(selected).text();
        AllSerachedData.push(serchItem.toString());
    });

}

function Search1EquipmentChange() {
    // var selectedItem = [];
    $('#chkSearchEquipment :selected').each(function (i, selected) {
        //  selectedItem[i] = $(selected).text();
        var serchItem = $(selected).text();
        AllSerachedData.push(serchItem.toString());
    });

}
function Search1EquipmentChangeSerch() {
    totalFilterexerciseRecords = [];
    var equipmentexercise1Records = jQuery.grep(totalexerciseRecords, function (a) {
        var isfind = false;
        for (var icounter = 0; icounter < AllSerachedData.length; icounter++) {
            var searchiitem = AllSerachedData[icounter];
            var item = a.Index.split(',');
            var isfound = false;
            for (var ii = 0; ii < item.length; ii++) {
                if (item[ii] === searchiitem) {
                    isfound = true;
                    break;
                } else {
                    isfound = false;
                }
            }
            isfind = isfound;
            if (!isfound) {
                break;
            }
        }

        if (isfind) {

            return true
        } else {
            return false;
        }
    });
    $.extend(totalFilterexerciseRecords, equipmentexercise1Records);
}
function Search1trainingZoneSerch() {
    totalFilterexerciseRecords = [];
    var trainingZoneExerciseRecords = jQuery.grep(totalexerciseRecords, function (a) {
        var isfind = false;
        //  AllSerachedData.forEach(function (i) {
        for (var icounter = 0; icounter < AllSerachedData.length; icounter++) {
            var searchiitem = AllSerachedData[icounter];
            var item = a.Index.split(',');
            var isfound = false;
            for (var ii = 0; ii < item.length; ii++) {
                if (item[ii] === searchiitem) {
                    isfound = true;
                    break;
                } else {
                    isfound = false;
                }
            }
            isfind = isfound;
            if (!isfound) {
                break;
            }
        }

        if (isfind) {

            return true
        } else {
            return false;
        }
    });

    $.extend(totalFilterexerciseRecords, trainingZoneExerciseRecords);
}
function Search1ExerciseTypeSerch() {
    totalFilterexerciseRecords = [];
    var FilterexerciseRecords = jQuery.grep(totalexerciseRecords, function (a) {
        var isfind = false;
        for (var icounter = 0; icounter < AllSerachedData.length; icounter++) {
            var searchiitem = AllSerachedData[icounter];
            var item = a.Index.split(',');
            var isfound = false;
            for (var ii = 0; ii < item.length; ii++) {
                if (item[ii] === searchiitem) {
                    isfound = true;
                    break;
                } else {
                    isfound = false;
                }
            }
            isfind = isfound;
            if (!isfound) {
                break;
            }
        }

        if (isfind) {

            return true
        } else {
            return false;
        }
    });
    $.extend(totalFilterexerciseRecords, FilterexerciseRecords);
}
jQuery(function () {
    $("#chkSearchEquipment").on("change", function () {
        AllSerachedData = [];
        Search1EquipmentChange();
        Search1TrainingZoneChange();
        Search1ExerciseTypeChange();
        Search1EquipmentChangeSerch();
        isSerached = true;
        $('#ExeName1').trigger("focus");

    });

    $("#chkTrainingZone").on("change", function () {

        AllSerachedData = [];
        Search1EquipmentChange();
        Search1TrainingZoneChange();
        Search1ExerciseTypeChange();
        Search1trainingZoneSerch();
        isSerached = true;
        $('#ExeName1').trigger("focus");

    });

    $("#chkSearchExerciseType").on("change", function () {

        AllSerachedData = [];
        Search1EquipmentChange();
        Search1TrainingZoneChange();
        Search1ExerciseTypeChange();
        Search1ExerciseTypeSerch();
        isSerached = true;
        $('#ExeName1').trigger("focus");

    });


});