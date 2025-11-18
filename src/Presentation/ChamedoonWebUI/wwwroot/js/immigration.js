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
            const progress = card.querySelector('[data-ring-progress]');
            const label = card.querySelector('.ring-value');
            const radius = 58;
            const circumference = 2 * Math.PI * radius;

            if (progress) {
                progress.style.strokeDasharray = `${circumference}`;
                progress.style.strokeDashoffset = `${circumference}`;

                requestAnimationFrame(() => {
                    const offset = circumference - (clamped / 100) * circumference;
                    progress.style.strokeDashoffset = `${offset}`;
                });
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

    const createRingImage = (score, accent = '#6366f1') => {
        const clamped = Math.max(0, Math.min(100, Number(score) || 0));
        const size = 180;
        const radius = 60;
        const center = size / 2;
        const canvas = document.createElement('canvas');
        canvas.width = size;
        canvas.height = size;

        const ctx = canvas.getContext('2d');
        if (!ctx) return '';

        ctx.translate(center, center);
        ctx.save();

        ctx.beginPath();
        ctx.arc(0, 0, radius, 0, Math.PI * 2);
        ctx.strokeStyle = 'rgba(148, 163, 184, 0.25)';
        ctx.lineWidth = 12;
        ctx.stroke();

        const startAngle = -Math.PI / 2;
        const endAngle = startAngle + (Math.PI * 2 * clamped) / 100;
        const gradient = ctx.createLinearGradient(-radius, -radius, radius, radius);
        gradient.addColorStop(0, accent);
        gradient.addColorStop(1, '#a5b4fc');

        ctx.beginPath();
        ctx.arc(0, 0, radius, startAngle, endAngle);
        ctx.strokeStyle = gradient;
        ctx.lineWidth = 12;
        ctx.lineCap = 'round';
        ctx.stroke();

        ctx.font = 'bold 20px Inter, Vazirmatn, sans-serif';
        ctx.fillStyle = '#0f172a';
        ctx.textAlign = 'center';
        ctx.fillText(`${clamped}%`, 0, 6);

        ctx.font = '14px Inter, Vazirmatn, sans-serif';
        ctx.fillStyle = '#475569';
        ctx.fillText('امتیاز', 0, 26);

        ctx.restore();
        return canvas.toDataURL('image/png');
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

        const accentPalette = ['#6366f1', '#0ea5e9', '#8b5cf6', '#10b981'];

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

                    const accent = accentPalette[index % accentPalette.length];
                    const ringImage = createRingImage(item.score, accent);

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

                    if (ringImage) {
                        doc.addImage(ringImage, 'PNG', cardX + 18, cardY + 18, 84, 84);
                    }

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
