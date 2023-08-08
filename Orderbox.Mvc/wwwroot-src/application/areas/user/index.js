import $ from 'jquery';
import UiHelper from '../../component/UiHelper';
import NewOrderNotification from '../../component/NewOrderNotification';

var uiHelper = new UiHelper();
const newOrderNotification = new NewOrderNotification();

class UserIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        $('.clipboard-container a', self.element)
            .off('click')
            .on('click', function (e) {
                e.preventDefault();

                var copyTextElement = $('.clipboard-container input', self.element)[0];
                copyTextElement.select();
                copyTextElement.setSelectionRange(0, 99999);

                document.execCommand('copy');

                uiHelper.notyf.success('Link Tersalin');
            });

        newOrderNotification.register();
    }
}

export default UserIndex;
