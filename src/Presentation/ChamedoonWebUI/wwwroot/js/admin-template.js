(function () {
    const sidebar = document.getElementById('adminSidebar');
    const overlay = document.getElementById('adminOverlay');
    const toggle = document.getElementById('sidebarToggle');
    const close = document.getElementById('sidebarClose');

    if (!sidebar) {
        return;
    }

    const setSidebarState = (open) => {
        const shouldOpen = open ?? !sidebar.classList.contains('open');
        sidebar.classList.toggle('open', shouldOpen);
        overlay?.classList.toggle('show', shouldOpen);
        document.body.classList.toggle('sidebar-open', shouldOpen);
    };

    toggle?.addEventListener('click', () => setSidebarState(true));
    close?.addEventListener('click', () => setSidebarState(false));
    overlay?.addEventListener('click', () => setSidebarState(false));

    const handleResize = () => {
        if (window.innerWidth >= 992) {
            sidebar.classList.add('open');
            overlay?.classList.remove('show');
            document.body.classList.remove('sidebar-open');
        } else {
            sidebar.classList.remove('open');
        }
    };

    window.addEventListener('resize', handleResize);
    handleResize();
})();
