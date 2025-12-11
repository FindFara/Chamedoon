(function () {
    const pageBody = document.body;

    const loaderElement = document.getElementById('pageLoader');
    if (loaderElement) {
        const hideLoader = () => {
            loaderElement.classList.add('is-hidden');
            pageBody.classList.remove('page-loading');
            setTimeout(() => loaderElement.remove(), 600);
        };

        if (document.readyState === 'complete') {
            requestAnimationFrame(hideLoader);
        } else {
            window.addEventListener('load', hideLoader);
        }
    }

    const darkModeToggle = document.getElementById('darkModeToggle');
    if (pageBody.classList.contains('landing-page')) {
        const labelElement = darkModeToggle?.querySelector('[data-role="label"]');
        const labelLight = darkModeToggle?.getAttribute('data-label-light') || 'حالت شب';
        const labelDark = darkModeToggle?.getAttribute('data-label-dark') || 'حالت روز';

        const applyMode = (mode) => {
            const isDark = mode === 'dark';
            pageBody.classList.toggle('dark-mode', isDark);
            if (darkModeToggle) {
                darkModeToggle.dataset.mode = mode;
                darkModeToggle.setAttribute('aria-pressed', isDark ? 'true' : 'false');
                if (labelElement) {
                    labelElement.textContent = isDark ? labelDark : labelLight;
                }
                const nextTitle = isDark ? labelDark : labelLight;
                darkModeToggle.dataset.label = nextTitle;
                darkModeToggle.setAttribute('aria-label', nextTitle);
                darkModeToggle.setAttribute('title', nextTitle);
            }
        };

        const readPreference = () => {
            try {
                return localStorage.getItem('landing-theme');
            } catch (error) {
                return null;
            }
        };

        const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        const storedPreference = readPreference();
        let activeMode = storedPreference === 'dark' || storedPreference === 'light'
            ? storedPreference
            : (prefersDark ? 'dark' : 'light');

        applyMode(activeMode);

        if (darkModeToggle) {
            darkModeToggle.addEventListener('click', () => {
                activeMode = activeMode === 'dark' ? 'light' : 'dark';
                applyMode(activeMode);
                try {
                    localStorage.setItem('landing-theme', activeMode);
                } catch (error) {
                    // Ignore storage write issues (e.g. Safari private mode)
                }
            });
        }

        if (window.matchMedia) {
            const schemeListener = (event) => {
                const newMode = event.matches ? 'dark' : 'light';
                const stored = readPreference();
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

    const mobileMenu = document.getElementById('mobileMenu');
    const mobileMenuToggle = document.getElementById('mobileMenuToggle');
    const mobileMenuClose = document.getElementById('mobileMenuClose');
    const mobileMenuBackdrop = document.getElementById('mobileMenuBackdrop');

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

    const preventMultiSubmitForms = document.querySelectorAll('form[data-prevent-multi-submit]');
    preventMultiSubmitForms.forEach(form => {
        form.addEventListener('submit', (event) => {
            if (form.dataset.submitting === 'true') {
                event.preventDefault();
                return;
            }

            if (!form.checkValidity()) {
                return;
            }

            form.dataset.submitting = 'true';
            const submitButtons = form.querySelectorAll('button[type="submit"], input[type="submit"]');
            submitButtons.forEach(button => {
                const processingText = button.getAttribute('data-processing-text');
                if (processingText && button.tagName === 'BUTTON') {
                    button.dataset.originalText = button.textContent;
                    button.textContent = processingText;
                }

                button.setAttribute('aria-busy', 'true');
                button.disabled = true;
            });
        });
    });

    const passwordToggleButtons = document.querySelectorAll('[data-toggle-password]');
    passwordToggleButtons.forEach(button => {
        const wrapper = button.closest('.password-input-wrapper');
        const input = wrapper?.querySelector('input');
        if (!input) {
            return;
        }

        const setState = (visible) => {
            input.type = visible ? 'text' : 'password';
            button.dataset.active = visible ? 'true' : 'false';
            const label = visible ? 'مخفی کردن رمز عبور' : 'نمایش رمز عبور';
            button.setAttribute('aria-pressed', visible ? 'true' : 'false');
            button.setAttribute('aria-label', label);
            button.setAttribute('title', label);
        };

        button.addEventListener('click', () => {
            const shouldShow = input.type === 'password';
            setState(shouldShow);
        });

        setState(false);
    });
})();
