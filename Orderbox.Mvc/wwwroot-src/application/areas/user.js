import $ from 'jquery';

import '../../lib/tabler/tabler.css';
import '../../lib/tabler/selectgroup.css';
import '../../lib/tabler/bootstrap.bundle.min.js';
import '../../css/custom.css';
import 'bootstrap4-toggle/css/bootstrap4-toggle.min.css';
import 'bootstrap4-toggle/js/bootstrap4-toggle.min';

import User from './user/index';

let req = require.context('./user', true, /\.(js)$/);
req.keys().forEach(function (key) {
    let noExtensionKey = key.replace('.js', '');
    let splittedFilePath = noExtensionKey.split('/');
    splittedFilePath.splice(0, 1);
    let scriptName = splittedFilePath[splittedFilePath.length - 1];
    let filePath = splittedFilePath.join('/');
    import(`./user/${filePath}`).then(m => window[scriptName] = m);
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

const user = new User($('body'));
user.register();