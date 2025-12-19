(function () {
    const isFieldFilled = (field) => {
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
        const steps = Array.from(document.querySelectorAll('.immigration-step'));
        if (!steps.length) return;

        const getRequiredFields = (step) => Array.from(step.querySelectorAll('[data-progress-field]:not([data-progress-optional])'));
        const isStepComplete = (step) => getRequiredFields(step).every(isFieldFilled);
        const getValidationMessageEl = (field) => {
            const name = field.getAttribute('name');
            if (!name) return null;
            return document.querySelector(`[data-valmsg-for="${name}"]`);
        };

        const validateStepFields = (step) => {
            const requiredFields = getRequiredFields(step);
            let complete = true;

            requiredFields.forEach((field) => {
                const filled = isFieldFilled(field);
                const wrapper = field.closest('.immigration-field');
                const messageEl = getValidationMessageEl(field);

                if (!filled) {
                    complete = false;
                    wrapper?.classList.add('is-invalid');
                    field.setAttribute('aria-invalid', 'true');
                    if (messageEl) {
                        messageEl.textContent = '';
                        messageEl.classList.remove('field-validation-error');
                        messageEl.classList.add('field-validation-valid');
                    }
                } else {
                    wrapper?.classList.remove('is-invalid');
                    field.removeAttribute('aria-invalid');
                    if (messageEl) {
                        messageEl.textContent = '';
                        messageEl.classList.remove('field-validation-error');
                        messageEl.classList.add('field-validation-valid');
                    }
                }
            });

            return complete || requiredFields.length === 0;
        };

        const updateToggleStates = () => {
            steps.forEach((step, index) => {
                const toggle = step.querySelector('[data-step-toggle]');
                if (!toggle) return;

                const unlocked = index === 0 || steps.slice(0, index).every(isStepComplete);
                toggle.disabled = !unlocked;
                toggle.setAttribute('aria-disabled', String(!unlocked));
                toggle.classList.toggle('is-disabled', !unlocked);
            });
        };

        const setExpanded = (step, expanded) => {
            step.dataset.stepExpanded = expanded ? 'true' : 'false';
            const content = step.querySelector('[data-step-content]');
            if (content) {
                content.hidden = !expanded;
            }

            const toggle = step.querySelector('[data-step-toggle]');
            if (toggle) {
                toggle.setAttribute('aria-expanded', String(expanded));
            }
        };

        steps.forEach((step) => {
            step.dataset.stepExpanded = 'false';
            const content = step.querySelector('[data-step-content]');
            if (content) {
                content.hidden = true;
            }
        });

        const firstStep = steps[0];
        if (firstStep) {
            setExpanded(firstStep, true);
        }

        steps.forEach((step, index) => {
            const toggle = step.querySelector('[data-step-toggle]');
            if (toggle) {
                toggle.addEventListener('click', () => {
                    if (toggle.disabled) return;
                    const expanded = step.dataset.stepExpanded === 'true';
                    setExpanded(step, !expanded);
                });
            }

            getRequiredFields(step).forEach((field) => {
                const runValidation = () => {
                    const isComplete = validateStepFields(step);
                    if (isComplete && index + 1 < steps.length) {
                        const nextStep = steps[index + 1];
                        setExpanded(nextStep, true);
                    }
                    updateToggleStates();
                };

                field.addEventListener('input', runValidation);
                field.addEventListener('change', runValidation);
                field.addEventListener('blur', runValidation);
            });
        });

        updateToggleStates();
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
