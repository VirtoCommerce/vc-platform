(async () => {
    const ATTEMPTS = 40;
    const hot = ['admin', 'administrator', 'root', 'support', 'test'];
    const pick = () => Math.random() < 0.6
        ? hot[Math.floor(Math.random() * hot.length)]
        : 'user' + Math.random().toString(36).slice(2, 8) + '@example.com';

    let sent = 0, failed = 0;

    for (let i = 0; i < ATTEMPTS; i++) {
        try {
            const r = await fetch('/api/platform/security/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName: pick(), password: 'nope', rememberMe: false })
            });
            sent++;
            if (!r.ok) failed++;
        } catch (e) {
            failed++;
        }
    }

    console.log(sent + ' attempts sent (' + failed + ' transport errors).');
    console.log('Writer flushes on a 5s timer - wait ~5s, then refresh Security > Sign-in log.');
})();
