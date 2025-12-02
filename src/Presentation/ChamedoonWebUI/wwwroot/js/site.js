(function () {
    const pageBody = document.body;

    const themeToggles = ['#darkModeToggle', '#darkModeToggleMobile', '#darkModeToggleMobilePanel']
        .map((selector) => document.querySelector(selector))
        .filter(Boolean);

    if (themeToggles.length && pageBody.classList.contains('landing-page')) {
        const applyMode = (mode) => {
            const isDark = mode === 'dark';
            pageBody.classList.toggle('dark-mode', isDark);

            themeToggles.forEach((toggle) => {
                const labelElement = toggle.querySelector('[data-role="label"]');
                const labelLight = toggle.getAttribute('data-label-light') || 'حالت شب';
                const labelDark = toggle.getAttribute('data-label-dark') || 'حالت روز';
                const nextTitle = isDark ? labelDark : labelLight;

                toggle.dataset.mode = mode;
                toggle.setAttribute('aria-pressed', isDark ? 'true' : 'false');
                toggle.dataset.label = nextTitle;
                toggle.setAttribute('aria-label', nextTitle);
                toggle.setAttribute('title', nextTitle);

                if (labelElement) {
                    labelElement.textContent = nextTitle;
                }
            });
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

        themeToggles.forEach((toggle) => {
            toggle.addEventListener('click', () => {
                activeMode = activeMode === 'dark' ? 'light' : 'dark';
                applyMode(activeMode);
                try {
                    localStorage.setItem('landing-theme', activeMode);
                } catch (error) {
                    // Ignore storage write issues (e.g. Safari private mode)
                }
            });
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

    const landingHamburger = document.querySelector('.landing-hamburger');
    const landingOffcanvasElement = document.getElementById('landingMobileNav');

    if (landingHamburger && landingOffcanvasElement && window.bootstrap?.Offcanvas) {
        const landingOffcanvas = bootstrap.Offcanvas.getOrCreateInstance(landingOffcanvasElement, {
            scroll: true,
            backdrop: true,
        });

        landingHamburger.addEventListener('click', (event) => {
            event.preventDefault();
            landingOffcanvas.toggle();
        });
    }

    const backToTop = document.getElementById('backToTop');
    if (backToTop) {
        const toggleButtonVisibility = () => {
            if (window.scrollY > 320) {
                backToTop.classList.add('show');
            } else {
                backToTop.classList.remove('show');
            }
        };

        toggleButtonVisibility();
        window.addEventListener('scroll', toggleButtonVisibility);
    }
})();
