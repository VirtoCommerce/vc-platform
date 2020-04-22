const fs = require('fs');
const spawn = require('child_process').spawn;
const app_js = './wwwroot/dist/app.js';
const vendor_js = './wwwroot/dist/vendor.js';
const style_css = './wwwroot/dist/style.css';

try {
    if (!fs.existsSync(app_js) || !fs.existsSync(vendor_js) || !fs.existsSync(style_css)) {

        const webpack = spawn('cmd.exe', ['/c', 'npm', 'run', 'webpack:dev']);

        webpack.stdout.on('data', (data) => { console.log(`${data}`); });

        webpack.stderr.on('data', (data) => { console.error(`${data}`); });
    }
} catch (err) {
    console.log(err);
}
