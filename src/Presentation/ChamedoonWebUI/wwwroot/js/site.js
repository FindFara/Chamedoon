(function () {
    const pageBody = document.body;
    const rootElement = document.documentElement;

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
            [rootElement, pageBody].forEach(element => {
                element?.classList.toggle('dark-mode', isDark);
            });
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

    const passwordToggleButtons = document.querySelectorAll('[data-password-toggle]');
    passwordToggleButtons.forEach(button => {
        const inputId = button.dataset.inputId;
        const input = inputId ? document.getElementById(inputId) : null;
        const icon = button.querySelector('img');
        const showIcon = button.dataset.iconShow;
        const hideIcon = button.dataset.iconHide;

        if (!input) {
            return;
        }

        button.addEventListener('click', () => {
            const willShow = input.type === 'password';
            input.type = willShow ? 'text' : 'password';

            if (icon && showIcon && hideIcon) {
                icon.src = willShow ? showIcon : hideIcon;
            }

            button.setAttribute('aria-pressed', willShow ? 'true' : 'false');
        });
    });

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

    const getAlertAnimationDuration = (alert) => {
        const box = alert?.querySelector('.floating-alert__box');
        return box ? parseFloat(getComputedStyle(box).animationDuration) * 1000 : 300;
    };

    const dismissAlert = (alert) => {
        if (!alert || alert.dataset.dismissing === 'true') return;
        alert.dataset.dismissing = 'true';
        alert.classList.add('is-dismissed');
        setTimeout(() => alert.remove(), getAlertAnimationDuration(alert));
    };

    const floatingAlerts = document.querySelectorAll('.floating-alert');
    floatingAlerts.forEach(alert => {
        const closeButton = alert.querySelector('[data-dismiss-alert]');
        const timerElement = alert.querySelector('[data-alert-timer]');
        const autoDismissAfter = parseInt(alert.getAttribute('data-auto-dismiss-after') || '0', 10);
        let autoDismissTimer = null;

        if (timerElement && autoDismissAfter > 0) {
            timerElement.style.setProperty('--alert-timer-duration', `${autoDismissAfter}ms`);
        }

        if (closeButton) {
            closeButton.addEventListener('click', () => {
                if (autoDismissTimer) {
                    clearTimeout(autoDismissTimer);
                }
                dismissAlert(alert);
            });
        }

        if (autoDismissAfter > 0) {
            autoDismissTimer = setTimeout(() => dismissAlert(alert), autoDismissAfter);
        }
    });

    const persianCharPattern = /[\u0600-\u06FF]/g;
    const enforceNonPersianInput = (input) => {
        if (!input) return;

        const removePersianCharacters = (value) => value.replace(persianCharPattern, '');

        input.addEventListener('input', () => {
            const sanitized = removePersianCharacters(input.value);
            const hadPersianText = sanitized !== input.value;

            if (hadPersianText) {
                input.value = sanitized;
                input.setCustomValidity('لطفاً از حروف فارسی استفاده نکنید.');
            } else {
                input.setCustomValidity('');
            }

            if (!document.activeElement || document.activeElement !== input) {
                input.reportValidity();
            }
        });

        input.addEventListener('blur', () => {
            if (persianCharPattern.test(input.value)) {
                input.setCustomValidity('لطفاً از حروف فارسی استفاده نکنید.');
            } else {
                input.setCustomValidity('');
            }
        });
    };

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const attachEmailValidator = (input) => {
        if (!input) return;

        const validate = () => {
            const value = input.value.trim();
            if (value === '') {
                input.setCustomValidity('');
                return true;
            }

            if (!emailPattern.test(value)) {
                input.setCustomValidity('فرمت ایمیل صحیح نیست.');
                return false;
            }

            input.setCustomValidity('');
            return true;
        };

        input.addEventListener('input', validate);
        input.addEventListener('blur', validate);
    };

    const englishOnlyInputs = document.querySelectorAll('[data-disallow-persian="true"]');
    englishOnlyInputs.forEach(enforceNonPersianInput);

    const emailInputs = document.querySelectorAll('[data-validate-email="true"]');
    emailInputs.forEach(attachEmailValidator);

    const formsWithEmailValidation = new Set();
    emailInputs.forEach(input => {
        if (input.form) {
            formsWithEmailValidation.add(input.form);
        }
    });

    formsWithEmailValidation.forEach(form => {
        form.addEventListener('submit', (event) => {
            let isValid = true;

            const formEmailInputs = form.querySelectorAll('[data-validate-email="true"]');
            formEmailInputs.forEach(input => {
                const value = input.value.trim();
                if (value && !emailPattern.test(value)) {
                    input.setCustomValidity('فرمت ایمیل صحیح نیست.');
                    isValid = false;
                    input.reportValidity();
                }
            });

            const formEnglishOnlyInputs = form.querySelectorAll('[data-disallow-persian="true"]');
            formEnglishOnlyInputs.forEach(input => {
                if (persianCharPattern.test(input.value)) {
                    input.setCustomValidity('لطفاً از حروف فارسی استفاده نکنید.');
                    isValid = false;
                    input.reportValidity();
                }
            });

            if (!isValid) {
                event.preventDefault();
            }
        });
    });
})();
