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

    const loadJsPdf = (() => {
        let loader;
        return async () => {
            if (loader) return loader;
            if (window.jspdf?.jsPDF || window.jsPDF) {
                loader = Promise.resolve(window.jspdf?.jsPDF || window.jsPDF);
                return loader;
            }

            loader = new Promise((resolve, reject) => {
                const script = document.createElement('script');
                script.src = 'https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js';
                script.async = true;
                script.onload = () => resolve(window.jspdf?.jsPDF || window.jsPDF);
                script.onerror = () => reject(new Error('jsPDF failed to load'));
                document.head.appendChild(script);
            });

            return loader;
        };
    })();

    const drawRing = (doc, cx, cy, radius, score) => {
        const clamped = Math.max(0, Math.min(100, score));
        const endAngle = (-90 + (clamped / 100) * 360) * (Math.PI / 180);
        const startAngle = -Math.PI / 2;
        const steps = Math.max(18, Math.round(clamped / 2));

        doc.setDrawColor(226, 232, 240);
        doc.setLineWidth(10);
        doc.circle(cx, cy, radius);

        doc.setDrawColor(99, 102, 241);
        doc.setLineCap('round');
        doc.setLineWidth(10);

        let prev = { x: cx + radius * Math.cos(startAngle), y: cy + radius * Math.sin(startAngle) };
        for (let i = 1; i <= steps; i++) {
            const t = startAngle + ((endAngle - startAngle) * i) / steps;
            const next = { x: cx + radius * Math.cos(t), y: cy + radius * Math.sin(t) };
            doc.line(prev.x, prev.y, next.x, next.y);
            prev = next;
        }

        doc.setLineWidth(1);
        doc.setTextColor(15, 23, 42);
        doc.setFontSize(12);
        doc.text(`${clamped}%`, cx, cy + 4, { align: 'center' });
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

        downloadButton.addEventListener('click', async () => {
            const originalText = downloadButton.textContent;
            downloadButton.disabled = true;
            downloadButton.textContent = 'در حال ساخت PDF...';

            try {
                const jsPDF = await loadJsPdf();
                const doc = new jsPDF({ orientation: 'p', unit: 'pt', format: 'a4' });
                const pageWidth = doc.internal.pageSize.getWidth();
                const pageHeight = doc.internal.pageSize.getHeight();
                const margin = 36;
                const contentWidth = pageWidth - margin * 2;

                const generatedAt = new Date().toLocaleDateString('fa-IR');
                const heroHeight = 130;

                doc.setFillColor(238, 242, 255);
                doc.roundedRect(margin, margin, contentWidth, heroHeight, 12, 12, 'F');
                doc.setDrawColor(199, 210, 254);
                doc.roundedRect(margin, margin, contentWidth, heroHeight, 12, 12, 'S');

                doc.setTextColor(49, 46, 129);
                doc.setFontSize(18);
                doc.text('گزارش خلاصه مهاجرت', pageWidth - margin, margin + 28, { align: 'right' });

                doc.setTextColor(71, 85, 105);
                doc.setFontSize(12);
                doc.text('نتیجه بر اساس آخرین پاسخ‌های شما تولید شد. برای اشتراک یا چاپ از این نسخه استفاده کن.', pageWidth - margin, margin + 48, {
                    align: 'right'
                });
                doc.text(`میانگین کل: ${averageScore}%  •  تاریخ تولید: ${generatedAt}`, pageWidth - margin, margin + 68, { align: 'right' });

                doc.setFillColor(226, 232, 240);
                doc.circle(margin + 54, margin + 54, 28, 'F');
                doc.setFillColor(99, 102, 241);
                doc.circle(margin + 54, margin + 54, 12, 'F');

                const badgeY = margin + heroHeight - 18;
                doc.setFillColor(255, 255, 255);
                doc.roundedRect(pageWidth - margin - 120, badgeY, 120, 26, 8, 8, 'F');
                doc.setTextColor(99, 102, 241);
                doc.text('گزارش آماده', pageWidth - margin - 12, badgeY + 17, { align: 'right' });

                let cursorY = margin + heroHeight + 20;
                const cardsPerRow = 2;
                const gap = 14;
                const cardWidth = (contentWidth - gap) / cardsPerRow;
                const cardHeight = 160;

                let row = 0;
                exportData.forEach((item, index) => {
                    const col = index % cardsPerRow;
                    if (col === 0 && index > 0) {
                        row += 1;
                    }

                    let cardY = cursorY + row * (cardHeight + 12);
                    if (cardY + cardHeight + margin > pageHeight) {
                        doc.addPage();
                        cursorY = margin;
                        row = 0;
                        cardY = cursorY;
                    }

                    const cardX = margin + (cardWidth + gap) * col;

                    doc.setFillColor(255, 255, 255);
                    doc.setDrawColor(226, 232, 240);
                    doc.roundedRect(cardX, cardY, cardWidth, cardHeight, 12, 12, 'FD');

                    doc.setTextColor(99, 102, 241);
                    doc.setFontSize(10);
                    doc.text(`#${index + 1}`, cardX + cardWidth - 12, cardY + 18, { align: 'right' });

                    doc.setTextColor(15, 23, 42);
                    doc.setFontSize(13);
                    doc.text(item.country || 'نامشخص', cardX + cardWidth - 12, cardY + 36, { align: 'right' });

                    doc.setTextColor(71, 85, 105);
                    doc.setFontSize(11);
                    doc.text(item.visa || 'ویزای پیشنهادی', cardX + cardWidth - 12, cardY + 52, { align: 'right' });

                    drawRing(doc, cardX + 60, cardY + 86, 32, Number(item.score || 0));

                    doc.setTextColor(31, 41, 55);
                    doc.setFontSize(10);
                    const detailX = cardX + cardWidth - 130;
                    doc.text(`شخصیت: ${item.personality || '-'}`, detailX, cardY + 78, { align: 'right' });
                    doc.text(`کار: ${item.job || '-'}`, detailX, cardY + 94, { align: 'right' });
                    doc.text(`تحصیل: ${item.education || '-'}`, detailX, cardY + 110, { align: 'right' });
                    doc.text(`اقتصاد: ${item.economy || '-'}`, detailX, cardY + 126, { align: 'right' });
                });

                doc.save('immigration-report.pdf');
            } catch (error) {
                console.error('PDF generation failed', error);
            } finally {
                downloadButton.disabled = false;
                downloadButton.textContent = originalText;
            }
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
