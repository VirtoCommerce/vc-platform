const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const test = require('node:test');
const vm = require('node:vm');

const webRoot = path.resolve(__dirname, '../../../src/VirtoCommerce.Platform.Web/wwwroot/js');

function createLoginApp(returnUrl) {
    const events = new Map();
    const navigations = [];
    const factories = new Map();
    let run;
    const app = new Proxy({}, {
        get: (_, method) => (...args) => {
            if (method === 'factory') factories.set(args[0], args[1]);
            if (method === 'run') run = args[0];
            return app;
        },
    });
    const window = {
        location: {
            origin: 'https://platform.example',
            search: returnUrl ? `?ReturnUrl=${encodeURIComponent(returnUrl)}` : '',
            hash: '',
            href: 'https://platform.example/#!/login',
        },
    };
    const context = vm.createContext({
        angular: { module: () => app, isDefined: value => value !== undefined },
        AppDependencies: [], window, URL, URLSearchParams,
    });
    vm.runInContext(fs.readFileSync(path.join(webRoot, 'common/urlHelper.js'), 'utf8'), context);
    const helperFactory = factories.get('platformWebApp.urlHelper');
    const urlHelper = helperFactory.at(-1)();
    vm.runInContext(fs.readFileSync(path.join(webRoot, 'app/app.js'), 'utf8'), context);
    const dependencies = {
        '$rootScope': {
            $on(name, callback) {
                events.set(name, [...(events.get(name) || []), callback]);
            },
        },
        '$state': { current: { name: 'loginDialog' }, go: name => navigations.push(name) },
        '$animate': { enabled() {} },
        '$timeout': callback => callback(),
        '$templateCache': { put() {} },
        '$templateRequest': () => ({ then() {} }),
        'platformWebApp.authService': { fillAuthData() {} },
        'platformWebApp.mainMenuService': { addMenuItem() {} },
        'platformWebApp.pushNotificationService': { startListening() {}, stopListening() {} },
        'platformWebApp.toolbarService': { tryRegister() {} },
        'platformWebApp.urlHelper': urlHelper,
    };
    run.at(-1)(...run.slice(0, -1).map(name => dependencies[name] || {}));
    return {
        urlHelper, window, navigations,
        login(authContext) {
            for (const callback of events.get('loginStatusChanged')) callback({}, authContext);
        },
    };
}

test('buyer without Manager access continues the actual login event to OAuth', () => {
    const returnUrl = '/connect/authorize?client_id=buyer-client&state=opaque';
    const app = createLoginApp(returnUrl);
    app.login({ isAuthenticated: true, canAccessAdminUI: false, passwordExpired: false });
    assert.equal(app.window.location.href, returnUrl);
    assert.deepEqual(app.navigations, []);
});

for (const returnUrl of [undefined, '/api/platform/security/users', 'https://other.example/connect/authorize', '//other.example/connect/authorize', '/connect/authorize#unexpected']) {
    test(`buyer cannot use an unrelated return URL: ${returnUrl}`, () => {
        const app = createLoginApp(returnUrl);
        app.login({ isAuthenticated: true, canAccessAdminUI: false, passwordExpired: false });
        assert.deepEqual(app.navigations, ['contact-admin']);
        assert.equal(app.urlHelper.getSafeAuthorizationReturnUrl(), undefined);
    });
}

test('OAuth return does not bypass an expired password', () => {
    const app = createLoginApp('/connect/authorize?client_id=buyer-client');
    app.login({ isAuthenticated: true, canAccessAdminUI: true, passwordExpired: true });
    assert.deepEqual(app.navigations, ['changePasswordDialog']);
});

test('OAuth return does not bypass authentication', () => {
    const app = createLoginApp('/connect/authorize?client_id=buyer-client');
    app.login({ isAuthenticated: false });
    assert.deepEqual(app.navigations, ['loginDialog']);
});
