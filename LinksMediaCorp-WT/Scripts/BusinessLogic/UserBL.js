
//Check username from database
jQuery(function () {
   jQuery('#UserName').blur(function () {
        var url = baseUrlString + "/Reporting/GetUserRecord";
        var name =jQuery('#UserName').val();
       jQuery.get(url, { input: name }, function (data) {
            if (data == "Available") {
               jQuery("#resultUser").html("<span style='color:red'>User already exist!</span>");
               jQuery("#UserName").css('background-color', '#e97878');
               jQuery('#CheckUser').val('NotPresent');
               jQuery('#CheckUser').val('NotPresent');
               jQuery("#emailValidation").html(null);
               jQuery("#emailValidation").text("");
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
    $('#EmailId').blur(function () {      
        var name;
        var url = baseUrlString + "/Reporting/GetEmailRecord";       
        if ($('#UserId').val() === undefined) {
            name = $('#EmailId').val();
        }
        else{           
            name = $('#EmailId').val() + ',' + $('#UserId').val();
        }

        $.get(url, { input: name }, function (data) {
            if (data == "Available") {
               
                $("#resultEmail").html("<span style='color:red'>Email already exist!</span>");
                $("#EmailId").css('background-color', '#e97878');
                $('#CheckEmail').val('NotAvailable');
                $("#emailValidation").html(null);
            }
            else {
                $("#resultEmail").html(null);
                $("#EmailId").css('background-color', '');
                $('#CheckEmail').val(null);              
            }
        });
    })
    jQuery("#Gender").keyup(function (event) {
        if (!isNaN(this.value)) {
            this.value = "";
            alert('Please enter numeric value.');
        }
    });
});
jQuery(function () {
    $(document).load(function () {
        if ($('#CheckEmail').val() != null) {           
            $("#resultEmail").html("<span style='color:red'>Email already exist!</span>");
            $("#emailValidation").html(null);
        }
        else {
            $("#resultEmail").html(null);
        }    
    })
});




