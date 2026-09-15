window.simulateSignIns = async function (options) {
    // Fires failed sign-in attempts at POST /api/platform/security/login (AllowAnonymous,
    // so no token is needed) to populate the sign-in log statistics.
    //
    //   simulateSignIns()                    -> 40 attempts against made-up accounts
    //   simulateSignIns({ attempts: 120 })   -> more volume
    //   simulateSignIns({ realUsers: ['b2badmin@test.com'], realAttempts: 2 })
    //
    // LOCKOUT WARNING: attempts against accounts that actually exist count towards Identity's
    // lockout counter (PasswordSignInAsync runs with lockoutOnFailure: true). Made-up accounts
    // cannot be locked out because there is nothing to lock. Leave realAttempts at 0 unless you
    // want to see the LockedOut reason, and expect to unlock the account afterwards in
    // Security > Accounts.

    var o = Object.assign({
        attempts: 40,
        realUsers: [],
        realAttempts: 0,
        concurrency: 4,
        password: 'definitely-not-the-password'
    }, options || {});

    // A few plausible-looking names so "Top accounts by failed attempts" gets a realistic
    // shape: some names hit repeatedly, most only once.
    var hotTargets = ['admin', 'administrator', 'root', 'support', 'b2badmin@test.com.bak'];
    var randomName = function () {
        return 'user' + Math.random().toString(36).slice(2, 8) + '@example.com';
    };

    var targets = [];

    for (var i = 0; i < o.attempts; i++) {
        targets.push(Math.random() < 0.6
            ? hotTargets[Math.floor(Math.random() * hotTargets.length)]
            : randomName());
    }

    o.realUsers.forEach(function (user) {
        for (var j = 0; j < o.realAttempts; j++) {
            targets.push(user);
        }
    });

    var attempt = async function (userName) {
        try {
            var response = await fetch('/api/platform/security/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName: userName, password: o.password, rememberMe: false })
            });
            var body = await response.json().catch(function () { return {}; });
            return { userName: userName, status: response.status, succeeded: !!body.succeeded, lockedOut: !!body.isLockedOut };
        } catch (e) {
            return { userName: userName, error: String(e) };
        }
    };

    console.log('Firing ' + targets.length + ' failed sign-in attempts...');

    var results = [];
    for (var k = 0; k < targets.length; k += o.concurrency) {
        var batch = targets.slice(k, k + o.concurrency);
        var settled = await Promise.all(batch.map(attempt));
        results = results.concat(settled);
    }

    var succeeded = results.filter(function (r) { return r.succeeded; }).length;
    var lockedOut = results.filter(function (r) { return r.lockedOut; }).length;
    var errors = results.filter(function (r) { return r.error; }).length;

    console.log('Done. ' + results.length + ' attempts | unexpectedly succeeded: ' + succeeded +
                ' | locked out: ' + lockedOut + ' | network errors: ' + errors);
    console.log('The writer flushes on a 5s timer - wait ~5 seconds, then refresh Security > Sign-in log.');

    return results;
};

simulateSignIns();
