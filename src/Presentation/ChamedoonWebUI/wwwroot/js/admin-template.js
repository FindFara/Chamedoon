(function () {
    const sidebar = document.getElementById('adminSidebar');
    const overlay = document.getElementById('adminOverlay');
    const toggle = document.getElementById('sidebarToggle');
    const close = document.getElementById('sidebarClose');
    const themeToggle = document.getElementById('themeToggle');
    const themeToggleIcon = themeToggle?.querySelector('.theme-toggle-icon');
    const themeToggleLabel = themeToggle?.querySelector('.visually-hidden');
    const root = document.documentElement;

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

    const themeStorageKey = 'admin-theme';
    const applyTheme = (theme) => {
        root.setAttribute('data-bs-theme', theme);
        if (themeToggle) {
            const label = theme === 'dark' ? 'حالت روز' : 'حالت شب';
            themeToggle.setAttribute('aria-label', label);
            themeToggle.setAttribute('title', label);
            if (themeToggleLabel) {
                themeToggleLabel.textContent = label;
            }
            if (themeToggleIcon) {
                themeToggleIcon.textContent = theme === 'dark' ? '☀️' : '🌙';
            }
        }
    };

    const storedTheme = localStorage.getItem(themeStorageKey) ?? 'light';
    applyTheme(storedTheme);

    themeToggle?.addEventListener('click', () => {
        const currentTheme = root.getAttribute('data-bs-theme') ?? 'light';
        const nextTheme = currentTheme === 'dark' ? 'light' : 'dark';
        localStorage.setItem(themeStorageKey, nextTheme);
        applyTheme(nextTheme);
    });
})();
