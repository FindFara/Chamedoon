(function () {
    const pageBody = document.body;

    const darkModeToggle = document.getElementById('darkModeToggle');
    if (darkModeToggle && pageBody.classList.contains('landing-page')) {
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

        if (window.matchMedia) {
            const schemeListener = (event) => {
                const newMode = event.matches ? 'dark' : 'light';
                const stored = (() => {
                    try {
                        return localStorage.getItem('landing-theme');
                    } catch (error) {
                        return null;
                    }
                })();
                if (stored !== 'dark' && stored !== 'light') {
                    activeMode = newMode;
                    applyMode(activeMode);
                }
            };
            try {
                window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', schemeListener);
            } catch (error) {
                // Older browsers use addListener
                const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
                if (mediaQuery.addListener) {
                    mediaQuery.addListener(schemeListener);
                }
            }
        }
    }

    const backToTop = document.getElementById('backToTop');
    if (!backToTop) {
        return;
    }

    const toggleButtonVisibility = () => {
        if (window.scrollY > 320) {
            backToTop.classList.add('show');
        } else {
            backToTop.classList.remove('show');
        }
    };

    toggleButtonVisibility();
    window.addEventListener('scroll', toggleButtonVisibility);
})();
