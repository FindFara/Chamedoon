document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('flagBubblesField');
    if (!container) return;

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

    requestAnimationFrame(step);
});
