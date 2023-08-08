import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';
import '../../../../lib/croppie/croppie.min.js';
import '../../../../lib/croppie/croppie.css';


const crudHelper = new CrudHelper();

class AgentAgencyCategoryEdit {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.registerButtonSave();
        self.registerCroppie();
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('form', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-save', self.element));
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.agencycategory').addClass('active');
    }

    registerCroppie() {
        var self = this;

        var iconCroppie = $('#icon-croppie').croppie({
            viewport: {
                width: 300,
                height: 300,
                type: 'square'
            },
            enableExif: true,
            enableResize: true,
            enableOrientation: true
        });

        $('#icon-modal').on('shown.bs.modal',
            function () {
                iconCroppie.croppie('bind');
            });

        $('#icon-modal #btn-upload').off('click').on('click',
            function () {
                iconCroppie.croppie('result',
                    {
                        type: 'canvas',
                        size: 'viewport',
                        format: 'png' //this is to make sure, the result is in png format
                    })
                    .then(function (resp) {
                        var fileName = $("#IconName").val();
                        var base64 = resp;

                        $("#Base64File", self.element).val(base64);
                        $("#FileName", self.element).val(fileName);
                        $(".agency-icon", self.element).css("background-image", "url('" + base64 + "')")
                        $('#icon-modal').modal("hide");
                    });
            });


        $("input[name=Icon]", self.element)
            .on('change', function () { self.readUploadedFile(this, iconCroppie, $('#icon-modal'), $("#IconName")); });
    }

    readUploadedFile(input, $croppieElement, $modal, $nameElement) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $croppieElement
                    .croppie('bind', { url: e.target.result })
                    .then(function () {
                        $modal.modal('show');
                        $nameElement.val(input.files[0].name);
                    });
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
}

export default AgentAgencyCategoryEdit;