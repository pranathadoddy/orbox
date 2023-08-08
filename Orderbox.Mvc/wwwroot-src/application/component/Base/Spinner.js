import '../../../lib/spinner/spinner.css';

class Spinner {
    constructor(element)
    {
        this.element = element;
    }

    show()
    {
        this.element.html(this.setupSpinner());
    }

    hide()
    {
        this.element.empty();
    }

    getSpinnerHtml()
    {
        return this.setupSpinner();
    }

    setupSpinner()
    {
        return '<div class="spinner"><div class="double-bounce1"></div><div class="double-bounce2"></div></div>';
    }
}

export default Spinner;