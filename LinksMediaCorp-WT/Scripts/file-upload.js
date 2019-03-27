

! function (e) {
    
    var t = function (t, n) {
        this.$element = e(t), this.type = this.$element.data("uploadtype") || (this.$element.find(".thumbnail").length > 0 ? "image" : "file"), this.$input = this.$element.find(":file");
        if (this.$input.length === 0) return;
        this.name = this.$input.attr("name") || n.name, this.$hidden = this.$element.find('input[type=hidden][name="' + this.name + '"]'), this.$hidden.length === 0 && (this.$hidden = e('<input type="hidden" />'), this.$element.prepend(this.$hidden)), this.$preview = this.$element.find(".fileupload-preview");
        var r = this.$preview.css("height");
        this.$preview.css("display") != "inline" && r != "0px" && r != "none" && this.$preview.css("line-height", r), this.original = {
            exists: this.$element.hasClass("fileupload-exists"),
            preview: this.$preview.html(),
            hiddenVal: this.$hidden.val()
        }, this.$remove = this.$element.find('[data-dismiss="fileupload"]'), this.$element.find('[data-trigger="fileupload"]').on("click.fileupload", e.proxy(this.trigger, this)), this.listen()
    };
    t.prototype = {
        listen: function () {
         
            this.$input.on("change.fileupload", e.proxy(this.change, this)), e(this.$input[0].form).on("reset.fileupload", e.proxy(this.reset, this)), this.$remove && this.$remove.on("click.fileupload", e.proxy(this.clear, this))
        },
        change: function (e, t) {
          
            if (t === "clear") return;
            var n = e.target.files !== undefined ? e.target.files[0] : e.target.value ? {
                name: e.target.value.replace(/^.+\\/, "")
            } : null;
            if (!n) {
                this.clear();
                return
            }
            this.$hidden.val(""), this.$hidden.attr("name", ""), this.$input.attr("name", this.name);
            if (this.type === "image" && this.$preview.length > 0 && (typeof n.type != "undefined" ? n.type.match("image.*") : n.name.match(/\.(gif|png|jpe?g)$/i)) && typeof FileReader != "undefined") {
              
                var r = new FileReader,
                    i = this.$preview,
                    s = this.$element;
                r.onload = function (e) {
                    $("#FeaturedImage").show();
                    $("#existingFeaturedImg").hide();
                    var img = $('#crop-avatar-target');
                    img.attr('src', e.target.result);
                    i.html('<img id="my-origin-image" src="' + e.target.result + '" ' + " />"), s.addClass("fileupload-exists").removeClass("fileupload-new")

                    //$('#my-origin-image').Jcrop({
                    //    onChange: setCoordsAndImgSize,
                    //    aspectRatio: 1
                       
                    //});

                    img.Jcrop({
                        onChange: updatePreviewPane,
                        onSelect: updatePreviewPane,
                        aspectRatio: xsize / ysize
                    }, function () {
                        var bounds = this.getBounds();
                        boundx = bounds[0];
                        boundy = bounds[1];

                        jcrop_api = this;
                        jcrop_api.setOptions({ allowSelect: true });
                        jcrop_api.setOptions({ allowMove: true });
                        jcrop_api.setOptions({ allowResize: true });
                        jcrop_api.setOptions({ aspectRatio: 1 });

                        // Maximise initial selection around the centre of the image,
                        // but leave enough space so that the boundaries are easily identified.
                        var padding = 10;
                        var shortEdge = (boundx < boundy ? boundx : boundy) - padding;
                        var longEdge = boundx < boundy ? boundy : boundx;
                        var xCoord = longEdge / 2 - shortEdge / 2;
                        jcrop_api.animateTo([xCoord, padding, shortEdge, shortEdge]);

                        var pcnt = $('#preview-pane .preview-container');
                        xsize = pcnt.width();
                        ysize = pcnt.height();
                        $('#preview-pane').appendTo(jcrop_api.ui.holder);
                        jcrop_api.focus();
                        $('#preview-pane .preview-container img').attr('src', e.target.result);
                       

                        if (!keepUploadBox) {
                            $('#avatar-upload-box').addClass('hidden');
                        }
                        $('#avatar-crop-box').removeClass('hidden');
                    });
                 
                }, r.readAsDataURL(n)
            } else this.$preview.text(n.name), this.$element.addClass("fileupload-exists").removeClass("fileupload-new")
        },
        clear: function (e) {
         
            this.$hidden.val(""), this.$hidden.attr("name", this.name), this.$input.attr("name", "");
            if (navigator.userAgent.match(/msie/i)) {
                var t = this.$input.clone(!0);
                this.$input.after(t), this.$input.remove(), this.$input = t
            } else this.$input.val("");
            this.$preview.html(""), this.$element.addClass("fileupload-new").removeClass("fileupload-exists"), e && (this.$input.trigger("change", ["clear"]), e.preventDefault())
        },
        reset: function (e) {
          
            this.clear(), this.$hidden.val(this.original.hiddenVal), this.$preview.html(this.original.preview), this.original.exists ? this.$element.addClass("fileupload-exists").removeClass("fileupload-new") : this.$element.addClass("fileupload-new").removeClass("fileupload-exists")
        },
        trigger: function (e) {
          
            this.$input.trigger("click"), e.preventDefault()
        }
    }, e.fn.fileupload = function (n) {
      
        return this.each(function () {
          
            var r = e(this),
                i = r.data("fileupload");
            i || r.data("fileupload", i = new t(this, n)), typeof n == "string" && i[n]()
        })
    }, e.fn.fileupload.Constructor = t, e(document).on("click.fileupload.data-api", '[data-provides="fileupload"]', function (t) {
      
        var n = e(this);
        if (n.data("fileupload")) return;
        n.fileupload(n.data());
        var r = e(t.target).closest('[data-dismiss="fileupload"],[data-trigger="fileupload"]');
        r.length > 0 && (r.trigger("click.fileupload"), t.preventDefault())
    })

    function updatePreviewPane(c) {
        if (parseInt(c.w) > 0) {
            var rx = xsize / c.w;
            var ry = ysize / c.h;

            $('#preview-pane .preview-container img').css({
                width: Math.round(rx * boundx) + 'px',
                height: Math.round(ry * boundy) + 'px',
                marginLeft: '-' + Math.round(rx * c.x) + 'px',
                marginTop: '-' + Math.round(ry * c.y) + 'px'
            });
        }
    } 
}(window.jQuery)

