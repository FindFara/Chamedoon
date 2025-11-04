(function () {
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
