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
        <svg class="water-cursor__icon" viewBox="0 0 48 48" focusable="false" aria-hidden="true">
            <path class="water-cursor__shape" d="M10 6 L38 24 L26 28 L21 40 Z" />
            <circle class="water-cursor__dot" cx="34" cy="14" r="4" />
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
