document.addEventListener('DOMContentLoaded', function () {
    const dataEl = document.getElementById('subscriptionData');
    const paymentSuccessValue = dataEl?.dataset.paymentSuccess;
    const paymentResult = {
        message: dataEl?.dataset.paymentMessage || null,
        success: paymentSuccessValue === 'true' ? true : paymentSuccessValue === 'false' ? false : null,
        returnUrl: dataEl?.dataset.paymentReturnUrl || null,
        hasResult: dataEl?.dataset.paymentHasResult === 'true'
    };
    const globalDiscount = document.getElementById('global-discount');
    const planDiscountInputs = document.querySelectorAll('.plan-discount');
    const planPriceElements = document.querySelectorAll('[data-plan-price]');
    const applyButton = document.getElementById('apply-discount-button');
    const discountFeedback = document.getElementById('discount-feedback');
    const numberFormatter = new Intl.NumberFormat('fa-IR');

    if (!globalDiscount) return;

    const formatMoney = (value) => `${numberFormatter.format(value)} تومان`;

    const syncDiscounts = (value) => {
        const syncedValue = value ?? globalDiscount.value;
        planDiscountInputs.forEach(input => input.value = syncedValue);
    };

    const updateFeedback = (message, type) => {
        if (!discountFeedback) return;
        discountFeedback.textContent = message || '';
        discountFeedback.classList.remove('discount-feedback--error', 'discount-feedback--success');

        if (type === 'error') {
            discountFeedback.classList.add('discount-feedback--error');
        } else if (type === 'success') {
            discountFeedback.classList.add('discount-feedback--success');
        }
    };

    const updatePlanDisplay = (planId, finalPrice, originalPrice, savings) => {
        const priceEl = document.querySelector(`[data-plan-price="${planId}"]`);
        const badgeEl = document.querySelector(`[data-plan-badge="${planId}"]`);
        const savingsEl = document.querySelector(`[data-plan-saving="${planId}"]`);
        const resolvedOriginalPrice = originalPrice > 0
            ? originalPrice
            : Number(priceEl?.dataset.originalPrice ?? finalPrice);

        if (priceEl) {
            priceEl.textContent = formatMoney(finalPrice);
        }

        if (badgeEl && resolvedOriginalPrice > 0) {
            const percent = Math.max(0, 100 - Math.round(finalPrice / resolvedOriginalPrice * 100));
            badgeEl.textContent = percent > 0 ? `${percent}% تخفیف` : 'بدون تخفیف';
        }

        if (savingsEl) {
            if (savings > 0) {
                savingsEl.textContent = `صرفه‌جویی ${formatMoney(savings)} با کد تخفیف`;
                savingsEl.hidden = false;
            } else {
                savingsEl.textContent = '';
                savingsEl.hidden = true;
            }
        }
    };

    const resetPlanDisplays = () => {
        planPriceElements.forEach(priceEl => {
            const planId = priceEl.dataset.planPrice;
            const basePrice = parseInt(priceEl.dataset.basePrice ?? '0', 10);
            const originalPrice = parseInt(priceEl.dataset.originalPrice ?? `${basePrice}`, 10);
            updatePlanDisplay(planId, basePrice, originalPrice, 0);
        });

        updateFeedback('', null);
        syncDiscounts(globalDiscount.value);
    };

    syncDiscounts();
    globalDiscount.addEventListener('input', () => syncDiscounts());

    applyButton?.addEventListener('click', async (event) => {
        event.preventDefault();
        const code = globalDiscount.value.trim();

        if (!code) {
            resetPlanDisplays();
            updateFeedback('کد تخفیف را وارد کن.', 'error');
            return;
        }

        applyButton.disabled = true;
        applyButton.classList.add('is-loading');
        updateFeedback('در حال بررسی کد تخفیف...', null);

        try {
            const response = await fetch('/subscriptions/apply-discount', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: new URLSearchParams({ code })
            });

            const payload = await response.json().catch(() => null);

            if (!response.ok || !payload) {
                resetPlanDisplays();
                updateFeedback(payload?.message || 'کد تخفیف معتبر نیست.', 'error');
                return;
            }

            const appliedCode = payload.code ?? code;
            const plans = payload.plans ?? [];

            plans.forEach(plan => {
                const planId = plan.planId || plan.PlanId;
                const basePrice = Number(plan.basePrice ?? plan.BasePrice ?? 0);
                const finalPrice = Number(plan.finalPrice ?? plan.FinalPrice ?? basePrice);
                const originalPrice = Number(plan.originalPrice ?? plan.OriginalPrice ?? basePrice);
                const savings = Number(plan.discountAmount ?? plan.DiscountAmount ?? 0);
                updatePlanDisplay(planId, finalPrice, originalPrice, savings);
            });

            globalDiscount.value = appliedCode;
            syncDiscounts(appliedCode);
            updateFeedback(payload.message || 'کد تخفیف اعمال شد.', 'success');
        } catch {
            resetPlanDisplays();
            updateFeedback('خطا در برقراری ارتباط. دوباره تلاش کن.', 'error');
        } finally {
            applyButton.disabled = false;
            applyButton.classList.remove('is-loading');
        }
    });

    const showPaymentResultModal = () => {
        if (!paymentResult.hasResult) return;
        if (!paymentResult.message) return;

        const isSuccess = paymentResult.success === true;
        const isFailure = paymentResult.hasResult && !isSuccess;
        const fallbackMessage = isFailure
            ? 'پرداخت تایید نشد.'
            : 'پرداخت با موفقیت انجام شد.';

        const modalElement = document.getElementById('paymentResultModal');
        if (!modalElement || typeof bootstrap === 'undefined') return;

        modalElement.setAttribute('data-return-url', paymentResult.returnUrl ?? '');
        modalElement.classList.toggle('payment-result-modal--success', isSuccess);
        modalElement.classList.toggle('payment-result-modal--failure', isFailure);

        const titleEl = modalElement.querySelector('[data-role="payment-title"]');
        const messageEl = modalElement.querySelector('[data-role="payment-message"]');
        const continueBtn = modalElement.querySelector('[data-role="payment-continue"]');

        if (titleEl) {
            titleEl.textContent = isFailure ? 'پرداخت ناموفق' : 'پرداخت موفق';
        }

        if (messageEl) {
            messageEl.textContent = paymentResult.message || fallbackMessage;
        }

        if (continueBtn) {
            continueBtn.addEventListener('click', () => {
                const destination = paymentResult.returnUrl || '/';
                window.location.href = destination;
            }, { once: true });
        }

        const modalInstance = bootstrap.Modal.getOrCreateInstance(modalElement, {
            backdrop: 'static',
            keyboard: false
        });
        modalInstance.show();
    };

    showPaymentResultModal();
});
