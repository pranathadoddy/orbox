import $ from 'jquery';

import '../../lib/tabler/tabler.css';
import '../../lib/tabler/selectgroup.css';
import '../../lib/tabler/bootstrap.bundle.min.js';
import 'bootstrap4-toggle/css/bootstrap4-toggle.min.css';
import 'bootstrap4-toggle/js/bootstrap4-toggle.min.js';

let req = require.context('./administrator', true, /\.(js)$/);
req.keys().forEach(function (key) {
    let noExtensionKey = key.replace('.js', '');
    let splittedFilePath = noExtensionKey.split('/');
    splittedFilePath.splice(0, 1);
    let scriptName = splittedFilePath[splittedFilePath.length - 1];
    let filePath = splittedFilePath.join('/');
    import(`./administrator/${filePath}`).then(m => window[scriptName] = m);
});

req = require.context('../controllers', true, /\.(js)$/);
req.keys().forEach(function (key) {
    let noExtensionKey = key.replace('.js', '');
    let splittedFilePath = noExtensionKey.split('/');
    splittedFilePath.splice(0, 1);
    let scriptName = splittedFilePath[splittedFilePath.length - 1];
    let filePath = splittedFilePath.join('/');
    import(`../controllers/${filePath}`).then(m => window[scriptName] = m);
});