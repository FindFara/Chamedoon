document.addEventListener("DOMContentLoaded", () => {
    const menuElement = document.getElementById("landingMobileNav");

    if (!menuElement || !window.bootstrap) {
        return;
    }

    const offcanvas = bootstrap.Offcanvas.getOrCreateInstance(menuElement);

    menuElement.querySelectorAll(".mobile-nav .nav-link").forEach((link) => {
        link.addEventListener("click", () => offcanvas.hide());
    });
});
