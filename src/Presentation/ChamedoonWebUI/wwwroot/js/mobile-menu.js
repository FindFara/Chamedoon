(function () {
    if (!window.bootstrap?.Offcanvas) {
        return;
    }

    const defaultOffcanvasSelector = '#landingMobileNav';

    document.querySelectorAll('.landing-hamburger').forEach((hamburger) => {
        const targetSelector = hamburger.getAttribute('data-bs-target')
            || hamburger.getAttribute('aria-controls')
            || defaultOffcanvasSelector;

        const offcanvasSelector = targetSelector.startsWith('#') ? targetSelector : `#${targetSelector}`;
        const offcanvasElement = document.querySelector(offcanvasSelector);

        if (!offcanvasElement || hamburger.dataset.offcanvasBound === 'true') {
            return;
        }

        const offcanvasInstance = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement, {
            scroll: true,
            backdrop: true,
        });

        hamburger.dataset.offcanvasBound = 'true';
        hamburger.addEventListener('click', (event) => {
            event.preventDefault();
            offcanvasInstance.toggle();
        });
    });
})();
