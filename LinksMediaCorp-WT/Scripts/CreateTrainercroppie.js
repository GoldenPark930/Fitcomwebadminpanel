var CreateTrainer = (function () {
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
        }
        $('#myform').submit();
        $uploadCrop = null;
        reader = null;
        return true;
    }
    function TrainerPhotoUpload() {
        var $uploadCrop = null;
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
                    $('#existingTrainertImgConatiner').hide();
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                //swal("Sorry - you're browser doesn't support the FileReader API");
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
        $('#btnSubmit,#btntopSubmit').on('click', function (ev) {
            $uploadCrop.croppie('result', {
                type: 'canvas',
                size: 'original'
            }).then(function (resp) {
                popupResult({
                    src: resp
                });
            });
            if (reader == null || reader == 'undefined') {
                $('#myform').submit();
                $uploadCrop = null;
                reader = null;
            }
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
