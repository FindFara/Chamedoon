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

    const mobileMenu = document.getElementById('mobileMenu');
    const mobileMenuToggle = document.getElementById('mobileMenuToggle');
    const mobileMenuClose = document.getElementById('mobileMenuClose');
    const mobileMenuBackdrop = document.getElementById('mobileMenuBackdrop');

    const openMobileMenu = () => {
        if (!mobileMenu || !mobileMenuBackdrop || !mobileMenuToggle) return;
        mobileMenu.classList.add('open');
        mobileMenuBackdrop.classList.add('show');
        document.body.classList.add('offcanvas-open');
        mobileMenuToggle.setAttribute('aria-expanded', 'true');
        mobileMenu.setAttribute('aria-hidden', 'false');
    };

    const closeMobileMenu = () => {
        if (!mobileMenu || !mobileMenuBackdrop || !mobileMenuToggle) return;
        mobileMenu.classList.remove('open');
        mobileMenuBackdrop.classList.remove('show');
        document.body.classList.remove('offcanvas-open');
        mobileMenuToggle.setAttribute('aria-expanded', 'false');
        mobileMenu.setAttribute('aria-hidden', 'true');
    };

    if (mobileMenu && mobileMenuToggle && mobileMenuBackdrop) {
        mobileMenuToggle.addEventListener('click', () => {
            if (mobileMenu.classList.contains('open')) {
                closeMobileMenu();
            } else {
                openMobileMenu();
            }
        });

        if (mobileMenuClose) {
            mobileMenuClose.addEventListener('click', closeMobileMenu);
        }

        mobileMenuBackdrop.addEventListener('click', closeMobileMenu);

        mobileMenu.querySelectorAll('a').forEach(link => {
            link.addEventListener('click', closeMobileMenu);
        });

        window.addEventListener('keydown', event => {
            if (event.key === 'Escape' && mobileMenu.classList.contains('open')) {
                closeMobileMenu();
            }
        });
    }
})();
