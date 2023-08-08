class Alert
{
    constructor(element)
    {
        this.element = element;
    }

    dismissableDanger(text)
    {
        this.element.html(this.setupAlert(text));
    }

    setupAlert(text)
    {
        return '' +
            '<div class="alert alert-danger alert-dismissable">' +
                '<button type="button" class="close" data-dismiss="alert" aria-hidden="true"></button>' +
                text +
            '</div>';
    }
}

export default Alert;