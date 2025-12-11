document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('flagBubblesField');
    if (!container) return;

    const stage = container.closest('.process-flag-stage') || container.parentElement;

    const bubbleData = Array.from(container.querySelectorAll('.flag-bubble')).map((bubble) => {
        const size = 54 + Math.random() * 18;
        const width = container.clientWidth;
        const height = container.clientHeight;
        const x = Math.random() * Math.max(width - size, 0);
        const y = Math.random() * Math.max(height - size, 0);
        const angle = Math.random() * Math.PI * 2;
        const speed = 0.65 + Math.random() * 0.9;

        bubble.style.width = `${size}px`;
        bubble.style.height = `${size}px`;
        bubble.style.transform = `translate(${x}px, ${y}px)`;

        return {
            el: bubble,
            x,
            y,
            size,
            vx: Math.cos(angle) * speed,
            vy: Math.sin(angle) * speed,
        };
    });

    const maxSpeed = 2.5;
    const repelRadius = 110;

    const clampSpeed = (value) => Math.max(Math.min(value, maxSpeed), -maxSpeed);

    const step = () => {
        const width = container.clientWidth;
        const height = container.clientHeight;

        bubbleData.forEach((bubble) => {
            bubble.x += bubble.vx;
            bubble.y += bubble.vy;

            if (bubble.x <= 0 || bubble.x + bubble.size >= width) {
                bubble.vx *= -1;
                bubble.x = Math.min(Math.max(bubble.x, 0), Math.max(width - bubble.size, 0));
            }

            if (bubble.y <= 0 || bubble.y + bubble.size >= height) {
                bubble.vy *= -1;
                bubble.y = Math.min(Math.max(bubble.y, 0), Math.max(height - bubble.size, 0));
            }

            bubble.vx = clampSpeed(bubble.vx);
            bubble.vy = clampSpeed(bubble.vy);

            bubble.el.style.transform = `translate(${bubble.x}px, ${bubble.y}px)`;
        });

        requestAnimationFrame(step);
    };

    window.addEventListener('resize', () => {
        const width = container.clientWidth;
        const height = container.clientHeight;

        bubbleData.forEach((bubble) => {
            bubble.x = Math.min(bubble.x, Math.max(width - bubble.size, 0));
            bubble.y = Math.min(bubble.y, Math.max(height - bubble.size, 0));
        });
    });

    const attachTarget = stage || container;

    attachTarget.addEventListener('mousemove', (event) => {
        const rect = container.getBoundingClientRect();
        const mouseX = event.clientX - rect.left;
        const mouseY = event.clientY - rect.top;

        bubbleData.forEach((bubble) => {
            const centerX = bubble.x + bubble.size / 2;
            const centerY = bubble.y + bubble.size / 2;
            const dx = centerX - mouseX;
            const dy = centerY - mouseY;
            const distance = Math.hypot(dx, dy) || 1;

            if (distance < repelRadius) {
                const force = (repelRadius - distance) / repelRadius;
                bubble.vx += (dx / distance) * (1.4 * force);
                bubble.vy += (dy / distance) * (1.4 * force);
            }
        });
    });

    requestAnimationFrame(step);
});
