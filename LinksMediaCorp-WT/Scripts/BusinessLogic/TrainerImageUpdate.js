var TrainerImagUpdate = (function () {
    function popupResult(result) {
        if (result.src) {
            $('#CropImageRowData').val(result.src);
        }
        $('#myform').submit();
        $uploadCrop = null;
        reader = null;
        return true;
    }
    function TrainerPhotoUpload() {
        var isCroped = false;
        var $uploadCrop=null;
        var reader = null;
        function readFile(input) {
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    $uploadCrop.croppie('bind', {
                        url: e.target.result
                    });
                    $('.upload-demo').addClass('ready');
                    $("#existingTrainertImg").hide();
                    $('.fileupload fileupload-new col-md-10').hide();
                    $("#cropTrainerbtn").hide();
                    $('#existingTrainertImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
              //  swal("Sorry - you're browser doesn't support the FileReader API");
            }
        }
        $uploadCrop = $('#upload-demo').croppie({
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

        $('#upload').on('change', function () { readFile(this); });
        $('#btnSubmit').on('click', function (ev) {
            $uploadCrop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupResult({
                    src: resp
                });
            });
            if (!isCroped && (reader == null || reader == 'undefined')) {
                $('#myform').submit();
                $uploadCrop = null;
                reader = null;
            }

        });
        $("#cropTrainerbtn").on("click", function (e) {
            isCroped = true;
            var imagesrc = $("#existingTrainertImg").attr("src");
            $("#existingTrainertImgConatiner").hide();
            if (imagesrc !== 'undefined' || imagesrc !== '' || imagesrc !== null) {
                $uploadCrop.croppie('bind', imagesrc);
                $('.upload-demo').addClass('ready');
            }
            return false;
        });
        reader = null;
    }
    function init() {
        TrainerPhotoUpload();
    }
    return {
        init: init
    };
})();

jQuery("[id*=ZipCode]").keyup(function () {
    if (isNaN(this.value)) {
        this.value = "";
        alert('Please enter numeric value.');
    }
});
