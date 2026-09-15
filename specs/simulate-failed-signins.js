/**
 * Sign-in log simulation — paste into the browser dev console on https://localhost:5001
 *
 * Fires failed sign-in attempts at POST /api/platform/security/login (AllowAnonymous, so no
 * token is needed) to populate the sign-in log statistics.
 *
 *   simulateSignIns()                       // 40 attempts against made-up accounts
 *   simulateSignIns({ attempts: 120 })      // more volume
 *   simulateSignIns({ realUsers: ['b2badmin@test.com'], realAttempts: 2 })
 *
 * LOCKOUT WARNING: attempts against accounts that actually exist count towards Identity's
 * lockout counter (PasswordSignInAsync runs with lockoutOnFailure: true). Made-up accounts
 * cannot be locked out because there is nothing to lock. Leave realAttempts at 0 unless you
 * want to test the LockedOut reason, and expect to unlock the account afterwards in
 * Security > Accounts.
 */
async function simulateSignIns(options) {
    const o = Object.assign({
        attempts: 40,          // attempts against non-existent accounts -> UserNotFound
        realUsers: [],         // existing accounts -> InvalidPassword, then LockedOut
        realAttempts: 0,       // attempts per real account
        concurrency: 4,        // parallel requests
        password: 'definitely-not-the-password',
    }, options || {});

    // A few plausible-looking names so the "Top accounts by failed attempts" panel has a
    // realistic shape: some names are hit repeatedly, most only once.
    const hotTargets = ['admin', 'administrator', 'root', 'b2badmin@test.com.bak', 'support'];
    const randomName = () => 'user' + Math.random().toString(36).slice(2, 8) + '@example.com';

    const targets = [];

    for (let i = 0; i < o.attempts; i++) {
        // ~60% of traffic concentrated on a handful of names, the rest spray.
        targets.push(Math.random() < 0.6
            ? hotTargets[Math.floor(Math.random() * hotTargets.length)]
            : randomName());
    }

    for (const user of o.realUsers) {
        for (let i = 0; i < o.realAttempts; i++) {
            targets.push(user);
        }
    }

    const attempt = async (userName) => {
        try {
            const response = await fetch('/api/platform/security/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName, password: o.password, rememberMe: false }),
            });
            const body = await response.json().catch(() => ({}));
            return { userName, succeeded: !!body.succeeded, lockedOut: !!body.isLockedOut };
        } catch (e) {
            return { userName, error: String(e) };
        }
    };

    console.log('Firing ' + targets.length + ' failed sign-in attempts...');

    const results = [];
    for (let i = 0; i < targets.length; i += o.concurrency) {
        const batch = targets.slice(i, i + o.concurrency);
        results.push(...await Promise.all(batch.map(attempt)));
    }

    const succeeded = results.filter(r => r.succeeded).length;
    const lockedOut = results.filter(r => r.lockedOut).length;
    const errors = results.filter(r => r.error).length;

    console.log('Done. ' + results.length + ' attempts | unexpectedly succeeded: ' + succeeded +
                ' | locked out: ' + lockedOut + ' | network errors: ' + errors);
    console.log('The writer flushes on a 5s timer — wait ~5 seconds, then refresh Security > Sign-in log.');

    return results;
}

// Auto-run with defaults. Comment out if you would rather call it yourself.
simulateSignIns();
