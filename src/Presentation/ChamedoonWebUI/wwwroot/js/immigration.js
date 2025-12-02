(function () {
    const initTooltips = () => {
        const prefersTouch = window.matchMedia('(hover: none)').matches;
        const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="tooltip"]'));

        tooltipTriggerList.forEach((triggerEl) => {
            const tooltip = new bootstrap.Tooltip(triggerEl, {
                trigger: prefersTouch ? 'manual' : 'hover focus',
                boundary: document.body
            });

            triggerEl.addEventListener('keydown', (event) => {
                if (event.key === 'Enter' || event.key === ' ') {
                    event.preventDefault();
                    tooltip.toggle();
                }
            });

            if (prefersTouch) {
                triggerEl.addEventListener('click', (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    tooltip.toggle();
                });
            }
        });

        if (prefersTouch) {
            document.addEventListener('click', (event) => {
                tooltipTriggerList.forEach((triggerEl) => {
                    const tooltipInstance = bootstrap.Tooltip.getInstance(triggerEl);
                    if (!tooltipInstance) {
                        return;
                    }

                    const tip = tooltipInstance.tip || (typeof tooltipInstance.getTipElement === 'function' ? tooltipInstance.getTipElement() : null);

                    if (triggerEl.contains(event.target)) {
                        return;
                    }

                    if (tip && tip.contains && tip.contains(event.target)) {
                        return;
                    }

                    tooltipInstance.hide();
                });
            });
        }
    };

    const initFieldHighlight = () => {
        document.querySelectorAll('.immigration-field .form-control, .immigration-field .form-select').forEach((input) => {
            const parent = input.closest('.immigration-field');
            if (!parent) return;

            input.addEventListener('focus', () => parent.classList.add('is-focused'));
            input.addEventListener('blur', () => parent.classList.remove('is-focused'));
        });
    };

    const initScrollButtons = () => {
        document.querySelectorAll('[data-scroll-target]').forEach((button) => {
            button.addEventListener('click', (event) => {
                const targetId = button.getAttribute('data-scroll-target');
                if (!targetId) return;
                const target = document.getElementById(targetId);
                if (!target) return;

                event.preventDefault();
                target.scrollIntoView({ behavior: 'smooth', block: 'start' });
            });
        });
    };

    const initProgress = () => {
        const fields = Array.from(document.querySelectorAll('[data-progress-field]'));
        const progressEl = document.querySelector('[data-form-progress]');
        if (!progressEl || !fields.length) return;

        const requiredFields = fields.filter((field) => !field.hasAttribute('data-progress-optional'));
        const optionalFields = fields.filter((field) => field.hasAttribute('data-progress-optional'));

        const isFilled = (field) => {
            if (field.type === 'checkbox') {
                return field.checked;
            }

            if (field.tagName === 'SELECT') {
                const value = field.value;
                return value !== '' && value !== '0';
            }

            if (field.type === 'number') {
                return Number(field.value || '0') > 0;
            }

            return Boolean(field.value && field.value.trim().length > 0);
        };

        const updateProgress = () => {
            const filledRequired = requiredFields.filter(isFilled).length;
            const filledOptional = optionalFields.filter(isFilled).length;

            const basePercent = requiredFields.length === 0 ? 0 : Math.round((filledRequired / requiredFields.length) * 100);
            const optionalBoost = optionalFields.length ? Math.round((filledOptional / optionalFields.length) * 10) : 0;
            const percent = Math.min(100, basePercent + optionalBoost);

            progressEl.style.setProperty('--progress', `${percent}%`);
            progressEl.setAttribute('data-progress-label', `${percent}% تکمیل شد`);
        };

        fields.forEach((field) => {
            field.addEventListener('input', updateProgress);
            field.addEventListener('change', updateProgress);
        });

        updateProgress();
    };

    const initScoreChart = () => {
        const rings = document.querySelector('[data-score-rings]');
        if (!rings) return;

        const updateRing = (card) => {
            if (!card) return;

            const score = Number(card.dataset.score || 0);
            const clamped = Math.max(0, Math.min(100, Math.round(score)));
            const progress = card.querySelector('[data-ring-progress]');
            const tip = card.querySelector('[data-ring-tip]');
            const tipValue = card.querySelector('[data-ring-value]');
            const ring = card.querySelector('.ring');
            const visual = card.querySelector('.ring-visual');

            const viewBoxSize = ring?.viewBox?.baseVal?.width || 140;
            const ringSize = visual?.getBoundingClientRect()?.width || viewBoxSize;
            const scale = ringSize / viewBoxSize;
            const baseRadius = Number(progress?.getAttribute('r') || 58);
            const radius = baseRadius * scale;
            const circumference = 2 * Math.PI * baseRadius;

            if (progress) {
                progress.style.strokeDasharray = `${circumference}`;
                const offset = circumference - (clamped / 100) * circumference;
                progress.style.strokeDashoffset = `${offset}`;
            }

            if (tip && tipValue) {
                const angle = (clamped / 100) * 360 - 90;
                const radians = (angle * Math.PI) / 180;
                const center = ringSize / 2;
                const tipRadius = radius + 6 * scale;
                const labelRadius = radius + 18 * scale;
                const dot = tip.querySelector('.tip-dot');
                const label = tip.querySelector('.tip-label');

                tipValue.textContent = `${clamped}%`;

                const dotX = center + tipRadius * Math.cos(radians);
                const dotY = center + tipRadius * Math.sin(radians);
                const labelX = center + labelRadius * Math.cos(radians);
                const labelY = center + labelRadius * Math.sin(radians);

                if (dot) {
                    dot.style.left = `${dotX}px`;
                    dot.style.top = `${dotY}px`;
                }

                if (label) {
                    label.style.left = `${labelX}px`;
                    label.style.top = `${labelY}px`;
                    label.setAttribute('aria-label', `${clamped}%`);
                }

                tip.classList.add('is-visible');
            }
        };

        const scheduleUpdate = (card) => {
            requestAnimationFrame(() => updateRing(card));
        };

        rings.querySelectorAll('.ring-item').forEach((card) => {
            updateRing(card);
            scheduleUpdate(card);

            if (window.ResizeObserver) {
                const visual = card.querySelector('.ring-visual');
                if (visual) {
                    const observer = new ResizeObserver(() => scheduleUpdate(card));
                    observer.observe(visual);
                }
            }
        });

        let resizeTimeout;
        window.addEventListener('resize', () => {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                rings.querySelectorAll('.ring-item').forEach((card) => updateRing(card));
            }, 150);
        });
    };


    const init = () => {
        initTooltips();
        initFieldHighlight();
        initScrollButtons();
        initProgress();
        initScoreChart();
        
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init, { once: true });
    } else {
        init();
    }
})();
