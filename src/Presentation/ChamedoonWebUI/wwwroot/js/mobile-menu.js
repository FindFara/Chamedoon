(function () {
    const landingHamburger = document.querySelector('.landing-hamburger');
    const landingOffcanvasElement = document.getElementById('landingMobileNav');

    if (landingHamburger && landingOffcanvasElement && window.bootstrap?.Offcanvas) {
        const landingOffcanvas = bootstrap.Offcanvas.getOrCreateInstance(landingOffcanvasElement, {
            scroll: true,
            backdrop: true,
        });

        if (!landingHamburger.dataset.offcanvasBound) {
            landingHamburger.dataset.offcanvasBound = 'true';

            landingHamburger.addEventListener('click', (event) => {
                event.preventDefault();

                if (!landingOffcanvasElement.classList.contains('show')) {
                    landingOffcanvas.show();
                }
            });
        }
    }
})();
