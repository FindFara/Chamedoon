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
        const fields = Array.from(document.querySelectorAll('.immigration-field .form-control, .immigration-field .form-select'));
        const progressEl = document.querySelector('[data-form-progress]');
        if (!progressEl || !fields.length) return;

        const updateProgress = () => {
            const filled = fields.filter((field) => {
                if (field.type === 'checkbox') {
                    return field.checked;
                }
                return Boolean(field.value);
            }).length;
            const percent = Math.min(100, Math.round((filled / fields.length) * 100));
            progressEl.style.setProperty('--progress', `${percent}%`);
            progressEl.setAttribute('data-progress-label', `${percent}% تکمیل شد`);
        };

        fields.forEach((field) => field.addEventListener('input', updateProgress));
        updateProgress();
    };

    const initScoreChart = () => {
        const rings = document.querySelector('[data-score-rings]');
        if (!rings) return;

        rings.querySelectorAll('.ring-card').forEach((card) => {
            const score = Number(card.dataset.score || 0);
            const clamped = Math.max(0, Math.min(100, Math.round(score)));
            const ring = card.querySelector('.ring');
            const label = card.querySelector('.ring-value');

            if (ring) {
                requestAnimationFrame(() => ring.style.setProperty('--value', clamped));
            }

            if (label) {
                label.textContent = `${clamped}%`;
            }
        });
    };

    const initPdfDownload = () => {
        const downloadButton = document.querySelector('[data-download-pdf]');
        const exportNode = document.getElementById('immigration-export-data');
        if (!downloadButton || !exportNode) return;

        const averageScore = Number(exportNode.dataset.average || 0);
        let exportData = [];

        try {
            exportData = JSON.parse(exportNode.textContent || '[]');
        } catch (error) {
            console.error('Failed to parse export data', error);
        }

        const illustration = encodeURIComponent(`
            <svg width="220" height="180" viewBox="0 0 220 180" xmlns="http://www.w3.org/2000/svg">
                <defs>
                    <linearGradient id="g1" x1="0%" x2="100%" y1="0%" y2="100%">
                        <stop offset="0%" stop-color="#6366f1" stop-opacity="0.95" />
                        <stop offset="100%" stop-color="#22d3ee" stop-opacity="0.9" />
                    </linearGradient>
                </defs>
                <rect x="14" y="16" width="190" height="150" rx="18" fill="url(#g1)" opacity="0.3" />
                <g fill="none" stroke="#312e81" stroke-width="3" opacity="0.3">
                    <circle cx="70" cy="86" r="34" />
                    <path d="M30 126 C80 156 140 156 186 116" />
                </g>
                <g fill="#0f172a" opacity="0.5">
                    <circle cx="150" cy="46" r="6" />
                    <circle cx="46" cy="54" r="4" />
                    <circle cx="172" cy="132" r="5" />
                </g>
                <path d="M70 44 L92 92 L48 92 Z" fill="#e0f2fe" opacity="0.6" />
                <rect x="112" y="72" width="64" height="46" rx="10" fill="#eef2ff" opacity="0.85" />
            </svg>
        `);

        const buildCard = (item, index) => {
            const score = Number(item.score || 0);
            const radius = 52;
            const circumference = Math.round(2 * Math.PI * radius);
            const offset = Math.round(circumference * (1 - Math.max(0, Math.min(100, score)) / 100));

            return `
                <div class="pdf-card">
                    <div class="pdf-card__header">
                        <div class="badge">#${index + 1}</div>
                        <div>
                            <div class="pdf-country">${item.country}</div>
                            <div class="pdf-visa">${item.visa}</div>
                        </div>
                    </div>
                    <div class="pdf-card__body">
                        <div class="ring-shell" aria-label="${score}%">
                            <svg viewBox="0 0 140 140" class="ring-svg" role="presentation">
                                <defs>
                                    <linearGradient id="pdfGradient" x1="0%" x2="100%" y1="0%" y2="100%">
                                        <stop offset="0%" stop-color="#6366f1" />
                                        <stop offset="100%" stop-color="#22d3ee" />
                                    </linearGradient>
                                </defs>
                                <circle class="ring-track" cx="70" cy="70" r="${radius}" />
                                <circle class="ring-progress" cx="70" cy="70" r="${radius}" stroke-dasharray="${circumference}" stroke-dashoffset="${offset}" />
                            </svg>
                            <div class="ring-svg__center">${score}%</div>
                        </div>
                        <div class="pdf-details">
                            <div><strong>شخصیت:</strong> ${item.personality}</div>
                            <div><strong>کار:</strong> ${item.job}</div>
                            <div><strong>تحصیل:</strong> ${item.education}</div>
                            <div><strong>اقتصاد:</strong> ${item.economy}</div>
                        </div>
                    </div>
                </div>
            `;
        };

        downloadButton.addEventListener('click', () => {
            const doc = window.open('', '_blank', 'width=900,height=1200');
            if (!doc) return;

            const rows = exportData.map(buildCard).join('');
            const today = new Date();
            const generatedAt = today.toLocaleDateString('fa-IR');
            const bestCountry = exportData[0] || {};

            doc.document.write(`
                <html>
                    <head>
                        <meta charset="utf-8">
                        <title>گزارش نتیجه مهاجرت</title>
                        <style>
                            * { box-sizing: border-box; }
                            body { font-family: 'Vazirmatn', sans-serif; color: #0f172a; background: #f8fafc; padding: 24px; direction: rtl; }
                            .pdf-shell { max-width: 1100px; margin: 0 auto; display: grid; gap: 18px; }
                            .pdf-hero { display: grid; grid-template-columns: 1.1fr 0.9fr; gap: 18px; align-items: center; background: linear-gradient(120deg, #eef2ff, #e0f2fe); border: 1px solid #c7d2fe; border-radius: 18px; padding: 20px; box-shadow: 0 10px 30px rgba(99,102,241,0.18); }
                            .pdf-hero h1 { margin: 0 0 6px; }
                            .pdf-hero p { margin: 0; color: #475569; }
                            .hero-illustration { width: 100%; max-width: 260px; margin-inline: auto; display: block; }
                            .pdf-summary { display: grid; gap: 12px; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); }
                            .summary-card { padding: 14px; background: #fff; border: 1px solid #e2e8f0; border-radius: 14px; box-shadow: 0 10px 30px rgba(15,23,42,0.06); }
                            .summary-card h4 { margin: 0 0 6px; }
                            .summary-card p { margin: 0; color: #475569; }
                            .pdf-card { background: #fff; border: 1px solid #e2e8f0; border-radius: 16px; padding: 14px; box-shadow: 0 12px 32px rgba(15,23,42,0.08); display: grid; gap: 12px; }
                            .pdf-card__header { display: flex; align-items: center; gap: 12px; justify-content: space-between; }
                            .badge { background: #eef2ff; color: #312e81; padding: 6px 12px; border-radius: 999px; font-weight: 700; }
                            .pdf-country { font-weight: 800; font-size: 1.05rem; }
                            .pdf-visa { color: #475569; font-size: 0.95rem; }
                            .pdf-card__body { display: grid; grid-template-columns: auto 1fr; gap: 14px; align-items: center; }
                            .ring-shell { position: relative; width: 140px; height: 140px; display: grid; place-items: center; }
                            .ring-svg { width: 140px; height: 140px; transform: rotate(-90deg); }
                            .ring-track { fill: none; stroke: #e2e8f0; stroke-width: 12; }
                            .ring-progress { fill: none; stroke: url(#pdfGradient); stroke-width: 12; stroke-linecap: round; transition: stroke-dashoffset 0.3s ease; }
                            .ring-svg__center { position: absolute; text-align: center; font-weight: 800; font-size: 1.1rem; color: #0f172a; }
                            .pdf-details { display: grid; gap: 4px; color: #1f2937; font-size: 0.98rem; }
                            .pdf-grid { display: grid; gap: 12px; }
                            @media print { body { padding: 0; } .pdf-card, .summary-card, .pdf-hero { box-shadow: none; } }
                        </style>
                    </head>
                    <body>
                        <div class="pdf-shell">
                            <div class="pdf-hero">
                                <div>
                                    <h1>گزارش خلاصه مهاجرت</h1>
                                    <p>نتیجه بر اساس آخرین پاسخ‌های شما تولید شد. برای اشتراک با مشاور یا چاپ، از این نسخه استفاده کن.</p>
                                    <p style="margin-top:10px;color:#312e81;font-weight:700;">میانگین کل: ${averageScore}% • تاریخ تولید: ${generatedAt}</p>
                                </div>
                                <img class="hero-illustration" src="data:image/svg+xml;utf8,${illustration}" alt="Immigration visual" />
                            </div>
                            <div class="pdf-summary">
                                <div class="summary-card">
                                    <h4>بهترین مقصد</h4>
                                    <p>${bestCountry.country || 'نامشخص'}</p>
                                </div>
                                <div class="summary-card">
                                    <h4>ویزای منتخب</h4>
                                    <p>${bestCountry.visa || '—'}</p>
                                </div>
                                <div class="summary-card">
                                    <h4>تعداد پیشنهاد</h4>
                                    <p>${exportData.length} کشور</p>
                                </div>
                            </div>
                            <div class="pdf-grid">${rows}</div>
                        </div>
                    </body>
                </html>
            `);

            doc.document.close();
            doc.focus();
            doc.print();
        });
    };

    const init = () => {
        initTooltips();
        initFieldHighlight();
        initScrollButtons();
        initProgress();
        initScoreChart();
        initPdfDownload();
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init, { once: true });
    } else {
        init();
    }
})();
