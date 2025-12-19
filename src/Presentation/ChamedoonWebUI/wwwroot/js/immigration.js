(function () {
    let activateValidation = () => {};

    const isFieldFilled = (field) => {
        if (!field) return false;

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

    const setFieldValidityState = (field, isActive) => {
        const parent = field.closest('.immigration-field');
        if (!parent) return;

        const isOptional = field.hasAttribute('data-progress-optional');
        parent.classList.toggle('is-invalid', Boolean(isActive && !isOptional && !isFieldFilled(field)));
    };

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

        const updateProgress = () => {
            const filledRequired = requiredFields.filter(isFieldFilled).length;
            const filledOptional = optionalFields.filter(isFieldFilled).length;

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

    const initDeferredValidation = () => {
        const form = document.querySelector('.immigration-form-card form');
        const fields = Array.from(document.querySelectorAll('[data-progress-field]'));
        const requiredFields = fields.filter((field) => !field.hasAttribute('data-progress-optional'));
        if (!form || !requiredFields.length) return;

        let validationActivated = false;

        const renderValidation = () => {
            requiredFields.forEach((field) => setFieldValidityState(field, validationActivated));
        };

        fields.forEach((field) => {
            field.addEventListener('input', () => {
                if (!validationActivated) return;
                renderValidation();
            });

            field.addEventListener('change', () => {
                if (!validationActivated) return;
                renderValidation();
            });
        });

        activateValidation = () => {
            validationActivated = true;
            renderValidation();
        };

        // Clear any pre-existing error borders on first paint
        renderValidation();
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


    const initLoadingOverlay = () => {
        const overlay = document.querySelector('[data-loading-overlay]');
        const form = document.querySelector('.immigration-form-card form');
        if (!overlay || !form) return;

        const submitButton = form.querySelector('button[type="submit"]');

        const showOverlay = () => {
            overlay.removeAttribute('hidden');
            overlay.classList.add('is-visible');
            document.body.setAttribute('aria-busy', 'true');

            if (submitButton) {
                submitButton.setAttribute('disabled', 'disabled');
            }
        };

        form.addEventListener('submit', () => {
            activateValidation();
            showOverlay();
        });
    };


    const initResultAccordions = () => {
        const countryCards = document.querySelectorAll('[data-country-card]');
        countryCards.forEach((card) => {
            const toggle = card.querySelector('[data-country-toggle]');
            const panel = card.querySelector('[data-country-panel]');
            if (!toggle || !panel) return;

            const setExpanded = (expanded) => {
                toggle.setAttribute('aria-expanded', String(expanded));
                panel.hidden = !expanded;
                card.classList.toggle('is-open', expanded);
            };

            setExpanded(false);

            toggle.addEventListener('click', () => {
                const isExpanded = toggle.getAttribute('aria-expanded') === 'true';
                setExpanded(!isExpanded);
            });
        });

        document.querySelectorAll('[data-detail-toggle]').forEach((toggle) => {
            const targetId = toggle.getAttribute('aria-controls');
            const panel = targetId ? document.getElementById(targetId) : null;
            if (!panel) return;

            const setExpanded = (expanded) => {
                toggle.setAttribute('aria-expanded', String(expanded));
                panel.hidden = !expanded;
            };

            setExpanded(false);

            toggle.addEventListener('click', () => {
                const isExpanded = toggle.getAttribute('aria-expanded') === 'true';
                setExpanded(!isExpanded);
            });
        });
    };


    const initStepFlow = () => {
        const steps = Array.from(document.querySelectorAll('[data-immigration-step]'));
        if (!steps.length) return;

        const getRequiredFields = (step) => Array.from(step.querySelectorAll('[data-progress-field]:not([data-progress-optional])'));

        const setExpanded = (step, expanded) => {
            const body = step.querySelector('[data-step-body]');
            const toggle = step.querySelector('[data-step-toggle]');

            if (body) {
                body.hidden = !expanded;
                step.classList.toggle('is-collapsed', !expanded);
            }

            if (toggle) {
                toggle.setAttribute('aria-expanded', String(expanded));
            }

            step.classList.toggle('is-open', expanded);
        };

        const setLocked = (step, locked) => {
            const toggle = step.querySelector('[data-step-toggle]');
            step.classList.toggle('is-locked', locked);

            if (toggle) {
                toggle.disabled = locked;
                toggle.setAttribute('aria-disabled', String(locked));
            }

            if (locked) {
                setExpanded(step, false);
            }
        };

        const isComplete = (step) => getRequiredFields(step).every(isFieldFilled);

        steps.forEach((step, index) => {
            if (index !== 0) {
                step.classList.add('is-collapsed');
                const body = step.querySelector('[data-step-body]');
                if (body) {
                    body.hidden = true;
                }
            }

            setLocked(step, index !== 0);
            setExpanded(step, index === 0);
        });

        const syncLocks = () => {
            steps.forEach((step, index) => {
                if (index === 0) {
                    setLocked(step, false);
                    return;
                }

                const wasLocked = step.classList.contains('is-locked');
                const previousComplete = isComplete(steps[index - 1]);

                setLocked(step, !previousComplete);

                if (previousComplete && wasLocked) {
                    setExpanded(step, true);
                }
            });
        };

        steps.forEach((step) => {
            const toggle = step.querySelector('[data-step-toggle]');
            if (!toggle) return;

            toggle.addEventListener('click', () => {
                if (step.classList.contains('is-locked')) return;

                const isExpanded = toggle.getAttribute('aria-expanded') === 'true';
                setExpanded(step, !isExpanded);
            });
        });

        document.querySelectorAll('[data-progress-field]').forEach((field) => {
            field.addEventListener('input', syncLocks);
            field.addEventListener('change', syncLocks);
        });

        syncLocks();
    };


    const initNumberSteppers = () => {
        const wrappers = document.querySelectorAll('[data-number-input]');
        if (!wrappers.length) return;

        wrappers.forEach((wrapper) => {
            const input = wrapper.querySelector('input[type="number"]');
            if (!input) return;

            const parseOrFallback = (value, fallback) => {
                const numeric = Number(value);
                return Number.isFinite(numeric) ? numeric : fallback;
            };

            const applyStep = (direction) => {
                const step = parseOrFallback(input.step, 1) || 1;
                const min = input.min === '' ? -Infinity : parseOrFallback(input.min, -Infinity);
                const max = input.max === '' ? Infinity : parseOrFallback(input.max, Infinity);
                const baseValue = input.value === '' ? 0 : parseOrFallback(input.value, 0);

                const delta = direction === 'up' ? step : -step;
                let next = baseValue + delta;

                next = Math.min(max, Math.max(min, next));
                input.value = String(next);
                input.dispatchEvent(new Event('input', { bubbles: true }));
                input.dispatchEvent(new Event('change', { bubbles: true }));
                input.focus({ preventScroll: true });
            };

            wrapper.querySelectorAll('[data-step]').forEach((button) => {
                button.addEventListener('mousedown', (event) => {
                    event.preventDefault();
                });

                button.addEventListener('click', (event) => {
                    event.preventDefault();
                    const direction = button.getAttribute('data-step');
                    if (!direction) return;
                    applyStep(direction);
                });
            });
        });
    };


    const init = () => {
        initTooltips();
        initFieldHighlight();
        initScrollButtons();
        initProgress();
        initDeferredValidation();
        initScoreChart();
        initLoadingOverlay();
        initResultAccordions();
        initNumberSteppers();
        initStepFlow();

    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init, { once: true });
    } else {
        init();
    }
})();
