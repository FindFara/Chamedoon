(function () {
    const finePointerQuery = window.matchMedia?.('(pointer: fine)');
    const reducedMotionQuery = window.matchMedia?.('(prefers-reduced-motion: reduce)');

    if (!finePointerQuery?.matches || reducedMotionQuery?.matches) {
        return;
    }

    const cursor = document.createElement('div');
    cursor.className = 'water-cursor';
    cursor.setAttribute('aria-hidden', 'true');
    cursor.innerHTML = `
        <svg class="water-cursor__icon" viewBox="0 0 96 96" focusable="false" aria-hidden="true">
            <defs>
                <linearGradient id="waterCursorGradient" x1="20" y1="6" x2="78" y2="92" gradientUnits="userSpaceOnUse">
                    <stop offset="0" stop-color="#6766f7" />
                    <stop offset="1" stop-color="#28b7ed" />
                </linearGradient>
            </defs>
            <g class="water-cursor__rays" fill="url(#waterCursorGradient)">
                <rect x="31" y="0" width="8" height="18" rx="4" />
                <rect x="10" y="13" width="8" height="18" rx="4" transform="rotate(-45 14 22)" />
                <rect x="0" y="33" width="18" height="8" rx="4" />
                <rect x="10" y="55" width="8" height="18" rx="4" transform="rotate(45 14 64)" />
                <rect x="56" y="14" width="8" height="18" rx="4" transform="rotate(45 60 23)" />
            </g>
            <path d="M30 28 L89 52 L69 63 L90 84 L84 90 L63 69 L52 89 Z" fill="none" stroke="url(#waterCursorGradient)" stroke-width="12" stroke-linecap="round" stroke-linejoin="round" />
        </svg>`;
    document.body.appendChild(cursor);
    document.documentElement.classList.add('has-water-cursor');

    const moveCursor = (event) => {
        cursor.style.setProperty('--cursor-x', `${event.clientX}px`);
        cursor.style.setProperty('--cursor-y', `${event.clientY}px`);
        cursor.classList.add('is-visible');
    };

    const hideCursor = () => cursor.classList.remove('is-visible');
    const setInteractive = (event) => {
        const target = event.target;
        const isInteractive = target?.closest?.('a, button, input, textarea, select, label, [role="button"], [tabindex]:not([tabindex="-1"])');
        cursor.classList.toggle('is-interactive', Boolean(isInteractive));
    };

    window.addEventListener('pointermove', moveCursor, { passive: true });
    window.addEventListener('pointerover', setInteractive, { passive: true });
    window.addEventListener('pointerout', setInteractive, { passive: true });
    document.addEventListener('mouseleave', hideCursor);
    document.addEventListener('mouseenter', () => cursor.classList.add('is-visible'));
})();
