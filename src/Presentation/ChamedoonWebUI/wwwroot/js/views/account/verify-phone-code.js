(() => {
    const root = document.querySelector('[data-verify-phone-code]');
    if (!root) return;

    const cooldownSeconds = 120;
    const countdownLabel = document.getElementById('verifyCountdown');
    const resendForm = document.querySelector('[data-form="resend-code"]');
    const resendButton = document.getElementById('resendCodeButton');
    const codeWasSent = root.dataset.codeSent === 'true';

    const formatTime = (seconds) => {
        const mins = Math.floor(seconds / 60);
        const secs = seconds % 60;
        return `${mins}:${secs.toString().padStart(2, '0')}`;
    };

    const updateCountdown = (remaining) => {
        if (!countdownLabel) return;
        countdownLabel.textContent = `ارسال مجدد کد پس از ${formatTime(remaining)}`;
    };

    const startCooldown = () => {
        if (!countdownLabel) return;
        let remaining = cooldownSeconds;
        countdownLabel.style.display = 'block';
        if (resendForm) {
            resendForm.style.display = 'none';
        }
        updateCountdown(remaining);

        const intervalId = setInterval(() => {
            remaining -= 1;
            if (remaining <= 0) {
                clearInterval(intervalId);
                countdownLabel.style.display = 'none';
                if (resendForm) {
                    resendForm.style.display = 'block';
                }
                return;
            }

            updateCountdown(remaining);
        }, 1000);
    };

    if (codeWasSent) {
        startCooldown();
    }

    if (resendForm) {
        resendForm.addEventListener('submit', () => {
            if (resendButton) {
                resendButton.disabled = true;
            }
        });
    }
})();
