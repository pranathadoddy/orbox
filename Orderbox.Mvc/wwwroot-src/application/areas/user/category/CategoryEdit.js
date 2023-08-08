﻿import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';

const crudHelper = new CrudHelper();

class CategoryEdit {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.registerButtonSave();
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
        $('.navbar-nav li.category').addClass('active');
    }
}

export default CategoryEdit;