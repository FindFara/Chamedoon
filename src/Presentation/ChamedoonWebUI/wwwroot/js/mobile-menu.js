document.addEventListener("DOMContentLoaded", () => {
    const menuElement = document.getElementById("landingMobileNav");
    const toggleButtons = document.querySelectorAll('[data-bs-target="#landingMobileNav"]');

    if (!menuElement || !window.bootstrap || toggleButtons.length === 0) {
        return;
    }

    const offcanvas = bootstrap.Offcanvas.getOrCreateInstance(menuElement);

    toggleButtons.forEach((btn) => {
        btn.addEventListener("click", (event) => {
            event.preventDefault();
            offcanvas.toggle();
        });
    });

    menuElement.querySelectorAll(".mobile-nav .nav-link").forEach((link) => {
        link.addEventListener("click", () => offcanvas.hide());
    });
});