import '../../../lib/slide-to-unlock/jquery.slideToUnlock';
import '../../../lib/slide-to-unlock/slideToUnlock.css';
import '../../../lib/slide-to-unlock/green.theme.css';

import UiHelper from '../../component/UiHelper';
const uiHelper = new UiHelper();

class VoucherIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        const self = this;

        self.registerOnClickHowToLink();
        self.registerOnClickRedeemButton();
        self.registerRedeemModalClose();
        self.registerSlideToUnlock();
        self.registerOnClickStoreLocationLink();
    }

    registerSlideToUnlock() {
        const self = this;

        if ($('div#slide-to-redeem', self.element).length > 0) {
            $('div#slide-to-redeem', self.element).slideToUnlock({
                lockText: 'Slide to Redeem',
                unlockfn: () => {
                    $('#redeem-modal').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                }
            });
        }
    }

    registerRedeemModalClose() {
        $('#redeem-modal').on('hidden.bs.modal',
            function () {
                window.location.reload();
            });
    }

    registerOnClickHowToLink() {
        $('#howtoredeem').on('click',
            function (e) {
                e.preventDefault();
                $('#howtoredeem-modal').modal('show');
            });
    }

    registerOnClickRedeemButton() {
        let self = this;

        $('#btn-redeem').on('click',
            function (e) {
                e.preventDefault();

                $('#StoreLocationId').removeClass('is-invalid');
                let storeLocationid = $('#StoreLocationId').val();
                if (storeLocationid === "") {
                    $('#StoreLocationId').addClass('is-invalid');
                    return;
                }
                uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });
                let url = $('div#slide-to-redeem', self.element).data('url');
                let code = $('div#slide-to-redeem', self.element).data('code');
                
                $.post(url, { code, storeLocationid }, function () {
                    window.location.reload();
                });
            });
    }

    registerOnClickStoreLocationLink() {
        let self = this;

        $('[btn-view-location]', self.element).on('click',
            function (e) {
                e.preventDefault();
                $('#stores-modal').modal('show');
            });
    }
}

export default VoucherIndex;