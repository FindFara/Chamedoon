(function () {
    const pageBody = document.body;
    const darkModeToggle = document.getElementById('darkModeToggle');

    if (!darkModeToggle || !pageBody.classList.contains('landing-page')) {
        return;
    }

    const labelElement = darkModeToggle.querySelector('[data-role="label"]');
    const labelLight = darkModeToggle.getAttribute('data-label-light') || 'حالت شب';
    const labelDark = darkModeToggle.getAttribute('data-label-dark') || 'حالت روز';

    const applyMode = (mode) => {
        const isDark = mode === 'dark';
        pageBody.classList.toggle('dark-mode', isDark);
        darkModeToggle.dataset.mode = mode;
        darkModeToggle.setAttribute('aria-pressed', isDark ? 'true' : 'false');

        if (labelElement) {
            labelElement.textContent = isDark ? labelDark : labelLight;
        }

        const nextTitle = isDark ? labelDark : labelLight;
        darkModeToggle.dataset.label = nextTitle;
        darkModeToggle.setAttribute('aria-label', nextTitle);
        darkModeToggle.setAttribute('title', nextTitle);
    };

    let storedPreference = null;
    try {
        storedPreference = localStorage.getItem('landing-theme');
    } catch (error) {
        storedPreference = null;
    }

    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    let activeMode = storedPreference === 'dark' || storedPreference === 'light'
        ? storedPreference
        : (prefersDark ? 'dark' : 'light');

    applyMode(activeMode);

    darkModeToggle.addEventListener('click', () => {
        activeMode = activeMode === 'dark' ? 'light' : 'dark';
        applyMode(activeMode);
        try {
            localStorage.setItem('landing-theme', activeMode);
        } catch (error) {
            // Ignore storage write issues (e.g. Safari private mode)
        }
    });

    if (!window.matchMedia) {
        return;
    }

    const schemeMedia = window.matchMedia('(prefers-color-scheme: dark)');
    const schemeListener = (event) => {
        const newMode = event.matches ? 'dark' : 'light';
        let stored = null;
        try {
            stored = localStorage.getItem('landing-theme');
        } catch (error) {
            stored = null;
        }

        if (stored !== 'dark' && stored !== 'light') {
            activeMode = newMode;
            applyMode(activeMode);
        }
    };

    try {
        schemeMedia.addEventListener('change', schemeListener);
    } catch (error) {
        if (schemeMedia.addListener) {
            schemeMedia.addListener(schemeListener);
        }
    }
})();
