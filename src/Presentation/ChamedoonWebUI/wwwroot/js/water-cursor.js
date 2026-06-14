(function () {
    const finePointerQuery = window.matchMedia?.('(pointer: fine)');
    const reducedMotionQuery = window.matchMedia?.('(prefers-reduced-motion: reduce)');

    if (!finePointerQuery?.matches || reducedMotionQuery?.matches) {
        return;
    }

    const cursor = document.createElement('div');
    cursor.className = 'water-cursor';
    cursor.setAttribute('aria-hidden', 'true');
    cursor.innerHTML = '<span class="water-cursor__pointer"></span><span class="water-cursor__ripple water-cursor__ripple--one"></span><span class="water-cursor__ripple water-cursor__ripple--two"></span>';
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
