import 'notyf/notyf.min.css';

import $ from 'jquery';
import 'gasparesganga-jquery-loading-overlay';
import { Notyf } from 'notyf';
import Swal from 'sweetalert2';
import Spinner from './Base/Spinner';

class UiHelper 
{
    constructor()
    {
        this.loadingOverlay = $.LoadingOverlay;
        this.spinner = new Spinner();
        this.notyf = new Notyf({ duration: 8000 });
        this.alert =
            Swal.mixin({
                confirmButtonClass: 'btn btn-primary',
                cancelButtonClass: 'btn btn-default',
                buttonsStyling: false
            });
        this.warningAlert =
            Swal.mixin({
                confirmButtonClass: 'btn btn-warning',
                cancelButtonClass: 'btn btn-default',
                buttonsStyling: false
            });
        this.dangerAlert =
            Swal.mixin({
                confirmButtonClass: 'btn btn-danger',
                cancelButtonClass: 'btn btn-default',
                buttonsStyling: false
            });
        this.successAlert =
            Swal.mixin({
                confirmButtonClass: 'btn btn-success',
                cancelButtonClass: 'btn btn-default',
                buttonsStyling: false
            });
    }
}

export default UiHelper;