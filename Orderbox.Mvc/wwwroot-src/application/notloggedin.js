import '../lib/tabler/tabler.css';
import '../lib/tabler/bootstrap.bundle.min.js';
import '../css/custom.css';

var req = require.context('./controllers/account', true, /\.(js)$/);
req.keys().forEach(function (key) {
    let noExtensionKey = key.replace('.js', '');
    let splittedFilePath = noExtensionKey.split('/');
    splittedFilePath.splice(0, 1);
    let scriptName = splittedFilePath[splittedFilePath.length - 1];
    let filePath = splittedFilePath.join('/');
    import(`./controllers/account/${filePath}`).then(m => window[scriptName] = m);
});

import(`./areas/tenant-jquery/order/TenantOrderOrderDetail`).then(m => window["TenantOrderOrderDetail"] = m);
import(`./controllers/voucher/VoucherIndex`).then(m => window["VoucherIndex"] = m);