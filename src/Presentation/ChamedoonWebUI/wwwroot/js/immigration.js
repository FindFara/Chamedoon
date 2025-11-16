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
        const chart = document.querySelector('[data-score-chart]');
        if (!chart) return;

        chart.querySelectorAll('.chart-bar').forEach((bar, index) => {
            const score = Number(bar.dataset.score || 0);
            const clamped = Math.max(0, Math.min(100, Math.round(score)));
            const fill = bar.querySelector('.chart-fill');
            if (fill) {
                requestAnimationFrame(() => fill.style.setProperty('--fill', `${clamped}%`));
            }
            const value = bar.parentElement?.querySelector('.chart-value');
            if (value) {
                value.textContent = `${clamped}%`;
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

        const buildRow = (item, index) => {
            const score = Number(item.score || 0);
            return `
                <tr>
                    <td style="padding:12px 10px;font-weight:600;">${index + 1}</td>
                    <td style="padding:12px 10px;">${item.country}</td>
                    <td style="padding:12px 10px;">${item.visa}</td>
                    <td style="padding:12px 10px;">
                        <div style="background:#eef2ff;border-radius:8px;overflow:hidden;">
                            <div style="width:${score}%;background:linear-gradient(120deg,#6366f1,#22d3ee);height:10px;"></div>
                        </div>
                        <div style="font-weight:700;margin-top:4px;">${score}%</div>
                    </td>
                    <td style="padding:12px 10px;font-size:12px;line-height:1.6;">
                        <div><strong>شخصیت:</strong> ${item.personality}</div>
                        <div><strong>کار:</strong> ${item.job}</div>
                        <div><strong>تحصیل:</strong> ${item.education}</div>
                        <div><strong>اقتصاد:</strong> ${item.economy}</div>
                    </td>
                </tr>
            `;
        };

        downloadButton.addEventListener('click', () => {
            const doc = window.open('', '_blank', 'width=900,height=1200');
            if (!doc) return;

            const rows = exportData.map(buildRow).join('');
            const today = new Date();
            const generatedAt = today.toLocaleDateString('fa-IR');

            doc.document.write(`
                <html>
                    <head>
                        <meta charset="utf-8">
                        <title>گزارش نتیجه مهاجرت</title>
                        <style>
                            body { font-family: 'Vazirmatn', sans-serif; color: #0f172a; background: #f8fafc; padding: 24px; direction: rtl; }
                            h1 { margin: 0 0 8px; }
                            .muted { color: #475569; margin: 0 0 16px; }
                            table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 12px; overflow: hidden; box-shadow: 0 8px 30px rgba(15,23,42,0.08); }
                            th { text-align: right; background: #eef2ff; color: #312e81; padding: 12px 10px; }
                            td { border-top: 1px solid #e2e8f0; vertical-align: top; }
                        </style>
                    </head>
                    <body>
                        <h1>گزارش خلاصه مهاجرت</h1>
                        <p class="muted">میانگین کلی: ${averageScore}% • تاریخ تولید: ${generatedAt}</p>
                        <table>
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>کشور</th>
                                    <th>ویزای پیشنهادی</th>
                                    <th>امتیاز</th>
                                    <th>توضیحات تکمیلی</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${rows}
                            </tbody>
                        </table>
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
