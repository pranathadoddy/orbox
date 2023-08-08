import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';

const crudHelper = new CrudHelper();

class AgentCreate {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.registerButtonSave();
        self.registerEvents();
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('form', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.submitFormWithFile($('#btn-save', self.element), undefined, function (data) {
            // not implemented
        });
    }

    registerEvents() {
        $("#File").on('change', function () {
            var filename = $('#File').val();
            $('.form-file-text').html(filename);
        });
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.dealsagency').addClass('active');
    }
}

export default AgentCreate;